using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ZombieBehaviour : MonoBehaviour
{
    [SerializeField] private float walkSpeed, runSpeed, aggroRange, attackRange, hitboxRange,
        attackDamage, attackInterval, minWalkInterval, maxWalkInterval, walkRange;
    
    private Animator anim;
    private ZombieState state = ZombieState.Idle;
    private NavMeshAgent agent;
    private GameObject player;
    private ZombieHealth zombieHealth;
    private bool isWalking = false, isInSuspiciousRange = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        zombieHealth = GetComponent<ZombieHealth>();
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
            case ZombieState.Suspicious:
                Suspicious();
                break;
            case ZombieState.Aggro:
                Aggro();
                break;
            case ZombieState.Attack:
                Attack();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player in Range");
            state = ZombieState.Suspicious;
            agent.speed = walkSpeed;
            isInSuspiciousRange = true;
            StopAllCoroutines();
            isWalking = false;
            agent.SetDestination(player.transform.position);
            StartCoroutine(CheckAggro());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isInSuspiciousRange = false;
    }

    private IEnumerator CheckAggro()
    {
        if (state == ZombieState.Suspicious) yield return null;
        
        while (true)
        {
            yield return new WaitForSeconds(1);
            
        }
    }

    private void Idle()
    {
        //Debug.Log("Idle");
        anim.SetBool("isWalking", false);
        anim.SetBool("isRunning", false);
        anim.SetBool("isIdle", true);
        if (!isWalking)
        {
            StartCoroutine(WalkTimer());
            isWalking = true;
        }
    }

    private void Walk()
    {
        anim.SetBool("isWalking", true);
        anim.SetBool("isRunning", false);
        anim.SetBool("isIdle", false);
        //Debug.Log("Walk");
    }

    private void Suspicious()
    {
        anim.SetBool("isWalking", true);
        anim.SetBool("isRunning", false);
        anim.SetBool("isIdle", false);
        //Debug.Log("Suspicious");
        //agent.SetDestination(player.transform.position);
        if (Vector3.Distance(transform.position, player.transform.position) <= aggroRange)
        {
            //Debug.Log("Player Aggro'd");
            state = ZombieState.Aggro;
            agent.speed = runSpeed;
            agent.SetDestination(player.transform.position);
        }

        if (agent.remainingDistance < 1)
        {
            if (isInSuspiciousRange)
            {
                agent.SetDestination(player.transform.position);
            } else
            {
                state = ZombieState.Idle;
            }
        }
    }   

    private void Aggro()
    {
        StopAllCoroutines();
        anim.SetBool("isWalking", false);
        anim.SetBool("isRunning", true);
        anim.SetBool("isIdle", false);
        //Debug.Log("Aggro");
        agent.SetDestination(player.transform.position);
        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
        {
            state = ZombieState.Attack;
        }
        if (Vector3.Distance(transform.position, player.transform.position) > aggroRange)
        {
            state = ZombieState.Suspicious;
        }
    }

    private void Attack()
    {
        anim.SetBool("isWalking", false);
        anim.SetBool("isRunning", false);
        anim.SetBool("isIdle", false);
        anim.SetTrigger("Attack");
        StartCoroutine(AttackCooldown());
    }

    public void AttackHitbox()
    {
        Physics.SphereCast(transform.position, hitboxRange, transform.forward, out RaycastHit hit);
        if (hit.transform.root.CompareTag("Player"))
        {
            PlayerBehaviour.instance.TakeDamage(attackDamage);
        }
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackInterval);
        anim.ResetTrigger("Attack");
        state = ZombieState.Aggro;
    }

    public void TakeDamage(float damage)
    {
        if (state == ZombieState.Dead) return;
        if (state != ZombieState.Aggro) 
        {
            state = ZombieState.Aggro;
            agent.speed = runSpeed;
            agent.SetDestination(player.transform.position);
        }

        zombieHealth.TakeDamage(damage);
        if (zombieHealth.Health <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        state = ZombieState.Dead;
        agent.enabled = false;
        anim.SetBool("isWalking", false);
        anim.SetBool("isRunning", false);
        anim.SetBool("isIdle", false);
        Debug.Log("Dead");
        anim.SetFloat("Random", Random.Range(0f, 1f));
        anim.SetTrigger("Die");
        Destroy(gameObject, 5);
    }

    private IEnumerator WalkTimer()
    {
        Coroutine coroutine = StartCoroutine(RandomWalk());
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWalkInterval, maxWalkInterval));
            if (state == ZombieState.Idle)
            {
                StopCoroutine(coroutine);
                coroutine = StartCoroutine(RandomWalk());
            }
        }
    }

    private IEnumerator RandomWalk()
    {
        state = ZombieState.Walk;
        anim.SetFloat("Random", Random.Range(0f, 1f));
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
