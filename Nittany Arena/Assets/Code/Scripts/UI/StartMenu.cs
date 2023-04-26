using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        foreach (InputData inputData in InputMatcher.Instance.InputDataRefs)
        {
            if (inputData.CurrentFrame.Start)
                SceneLoader.Instance.LoadScene("Sean Dev Scene");
        }
    }
}
