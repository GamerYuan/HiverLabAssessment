using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerHealth playerHealth;
    private PlayerShoot playerShoot;
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerShoot = GetComponent<PlayerShoot>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            other.GetComponent<ICollectable>().DoAction(this);
        }
    }

    public void OnHealthPickup(float val)
    {
        Debug.Log("Player picked up Health Pack");
        playerHealth.Heal(val);
    }

    public void OnPowerUpPickup(float val)
    {
        Debug.Log("Player picked up Power Up");
        playerShoot.PowerUp(val);
    }
}
