using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private float lifeTime, damage, bulletSpeed;

    private Rigidbody rb;
    private ObjectPool<GameObject> pool;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        Invoke("DisableBullet", lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        DisableBullet();
    }

    public void InitializeBullet(Transform bulletSpawn, ObjectPool<GameObject> pool, float spread)
    {
        gameObject.SetActive(true);
        this.pool = pool;
        
        transform.position = bulletSpawn.position;

        float randX = Random.Range(-spread, spread);
        float randY = Random.Range(-spread, spread);

        transform.rotation = bulletSpawn.rotation;

        transform.Rotate(randX, randY, 0);

        rb.velocity = transform.forward * bulletSpeed;
    }

    void DisableBullet()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            pool.Release(gameObject);
        }
    }
}
