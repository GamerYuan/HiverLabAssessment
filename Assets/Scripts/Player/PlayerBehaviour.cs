using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public static PlayerBehaviour instance;

    [SerializeField] private GameEvent onPlayerDie;
    
    private PlayerHealth playerHealth;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } 
        else
        {
            Destroy(this);
        }
        playerHealth = GetComponent<PlayerHealth>();
    }

    public void TakeDamage(float damage)
    {
        playerHealth.Damage(damage);
        if (playerHealth.Health <= 0)
        {
            onPlayerDie.Raise(this, "Player Dead");
        }
    }
}
