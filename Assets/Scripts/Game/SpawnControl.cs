using System.Collections;
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
    
    private List<int> spawnOrder;

    void Start()
    {
        GenerateSpawnOrder();
        ExecuteSpawn();
    }

    void GenerateSpawnOrder()
    {
        spawnOrder = new List<int>();

        int previousObject = 0;
        int coinsInARow = 0;
        int objectsToSpawn = spawnCount;

        while (objectsToSpawn > 0)
        {
            int nextObject;

            if (coinsInARow > 0)
            {
                nextObject = 1; // next object will be a coin
                coinsInARow--;
            }
            else
            {
                float probability = Random.Range(0f, 1f);

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
                            !spawnOrder.Contains(3) &&
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
                            if (GameManager.Instance.player.transform.position.z > minDistance)
                            {
                                nextObject = Random.Range(1, 3); // if points are less than minPointsForCard or isMagnetic is true or card has been instantiated, switch to coin or obstacle
                            }
                            else
                            {
                                nextObject = 1;
                            }
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

            spawnOrder.Add(nextObject);

            previousObject = nextObject;
            objectsToSpawn--;
        }
    }


    void ExecuteSpawn()
    {
        int z_offset = 0;
        foreach (int spawnObject in spawnOrder)
        {
            Transform obj = spawnPoints[Random.Range(0, spawnPoints.Length)].transform;
            Vector3 newLocation;
            Quaternion newRotation;

            switch (spawnObject)
            {
                case 1:
                    // coin
                    int coinCount = Random.Range(minCoins, maxCoins + 1);

                    for (int i = 0; i < coinCount; i++)
                    {
                        newLocation = obj.position + new Vector3(0, 0.5f, -z_offset - i);
                        newRotation = Quaternion.Euler(obj.rotation.x + 90, obj.rotation.y, obj.rotation.z);

                        SpawnCoin(newLocation, newRotation, obj);
                    }

                    z_offset += coinCount;
                    break;

                case 2:
                    // obstacle
                    newLocation = obj.position + new Vector3(0, 0, -z_offset);
                    newRotation = Quaternion.Euler(obj.rotation.x, obj.rotation.y, obj.rotation.z);

                    SpawnObstacle(newLocation, newRotation, obj);

                    z_offset++;
                    break;

                case 3:
                    // wallet
                    newLocation = obj.position + new Vector3(0, 0.5f, -z_offset);
                    newRotation = Quaternion.Euler(obj.rotation.x, obj.rotation.y, obj.rotation.z);

                    SpawnWallet(newLocation, newRotation, obj);

                    z_offset++;
                    break;
            }
        }
    }

    void SpawnObstacle(Vector3 n_position, Quaternion n_rotation, Transform parent)
    {
        if (obstacles.Length > 0)
        {
            GameObject m_obstacle = Instantiate(obstacles[Random.Range(0, obstacles.Length - 1)], n_position, n_rotation, parent);
        }
    }

    void SpawnCoin(Vector3 n_position, Quaternion n_rotation, Transform parent)
    {
        if (coin != null)
        {
            GameObject m_coin = Instantiate(coin, n_position, n_rotation, parent);
        }
    }

    void SpawnWallet(Vector3 n_position, Quaternion n_rotation, Transform parent)
    {
        if (wallet != null)
        {
            GameObject m_wallet = Instantiate(wallet, n_position, n_rotation, parent);
        }
    }
}
