using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMatras : MonoBehaviour
{
    private Animator _animator;

    private Rigidbody2D _bimRigid;
    private FollowFinger _followFinger;

    public enum Direction { Left, Right }
    public Direction direction;

    private bool _bouncesLeft;

    public enum BoostDirection {None, Up, Down}
    public BoostDirection VerticalBoostDirection;
    private bool _bounceNormal, _bounceUp, _bounceDown;
    [SerializeField]
    private float _bounceStrength;
    
    private Transform _playerParent;

    private float _timeLostControl = 0.6f;


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

        if(VerticalBoostDirection == BoostDirection.None)
        {
            _bounceNormal = true;
            _bounceUp = false;
            _bounceDown = false;
        }
        else if(VerticalBoostDirection == BoostDirection.Up)
        {
            _bounceNormal = false;
            _bounceUp = true;
            _bounceDown = false;
        }
        else
        {
            _bounceNormal = false;
            _bounceUp = false;
            _bounceDown = true;
        }

        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        _animator.SetTrigger("Bounce");

        _bimRigid = collider.GetComponent<Rigidbody2D>();
        _followFinger = collider.GetComponent<FollowFinger>();

        _playerParent = collider.transform.parent;
        if (_playerParent.TryGetComponent(out ChangeDirection changeDirection))
        {
            if (_bouncesLeft == false)
            {
                changeDirection.GoRight();
            }
            else
                changeDirection.GoLeft();

            if (_bounceUp)
            {
                BoostActivate(1);
            }
            else if (_bounceDown)
            {
                BoostActivate(2);
            }
            else
                BoostActivate(0);
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


    private void BoostActivate(int enumValue)
    {
        if(enumValue == 0) // None
        {
         // nothing special here
        }
        else if (enumValue == 1) // Up
        {
            _followFinger.TurnOffControl(_timeLostControl, true, true);
            _bimRigid.velocity = Vector2.up * _bounceStrength;          
        }
        else // Down
        {
            _followFinger.TurnOffControl(_timeLostControl, true, true);
            _bimRigid.gravityScale = -1;
            _bimRigid.velocity = -Vector2.up * _bounceStrength;
        }

        _animator.SetTrigger("Bounce");
    }
}
