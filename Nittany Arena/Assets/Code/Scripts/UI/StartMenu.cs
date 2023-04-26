using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private InputData m_inputData;
    
    // Update is called once per frame
    void Update()
    {
        if (m_inputData.CurrentFrame.ButtonSouth)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single); // Load the persistent scene asynchronously
            SceneManager.LoadScene("Sean Dev Scene", LoadSceneMode.Additive); // Load the new scene additively
        }
    }
}
