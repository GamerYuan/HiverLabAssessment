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

    public void TakeDamage(float damage)
    {
        Health -= damage;
        Debug.Log($"{transform} took {damage} damage!");
    }

    public void ResetHealth(float diffMultiplier)
    {
        Health = initialHealth * diffMultiplier;
    }
}
