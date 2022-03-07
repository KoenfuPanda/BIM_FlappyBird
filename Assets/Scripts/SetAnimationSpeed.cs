using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimationSpeed : MonoBehaviour
{
    public float animationSpeed = 1;
    public bool reverseAnimation = false;

    private void Awake()
    {
        GetComponent<Animator>().speed = animationSpeed;
    }
    //void Start()
    //{
    //    GetComponent<Animator>().speed = animationSpeed;

    //    if (reverseAnimation)
    //    {
    //        GetComponent<Animator>().SetTrigger("Reverse");
    //    }
    //}

    private void OnEnable()
    {
        GetComponent<Animator>().speed = animationSpeed;

        if (reverseAnimation)
        {
            GetComponent<Animator>().SetTrigger("Reverse");
        }
    }
}
