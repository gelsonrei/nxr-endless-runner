using System.Collections.Generic;
using UnityEngine;

public class SpawnControl : MonoBehaviour
{
    public GameObject[] spawnPoints;
    public GameObject coin;
    public GameObject wallet;
    public GameObject[] obstacles;
    public int spawnCount = 10;
    public int minCoins = 1;
    public int maxCoins = 5;
    public int minPointsForCard = 20;
    public float minDistance = 10.0f;
    [Range(0f, 1f)]
    public float cardSpawnProbability = 0.5f;
    [Range(0f, 1f)]
    public float obstacleSpawnProbability = 0.6f; // 60% chance to spawn an obstacle
    
    private List<int> _spawnOrder;
    private int _lastObstacleLane = -1;

    private void Start()
    {
        GenerateSpawnOrder();
        ExecuteSpawn();
    }

    private void GenerateSpawnOrder()
    {
        _spawnOrder = new List<int>();

        var previousObject = 0;
        var coinsInARow = 0;
        var objectsToSpawn = spawnCount;

        while (objectsToSpawn > 0)
        {
            int nextObject;
            
            // Verificar se estamos no início ou no final de uma lane
            var currentPosition = spawnCount - objectsToSpawn + 1;
            if (currentPosition == 1 || objectsToSpawn == 1)  // Primeira ou última posição de uma lane
            {
                nextObject = Random.Range(1, 3); // Garante que seja moeda ou carteira, mas não obstáculo
                _spawnOrder.Add(nextObject);
                previousObject = nextObject;
                objectsToSpawn--;
                continue;  // Pula o resto do loop
            }
            
            if (coinsInARow > 0)
            {
                nextObject = 1; // next object will be a coin
                coinsInARow--;
            }
            else
            {
                var probability = Random.Range(0f, 1f);

                // if the probability is less than the obstacleSpawnProbability and the previous object was not an obstacle
                if (probability < obstacleSpawnProbability && previousObject != 2 && GameManager.Instance.player.transform.position.z > minDistance)
                {
                    nextObject = 2; // obstacle
                }
                else
                {
                    do
                    {
                        nextObject = Random.Range(1, 4); // random between 1 (coin), 2 (obstacle), and 3 (wallet)
                    }
                    while ((nextObject == 2 && previousObject == 2) || (nextObject == 3 && previousObject == 3)); // ensure no consecutive obstacle or wallet

                    if (nextObject == 3)
                    {
                        if (GameManager.Instance.points >= minPointsForCard &&
                            !GameManager.Instance.player.GetComponent<PlayerControl>().isMagnetic &&
                            !_spawnOrder.Contains(3) &&
                            GameManager.Instance.player.transform.position.z > minDistance)
                        {
                            probability = Random.Range(0f, 1f);

                            if (probability > cardSpawnProbability)
                            {
                                nextObject = Random.Range(1, 3); // if it didn't fall in the probability, switch to coin or obstacle
                            }
                        }
                        else
                        {
                            nextObject = GameManager.Instance.player.transform.position.z > minDistance ? Random.Range(1, 3) : // if points are less than minPointsForCard or isMagnetic is true or card has been instantiated, switch to coin or obstacle
                                1;
                        }
                    }

                    if (nextObject == 2 && GameManager.Instance.player.transform.position.z < minDistance)
                    {
                        nextObject = 1;
                    }
                }

                if (nextObject == 1)
                {
                    coinsInARow = Random.Range(minCoins, maxCoins + 1) - 1; // decide how many coins to spawn in a row
                    objectsToSpawn -= coinsInARow;
                }
            }

            _spawnOrder.Add(nextObject);
            previousObject = nextObject;
            objectsToSpawn--;
        }
    }

    private void ExecuteSpawn()
    {
        var zOffset = 0;
        foreach (var spawnObject in _spawnOrder)
        {
            var selectedSpawnPointIndex = Random.Range(0, spawnPoints.Length);
            if (spawnObject == 2)
            {
                do
                {
                    selectedSpawnPointIndex = Random.Range(0, spawnPoints.Length);
                } while (selectedSpawnPointIndex == _lastObstacleLane); // Ensure the obstacle is not in the same lane as the last one
                _lastObstacleLane = selectedSpawnPointIndex; // Save the lane where the obstacle was spawned
            }
            var obj = spawnPoints[selectedSpawnPointIndex].transform;
            var rotation = obj.rotation;
            Vector3 newLocation;
            Quaternion newRotation;
            
            switch (spawnObject)
            {
                case 1:
                    // coin
                    var coinCount = Random.Range(minCoins, maxCoins + 1);
                    
                    for (var i = 0; i < coinCount; i++)
                    {
                        newLocation = obj.position + new Vector3(0, 0.5f, -zOffset - i);
                        
                        newRotation = Quaternion.Euler(rotation.x + 90, rotation.y, rotation.z);

                        SpawnCoin(newLocation, newRotation, obj);
                    }

                    zOffset += coinCount;
                    break;

                case 2:
                    // obstacle
                    newLocation = obj.position + new Vector3(0, 0, -zOffset);
                    newRotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);

                    SpawnObstacle(newLocation, newRotation, obj);

                    zOffset++;
                    break;

                case 3:
                    // wallet
                    newLocation = obj.position + new Vector3(0, 0.5f, -zOffset);
                    newRotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);

                    SpawnWallet(newLocation, newRotation, obj);

                    zOffset++;
                    break;
            }
        }
    }

    private void SpawnObstacle(Vector3 nPosition, Quaternion nRotation, Transform parent)
    {
        if (obstacles.Length > 0)
        {
            Instantiate(obstacles[Random.Range(0, obstacles.Length)], nPosition, nRotation, parent);
        }
    }

    private void SpawnCoin(Vector3 nPosition, Quaternion nRotation, Transform parent)
    {
        if (coin != null)
        {
            Instantiate(coin, nPosition, nRotation, parent);
        }
    }

    private void SpawnWallet(Vector3 nPosition, Quaternion nRotation, Transform parent)
    {
        if (wallet != null)
        {
            Instantiate(wallet, nPosition, nRotation, parent);
        }
    }
}
