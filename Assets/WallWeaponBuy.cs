using UnityEngine;

public class WallWeaponBuy : MonoBehaviour
{
    public int cost = 750;
    public KeyCode interactKey = KeyCode.E;

    public GameObject weaponToGive;

    private bool playerNear = false;
    private PlayerPoints playerPoints;

    void Update()
    {
        if (playerNear && Input.GetKeyDown(interactKey))
        {
            if (playerPoints.SpendPoints(cost))
            {
                weaponToGive.SetActive(true);
                Debug.Log("Weapon bought!");
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