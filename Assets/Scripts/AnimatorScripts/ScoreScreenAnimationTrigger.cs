using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreScreenAnimationTrigger : MonoBehaviour
{
    [SerializeField]
    private FinishingLine _finishingLineScript;

    public void TriggerScoreCount()
    {
        _finishingLineScript.EnableThisUpdate();
    }
}
