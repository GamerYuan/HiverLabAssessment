using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ZombieBehaviour : MonoBehaviour
{
    [SerializeField] private float walkSpeed, runSpeed, aggroRange, attackRange, attackDamage, attackInterval;
    
    private Rigidbody rb;
    private Animator anim;
    private ZombieState state = ZombieState.Idle;
    private NavMeshAgent agent;

    private GameObject player;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (state == ZombieState.Aggro)
        {
            agent.SetDestination(player.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player in Range");
            state = ZombieState.Suspicious;
            agent.speed = walkSpeed;
            agent.SetDestination(player.transform.position);
            StartCoroutine(CheckAggro());
        }
    }

    private IEnumerator CheckAggro()
    {
        if (state == ZombieState.Suspicious) yield return null;
        
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (Vector3.Distance(transform.position, player.transform.position) <= aggroRange)
            {
                Debug.Log("Player Aggro'd");
                state = ZombieState.Aggro;
                agent.speed = runSpeed;
                agent.SetDestination(player.transform.position);
                yield return null;
            }
        }
    }
}
enum ZombieState
{
    Idle,
    Walk,
    Suspicious,
    Aggro,
    Attack,
    Dead
}
