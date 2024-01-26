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
    [SerializeField] private LayerMask zombieLayer;

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

        spread = maxSpread * (spreadTimer / timeoutPeriod);

        Vector2 randPoint = Random.insideUnitCircle * spread;

        // Clamp bullet spread to top half of the circle
        Vector3 bulletPoint = new Vector3(bulletSpawn.position.x + randPoint.x,
            bulletSpawn.position.y + Mathf.Clamp(randPoint.y, 0, 1), bulletSpawn.position.z);

        RaycastHit hit;
        Debug.DrawRay(bulletPoint, bulletSpawn.transform.forward * 30, Color.black, 5);
        // If raycast hits something
        if (Physics.Raycast(bulletPoint, bulletSpawn.transform.forward, out hit, 30, Physics.DefaultRaycastLayers, 
            QueryTriggerInteraction.Ignore))
        {
            Debug.Log(hit.point);
            Debug.Log("Shoot");
            
            // Checks for zombie
            if (hit.transform.root.CompareTag("Zombie"))
            {
                Debug.Log("Zombie hit");                
                //hit.transform.root.GetComponent<ZombieBehaviour>().TakeDamage(1);
            }
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        Debug.Log("Test");
        isShooting = context.action.IsPressed();
    }
}
