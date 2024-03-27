using UnityEngine;

public class MyShoot : MonoBehaviour
{
    public GameObject fireballPrefab;
    private float fireballSpeed = 80f;
    private float lastFireTime = 0f; // Temps du dernier tir
    private float fireRate = 5f; // Interval entre les tirs en secondes

    void Update()
    {
        // Vérifie si les deux boutons du joystick sont pressés et si le personnage peut tirer
        if (Input.GetKey(KeyCode.JoystickButton4) && Input.GetKey(KeyCode.JoystickButton5) && CanFire())
        {
            Fire();
        }
    }

    bool CanFire()
    {
        // Vérifie si suffisamment de temps s'est écoulé depuis le dernier tir
        if (Time.time - lastFireTime >= fireRate)
        {
            return true;
        }
        return false;
    }

    void Fire()
    {
        // Instancie la boule de feu et la projette vers l'avant
        GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
        Rigidbody rb = fireball.GetComponent<Rigidbody>();
        Vector3 direction = transform.forward; // Direction vers laquelle le personnage regarde
        rb.velocity = direction * fireballSpeed;
        lastFireTime = Time.time; // Met à jour le temps du dernier tir
        Debug.Log("Fireball shot!");
    }
}
