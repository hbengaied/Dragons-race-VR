using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shoot : MonoBehaviour
{
    public GameObject fireballPrefab;
    private float fireballSpeed = 80f;
    private float raycastDistance = 180f;

    private float closeDistanceThreshold = 20f; // Seuil pour �tre consid�r� comme proche
    private float farDistanceThreshold = 180f;   // Seuil pour �tre consid�r� comme loin
    private float middleDistanceTreshhold = 140;
    private float lastFireTime = 0f;            // Temps du dernier tir
    private float fireRate = 5f;                // Interval entre les tirs en secondes

    void Update()
    {
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward) * raycastDistance;

        //Debug.DrawRay(transform.position, forward, Color.green);

        if (Physics.Raycast(transform.position, forward, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("MyDragon") || hit.collider.CompareTag("DragonIA"))
            {
                float distance = Vector3.Distance(transform.position, hit.point);
                float distanceScore = FuzzifyDistance(distance);

                if (ShouldFire(distanceScore) && CanFire())
                {
                    Fire(hit.point);
                    Debug.Log("FIIIIIIIIIIIIIIIIIIIIIIIIRE");
                }
            }
        }
    }

    float FuzzifyDistance(float distance)
    {
        if (distance < 10f) // Tr�s proche
        {
            return 1.0f;
        }
        else if (distance < closeDistanceThreshold) // Proche
        {
            return 0.8f;
        }
        else if (distance < middleDistanceTreshhold) // Interm�diaire
        {
            return 0.5f;
        }
        else if (distance < farDistanceThreshold) // Loin
        {
            return 0.3f;
        }
        else // Tr�s loin
        {
            return 0.0f;
        }
    }

    bool ShouldFire(float distanceScore)
    {
        // D�cision de tir bas�e sur le score de distance
        return distanceScore >= 0.5f; // Tirer si l'ennemi est au moins � une distance interm�diaire
    }

    bool CanFire()
    {
        // V�rifie si suffisamment de temps s'est �coul� depuis le dernier tir
        if (Time.time - lastFireTime >= fireRate)
        {
            return true;
        }
        return false;
    }

    void Fire(Vector3 target)
    {
        GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
        Rigidbody rb = fireball.GetComponent<Rigidbody>();
        Vector3 direction = (target - transform.position).normalized;
        rb.velocity = direction * fireballSpeed;
        lastFireTime = Time.time; // Met � jour le temps du dernier tir
    }
}
