using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLogic : MonoBehaviour
{
    public bool IsFirstTutorial;

    private bool _activated;

    private FollowFinger followFingerScript;

    [SerializeField]
    private float _timeInfluencer;

    [SerializeField]
    private GameObject PopupPanel;

    private void Start()
    {
        // disable start controls if first tutorial
        if (IsFirstTutorial)
        {
            followFingerScript = FindObjectOfType<FollowFinger>();
            followFingerScript.enabled = false;
        }

        // disable draggable objects if that tutorial
    }

    private void Update()
    {
        // slowdown
        if (_activated)
        {
            Time.timeScale -= _timeInfluencer;

            if(Time.timeScale <= 0)
            {
                Time.timeScale = 0;

                // darken game

                // show finger popup  // or show image of bim hitting lever..
                PopupPanel.SetActive(true);
                // play finger popup animation

                // if the mouseDown pos.y is equal or greater than X, 
                //.. un-darken game
                //.. destroy popup
                Destroy(PopupPanel);
                //.. timeScale to normal
                Time.timeScale = 1;
            }
        }
            
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        _activated = true;
    }
}
