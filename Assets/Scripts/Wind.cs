using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    private GameObject playerRoot;
    private Transform _playerParent;

    private ChangeDirection _changeDirection;
    private ChangeDirection_Updated _changeDirectionU;


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
        else
        {
            _changeDirectionU.speed = 10;

            yield return new WaitForSeconds(0.5f);
            _changeDirectionU.speed = 8;

            yield return new WaitForSeconds(0.2f);
            _changeDirectionU.speed = 7;
        }
    }
}
