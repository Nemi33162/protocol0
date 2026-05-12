using UnityEngine;

public class buyGate : MonoBehaviour
{
    public int cost = 500;
    public KeyCode interactKey = KeyCode.E;
    public GameObject gateToOpen;

    private bool playerNear = false;
    private PlayerPoints playerPoints;

    public AudioSource audioSource;
    public AudioClip buySound;

    void Update()
    {
        if (playerNear && playerPoints != null && Input.GetKeyDown(interactKey))
        {
            if (playerPoints.SpendPoints(cost))
            {

		audioSource.PlayOneShot(buySound);

                gateToOpen.transform.position += new Vector3(0, 5, 0);

                Debug.Log("Gate opened!");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            playerPoints = other.GetComponent<PlayerPoints>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            playerPoints = null;
        }
    }
}