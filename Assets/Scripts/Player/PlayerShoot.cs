using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private float maxSpread, timeoutPeriod, shootInterval, damage, range;
    [SerializeField] private LayerMask zombieLayer;
    [SerializeField] private GameEvent onBulletSpread;

    private float spreadTimer = 0f, spread = 0f, shootTimer;
    private bool isShooting = false;
    private bool canShoot = true;

    void Update()
    {
        if (spreadTimer > 0)
        {
            spreadTimer = Mathf.Clamp(spreadTimer - Time.deltaTime, 0f, timeoutPeriod);
            spread = maxSpread * (spreadTimer / timeoutPeriod);
            onBulletSpread.Raise(this, spread);
        }

        if (canShoot && isShooting)
        {
            TriggerBullet();
            canShoot = false;
            shootTimer = shootInterval;
        }

        if (!canShoot)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                canShoot = true;
            }
        }
    }

    void TriggerBullet()
    {
        spreadTimer = Mathf.Clamp(spreadTimer + 0.2f, 0f, timeoutPeriod);
        
        Vector2 randPoint = Random.insideUnitCircle * spread;

        // Clamp bullet spread to top half of the circle
        Vector3 bulletPoint = new Vector3(bulletSpawn.position.x + randPoint.x,
            bulletSpawn.position.y + Mathf.Clamp(randPoint.y, 0, 1), bulletSpawn.position.z);

        RaycastHit hit;
        Debug.DrawRay(bulletPoint, bulletSpawn.transform.forward * range, Color.black, 5);
        // If raycast hits something
        if (Physics.Raycast(bulletPoint, bulletSpawn.transform.forward, out hit, range, zombieLayer, 
            QueryTriggerInteraction.Ignore))
        {
            Debug.Log($"Zombie {hit.transform.GetInstanceID()}: Got hit");
            hit.transform.root.GetComponent<ZombieBehaviour>().TakeDamage(damage);
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        isShooting = context.action.IsPressed();
    }

    public void PowerUp(float val)
    {
        damage *= val;
        Debug.Log($"Player damage increased! Current damage: {damage}");
    }
}
