using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth
{
    [SerializeField] private float initialHealth;
    [SerializeField] private GameEvent onHealthChanged;

    private float _health;
    public float Health
    {
        get => _health;
        private set => _health = value;
    }
    
    void Awake()
    {
        Health = initialHealth;
        onHealthChanged.Raise(this, Health);
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        Debug.Log("Player took " + damage + " damage!");
        onHealthChanged.Raise(this, Health);
    }

    public void Heal(float healAmount)
    {
        Health = Mathf.Min(Health + healAmount, initialHealth);
        Debug.Log($"Player healed {healAmount} health!");
        onHealthChanged.Raise(this, Health);
    }
}
