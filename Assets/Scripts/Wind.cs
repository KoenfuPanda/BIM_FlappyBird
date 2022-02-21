using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    private GameObject playerRoot;
    private Transform _playerParent;

    private ChangeDirection _changeDirection;
    private ChangeDirection_Updated _changeDirectionU;
    private MoveDirection _moveDirection;

    private float _originalSpeed;


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.TryGetComponent(out HitObstacle hitObstacle))
        {
            playerRoot = hitObstacle.gameObject;
            _playerParent = playerRoot.transform.parent;

            if (_playerParent.TryGetComponent(out ChangeDirection changeDirection))
            {
                changeDirection.speed = 12;
                _changeDirection = changeDirection;
            }
            if (_playerParent.TryGetComponent(out ChangeDirection_Updated changeDirectionU))
            {
                changeDirectionU.speed = 12;
                _changeDirectionU = changeDirectionU;
            }
            if(_playerParent.GetComponentInParent<MoveDirection>() != null)
            {
                _moveDirection = _playerParent.GetComponentInParent<MoveDirection>();
                _originalSpeed = _moveDirection.Speed;
                _moveDirection.Speed = 12;
            }
        }      
    }


    private void OnTriggerExit2D(Collider2D collider)
    {
        StartCoroutine(SlowDown());
    }


    IEnumerator SlowDown()
    {
        if(_changeDirection != null)
        {
            _changeDirection.speed = 10;

            yield return new WaitForSeconds(0.5f);
            _changeDirection.speed = 8;

            yield return new WaitForSeconds(0.2f);
            _changeDirection.speed = 7;
        }
        else if (_changeDirectionU != null)
        {
            _changeDirectionU.speed = 10;

            yield return new WaitForSeconds(0.5f);
            _changeDirectionU.speed = 8;

            yield return new WaitForSeconds(0.2f);
            _changeDirectionU.speed = 7;
        }
        else
        {
            _moveDirection.Speed = 10;

            yield return new WaitForSeconds(0.5f);
            _moveDirection.Speed = 7;

            yield return new WaitForSeconds(0.2f);
            _moveDirection.Speed = _originalSpeed;
        }
    }
}
