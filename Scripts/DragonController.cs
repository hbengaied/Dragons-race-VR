using System;
using UnityEngine;

public class DragonController : MonoBehaviour
{
    public float moveSpeed = 20.0f;
    public float autoMoveSpeed = 30.0f;
    public float rotationSpeed = 30.0f;
    public float maxVerticalSpeed = 5.0f;
    public float gravity = 9.81f;
    public float altitudeChangeSpeed = 1.0f;
    public float maxAltitude = 1000.0f;
    public float maxRiseRotationSpeed = 60.0f;
    public float rotationLerpSpeed = 1;
    private Animator animator;
    private float targetAltitude = -1000.0f;
    private Rigidbody rb;
    private bool isRising = false;
    private bool isFalling = false;
    private float currentXRotation = 0.0f;
    private float currentZRotation = 0.0f;


    private bool StartMyGame = false;



    // Rotation en Z en r�ponse aux mouvements horizontaux du joueur
    float maxZRotation = 90.0f; // Angle maximal de rotation en Z
    float zRotationFactor = 0.5f; // Facteur de rotation en Z en fonction de la pression de la touche

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private bool StratGame()
    {
        float ValueTrigger = Input.GetAxis("Horizontal");
        if (ValueTrigger > 0.1f || ValueTrigger < 0)
        {
            return true;
        }

        return false;
    }

    private void Update()
    {

        if (StartMyGame == false)
        {
            StartMyGame = StratGame();
        }
        else if (StartMyGame == true)
        {

            // D�placement horizontal (gauche, droite)
            float horizontalInput = Input.GetAxis("Horizontal");
            float rotation = horizontalInput * rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up * rotation);

            // Rotation en Z en r�ponse aux mouvements horizontaux du joueur
            float stickHorizontalInput = Input.GetAxis("Horizontal");

            if (Mathf.Abs(stickHorizontalInput) > 0.1f)
            {
                // Rotation en Z en r�ponse aux mouvements horizontaux du joueur
                if (stickHorizontalInput < 0)
                {
                    // Rotation en Z positive lorsque le joueur tourne � gauche
                    currentZRotation = Mathf.Lerp(currentZRotation, maxZRotation * -stickHorizontalInput * zRotationFactor, rotationLerpSpeed * Time.deltaTime);
                }
                else if (stickHorizontalInput > 0)
                {
                    // Rotation en Z n�gative lorsque le joueur tourne � droite
                    currentZRotation = Mathf.Lerp(currentZRotation, maxZRotation * -stickHorizontalInput * zRotationFactor, rotationLerpSpeed * Time.deltaTime);
                }
            }
            else
            {
                // R�initialisation progressive de la rotation en Z lorsque le joueur ne tourne pas
                currentZRotation = Mathf.Lerp(currentZRotation, 0.0f, rotationLerpSpeed * Time.deltaTime);
            }

            // Contr�le de l'altitude en r�ponse � Input.GetAxis("Vertical")
            float verticalInput = Input.GetAxis("Vertical");
            if (verticalInput > 0 && targetAltitude > 0) // Invers� ici, v�rifiez que targetAltitude est sup�rieur � 0
            {
                targetAltitude = transform.position.y - altitudeChangeSpeed * Time.deltaTime; // Inverse la direction de la mont�e
                isRising = true;
            }
            else
            {
                isRising = false;
            }

            if (verticalInput < 0 && targetAltitude < maxAltitude) // Invers� ici, v�rifiez que targetAltitude est inf�rieur � maxAltitude
            {
                targetAltitude = transform.position.y + altitudeChangeSpeed * Time.deltaTime; // Inverse la direction de la descente
                isFalling = true;
            }
            else
            {
                isFalling = false;
            }

            float currentAltitude = transform.position.y;
            float altitudeDifference = targetAltitude - currentAltitude;

            if (Mathf.Abs(altitudeDifference) > 0)
            {
                rb.AddForce(Vector3.up * gravity);

                if (altitudeDifference > 0 && rb.velocity.y < maxVerticalSpeed)
                {
                    rb.AddForce(Vector3.up * moveSpeed);
                }
                else if (altitudeDifference < 0)
                {
                    rb.AddForce(Vector3.down * moveSpeed);
                }
            }
            else if (!isRising && !isFalling)
            {
                rb.velocity = Vector3.zero;
            }

            if (isRising)
            {
                float targetXRotation = 50.0f;
                currentXRotation = Mathf.MoveTowards(currentXRotation, targetXRotation, maxRiseRotationSpeed * Time.deltaTime);
            }
            else if (isFalling)
            {
                float targetXRotation = -50.0f;
                currentXRotation = Mathf.MoveTowards(currentXRotation, targetXRotation, maxRiseRotationSpeed * Time.deltaTime);
            }
            else
            {
                float targetXRotation = 0.0f;
                currentXRotation = Mathf.MoveTowards(currentXRotation, targetXRotation, maxRiseRotationSpeed * Time.deltaTime);
            }

            transform.rotation = Quaternion.Euler(currentXRotation, transform.rotation.eulerAngles.y, currentZRotation);

            // D�placement automatique vers l'avant
            Vector3 autoMovement = transform.forward * autoMoveSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + autoMovement);

            // Activer l'animation de vol lorsque le dragon bouge
            if (Mathf.Abs(horizontalInput) > 0.1f)
            {
                animator.SetBool("IsFlying", true);
            }
            else
            {
                animator.SetBool("IsFlying", false);

                // Si aucune touche de mouvement n'est enfonc�e, descendre jusqu'au sol
                if (currentAltitude > 0)
                {
                    targetAltitude = currentAltitude;
                }
            }
        }
    }
}
