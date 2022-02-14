using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimationSpeed : MonoBehaviour
{
    public float animationSpeed = 1;
    public bool reverseAnimation = false;

    void Start()
    {
        GetComponent<Animator>().speed = animationSpeed;

        if (reverseAnimation)
        {
            GetComponent<Animator>().SetTrigger("Reverse");
        }
    }
}
