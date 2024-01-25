using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBehaviour : MonoBehaviour
{
    [SerializeField] private float speed, attackRange, attackDamage, attackInterval;
    
    private Rigidbody rb;
    private Animator anim;
    private ZombieState state = ZombieState.Idle;

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
}
enum ZombieState
{
    Idle,
    Walk,
    Attack,
    Dead
}
