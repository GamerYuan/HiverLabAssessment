using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private float maxSpread, timeoutPeriod, shootInterval;

    private ObjectPool<GameObject> bulletPool;
    private float spreadTimer = 0f, spread = 0f, shootTimer;
    private bool isShooting = false;
    private bool canShoot = true;

    void Start()
    {
        bulletPool = new ObjectPool<GameObject>(() => Instantiate(bulletPrefab));
    }

    void Update()
    {
        spreadTimer = Mathf.Clamp(spreadTimer - Time.deltaTime, 0f, timeoutPeriod);
        spread = maxSpread * (spreadTimer / timeoutPeriod);

        if (canShoot && isShooting)
        {
            TriggerBullet();
            canShoot = false;
            shootTimer = shootInterval;
        }

        if (canShoot == false)
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
        GameObject bullet = bulletPool.Get();
        bullet.GetComponent<BulletBehaviour>().InitializeBullet(bulletSpawn, bulletPool, spread);
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        isShooting = context.action.IsPressed();
    }
}
