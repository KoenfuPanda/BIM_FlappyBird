﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGoal : MonoBehaviour
{
    [SerializeField]
    private TutorialLogic _tutorialTrigger;

    [SerializeField]
    private bool _loseControl;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _tutorialTrigger.ContinueTheGame();

        if (_loseControl == true)
        {
            _tutorialTrigger.FollowFingerScript.TurnOffControl(1000, true, false, true);
        }
    }
}
