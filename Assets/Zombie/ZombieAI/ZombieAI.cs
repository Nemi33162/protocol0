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

    public float chaseDistance = 20f;
    public float attackDistance = 2f;
    public float attackCooldown = 2f;
    public float attackDelay = 1.5f;

    public int damage = 10;
    public int health = 100;

	
	public int hitPoints = 10;
	public int killPoints = 50;


    public float roamRadius = 10f;
    public float idleWaitTime = 2f;

    private float idleTimer = 0f;
    private float lastAttackTime;
    private bool isAttacking;
    private bool isDead = false;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogError("PLAYER NOT FOUND - check tag!");
        }

        if (!navAgent.isOnNavMesh)
        {
            Debug.LogError("NavAgent NOT on NavMesh!");
        }
    }

    void Update()
    {
        if (isDead) return;
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

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
                    break;
                }

                if (idleTimer >= idleWaitTime)
                {
                    Vector3 newPos = RandomNavSphere(transform.position, roamRadius, -1);
                    navAgent.SetDestination(newPos);
                    animator.SetBool("IsWalking", true);

                    idleTimer = 0;
                    currentState = ZombieState.Roam;
                }
                break;

            case ZombieState.Roam:
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsAttacking", false);

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

                navAgent.SetDestination(player.position);

                if (distance <= attackDistance)
                {
                    currentState = ZombieState.Attack;
                }

                if (distance > chaseDistance)
                {
                    currentState = ZombieState.Idle;
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
                    navAgent.isStopped = false;
                    currentState = ZombieState.Chase;
                }
                break;

            case ZombieState.Dead:
                // do nothing (handled in Die)
                break;
        }
    }

    private IEnumerator AttackWithDelay()
    {
        isAttacking = true;

        yield return new WaitForSeconds(attackDelay);

        if (isDead) yield break;

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
    if (isDead) return;

    // find player points system
    PlayerPoints playerPoints = FindObjectOfType<PlayerPoints>();

    // +10 per hit
    if (playerPoints != null)
    {
        playerPoints.AddPoints(hitPoints);
    }

    health -= damageAmount;

    Debug.Log("Damage taken: " + damageAmount);

    if (health <= 0)
    {
        health = 0;

        // +50 on kill
        if (playerPoints != null)
        {
            playerPoints.AddPoints(killPoints);
        }

        Die();
    }
}

void Die()
{
    if (isDead) return;
    isDead = true;

    // stop movement
    if (navAgent != null)
    {
        navAgent.isStopped = true;
        navAgent.enabled = false;
    }

    if (animator != null)
    {
        animator.SetTrigger("Die");
    }

    Destroy(gameObject, 3f); // adjust based on animation length
}

    IEnumerator DisableAgent()
    {
        yield return new WaitForSeconds(0.2f);
        navAgent.enabled = false;
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