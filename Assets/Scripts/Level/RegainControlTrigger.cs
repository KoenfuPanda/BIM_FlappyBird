using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegainControlTrigger : MonoBehaviour
{
    [SerializeField]
    private bool _regainsControl, _normalizesSpeed;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_regainsControl)
        {
            collision.GetComponent<FollowFinger>().TurnOnControl();
        }

        if (_normalizesSpeed)
        {
            var moveDir = collision.transform.GetComponentInParent<MoveDirection>();
            moveDir.BoostedTimer = moveDir.BoostedTimeLimit;
        }       
    }

}
