using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100f;

    private Animator animator;
    private NavMeshAgent agent;
    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        animator.SetTrigger("Dead");

        if (agent != null)
            agent.enabled = false;

        GetComponent<Collider>().enabled = false;

        Destroy(gameObject, 3f); 
    }
}