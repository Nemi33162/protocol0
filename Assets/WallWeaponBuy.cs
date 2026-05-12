using UnityEngine;

public class WallWeaponBuy : MonoBehaviour
{
    public int cost = 750;
    public KeyCode interactKey = KeyCode.E;

    public GameObject weaponToGive;
    public GameObject[] allWeapons;

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
                GiveWeapon();
            }
        }
    }

    void GiveWeapon()
    {
        // turn off all guns
        foreach (GameObject gun in allWeapons)
        {
            gun.SetActive(false);
        }

        // turn on selected gun
        weaponToGive.SetActive(true);

        Debug.Log("Bought weapon: " + weaponToGive.name);
    }

    void OnTriggerEnter(Collider other)
    {

	Debug.Log("Entered trigger");


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