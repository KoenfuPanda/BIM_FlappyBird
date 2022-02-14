using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void LoadTheSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
