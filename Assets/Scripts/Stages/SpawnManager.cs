using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> objList;
    [SerializeField] protected float minSpawnInterval, maxSpawnInterval, spawnRadius, minPlayerDist;
    [SerializeField] protected int maxObjCount;

    protected ObjectPool<GameObject> objPool;
    protected Transform playerPos;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        objPool = new ObjectPool<GameObject>(() => Instantiate(objList[Random.Range(0, objList.Count)],
            transform.position, Quaternion.identity), OnTakeFromPool, OnReleaseFromPool, null, true, 10, maxObjCount);
    }

    protected virtual void OnTakeFromPool(GameObject obj)
    {
        Vector3 spawnPoint = GenerateSpawnPoint();
        obj.transform.position = spawnPoint;
        Debug.Log($"Spawning {obj.transform} {obj.GetInstanceID()} at {spawnPoint}");
    }

    protected virtual void OnReleaseFromPool(GameObject obj)
    {
        Debug.Log($"{obj.transform} {obj.GetInstanceID()}: Object released");
        obj.SetActive(false);
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
}
