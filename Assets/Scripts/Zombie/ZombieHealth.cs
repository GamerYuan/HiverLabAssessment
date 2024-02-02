using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHealth : MonoBehaviour, IHealth
{
    [SerializeField] private float initialHealth;

    private float _health;

    public float Health
    {
        get => _health;
        private set => _health = value;
    }

    public void Damage(float damage)
    {
        Health -= damage;
        Debug.Log($"{transform.GetInstanceID()}: Took {damage} damage!");
    }

    public void ResetHealth(float diffMultiplier)
    {
        Health = initialHealth * diffMultiplier;
    }

    public void Heal(float healAmount)
    {
        Debug.LogError("Zombies can't heal!");
    }   
}
