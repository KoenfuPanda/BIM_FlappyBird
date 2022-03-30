using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInstance : MonoBehaviour
{
    static public int GameState = 0;

    public GameObject StartCanvas;
    public GameObject LevelCanvas;

    static public bool[,] CollectedEggs;
    static public int[] CollectedFeathers;
    static public int[] MaxFeathers;


    private void Start()
    {
        CollectedEggs = new bool[6, 3];
        CollectedFeathers = new int[6];
        MaxFeathers = new int[6];

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
        if (SceneManager.GetActiveScene().name == "Alpha_MainMenu" || SceneManager.GetActiveScene().name == "MainMenu" || SceneManager.GetActiveScene().name == "LevelSelect")
        {
            GameObject reference = GameObject.Find("SceneReferences");

            StartCanvas = reference.GetComponent<SceneReferences>().SceneObjects[0];
            LevelCanvas = reference.GetComponent<SceneReferences>().SceneObjects[1];
        }

        if (GameState > 0)
        {
            StartCanvas.SetActive(false);
            LevelCanvas.SetActive(true);

            //for (int i = 0; i < GameState; i++)
            //{
            //    LevelCanvas.GetComponent<LevelCollection>().LevelButtons[i].GetComponent<Button>().interactable = true;
            //    LevelCanvas.GetComponent<LevelCollection>().LevelButtons[i].transform.GetChild(0).gameObject.SetActive(true);
            //}
        }
    }


}
