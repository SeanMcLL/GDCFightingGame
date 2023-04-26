using UnityEngine;
using UnityEngine.SceneManagement;

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
            gameOver = true;
    }

    public void ReloadScene()
    {
        gameOver = false;
        PlayerCount = 2;
        SceneLoader.Instance.ReloadScene();
    }
}
