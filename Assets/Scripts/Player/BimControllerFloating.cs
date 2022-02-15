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


    private void Awake()
    {
        _rigidBodyBim = GetComponentInChildren<Rigidbody2D>();
    }


    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _holdingDown = true;
        }
        else
        {
            _holdingDown = false;
        }
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
}
