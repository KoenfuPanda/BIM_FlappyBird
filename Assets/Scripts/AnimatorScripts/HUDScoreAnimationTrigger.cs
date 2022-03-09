using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDScoreAnimationTrigger : MonoBehaviour
{
    [SerializeField]
    private FinishingLine _finishingLineScript;

    public void TriggerScoreScreen()
    {
        _finishingLineScript.PopOpenScore();
    }
}
