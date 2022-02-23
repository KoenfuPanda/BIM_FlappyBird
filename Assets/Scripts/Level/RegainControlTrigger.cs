using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegainControlTrigger : MonoBehaviour
{
    private FollowFinger _followFinger;

    private void Start()
    {
        _followFinger = FindObjectOfType<FollowFinger>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("test");
        _followFinger.TurnOnControl();
    }

}
