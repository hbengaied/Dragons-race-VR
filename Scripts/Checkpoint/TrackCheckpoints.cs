using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour
{
    private List<CheckpointSingle> checkpointSinglesList;
    private int nextCheckpointSingleIndex;

    private void Awake()
    {
        Transform checkpointsTransform = transform.Find("Checkpoints");

        checkpointSinglesList = new List<CheckpointSingle>();
        foreach (Transform checkpointSingleTransform in checkpointsTransform)
        {
            CheckpointSingle checkpointSingle = checkpointSingleTransform.GetComponent<CheckpointSingle>();
            checkpointSingle.SetTrackCheckpoints(this);
            checkpointSinglesList.Add(checkpointSingle);
        }
        nextCheckpointSingleIndex = 0;
    }

    // Ajoutez cette m�thode pour r�initialiser les checkpoints
    public void ResetCheckpoints()
    {
        nextCheckpointSingleIndex = 0;
    }

    public int PlayerThroughCheckpoint(CheckpointSingle checkpointSingle)
    {
        if (checkpointSinglesList.IndexOf(checkpointSingle) == nextCheckpointSingleIndex)
        {
            // Passage du bon checkpoint
            // nextCheckpointSingleIndex++; // Si on ne veut pas faire le tour
            nextCheckpointSingleIndex = (nextCheckpointSingleIndex + 1) % checkpointSinglesList.Count; // Pour faire le tour
            //Debug.Log("Correct");
            return 1;
            // Ajouter r�compense
        }
        else
        {
            //Debug.Log("Wrong");
            // P�nalit�
            return -1; // Ou une autre valeur appropri�e pour indiquer un passage incorrect
        }
    }
}
