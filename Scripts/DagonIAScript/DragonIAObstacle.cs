using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class DragonIAObstacle : Agent
{

    [SerializeField] private TrackCheckpoints trackCheckpoints;
    [SerializeField] private Transform targetTransform;
    private GameManager gameManager;

    private float moveSpeed = 50f;
    private float normalSpeed = 50f;
    private float bonusSpeed = 70f;

    private float reducedSpeedDragonContact = 50f;
    private float reducedSpeedRockContact = 35f;
    private float reducdeSpeedHitByFire = 25f;

    private float collisionDragonDuration = 1f;
    private float collisionRockDuration = 1f;
    private float bonusDuration = 2f;
    private float collisionFireDuration = 2f;

    private bool crossedFinishLigne = false;
    private bool isSpeedReduced = false;
    private bool isStopped = false;

    private float XPos;
    private float ZPos;

    private Rigidbody rb;

    private void Start()
    {
        gameManager = GetComponent<GameManager>();
        if (trackCheckpoints == null)
        {
            Debug.LogError("TrackCheckpoints non assigné à MoveToGoalAgent");
        }
    }

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }
    public override void OnEpisodeBegin()
    {
        XPos = transform.localPosition.x;
        ZPos = transform.localPosition.z;
        transform.localPosition = new Vector3(XPos, -45.36f, ZPos);
        trackCheckpoints.ResetCheckpoints();
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (!gameManager.IsGameStarted || isStopped)
        {
            return;
        }
        else
        {
            float deplacementX = 2; // Ne rien faire par défaut
            float deplacementZ = 1; // Toujours avancer vers l'avant

            // Décision du mouvement gauche/droite
            float actionValue = actions.ContinuousActions[0];

            // Si l'action est 0, déplacez-vous vers la gauche
            if (actionValue == 0)
            {
                deplacementX = 0; // rien faire
                SetReward(+4f);
            }
            // Si l'action est 1, déplacez-vous vers la gauche
            else if (actionValue != 0)
            {
                deplacementX = actionValue; // Déplacement vers la gauche
            }

            // Calculer la nouvelle position en X
            float newXPosition = transform.localPosition.x + deplacementX * Time.deltaTime * moveSpeed;

            // Vérifier si la nouvelle position en X est dans les limites
            if (newXPosition < -11.8f || newXPosition > 195f)
            {
                deplacementX = 0; // Forcer le déplacement en X à 0 si hors des limites
            }
            transform.localPosition += new Vector3(deplacementX, 0, deplacementZ) * Time.deltaTime * moveSpeed;
    }
        }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Goal")
        {
            //Debug.Log("Objectif atteint");
            //SetReward(+10000f);
            //EndEpisode();
            StopAgent();
            gameManager.PlayerFinished();
        }

        if (other.gameObject.tag == "Wall")
        {
            //Debug.Log("Collision avec un mur");
            //SetReward(-200f);
            //EndEpisode();
            StartCoroutine(ReduceSpeedHitObstacleTemporarily());
        }

        if (other.gameObject.tag == "Rock")
        {
            //Debug.Log("Collision avec un obstacle");
            //SetReward(-1000f);
            //EndEpisode();
            StartCoroutine(ReduceSpeedHitObstacleTemporarily());
        }

        if (other.gameObject.tag == "Bonus")
        {
            //Debug.Log("Manger Bonus");
            //SetReward(+10f);
            StartCoroutine(BonusSpeedTemporarily());
            EndEpisode();
        }

        if (other.gameObject.tag == "MyDragon" || other.gameObject.tag == "DragonIA")
        {
            //Debug.Log("Collisions avec un autre dragon");
            //SetReward(-1f);
            StartCoroutine(ReduceSpeedTemporarily());

        }

        if (other.TryGetComponent<CheckpointSingle>(out CheckpointSingle checkpoint))
        {
            int checkpointResult = trackCheckpoints.PlayerThroughCheckpoint(checkpoint);
            if (checkpointResult == 1)
            {
                // Debug.Log("Bon checkpoint atteint");
                //SetReward(+100f);
            }
        }

        if (other.gameObject.tag == "FireBall")
        {
            StartCoroutine(ReduceSpeedHitByFireTemporarily());
        }

    }

    private IEnumerator ReduceSpeedTemporarily()
    {
        if (isSpeedReduced)
        {
            yield break;
        }
        isSpeedReduced = true;
        moveSpeed = reducedSpeedDragonContact; // Réduisez la vitesse
        yield return new WaitForSeconds(collisionDragonDuration); // Attendez 2 secondes
        isSpeedReduced = false;
        moveSpeed = normalSpeed; // Rétablissez la vitesse normale
    }

    private IEnumerator ReduceSpeedHitObstacleTemporarily()
    {
        if (isSpeedReduced)
        {
            yield break;
        }
        isSpeedReduced = true;
        moveSpeed = reducedSpeedRockContact; // Réduisez la vitesse
        yield return new WaitForSeconds(collisionRockDuration); // Attendez 2 secondes
        isSpeedReduced = false;
        moveSpeed = normalSpeed; // Rétablissez la vitesse normale
    }

    private IEnumerator BonusSpeedTemporarily()
    {
        moveSpeed = bonusSpeed; // augmente la vitesse
        yield return new WaitForSeconds(bonusDuration); // Attendez 2 secondes
        moveSpeed = normalSpeed; // Rétablissez la vitesse normale
    }

    private IEnumerator ReduceSpeedHitByFireTemporarily()
    {
        if (isSpeedReduced)
        {
            yield break;
        }
        isSpeedReduced = true;
        moveSpeed = reducdeSpeedHitByFire; // Réduisez la vitesse
        yield return new WaitForSeconds(collisionFireDuration); // Attendez 2 secondes
        isSpeedReduced = false;
        moveSpeed = normalSpeed; // Rétablissez la vitesse normale
    }

    private void StopAgent()
    {
        isStopped = true;
    }

}
