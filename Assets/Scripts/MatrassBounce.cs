using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrassBounce : MonoBehaviour
{
    private Rigidbody2D _bimRigid;

    [SerializeField]
    private float _bounceStrength;

    [SerializeField]
    private float _timeAmountControlLost;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _bimRigid = collision.attachedRigidbody;
            
            if(collision.TryGetComponent(out FollowFinger followFinger))
            {
                followFinger.TurnOffControl(_timeAmountControlLost);
            }

            BounceActivate();
        }
    }

    private void BounceActivate()
    {
        _bimRigid.AddForce(Vector2.up * _bounceStrength, ForceMode2D.Impulse);

    }


}
