using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatCode : MonoBehaviour
{
    public GameObject _gameInstance;
    public int _timesPressed = 0;

    private void Start()
    {
        _gameInstance = GameObject.Find("GameInstance(Clone)");       
    }
    public void UnlockEverything()
    {
        _timesPressed++;

        if (_timesPressed > 10)
        {
            GameInstance.GameState = 6;
            _gameInstance.GetComponent<GameInstance>().SetLevelButtons();
        }
    }
}

