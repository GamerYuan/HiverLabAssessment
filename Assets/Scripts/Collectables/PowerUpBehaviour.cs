using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBehaviour : MonoBehaviour, ICollectable, ISpawnable, IDifficulty
{
    [SerializeField] private float powerUpAmount;

    public void SetDiff(float difficultyModifier)
    {
        // Do nothing
    }

    public void Init(Vector3 spawnPosition)
    {
        gameObject.SetActive(true);
        transform.position = spawnPosition;
    }

    public void DoAction(Component sender)
    {
        if (sender is PlayerInteraction)
        {
            PlayerInteraction player = (PlayerInteraction)sender;
            player.OnPowerUpPickup(powerUpAmount);
        }
        CollectableSpawnManager.instance.ReleaseObject(gameObject);
    }
}
