using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    void Start()
    {
        // D�truit la boule de feu apr�s 3 secondes
        Destroy(gameObject, 3f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MyDragon") || other.CompareTag("DragonIA") || other.CompareTag("Wall") || other.CompareTag("Rock")) // Remplacez "Dragon" par le tag appropri� de vos ennemis
        {
            // Ajoutez ici toute autre logique n�cessaire, comme infliger des d�g�ts
            Destroy(gameObject); // D�truit la boule de feu lorsqu'elle touche un ennemi
        }
    }
}
