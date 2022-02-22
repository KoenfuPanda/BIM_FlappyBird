using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnetizer : MonoBehaviour
{
    private float _t;
    private float _timeToReachTarget = 0.3f;

    [HideInInspector]
    public Vector3 StartPosition;
    private Vector3 _target;

    private GameObject _player;

    private void Start()
    {
        _player = FindObjectOfType<HitObstacle>().gameObject;
        StartPosition = this.transform.position;
    }

    private void Update()
    {
        _target = _player.transform.position;

        _t += Time.deltaTime / _timeToReachTarget;
        //transform.position = Vector3.Lerp(startPosition, target, t);
        transform.position = Vector3.MoveTowards(StartPosition, _target, _t);  // movetowards makes more sense (lerp should be used rarely i think)
    }
}
