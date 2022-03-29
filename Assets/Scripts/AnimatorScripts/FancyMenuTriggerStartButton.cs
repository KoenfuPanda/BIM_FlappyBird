using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FancyMenuTriggerStartButton : MonoBehaviour
{
    [SerializeField]
    private Animator _startCanvasAnimator;




    private void StartShowingButton()
    {
        _startCanvasAnimator.enabled = true;
    }
}
