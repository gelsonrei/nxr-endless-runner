using System;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private enum ObjectType
    {
        Empty,
        Coin,
        Obstacle,
        Card
    }

    private struct Slot
    {
        public GameObject Occupant;
        public GameObject Prefab;
        public ObjectType Type;
    }

    [Header("Objects to Spawn")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject walletPrefab;
    [SerializeField] private GameObject[] obstaclesPrefab;

    [Header("Grid Settings")]
    [SerializeField] private int width = 3;
    [SerializeField] private int height = 10;
    [SerializeField] private float size = 2.0f;

    [Header("Spawn Settings")]
    [SerializeField] [Range(0f, 1f)] private float obstacleRatio = 0.5f;
    [SerializeField] [Range(0f, 1f)] private float emptyRatio = 0.5f;
    [SerializeField] [Range(0f, 1f)] private float coinRatio = 0.5f;
    [SerializeField] [Range(0f, 1f)] private float cardRatio = 0.05f;
    [SerializeField] private int cardLimit = 1;

    private int obstacleCount = 0;
    private int coinCount = 0;
    private int cardCount = 0;

    private Slot[] _slots;

    public bool autoStart = true;

    private void Awake()
    {
        _slots = new Slot[width * height];

        for (var i = 0; i < _slots.Length; i++)
        {
            _slots[i].Type = ObjectType.Empty;
        }
    }

    private void Start()
    {
        if (!autoStart) return;
        GenerateSpawnOrder();
        ExecuteSpawn();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        var position = transform.position;
        var xOffset = width * size * 0.5f;

        for (var x = 0; x <= width; x++)
        {
            var start = position + new Vector3(x * size - xOffset, 0, 0);
            var end = position + new Vector3(x * size - xOffset, 0, height * size);
            Gizmos.DrawLine(start, end);
        }

        for (var z = 0; z <= height; z++)
        {
            var start = position + new Vector3(-xOffset, 0, z * size);
            var end = position + new Vector3(width * size - xOffset, 0, z * size);
            Gizmos.DrawLine(start, end);
        }
    }

    private bool IsOccupied(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            throw new ArgumentOutOfRangeException();
        }

        return _slots[x + y * width].Type != ObjectType.Empty;
    }

    private Slot GetSlot(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            throw new ArgumentOutOfRangeException();
        }

        return _slots[x + y * width];
    }

    private void SetSlot(int x, int y, GameObject prefab, ObjectType type)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            throw new ArgumentOutOfRangeException();
        }

        var slotIndex = x + y * width;
        _slots[slotIndex].Type = type;
        _slots[slotIndex].Occupant = prefab;
    }

    private void NormalizeRatios()
    {
        var total = obstacleRatio + cardRatio + coinRatio + emptyRatio;

        obstacleRatio /= total;
        cardRatio /= total;
        coinRatio /= total;
        emptyRatio /= total;
    }

    private ObjectType GetRandomObjectType()
    {
        while (true)
        {
            NormalizeRatios();

            var rand = UnityEngine.Random.value;

            if (rand < obstacleRatio && obstacleCount < Mathf.FloorToInt(obstacleRatio * width * height))
            {
                obstacleCount++;
                return ObjectType.Obstacle;
            }
            else if (rand < obstacleRatio + cardRatio && cardCount < Mathf.FloorToInt(cardRatio * width * height) && cardCount < cardLimit)
            {
                cardCount++;
                return ObjectType.Card;
            }
            else if (rand < obstacleRatio + cardRatio + coinRatio && coinCount < Mathf.FloorToInt(coinRatio * width * height))
            {
                coinCount++;
                return ObjectType.Coin;
            }
            else
            {
                return ObjectType.Empty;
            }
        }
    }

    private void GenerateSpawnOrder()
    {
        for (var y = 0; y < height; y++)
        {
            var isEdgeRow = y == 0 || y == height - 1;

            for (var x = 0; x < width; x++)
            {
                if (IsOccupied(x, y)) continue;

                ObjectType objectType = GetRandomObjectType();

                // Regra 01 - Não pode ser um obstáculo se for a primeira ou a última linha da grade.
                if (isEdgeRow && objectType == ObjectType.Obstacle)
                {
                    continue; // Pula a geração de obstáculo nas linhas de borda.
                }

                // Regra 02 - Se não for uma linha de borda, verifique se o slot anterior na coluna é transitável
                if (!isEdgeRow && y > 0)
                {
                    var previousSlot = GetSlot(x, y - 1);
                    if (previousSlot.Type == ObjectType.Obstacle && objectType == ObjectType.Obstacle)
                    {
                        continue; // Evita obstáculos consecutivos para manter a transitabilidade.
                    }
                }

                SetSlot(x, y, GetPrefabFromObjectType(objectType), objectType);
            }
        }
    }

    private void ExecuteSpawn()
    {
        for (var i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].Type == ObjectType.Empty || _slots[i].Occupant == null) continue;

            var spawnPosition = CalculatePositionFromIndex(i);
            _slots[i].Occupant = Instantiate(_slots[i].Occupant, spawnPosition, Quaternion.identity, transform);
        }
    }

    private Vector3 CalculatePositionFromIndex(int index)
    {
        var x = index % width;
        var y = index / width;
        var basePosition = new Vector3(
            -width * size * 0.5f + size * 0.5f,
            0,
            -height * size * 0.5f + size * 0.5f + 10
        );

        return new Vector3(x * size, 0, y * size) + basePosition + transform.position;
    }

    private GameObject GetPrefabFromObjectType(ObjectType objectType)
    {
        return objectType switch
        {
            ObjectType.Coin => coinPrefab,
            ObjectType.Card => walletPrefab,
            ObjectType.Obstacle => obstaclesPrefab[UnityEngine.Random.Range(0, obstaclesPrefab.Length)],
            _ => null
        };
    }
}
