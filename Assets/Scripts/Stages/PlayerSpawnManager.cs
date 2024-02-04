using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private float spawnRange;

    void Awake()
    {
        Instantiate(playerPrefab, GenerateSpawnPoint(), Quaternion.identity);
    }

    private Vector3 GenerateSpawnPoint()
    {
        NavMeshHit hitPoint;

        float randX = Random.Range(-spawnRange, spawnRange);
        float randZ = Random.Range(-spawnRange, spawnRange);

        Vector3 point = new Vector3(randX, 0, randZ);

        NavMesh.SamplePosition(point, out hitPoint, Mathf.Infinity, NavMesh.AllAreas);

        return hitPoint.position;
    }
}
