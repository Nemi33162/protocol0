using UnityEngine;

public class SimpleGun : MonoBehaviour
{
    public Camera playerCamera;
    public float damage = 25f;
    public float range = 100f;
    public KeyCode shootKey = KeyCode.Mouse0;
    public AudioSource audioSource;
    public AudioClip shootSound;

    void Update()
    {
        if (Input.GetKeyDown(shootKey))
        {
            Shoot();
        }
    }

void Shoot()
{
    Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
    RaycastHit hit;

	audioSource.PlayOneShot(shootSound);


    if (Physics.Raycast(ray, out hit, range))
    {
        Debug.Log("Hit: " + hit.collider.name);

        ZombieAI zombie = hit.collider.GetComponentInParent<ZombieAI>();

        if (zombie != null)
        {
            int finalDamage = (int)damage;

            if (hit.collider.CompareTag("Head"))
            {
                finalDamage *= 2;
                Debug.Log("HEADSHOT");
            }

            zombie.TakeDamage(finalDamage);
        }
    }
}
}