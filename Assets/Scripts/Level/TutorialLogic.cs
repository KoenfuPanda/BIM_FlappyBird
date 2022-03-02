﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLogic : MonoBehaviour
{
    public bool IsFirstTutorial;
    [SerializeField]
    private bool _isDraggableTutorial, _isGateTutorial;

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
    [SerializeField]
    private Draggable _draggableTutorial;
    [SerializeField]
    private Gate _gateTutorial;

    private void Start()
    {
        _moveDirection = FindObjectOfType<MoveDirection>();

        // disable start controls if first tutorial
        if (IsFirstTutorial)
        {
            _followFingerScript = FindObjectOfType<FollowFinger>();
            _followFingerScript.TurnOffControl(1000, true, false);
        }

        // disable draggable objects if that tutorial
    }

    private void Update()
    {
        if (_activated == true && _showingTutorial == false)
        {
            _moveDirection.Speed -= _speedDecreaser * Time.deltaTime;

            if (_moveDirection.Speed <= 0)
            {
                _moveDirection.Speed = 0;
                PopupPanel.SetActive(true);

                if (_isDraggableTutorial == true)
                {
                    _draggableTutorial.IsActive = true;
                }
                else if (_isGateTutorial == true)
                {
                    _gateTutorial.IsActive = true;
                }

                _showingTutorial = true;
                _activated = false;
            }  
        }

        if (_showingTutorial == true)
        {
            if (_tutorialGoal != null)
            {
                _tutorialGoal.SetActive(true);
            }            

            if (IsFirstTutorial)
            {
                _followFingerScript.TurnOffControl(0.2f, true, false);
            }

            _showingTutorial = false;
        }
    }

    public void ContinueTheGame()
    {
        PopupPanel.SetActive(false);
        _moveDirection.Speed = _moveDirection.IntendedLevelSpeed;

        // delete this object
        Destroy(this);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        _activated = true;
    }
}
