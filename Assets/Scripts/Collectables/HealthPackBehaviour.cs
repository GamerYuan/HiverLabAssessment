using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPackBehaviour : MonoBehaviour, ICollectable, IDifficulty, ISpawnable
{
    [SerializeField] private float initialHealthAmount;

    private float healthAmount;

    public void Init(Vector3 spawnPosition)
    {
        gameObject.SetActive(true);
        transform.position = spawnPosition;
    }

    public void SetDiff(float difficultyModifier)
    {
        healthAmount = initialHealthAmount * difficultyModifier;
    }

    public void DoAction(Component sender)
    {
        if (sender is PlayerInteraction)
        {
            PlayerInteraction player = (PlayerInteraction)sender;
            player.OnHealthPickup(healthAmount);
        }
        CollectableSpawnManager.instance.ReleaseObject(gameObject);
    }
}
