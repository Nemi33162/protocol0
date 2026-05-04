using UnityEngine;

public class buyGate : MonoBehaviour
{
    public int cost = 500;
    public KeyCode interactKey = KeyCode.E;

    private bool playerNear = false;
    private PlayerPoints playerPoints;

    void Update()
    {
        if (playerNear && Input.GetKeyDown(interactKey))
        {
            if (playerPoints.SpendPoints(cost))
            {
                gameObject.SetActive(false); // removes gate
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