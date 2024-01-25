using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ZombieBehaviour : MonoBehaviour
{
    [SerializeField] private float walkSpeed, runSpeed, aggroRange, attackRange, 
        attackDamage, attackInterval, minWalkInterval, maxWalkInterval, walkRange;
    
    private Animator anim;
    private ZombieState state = ZombieState.Idle;
    private NavMeshAgent agent;
    private Coroutine walkCoroutine;

    private GameObject player;

    void Awake()
    {
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
        switch (state)
        {
            case ZombieState.Idle:
                Idle();
                break;
            case ZombieState.Walk:
                Walk();
                break;
            case ZombieState.Aggro:
                Aggro();
                break;
            case ZombieState.Attack:
                Attack();
                break;
            case ZombieState.Dead:
                Dead();
                break;
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

    private void Idle()
    {
        StopAllCoroutines();
        anim.SetBool("isWalking", false);
    }

    private void Walk()
    {
        anim.SetBool("isWalking", true);
        if (walkCoroutine == null)
        {
            walkCoroutine = StartCoroutine(WalkTimer());
        }
    }

    private void Aggro()
    {
        StopAllCoroutines();
        anim.SetBool("isWalking", false);
        anim.SetBool("isRunning", true);
        agent.SetDestination(player.transform.position);
        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
        {
            state = ZombieState.Attack;
        }
    }

    private void Attack()
    {
        StopAllCoroutines();
        anim.SetBool("isWalking", false);
        anim.SetBool("isRunning", false);
        anim.SetTrigger("Attack");
        //player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackInterval);
        state = ZombieState.Aggro;
    }

    private void Dead()
    {
        anim.SetBool("isWalking", false);
        anim.SetBool("isRunning", false);
        anim.SetTrigger("Dead");
        agent.enabled = false;
        Destroy(gameObject, 5);
    }

    public void TakeDamage(float damage)
    {
        if (state == ZombieState.Dead) return;
        state = ZombieState.Aggro;
        agent.speed = runSpeed;
        agent.SetDestination(player.transform.position);
        //if (GetComponent<ZombieHealth>().health <= 0)
        //{
        //    Die();
        //}
        //GetComponent<ZombieHealth>().TakeDamage(damage);
        //if (GetComponent<ZombieHealth>().health <= 0)
        //{
        //    state = ZombieState.Dead;
        //}
    }

    private void Die()
    {
        state = ZombieState.Dead;
    }

    private IEnumerator WalkTimer()
    {
        Coroutine coroutine = StartCoroutine(RandomWalk());
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWalkInterval, maxWalkInterval));
            StopCoroutine(coroutine);
            coroutine = StartCoroutine(RandomWalk());
        }
    }

    private IEnumerator RandomWalk()
    {
        state = ZombieState.Walk;
        Vector3 randVec = new Vector3(transform.position.x + Random.Range(-10, 10), 0, transform.position.z + Random.Range(-10, 10));
        agent.SetDestination(randVec);
        yield return new WaitForSeconds(Random.Range(minWalkInterval, maxWalkInterval));
        agent.ResetPath();
        state = ZombieState.Idle;
        yield return null;
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
