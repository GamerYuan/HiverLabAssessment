using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ZombieBehaviour : MonoBehaviour, IScorable
{
    [SerializeField] private float walkSpeed, runSpeed, aggroRange, attackRange, hitboxRange,
        initialAttackDamage, attackInterval, minWalkInterval, maxWalkInterval, walkRange;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private int initialScore;
    [SerializeField] private GameEvent OnScoreChange;

    private Animator anim;
    private ZombieState state = ZombieState.Idle;
    private NavMeshAgent agent;
    private Transform player;
    private ZombieHealth zombieHealth;
    private bool isWalking = false, isInSuspiciousRange = false;
    private float attackDamage;
    private int score;

    void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        zombieHealth = GetComponent<ZombieHealth>();
        agent.speed = walkSpeed;
    }

    void OnDisable()
    {
        agent.enabled = false;
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
            Debug.Log($"Zombie ({gameObject.GetInstanceID()}): Player in sus range");
            state = ZombieState.Suspicious;
            agent.speed = walkSpeed;
            isInSuspiciousRange = true;
            StopAllCoroutines();
            isWalking = false;
            agent.SetDestination(player.position);
            StartCoroutine(CheckAggro());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isInSuspiciousRange = false;
    }

    public void Init(float diffMultiplier, Transform player)
    {
        this.player = player;
        state = ZombieState.Idle;
        
        agent.enabled = true;
        zombieHealth.ResetHealth(diffMultiplier);
        attackDamage = initialAttackDamage * diffMultiplier;
        score = Mathf.CeilToInt(initialScore * diffMultiplier);
        gameObject.SetActive(true);
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
        if (agent.remainingDistance < 1)
        {
            state = ZombieState.Idle;
        }
    }

    private void Suspicious()
    {
        anim.SetBool("isWalking", true);
        anim.SetBool("isRunning", false);
        anim.SetBool("isIdle", false);
        if (Vector3.Distance(transform.position, player.position) <= aggroRange)
        {
            Debug.Log($"Zombie ({gameObject.GetInstanceID()}): Player in aggro range");
            state = ZombieState.Aggro;
            agent.speed = runSpeed;
            agent.SetDestination(player.position);
        }

        if (agent.remainingDistance < 1)
        {
            if (isInSuspiciousRange)
            {
                agent.SetDestination(player.position);
            } 
            else
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
        agent.SetDestination(player.position);
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            state = ZombieState.Attack;
        }
        if (Vector3.Distance(transform.position, player.position) > aggroRange)
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
        agent.ResetPath();
        StartCoroutine(AttackCooldown());
    }

    public void AttackHitbox()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, hitboxRange, transform.forward, out hit, 1, 
            playerLayer, QueryTriggerInteraction.Ignore))
        {
            Debug.Log($"Zombie ({gameObject.GetInstanceID()}): {transform} hit {hit.transform}");
            if (hit.transform.root.CompareTag("Player"))
            {
                PlayerBehaviour.instance.TakeDamage(attackDamage);
            }
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
            agent.SetDestination(player.position);
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
        Debug.Log($"Zombie ({gameObject.GetInstanceID()}): Dead");
        anim.SetFloat("Random", Random.Range(0f, 1f));
        anim.SetTrigger("Die");
        AddScore();
        StartCoroutine(DeadTimer());
    }

    private IEnumerator WalkTimer()
    {
        Coroutine coroutine = StartCoroutine(RandomWalk());

        // Starts new walk coroutine after random interval
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWalkInterval, maxWalkInterval));
            if (state == ZombieState.Idle)
            {
                Debug.Log($"Zombie ({gameObject.GetInstanceID()}): Starts new walk coroutine");
                StopCoroutine(coroutine);
                coroutine = StartCoroutine(RandomWalk());
            }
        }
    }

    private IEnumerator RandomWalk()
    {
        state = ZombieState.Walk;
        anim.SetFloat("Random", Random.Range(0f, 1f));
        
        // Generate random point within walk range and set destination
        Vector2 randPoint = Random.insideUnitCircle * walkRange;
        Vector3 randVec = new Vector3(transform.position.x + randPoint.x, 0, transform.position.z + randPoint.y);
        agent.SetDestination(randVec);

        // Resets walk state after random interval
        yield return new WaitForSeconds(Random.Range(minWalkInterval, maxWalkInterval));
        Debug.Log($"Zombie ({gameObject.GetInstanceID()}): Reset Path");
        agent.ResetPath();
        state = ZombieState.Idle;
        yield return null;
    }

    private IEnumerator DeadTimer()
    {
        yield return new WaitForSeconds(3);
        EnemySpawnManager.instance.ReleaseObject(gameObject);
        yield return null;
    }

    public void AddScore()
    {
        OnScoreChange.Raise(this, score);
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
