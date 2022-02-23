using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLogic : MonoBehaviour
{
    public bool IsFirstTutorial;

    private bool _activated, _showingTutorial;
    [HideInInspector]
    public bool ReachedGoal;

    private MoveDirection _moveDirection;
    private FollowFinger _followFingerScript;

    [SerializeField]
    private float _speedDecreaser;

    [SerializeField]
    private GameObject PopupPanel;

    [SerializeField]
    private GameObject _tutorialGoal;

    private void Start()
    {
        _moveDirection = FindObjectOfType<MoveDirection>();

        // disable start controls if first tutorial
        if (IsFirstTutorial)
        {
            _followFingerScript = FindObjectOfType<FollowFinger>();
            //_followFingerScript.TurnOffControl(1000, true, false);
        }

        // disable draggable objects if that tutorial
    }

    private void Update()
    {
        // slowdown time (prolly not good)
        //if (_activated)
        //{
        //    Time.timeScale -= _timeInfluencer;

        //    if(Time.timeScale <= 0)
        //    {
        //        Time.timeScale = 0;

        //        // darken game

        //        // show finger popup  // or show image of bim hitting lever..
        //        PopupPanel.SetActive(true);
        //        // play finger popup animation

        //        // if the mouseDown pos.y is equal or greater than X, 
        //        //.. un-darken game
        //        //.. destroy popup
        //        Destroy(PopupPanel);
        //        //.. timeScale to normal
        //        Time.timeScale = 1;
        //    }
        //}

        if (_activated == true && _showingTutorial == false)
        {
            _moveDirection.Speed -= _speedDecreaser * Time.deltaTime;

            if (_moveDirection.Speed <= 0)
            {
                _moveDirection.Speed = 0;

                // show finger popup  // or show image of bim hitting lever..
                PopupPanel.SetActive(true);

                _showingTutorial = true;
                _activated = false;
            }

                // if bimpivot pos.y is equal or greater than X, 
                //.. destroy popup      
        }

        if (_showingTutorial == true)
        {
            _tutorialGoal.SetActive(true);
            
            _followFingerScript.TurnOffControl(2, true, false);

            _showingTutorial = false;
        }

    }

    public void ContinueTheGame()
    {
        _moveDirection.Speed = _moveDirection.IntendedLevelSpeed;

        // delete this object
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        _activated = true;
    }
}
