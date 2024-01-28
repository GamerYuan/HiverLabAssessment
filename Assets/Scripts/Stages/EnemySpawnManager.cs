using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : SpawnManager
{
    public static EnemySpawnManager instance;
    
    // Start is called before the first frame update
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

    protected override void OnTakeFromPool(GameObject obj)
    {
        base.OnTakeFromPool(obj);
        obj.GetComponent<ZombieBehaviour>().Init(DifficultyManager.Difficulty, playerPos);
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

            for (int i = 0; i < DifficultyManager.Difficulty; i++)
            {
                if (objPool.CountActive >= maxObjCount)
                {
                    Debug.Log("Max enemy count reached");
                    break;
                }
                objPool.Get();
            }

            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));

            // Reduce max spawn time of subsequent spawns
            maxSpawnInterval *= 0.98f;
        }
        yield return null;
    }
}
