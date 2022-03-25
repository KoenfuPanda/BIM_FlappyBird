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

    public void LoadTheSceneWithDelay(string sceneName)
    {
        StartCoroutine(LoadDelay(sceneName));
    }

    IEnumerator LoadDelay(string sceneName)
    {
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(sceneName);
    }
}
