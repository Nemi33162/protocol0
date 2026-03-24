using UnityEngine;
public class ZombieFollow : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public float rotationSpeed = 5f; 
    public float damageDistance = 2f;
    public float damageRate = 1f;
    private float damageTimer = 0f; 

    void Update()
    {
        if (player != null)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0f;
            direction.Normalize();

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

            float distance = Vector3.Distance(transform.position, player.position);

            if (distance > 1.2f)
            {
                transform.position += transform.forward * speed * Time.deltaTime;
            }


                if (distance < damageDistance)
                {
                    damageTimer += Time.deltaTime;
                    if (damageTimer >= damageRate)
                    {

                    PlayerHealth health = player.GetComponent<PlayerHealth>();
                    if (health != null)
                    {
                        health.TakeDamage(10);
                    }
                    damageTimer = 0f;
                }
            }
            else
            {
                damageTimer = 0f; 
            }
        }
    }
}