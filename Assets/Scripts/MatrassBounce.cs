﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrassBounce : MonoBehaviour
{
    private Rigidbody2D _bimRigid;
    private MoveDirection _moveDirection;

    [SerializeField]
    private float _bounceStrength;

    [SerializeField]
    private float _timeAmountControlLost;

    private Animator _animator;

    [SerializeField]
    private bool _speedsUpGame;
    [SerializeField]
    private float _speedBoost, _timeBoosted;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _bimRigid = collision.attachedRigidbody;
            
            if(collision.TryGetComponent(out FollowFinger followFinger))
            {
                followFinger.TurnOffControl(_timeAmountControlLost, false, true);

                if(collision.GetComponentInParent<MoveDirection>() != null)
                {
                    var moveDir = collision.GetComponentInParent<MoveDirection>(); 
                    if (_speedsUpGame)
                    {
                        moveDir.VertcalBounceBoost(_speedBoost, _timeBoosted); // adds level speed to the stage
                    }
                    else
                    {
                        moveDir.VerticalBounceMatrass(); // resets level speed and some values
                    }
                }

                BounceActivate();
            }        
        }
    }

    private void BounceActivate()
    {
        //_bimRigid.AddForce(Vector2.up * _bounceStrength, ForceMode2D.Impulse);

        _bimRigid.velocity = Vector2.up * _bounceStrength; // this is more reliable than addforce

        _animator.SetTrigger("Bounce");
    }


}
