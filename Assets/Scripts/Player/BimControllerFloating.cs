using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BimControllerFloating : MonoBehaviour
{
    private Rigidbody2D _rigidBodyBim;
    private bool _holdingDown;

    [SerializeField]
    private float _speed;

    private bool bounce = false;
    //private static Rigidbody2D rigidBody;
    public static bool controlCharacter = true;
    private float _lostControlTimer;


    private void Awake()
    {
        //_rigidBodyBim = GetComponentInChildren<Rigidbody2D>();
        _rigidBodyBim = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (controlCharacter == false)
        {
            _lostControlTimer -= Time.deltaTime;

            if (_lostControlTimer <= 0)
            {
                TurnOnControl();
                _lostControlTimer = 0;
            }
        }


        if (Input.GetMouseButton(0))
        {
            _holdingDown = true;
        }
        else
        {
            _holdingDown = false;
        }

        //BimRotationLogic();
    }


    private void FixedUpdate()
    {
        if (_holdingDown)
        {
            IncreaseHeight();
        }
    }

    private void IncreaseHeight()
    {
        if (_rigidBodyBim.velocity.y <= 10) // limiting speed upwards
        {
            _rigidBodyBim.velocity += Vector2.up * Time.deltaTime * _speed;
        }
        
    }


    public void TurnOffControl(float timeLostControl, bool shouldOverrideTimerNoMatterWhat, bool wantsGravity)
    {
        bounce = true;
        controlCharacter = false;

        if (wantsGravity == true)
        {
            _rigidBodyBim.gravityScale = 1;
        }

        // start coroutine to regain control
        //StartCoroutine(RegainControl(timeLostControl)); cgange this to update logic

        if (shouldOverrideTimerNoMatterWhat == false && timeLostControl > _lostControlTimer) // if statement for matrass bounces
        {
            _lostControlTimer = timeLostControl;
        }
        if (shouldOverrideTimerNoMatterWhat == true)
        {
            _lostControlTimer = timeLostControl;
        }

    }

    public void TurnOnControl()
    {
        bounce = false;
        controlCharacter = true;
        _rigidBodyBim.gravityScale = 1;
        _lostControlTimer = 0;
    }



    private void BimRotationLogic()
    {
        if (_rigidBodyBim.velocity.y > 0 && _rigidBodyBim.transform.localRotation.z < 0.05f)
        {
            _rigidBodyBim.transform.Rotate(0, 0, 1.5f);
        }
        else if (_rigidBodyBim.velocity.y < 0 && _rigidBodyBim.transform.localRotation.z > -0.30f)
        {
            _rigidBodyBim.transform.Rotate(0, 0, -1.5f);
        }
        else // When flying straight
        {
            if (_rigidBodyBim.transform.localRotation.z > -0.15)
            {
                _rigidBodyBim.transform.Rotate(0, 0, -1.5f);
            }
            else if (_rigidBodyBim.transform.localRotation.z < -0.16)
            {
                _rigidBodyBim.transform.Rotate(0, 0, +1.5f);
            }
        }
    }
}
