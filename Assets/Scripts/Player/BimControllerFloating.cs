using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BimControllerFloating : MonoBehaviour
{
    private Rigidbody2D _rigidBodyBim;
    private bool _holdingDown;


    private void Awake()
    {
        _rigidBodyBim = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
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
            Debug.Log("flying !!!!!!!");
        }
    }

    private void IncreaseHeight()
    {
        _rigidBodyBim.velocity += Vector2.up * Time.deltaTime;
    }
}
