using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameInstance : MonoBehaviour
{
    public int GameState = 0;
    
    public GameObject StartCanvas;
    public GameObject LevelCanvas;

    static public bool[,] CollectedEggs;

    private void Start()
    {
        CollectedEggs = new bool[6, 3];

        DontDestroyOnLoad(this.gameObject);
        SetLevelButtons();
    }

    public void SetGameState(int levelNumber)
    {
        if (GameState < levelNumber)
        {
            GameState = levelNumber;
        }
    }
    
    public void SetLevelButtons()
    {
        print("set level buttons");

        if (SceneManager.GetActiveScene().name == "Alpha_MainMenu")
        {
            GameObject reference = GameObject.Find("SceneReferences");

            StartCanvas = reference.GetComponent<SceneReferences>().SceneObjects[0];
            LevelCanvas = reference.GetComponent<SceneReferences>().SceneObjects[1];
        }

        if (GameState > 0)
        {
            StartCanvas.SetActive(false);
            LevelCanvas.SetActive(true);

            for (int i = 0; i < GameState; i++)
            {
                LevelCanvas.GetComponent<LevelCollection>().LevelButtons[i].GetComponent<Button>().interactable = true;
                LevelCanvas.GetComponent<LevelCollection>().LevelButtons[i].transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }


}
