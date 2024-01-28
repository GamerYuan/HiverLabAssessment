using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBehaviour : MonoBehaviour, ICollectable
{
    [SerializeField] private float powerUpAmount;

    public void Init(float difficultyModifier)
    {
        gameObject.SetActive(true);
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
