using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawnManager : SpawnManager
{
    public static CollectableSpawnManager instance;

    [SerializeField] private Terrain terrain;

    protected override void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        base.Awake();
    }

    void Start()
    {
        StartCoroutine(SpawnObj());
    }

    protected override void OnTakeFromPool(GameObject obj)
    {
        Vector3 spawnPoint = GenerateSpawnPoint();
        obj.transform.position = spawnPoint;
        obj.transform.Translate(0, -0.1f, 0);
        obj.transform.Rotate(Vector3.up, Random.Range(0, 360));
        obj.GetComponent<ICollectable>().Init(DifficultyManager.Difficulty);

        Debug.Log($"Spawning {obj.transform} {obj.GetInstanceID()} at {spawnPoint}");
    }

    protected override IEnumerator SpawnObj()
    {
        while (StageManager.instance.isStageActive)
        {
            if (playerPos == null)
            {
                Debug.LogError($"{instance.GetInstanceID()}: Player reference null, finding player instance");
                playerPos = GameObject.FindGameObjectWithTag("Player").transform;
            }

            for (int i = 0; i < 5; i++)
            {
                if (objPool.CountActive >= maxObjCount)
                {
                    Debug.Log("Max item count reached");
                    break;
                }
                objPool.Get();
            }

            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));
        }
        yield return null;
    }
}
