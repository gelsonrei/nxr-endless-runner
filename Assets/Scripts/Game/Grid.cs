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

    [Header("Objects to Spawn")] [SerializeField]
    private GameObject coinPrefab;

    [SerializeField] private GameObject walletPrefab;
    [SerializeField] private GameObject[] obstaclesPrefab;

    [Header("Grid Settings")] [SerializeField]
    private int width = 3;

    [SerializeField] private int height = 10;

    [SerializeField] private float size = 2.0f;

    [Header("Spawn Settings")] [SerializeField] [Range(0f, 1f)]
    private float obstacleRatio = 0.5f;

    [SerializeField] [Range(0f, 1f)] private float emptyRatio = 0.5f;

    [SerializeField] [Range(0f, 1f)] private float coinRatio = 0.5f;

    [SerializeField] [Range(0f, 1f)] private float cardRatio = 0.05f;

    [SerializeField] private int cardLimit = 1;

    private int _cardCount = 0;

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

            if (rand < obstacleRatio)
            {
                return ObjectType.Obstacle;
            }
            else if (rand < obstacleRatio + cardRatio)
            {
                if (_cardCount >= cardLimit)
                {
                    continue;
                }

                _cardCount++;

                return ObjectType.Card;
            }
            else if (rand < obstacleRatio + cardRatio + coinRatio)
            {
                return ObjectType.Coin;
            }
            else
            {
                return ObjectType.Empty;
            }

            break;
        }
    }

    private void GenerateSpawnOrder()
    {
        for (var y = 0; y < height; y++)
        {
            var isEdgeRow = y == 0 || y == height - 1;
            // Flag to check if there's at least one passable obstacle in the row.
            var mustHavePassableSpace = !isEdgeRow;
            var hasPassableSpace = isEdgeRow;

            for (var x = 0; x < width; x++)
            {
                if (IsOccupied(x, y)) continue;

                var objectType = GetRandomObjectType();

                // Rule 01 - Cannot be a obstacle if it's the first ou the last line of the grid.
                while (isEdgeRow && objectType == ObjectType.Obstacle)
                {
                    objectType = GetRandomObjectType();
                }

                // Continue to the next iteration if it's an edge row after setting the objectType.
                if (isEdgeRow)
                {
                    SetSlot(x, y, GetPrefabFromObjectType(objectType), objectType);
                    continue;
                }

                // Rule 02 - If not an edge row, check if the previous slot in the column is passable
                var previousSlot = GetSlot(x, y - 1);
                // var previousObstacle = previousSlot.Occupant.GetComponent<Obstacle>();
                Obstacle previousObstacle = null;
                if (previousSlot.Occupant != null)
                {
                    previousObstacle = previousSlot.Occupant.GetComponent<Obstacle>();
                }

                while (previousSlot.Type == ObjectType.Obstacle && objectType == ObjectType.Obstacle)
                {
                    objectType = GetRandomObjectType();
                }

                // TODO:
                SetSlot(x, y, GetPrefabFromObjectType(objectType), objectType);

                // If the slot type determined is not an obstacle, or it's a passable obstacle, set the flag to true
                // if (objectType != ObjectType.Obstacle || IsObstaclePassable(previousObstacle))
                // {
                //     hasPassableSpace = true;
                // }

                //
                // // //SetSlot(x, y, objectType);
                // //
                // // if (!hasPassableSpace)
                // // {
                // //
                // // }
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

    private static bool IsObstaclePassable(Obstacle obstacle)
    {
        return obstacle == null || obstacle.Type != Obstacle.ObstacleType.Dodge;
    }
}