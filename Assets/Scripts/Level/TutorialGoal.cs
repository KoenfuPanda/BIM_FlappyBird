using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGoal : MonoBehaviour
{
    [SerializeField]
    private TutorialLogic _tutorialTrigger;

    [SerializeField]
    private bool _loseControl, _freezeWithoutContinue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_tutorialTrigger != null && _freezeWithoutContinue == false)
        {
            _tutorialTrigger.ContinueTheGame();
        }
        
        if (_loseControl == true)
        {
            _tutorialTrigger.FollowFingerScript.TurnOffControl(1000, true, false, true);
        }
    }
}
