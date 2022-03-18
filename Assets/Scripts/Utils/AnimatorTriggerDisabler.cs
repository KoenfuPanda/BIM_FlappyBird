using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorTriggerDisabler : MonoBehaviour
{

    private void SetActiveToFalse()
    {
        this.gameObject.SetActive(false);
    }



    private void OnEnable()
    {
        this.GetComponent<Animator>().Play("Egg_Moving");
    }
}
