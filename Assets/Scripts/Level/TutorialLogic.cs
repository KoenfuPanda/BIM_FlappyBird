using System.Collections;
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
    [HideInInspector]
    public FollowFinger FollowFingerScript;

    [SerializeField]
    private float _speedDecreaser;

    [SerializeField]
    private GameObject _popupPanel;

    [SerializeField]
    private GameObject _canvasDarkened;

    [SerializeField]
    private GameObject _tutorialGoal;
    [SerializeField]
    private Draggable _draggableTutorial;
    [SerializeField]
    private GateTutorial _gateTutorial;


    private enum TutorialType { popup1, popup2, popup3 }
    [SerializeField] private TutorialType _tutorialType;

    private void Start()
    {
        // disable start controls if first tutorial
        if (IsFirstTutorial)
        {
            FollowFingerScript = FindObjectOfType<FollowFinger>();
            FollowFingerScript.TurnOffControl(1000, true, false,false);
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

                _popupPanel.SetActive(true);

                // if tut_1, play darken_1, else play darken_2
                if (_canvasDarkened != null)
                {
                    _canvasDarkened.SetActive(true);
                    if (_tutorialType == TutorialType.popup1)
                    {
                        _canvasDarkened.GetComponent<Animator>().Play("Tutorial_1_1_Spotlight");
                    }
                    else
                    {
                        _canvasDarkened.GetComponent<Animator>().Play("Tutorial_1_2_Spotlight");
                    }                 
                }

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
                FollowFingerScript.TurnOffControl(0.2f, true, false, false);
            }

            _showingTutorial = false;
        }
    }

    public void ContinueTheGame()
    {
        _popupPanel.SetActive(false);
        if (_canvasDarkened != null)
        {
            _canvasDarkened.SetActive(false);
        }

        _moveDirection.Speed = _moveDirection.IntendedLevelSpeed;

        // delete this object
        Destroy(this);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        _moveDirection = collision.GetComponentInParent<MoveDirection>();

        _activated = true;
    }
}
