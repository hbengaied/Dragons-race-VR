using System.Collections;
using UnityEngine;

public class DragonControllerOnlyHorizontal : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;

    private float autoMoveSpeed = 50.0f;
    private float normalSpeed = 50f;
    private float bonusSpeed = 70f;

    private float rotationSpeed = 30.0f;
    private float rotationLerpSpeed = 1;

    private float currentZRotation = 0.0f;
    private float smoothRotationLerpSpeed = 5.0f; // Ajustez cette valeur selon vos besoins

    private float reducedSpeedDragonContact = 50f;
    private float reducedSpeedRockContact = 35f;
    private float reducdeSpeedHitByFire = 25f;

    private float collisionDragonDuration = 1f;
    private float collisionRockDuration = 2f;
    private float collisionFireDuration = 2f;
    private float bonusDuration = 2f;

    private bool isSpeedReduced = false;
    private bool isStopped = false;

    float maxZRotation = 90.0f;
    float zRotationFactor = 0.5f;

    private PlateformOnlyHorizontal MovePlateforme; // Ne pas créer une nouvelle instance ici
    private GameManager gameManager;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        MovePlateforme = GetComponent<PlateformOnlyHorizontal>();
        gameManager = GetComponent<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bonus")
        {
            StartCoroutine(BonusSpeedTemporarily());
        }

        if (other.gameObject.tag == "Rock")
        {
            Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAA");
            StartCoroutine(ReduceSpeedHitObstacleTemporarily());
        }

        if (other.gameObject.tag == "DragonIA")
        {
            StartCoroutine(ReduceSpeedTemporarily());
        }

        if (other.gameObject.tag == "FireBall")
        {
            StartCoroutine(ReduceSpeedHitByFireTemporarily());
        }

        if (other.gameObject.tag == "DragonIA")
        {
            //Debug.Log("Collisions avec un autre dragon");
            //SetReward(-1f);
            StartCoroutine(ReduceSpeedTemporarily());

        }

        if (other.gameObject.tag == "Goal")
        {
            StopDragon();
        }
    }

    public void StopDragon()
    {
        isStopped = true;
        gameManager.PlayerFinished();
    }

    private IEnumerator ReduceSpeedTemporarily()
    {
        if (isSpeedReduced)
        {
            yield break;
        }
        isSpeedReduced = true;
        autoMoveSpeed = reducedSpeedDragonContact; // Réduisez la vitesse
        yield return new WaitForSeconds(collisionDragonDuration); // Attendez 2 secondes
        autoMoveSpeed = normalSpeed; // Rétablissez la vitesse normale
        isSpeedReduced = false;
    }

    private IEnumerator BonusSpeedTemporarily()
    {
        autoMoveSpeed = bonusSpeed; // augmente la vitesse
        yield return new WaitForSeconds(bonusDuration); // Attendez 2 secondes
        autoMoveSpeed = normalSpeed; // Rétablissez la vitesse normale
    }

    private IEnumerator ReduceSpeedHitObstacleTemporarily()
    {
        if (isSpeedReduced)
        {
            yield break;
        }
        isSpeedReduced = true;
        autoMoveSpeed = reducedSpeedRockContact; // Réduisez la vitesse
        //MovePlateforme.inverseModule();
        yield return new WaitForSeconds(collisionRockDuration); // Attendez 2 secondes
        autoMoveSpeed = normalSpeed; // Rétablissez la vitesse normale
        isSpeedReduced = false;
        //MovePlateforme.inverseModule();
    }

    private IEnumerator ReduceSpeedHitByFireTemporarily()
    {
        if (isSpeedReduced)
        {
            yield break;
        }
        isSpeedReduced = true;
        autoMoveSpeed = reducdeSpeedHitByFire; // Réduisez la vitesse
        //MovePlateforme.inverseModule();
        yield return new WaitForSeconds(collisionFireDuration); // Attendez 2 secondes
        autoMoveSpeed = normalSpeed; // Rétablissez la vitesse normale
        isSpeedReduced = false;
        //MovePlateforme.inverseModule();
    }

    private void Update()
    {
        if (!gameManager.IsGameStarted || isStopped)
        {
            return;
        }
        else
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            Vector3 currentPosition = transform.position;

            float rotation = horizontalInput * rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up * rotation);

            Vector3 projectedPosition = currentPosition + Quaternion.Euler(0, rotation, 0) * transform.forward * autoMoveSpeed * Time.deltaTime;

            if (projectedPosition.x > -11.8f && projectedPosition.x < 195f)
            {
                float stickHorizontalInput = Input.GetAxis("Horizontal");

                if (Mathf.Abs(stickHorizontalInput) > 0.1f)
                {
                    if (stickHorizontalInput < 0)
                    {
                        currentZRotation = Mathf.Lerp(currentZRotation, maxZRotation * -stickHorizontalInput * zRotationFactor, rotationLerpSpeed * Time.deltaTime);
                    }
                    else if (stickHorizontalInput > 0)
                    {
                        currentZRotation = Mathf.Lerp(currentZRotation, maxZRotation * -stickHorizontalInput * zRotationFactor, rotationLerpSpeed * Time.deltaTime);
                    }
                }
                else
                {
                    currentZRotation = Mathf.Lerp(currentZRotation, 0.0f, rotationLerpSpeed * Time.deltaTime);
                }

                Vector3 autoMovement = transform.forward * autoMoveSpeed * Time.deltaTime;
                rb.MovePosition(rb.position + autoMovement);

                if (Mathf.Abs(horizontalInput) > 0.1f)
                {
                    animator.SetBool("IsFlying", true);
                }
            }
            else
            {
                // Définir la rotation cible avec y = 0
                Quaternion targetRotation = Quaternion.Euler(0, 0, 0);

                // Interpolation lente (Lerp) vers la rotation cible
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * smoothRotationLerpSpeed);
                Vector3 autoMovement = transform.forward * autoMoveSpeed * Time.deltaTime;
                rb.MovePosition(rb.position + autoMovement);
            }
        }
    }
}
