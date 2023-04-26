using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private Scene m_CurrentScene;

    private static LoadSceneParameters s_LoadParameters = new LoadSceneParameters(LoadSceneMode.Additive);

    public static SceneLoader Instance { get; private set; }

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
        m_CurrentScene = SceneManager.GetSceneAt(1);
        Debug.Log(m_CurrentScene.name + " " + m_CurrentScene.IsValid());
    }

    public void ReloadScene()
    {
        string name = m_CurrentScene.name;
        LoadScene(name);
    }

    public void LoadScene(string name)
    {
        SceneManager.UnloadSceneAsync(m_CurrentScene).completed += _ =>
        m_CurrentScene = SceneManager.LoadScene(name, s_LoadParameters);
    }
}
