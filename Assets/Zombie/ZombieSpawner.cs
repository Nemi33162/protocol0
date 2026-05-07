using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public Transform player;
    public float spawnRate = 3f;
    public float spawnDistance = 20f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            SpawnZombie();
            timer = 0f;
        }
    }

    void SpawnZombie()
    {
        Vector3 randomDirection = Random.insideUnitSphere * spawnDistance;
        randomDirection.y = 0;

        Vector3 spawnPosition = player.position + randomDirection;

        GameObject zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);

        ZombieAI zf = zombie.GetComponent<ZombieAI>();
        if (zf != null)
        {
            zf.player = player;
        }
    }
}