using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGoal : MonoBehaviour
{
    [SerializeField]
    private TutorialLogic _tutorialLogic;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _tutorialLogic.ReachedGoal = true;
    }
}
