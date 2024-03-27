using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    void Start()
    {
        // Détruit la boule de feu après 3 secondes
        Destroy(gameObject, 3f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MyDragon") || other.CompareTag("DragonIA") || other.CompareTag("Wall") || other.CompareTag("Rock")) // Remplacez "Dragon" par le tag approprié de vos ennemis
        {
            // Ajoutez ici toute autre logique nécessaire, comme infliger des dégâts
            Destroy(gameObject); // Détruit la boule de feu lorsqu'elle touche un ennemi
        }
    }
}
