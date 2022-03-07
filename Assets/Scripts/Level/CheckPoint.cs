using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    // if the player passes me...
    // 1) set other checkpoints to false, set this one to true
    // 2) call a method on the gamemanager that will make a save of the collected feathers

    private GameManager _gameManager;

    public bool IsStartLevel;
    [HideInInspector]
    public bool IsActive;

    public Transform SpawnPoint;

    private bool _hasBeenActivated;

    void Start()
    {
        _hasBeenActivated = false;
        _gameManager = FindObjectOfType<GameManager>();

        if(IsStartLevel == true)
        {
            IsActive = true;
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_hasBeenActivated == false)
        {
            foreach (CheckPoint checkPoint in _gameManager.CheckPoints) // disable others
            {
                checkPoint.IsActive = false;
            }

            IsActive = true; // enable this one

            // change color/sprite
            if (GetComponentInChildren<SpriteRenderer>() != null)
            {
                GetComponentInChildren<SpriteRenderer>().color = Color.green;
            }

            _gameManager.SaveFeathersCollectedSoFar(); // save feathers

            // fill HUD health
            
            if(IsStartLevel == true)
            {
                _gameManager.RefillHealth(true, true);
            }
            else
            {
                _gameManager.RefillHealth(false,false);
            }
            
            _hasBeenActivated = true;
        }

    }
}
