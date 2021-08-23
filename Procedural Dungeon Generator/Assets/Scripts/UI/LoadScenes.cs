using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Load scenes 
public class LoadScenes : MonoBehaviour
{ 
    public void LoadScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
