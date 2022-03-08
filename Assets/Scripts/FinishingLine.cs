using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishingLine : MonoBehaviour
{
    private MoveDirection _moveDirection;
    private FollowFinger _followFinger;
    private Rigidbody2D _rigidBim;

    public GameObject EndScreen;
    public GameObject FX;

    [SerializeField] private int _levelNumber;

    private GameInstance _gameInstance;


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out FollowFinger followFinger)) // if it's the player...
        {
            _followFinger = followFinger;
            _rigidBim = _followFinger.GetComponent<Rigidbody2D>();
            _moveDirection = _followFinger.GetComponentInParent<MoveDirection>();

            // disable controls
            _followFinger.TurnOffControl(1000, true, false, false);
            // set the movedirection speed to 0
            //_moveDirection.Speed = 0;
            StartCoroutine(SlowdownSpeed());
            // unfreeze bim x, freeze other
            _rigidBim.constraints = RigidbodyConstraints2D.None;
            _rigidBim.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            // add velocity to x
            _rigidBim.velocity = Vector2.right * _moveDirection.IntendedLevelSpeed;


            EndScreen.SetActive(true);
            FX.SetActive(true);
            GetComponent<AudioSource>().Play();

            if (GameObject.Find("GameInstance(Clone)").GetComponent<GameInstance>() != null)
            {
                _gameInstance = GameObject.Find("GameInstance(Clone)").GetComponent<GameInstance>();
                _gameInstance.SetGameState(_levelNumber);
            }
        }

    }


    private IEnumerator SlowdownSpeed()
    {
        yield return new WaitForSeconds(0);
        _moveDirection.Speed = 4;

        yield return new WaitForSeconds(0.5f);
        _moveDirection.Speed = 3;

        yield return new WaitForSeconds(0.5f);
        _moveDirection.Speed = 2;

        yield return new WaitForSeconds(0.5f);
        _moveDirection.Speed = 1;

        yield return new WaitForSeconds(0.5f);
        _moveDirection.Speed = 0;

    }




}
