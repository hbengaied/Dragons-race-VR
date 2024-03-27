using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private bool isGameStarted = false;

    private int finishedPlayers = 0;
    private int totalPlayers = 8; 

    public bool IsGameStarted
    {
        get { return isGameStarted; }
        private set { isGameStarted = value; }
    }

    public void PlayerFinished()
    {
        finishedPlayers++;
        Debug.Log(finishedPlayers);
        if(finishedPlayers >= totalPlayers) 
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        isGameStarted = false;
        Debug.Log("Tous les joueurs ont terminé la course");
    }
    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    void Update()
    {
        // Vérifie si les deux boutons du joystick sont pressés pour démarrer la partie
        if (Input.GetKey(KeyCode.JoystickButton4) && Input.GetKey(KeyCode.JoystickButton5) && !IsGameStarted)
        {
            IsGameStarted = true;
            Debug.Log("Game Started!");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadScene();
        }
    }
}