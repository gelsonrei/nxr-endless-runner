using System;
using System.Collections.Generic;
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
    private float obstacleRatio = 0.7f;

    [SerializeField] [Range(0f, 1f)] private float coinRatio = 0.3f;
    [SerializeField] [Range(0f, 1f)] private float cardRatio = 0f; // Definido como 0 para exemplo
    [SerializeField] private int cardLimit = 1;

    private int maxObstacleCount;
    private int maxCoinCount;
    private int maxCardCount;
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

        CalculateMaxCounts();
        GenerateSpawnOrder();
        ExecuteSpawn();
    }

    private void CalculateMaxCounts()
    {
        int totalSlots = width * height;
        maxObstacleCount = Mathf.FloorToInt(obstacleRatio * totalSlots);
        maxCoinCount = Mathf.FloorToInt(coinRatio * totalSlots);
        maxCardCount = Mathf.FloorToInt(cardRatio * totalSlots);
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

    private ObjectType GetRandomObjectType()
    {
        if (obstacleCount < maxObstacleCount)
        {
            obstacleCount++;
            return ObjectType.Obstacle;
        }
        else if (coinCount < maxCoinCount)
        {
            coinCount++;
            return ObjectType.Coin;
        }
        else if (cardCount < maxCardCount)
        {
            cardCount++;
            return ObjectType.Card;
        }
        else
        {
            return ObjectType.Empty;
        }
    }

    private List<ObjectType> CreateShuffledObjectList()
    {
        var objectList = new List<ObjectType>();

        // Adiciona os objetos de acordo com suas contagens máximas
        for (var i = 0; i < maxObstacleCount; i++)
            objectList.Add(ObjectType.Obstacle);
        for (var i = 0; i < maxCoinCount; i++)
            objectList.Add(ObjectType.Coin);
        for (var i = 0; i < maxCardCount; i++)
            objectList.Add(ObjectType.Card);

        // Preencher o restante com espaços vazios
        while (objectList.Count < width * height)
            objectList.Add(ObjectType.Empty);

        // Embaralhar a lista
        var rng = new System.Random();
        var n = objectList.Count;
        while (n > 1)
        {
            n--;
            var k = rng.Next(n + 1);
            (objectList[k], objectList[n]) = (objectList[n], objectList[k]);
        }

        return objectList;
    }

    private void GenerateSpawnOrder()
    {
        List<ObjectType> objectList = CreateShuffledObjectList();
        int listIndex = 0;

        for (var y = 0; y < height; y++)
        {
            var isEdgeRow = y == 0 || y == height - 1;

            for (var x = 0; x < width; x++)
            {
                if (listIndex >= objectList.Count)
                {
                    break; // Todos os objetos foram alocados
                }

                ObjectType objectType = objectList[listIndex];

                // Verifica a Regra 01 - Não colocar obstáculo nas linhas de borda
                if (isEdgeRow && objectType == ObjectType.Obstacle)
                {
                    listIndex++; // Pula este objeto e tenta o próximo
                    x--; // Reconsidera este slot
                    continue;
                }

                // Verifica a Regra 02 - Evitar obstáculos consecutivos para manter a transitabilidade
                if (!isEdgeRow && y > 0)
                {
                    var previousSlot = GetSlot(x, y - 1);
                    if (previousSlot.Type == ObjectType.Obstacle && objectType == ObjectType.Obstacle)
                    {
                        listIndex++; // Pula este objeto e tenta o próximo
                        x--; // Reconsidera este slot
                        continue;
                    }
                }

                // Se passar pelas verificações, define o objeto e avança para o próximo na lista
                SetSlot(x, y, GetPrefabFromObjectType(objectType), objectType);
                listIndex++;
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