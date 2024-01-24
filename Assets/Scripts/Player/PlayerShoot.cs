using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private float maxSpread, timeoutPeriod;

    private ObjectPool<GameObject> bulletPool;
    private float timer = 0f, spread = 0f;

    void Start()
    {
        bulletPool = new ObjectPool<GameObject>(() => Instantiate(bulletPrefab));
    }

    void Update()
    {
        timer = Mathf.Clamp(timer - Time.deltaTime, 0f, timeoutPeriod);
        spread = maxSpread * (timer / timeoutPeriod);
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        timer = Mathf.Clamp(timer + 0.2f, 0f, timeoutPeriod);
        GameObject bullet = bulletPool.Get();
        bullet.GetComponent<BulletBehaviour>().InitializeBullet(bulletSpawn, bulletPool, spread);
    }
}
