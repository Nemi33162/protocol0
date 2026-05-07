using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    public Transform firePoint;
    public float fireRate = 0.1f;
    public float fireRange = 10f;
    public bool isAuto = false;

    private float nextFireTime = 0f;

    void Update()
    {
        if(isAuto == true)
        {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;
            Shoot();
        }
    }
    else
    {   
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;
            Shoot();
        }
    }
}


    private void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, fireRange))
        {
            Debug.Log(hit.transform.name);

            ZombieAI zombie = hit.transform.GetComponent<ZombieAI>();
            if (zombie != null)
            {
                zombie.TakeDamage(25); // you can change damage here
            }
        }
    }
}