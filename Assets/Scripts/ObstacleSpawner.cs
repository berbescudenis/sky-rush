using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject staticObstaclePrefab;
    public GameObject movingObstaclePrefab;
    public GameObject gapPrefab;
    public GameObject rotatingBladePrefab;
    public GameObject fakePathPrefab;
    public GameObject hammerPrefab;
    public GameObject laserPrefab;
    public GameObject swapGatePrefab;
    public GameObject coinPrefab;
    public Transform ball;
    public float spawnDistance = 80f;
    public float spawnInterval = 20f;
    [HideInInspector] public float movingChance = 0f;
    [HideInInspector] public bool canSpawnGaps = false;
    [HideInInspector] public bool canSpawnBlades = false;
    [HideInInspector] public bool canSpawnFakePaths = false;
    [HideInInspector] public bool canSpawnHammers = false;
    [HideInInspector] public bool canSpawnLasers = false;
    [HideInInspector] public bool canSpawnPortals = false;

    private float nextSpawnZ = 50f;

    void Update()
    {
        if (ball.position.z + spawnDistance > nextSpawnZ)
        {
            SpawnObstacle();
        }
    }

    void SpawnObstacle()
    {
        int lane = Random.Range(0, 3);
        float xPos = (lane - 1) * 2f;
        float roll = Random.value;

        if (roll < 0.15f)
        {
            SpawnCoinRow(xPos);
        }
        else if (canSpawnPortals && roll < 0.25f)
        {
            Spawn(swapGatePrefab, new Vector3(0f, 0f, nextSpawnZ));
        }
        else if (canSpawnLasers && roll < 0.35f)
        {
            Spawn(laserPrefab, new Vector3(0f, 0f, nextSpawnZ));
        }
        else if (canSpawnHammers && roll < 0.45f)
        {
            int hammerLane = Random.Range(0, 3);
            float hammerX = (hammerLane - 1) * 2f;
            Spawn(hammerPrefab, new Vector3(hammerX, 2.5f, nextSpawnZ));
        }
        else if (canSpawnFakePaths && roll < 0.55f)
        {
            Spawn(fakePathPrefab, new Vector3(xPos, 0f, nextSpawnZ));
        }
        else if (canSpawnBlades && roll < 0.65f)
        {
            Spawn(rotatingBladePrefab, new Vector3(xPos, 0.6f, nextSpawnZ));
        }
        else if (canSpawnGaps && roll < 0.75f)
        {
            Spawn(gapPrefab, new Vector3(0, 0f, nextSpawnZ));
        }
        else if (roll < 0.75f + movingChance)
        {
            Spawn(movingObstaclePrefab, new Vector3(xPos, 1f, nextSpawnZ));
        }
        else
        {
            Spawn(staticObstaclePrefab, new Vector3(xPos, 1f, nextSpawnZ));
        }

        nextSpawnZ += spawnInterval;
    }

    GameObject Spawn(GameObject prefab, Vector3 position)
    {
        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        obj.AddComponent<AutoDestroy>();
        return obj;
    }

    void SpawnCoinRow(float xPos)
    {
        for (int i = 0; i < 5; i++)
        {
            Spawn(coinPrefab, new Vector3(xPos, 0.8f, nextSpawnZ + i * 2f));
        }
    }
}