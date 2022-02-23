using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMatras : MonoBehaviour
{
    private Animator _animator;

    public enum Direction { Left, Right }
    public Direction direction;

    private bool _bouncesLeft;

    private Transform _playerParent;


    private void Start()
    {
        if (direction.ToString() == "Right")
        {
            _bouncesLeft = false;
        }
        else
        {
            _bouncesLeft = true;
        }

        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        _animator.SetTrigger("Bounce");

        _playerParent = collider.transform.parent;

        if (_playerParent.TryGetComponent(out ChangeDirection changeDirection))
        {
            if (_bouncesLeft == false)
            {
                changeDirection.GoRight();
            }
            else
                changeDirection.GoLeft();
        }
        if (_playerParent.TryGetComponent(out ChangeDirection_Updated changeDirectionU))
        {
            if (_bouncesLeft == false)
            {
                changeDirectionU.GoRight();
            }
            else
                changeDirectionU.GoLeft();
        }
    }
}
