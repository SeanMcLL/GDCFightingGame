using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int PlayerCount { get; private set; }
    public bool gameOver { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        PlayerCount = 2;
    }

    public void PlayerDefeated()
    {
        PlayerCount--;

        if (PlayerCount == 1)
        {
            GameOver();
        }
    }

    public void GameOver(){
        gameOver = true;
        Debug.Log("gameOver state = " + gameOver);
    }

    public void ReloadScene() 
    {
        SceneManager.UnloadSceneAsync("HitboxTestingZone Aryan");
        SceneManager.LoadScene("HitboxTestingZone Aryan", LoadSceneMode.Additive); // Load the new scene additively
    }
}
