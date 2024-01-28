using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPackBehaviour : MonoBehaviour, ICollectable
{
    [SerializeField] private float initialHealthAmount;

    private float healthAmount;

    public void Init(float difficultyModifier)
    {
        healthAmount = initialHealthAmount * difficultyModifier;
        gameObject.SetActive(true);
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
