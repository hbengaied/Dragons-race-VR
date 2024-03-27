using UnityEngine;
using System.Collections;

public class BonusScript : MonoBehaviour
{
    public GameObject bonusPrefab;
    private GameObject currentBonus;
    private Vector3 localSpawnPosition = Vector3.zero;
    private bool isCoroutineRunning = false;

    void Start()
    {
        SpawnBonus();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MyDragon" || other.gameObject.tag == "DragonIA")
        {
            //Debug.Log("HELLOOOO - OnTriggerEnter");
            if (currentBonus != null)
            {
                //Debug.Log("Destroying current bonus");
                Destroy(currentBonus, 0f);
                currentBonus = null;
            }

            if (!isCoroutineRunning)
            {
                StartCoroutine(RecreateAfterDelay(4f));
            }
        }
    }

    IEnumerator RecreateAfterDelay(float delay)
    {
        isCoroutineRunning = true;
        yield return new WaitForSeconds(delay);
        SpawnBonus();
        isCoroutineRunning = false;
    }

    private void SpawnBonus()
    {
        Quaternion rotationX90 = Quaternion.Euler(90f, 0f, 0f);
        currentBonus = Instantiate(bonusPrefab, Vector3.zero, rotationX90, transform);
        currentBonus.transform.localPosition = localSpawnPosition;
        currentBonus.SetActive(true);
        //Debug.Log("Bonus spawned");
    }
}
