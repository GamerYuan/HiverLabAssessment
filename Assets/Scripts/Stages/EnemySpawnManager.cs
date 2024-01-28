using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager instance;

    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private float minSpawnInterval, maxSpawnInterval, spawnRadius, minPlayerDist, maxDifficulty;
    [SerializeField] private int maxEnemyCount;

    private ObjectPool<GameObject> enemyPool;
    private Transform playerPos;
    private float baseDifficulty = 1, difficulty = 1;
    
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        enemyPool = new ObjectPool<GameObject>(() => Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)],
            transform.position, Quaternion.identity), OnTakeFromPool, OnReleaseFromPool, null, true, 10, maxEnemyCount);
    }

    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    private void OnTakeFromPool(GameObject obj)
    {
        NavMeshHit spawnPoint;

        // Generate random point that is far enough from the centre
        while (true)
        {
            Vector2 randPoint = Random.insideUnitCircle;
            if (Vector2.Distance(randPoint, Vector2.zero) < minPlayerDist)
                continue;
            randPoint *= spawnRadius;

            // Find a point on the navmesh to spawn the enemy
            NavMesh.SamplePosition(new Vector3(randPoint.x + playerPos.position.x, playerPos.position.y, randPoint.y + playerPos.position.z), out spawnPoint, 100, NavMesh.AllAreas);
            if (spawnPoint.distance == Mathf.Infinity)
            {
                continue;
            }
            break;
        }

        
        obj.transform.position = spawnPoint.position;
        obj.GetComponent<ZombieBehaviour>().Init(difficulty, playerPos);

        Debug.Log($"Spawning enemy {obj.GetInstanceID()} at {spawnPoint.position}");
    }

    private void OnReleaseFromPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void ReleaseObject(GameObject obj)
    {
        enemyPool.Release(obj);
    }

    public void IncreaseDifficulty(float difficultyModifier)
    {
        difficulty = baseDifficulty + maxDifficulty * difficultyModifier;
    }

    private IEnumerator SpawnEnemy()
    {
        while (StageManager.instance.isStageActive)
        {
            if (playerPos == null) playerPos = GameObject.FindGameObjectWithTag("Player").transform;

            for (int i = 0; i < difficulty; i++)
            {
                if (enemyPool.CountActive >= maxEnemyCount)
                {
                    Debug.LogError("Max enemy count reached");
                    break;
                }
                enemyPool.Get();
            }

            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));

            // Reduce max spawn time of subsequent spawns
            maxSpawnInterval *= 0.98f;
        }
        yield return null;
    }
}
