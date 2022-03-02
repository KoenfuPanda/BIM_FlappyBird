using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstance : MonoBehaviour
{
    public GameObject StartCanvas;
    public GameObject LevelCanvas;
    [SerializeField] public int _gameState = 0;

    private void Start()
    {
        if (_gameState > 0)
        {
            StartCanvas.SetActive(false);
            LevelCanvas.SetActive(true);
        }
    }
}
