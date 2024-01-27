using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth
{
    [SerializeField] private float _health;

    public float Health
    {
        get => _health;
        private set => _health = value;
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        Debug.Log("Player took " + damage + " damage!");
    }
}
