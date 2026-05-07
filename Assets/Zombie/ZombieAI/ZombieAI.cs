using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public NavMeshAgent navAgent;
    public CapsuleCollider capsuleCollider;
    public Animator animator;
    public enum ZombieState { Idle, Roam, Chase, Attack, Dead }
    public ZombieState currentState = ZombieState.Idle;
    public Transform player;
    public float chaseDistance = 20f; // increased for testing
    public float attackDistance = 2f;
    public float attackCooldown = 2f;
    public float attackDelay = 1.5f;
    public int damage = 10;
    public int health = 100;
    public float roamRadius = 10f;
    public float roamDelay = 3f;
    private float roamTimer;
    private float lastAttackTime;
    private bool isAttacking;
    public float idleWaitTime = 2f;
    private float idleTimer = 0f;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
            {
                player = p.transform;
                Debug.Log("Player found!");
            }
            else
            {
                Debug.LogError("PLAYER NOT FOUND - check tag!");
            }
        }

        if (!navAgent.isOnNavMesh)
        {
            Debug.LogError("NavAgent NOT on NavMesh!");
        }
    }

void Update()
{
    if (player == null) return;

    float distance = Vector3.Distance(transform.position, player.position);

    Debug.Log("State: " + currentState + " | Distance: " + distance);

    switch (currentState)
    {
        case ZombieState.Idle:
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsAttacking", false);

            idleTimer += Time.deltaTime;

            if (distance <= chaseDistance)
            {
                idleTimer = 0;
                currentState = ZombieState.Chase;
                Debug.Log("Switch to CHASE");
                break;
            }

            if (idleTimer >= idleWaitTime)
            {
                Vector3 newPos = RandomNavSphere(transform.position, roamRadius, -1);
                navAgent.isStopped = false;
                navAgent.SetDestination(newPos);

                animator.SetBool("IsWalking", true);

                idleTimer = 0;
                currentState = ZombieState.Roam;
            }

            break;

        case ZombieState.Roam:
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsAttacking", false);

            navAgent.isStopped = false;

            if (distance <= chaseDistance)
            {
                currentState = ZombieState.Chase;
                break;
            }

            if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
            {
                animator.SetBool("IsWalking", false);

                idleWaitTime = Random.Range(2f, 5f);

                currentState = ZombieState.Idle;
            }

            break;

        case ZombieState.Chase:
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsAttacking", false);

            navAgent.isStopped = false;

            if (distance <= chaseDistance)
            {
                navAgent.SetDestination(player.position);
            }

            if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance && distance > chaseDistance)
            {
                animator.SetBool("IsWalking", false);
                currentState = ZombieState.Idle;
            }

            if (distance <= attackDistance)
            {
                currentState = ZombieState.Attack;
                Debug.Log("Switch to ATTACK");
            }

            break;

        case ZombieState.Attack:
            animator.SetBool("IsAttacking", true);

            navAgent.isStopped = true;

            if (!isAttacking && Time.time - lastAttackTime >= attackCooldown)
            {
                StartCoroutine(AttackWithDelay());
            }

            if (distance > attackDistance)
            {
                animator.SetBool("IsAttacking", false);
                currentState = ZombieState.Chase;
            }

            break;

        case ZombieState.Dead:
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsAttacking", false);
            animator.SetBool("IsDead", true);

            navAgent.enabled = false;
            capsuleCollider.enabled = false;
            enabled = false;

            Debug.Log("Dead");
            break;
    }
}

    private IEnumerator AttackWithDelay()
    {
        isAttacking = true;

        yield return new WaitForSeconds(attackDelay);

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }

        lastAttackTime = Time.time;
        isAttacking = false;
    }

    public void TakeDamage(int damageAmount)
    {
        if (currentState == ZombieState.Dead) return;

        health -= damageAmount;

        if (health <= 0)
        {
            health = 0;
            currentState = ZombieState.Dead;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }
}