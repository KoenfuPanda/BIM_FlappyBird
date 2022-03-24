using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrassBounce : MonoBehaviour
{
    private Rigidbody2D _bimRigid;
    private MoveDirection _moveDirection;

    [SerializeField]
    private bool _bouncesDownwards;

    [SerializeField]
    private float _bounceStrength;

    [SerializeField]
    private float _timeAmountControlLost;

    [SerializeField] 
    private Animator _animator;

    [SerializeField]
    private bool _speedsUpGame;
    [SerializeField]
    private float _speedBoost, _timeBoosted;

    [SerializeField]
    private GameObject _particleMatrassBouncePrefab;

    private void Start()
    {
        //_animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _bimRigid = collision.attachedRigidbody;
            
            if(collision.TryGetComponent(out FollowFinger followFinger))
            {
                followFinger.TurnOffControl(_timeAmountControlLost, false, true, false);

                if(collision.GetComponentInParent<MoveDirection>() != null)
                {
                    _moveDirection = collision.GetComponentInParent<MoveDirection>(); 

                    if (_speedsUpGame)
                    {
                        _moveDirection.VertcalBounceBoost(_speedBoost, _timeBoosted); // adds level speed to the stage
                    }
                    else
                    {
                        _moveDirection.VerticalBounceMatrass(); // resets level speed and some values
                    }
                }

                if (_bouncesDownwards == false)
                {
                    BounceUpActivate();
                    _particleMatrassBouncePrefab.SetActive(true);
                }
                else
                {
                    BounceDownActivate();
                    _particleMatrassBouncePrefab.transform.rotation = Quaternion.Euler(0,0,180);
                    _particleMatrassBouncePrefab.SetActive(true);
                }                           
            }        
        }
    }

    private void BounceUpActivate()
    {
        _bimRigid.velocity = Vector2.up * _bounceStrength; // this is more reliable than addforce

        _animator.SetTrigger("Bounce");
    }
    private void BounceDownActivate()
    {
        _bimRigid.gravityScale = -1;
        _bimRigid.velocity = -Vector2.up * _bounceStrength; 
        _animator.SetTrigger("Bounce");
    }



}
