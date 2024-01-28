using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> objList;
    [SerializeField] protected float minSpawnInterval, maxSpawnInterval, spawnRadius, minPlayerDist, despawnInterval, despawnDistance;
    [SerializeField] protected int maxObjCount;

    protected ObjectPool<GameObject> objPool;
    protected Transform playerPos;
    protected List<GameObject> activeObjList = new List<GameObject>();

    WaitForSeconds despawnTimer;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        objPool = new ObjectPool<GameObject>(() => Instantiate(objList[Random.Range(0, objList.Count)],
            transform.position, Quaternion.identity), OnTakeFromPool, OnReleaseFromPool, null, true, 10, maxObjCount);
        despawnTimer = new WaitForSeconds(despawnInterval);
    }

    protected virtual void Start()
    {
        StartCoroutine(SpawnObj());
        StartCoroutine(DespawnObj());
    }

    protected virtual void OnTakeFromPool(GameObject obj)
    {
        Vector3 spawnPoint = GenerateSpawnPoint();
        obj.transform.position = spawnPoint;
        Debug.Log($"Spawning {obj.transform} {obj.GetInstanceID()} at {spawnPoint}");
        activeObjList.Add(obj);
    }

    protected virtual void OnReleaseFromPool(GameObject obj)
    {
        Debug.Log($"{obj.transform} {obj.GetInstanceID()}: Object released");
        obj.SetActive(false);
        activeObjList.Remove(obj);
    }

    public void ReleaseObject(GameObject obj)
    {
        objPool.Release(obj);
    }

    protected Vector3 GenerateSpawnPoint()
    {
        NavMeshHit hitPoint;
        Vector3 point = Vector3.zero;
        // Generate random point that is far enough from the centre
        while (true)
        {
            Vector2 randPoint = Random.insideUnitCircle;
            if (Vector2.Distance(randPoint, Vector2.zero) < minPlayerDist)
                continue;
            randPoint *= spawnRadius;

            // Find a point on the navmesh to spawn the enemy
            point.Set(randPoint.x + playerPos.position.x,
                playerPos.position.y, randPoint.y + playerPos.position.z);
            NavMesh.SamplePosition(point, out hitPoint, 100, NavMesh.AllAreas);
            if (hitPoint.distance == Mathf.Infinity)
            {
                continue;
            }
            return hitPoint.position;
        }
    }

    protected virtual IEnumerator SpawnObj()
    {
        yield return null;
    }

    protected IEnumerator DespawnObj()
    {
        while (StageManager.instance.isStageActive)
        {
            yield return despawnTimer;
            Vector3 playerVec = playerPos.position;
            for (int i = 0; i < activeObjList.Count; i++)
            {
                if (Vector3.Distance(activeObjList[i].transform.position, playerVec) > despawnDistance)
                {
                    Debug.Log($"Despawning {activeObjList[i].transform} {activeObjList[i].GetInstanceID()}");
                    ReleaseObject(activeObjList[i]);
                }
            }
        }
        yield return null;
    }
}
