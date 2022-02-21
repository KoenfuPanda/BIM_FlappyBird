using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObstacle : MonoBehaviour
{
    private GameManager _gameManager;

    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private FollowFinger _followFinger;

    private bool _isImmune;

    private MoveDirection _moveDirection;

    [SerializeField]
    private float _immunityTime;



    private void Start()
    {
        _followFinger = GetComponent<FollowFinger>();

        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _moveDirection = GetComponentInParent<MoveDirection>();

        _gameManager = FindObjectOfType<GameManager>();      
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Block")
        {
            if(_isImmune == false)
            {
                // become immune
                StartCoroutine(GainImmunity(_immunityTime));
                // lose health
                _gameManager.HealthBiM -= 1;
                // update sprites
                _gameManager.UpdateHUDHealth();
                // if health == 0  -> checkpoint
                if (_gameManager.HealthBiM <= 0)
                {
                    _followFinger.enabled = false;

                    _collider.enabled = false;
                    _rigidbody.gravityScale = 1;
                    _rigidbody.constraints = RigidbodyConstraints2D.None;

                    _gameManager.StartCoroutine(_gameManager.RespawnLatestPoint());
                }
            }

            Debug.Log(collision.contacts[0].normal.normalized.x + " is the normal X normalized");
            Debug.Log(collision.contacts[0].normal.normalized.y + " is the normal Y normalized");



            //  check for the normal of the collision ... //
            if (collision.contacts[0].normal.normalized.x <= -0.3f)  // bounce backwards with moveDirection script
            {
                // 0) lose control (maybe not this)
                //StartCoroutine(LostControl(_immunityTime / 2f));

                // 1) reverse the speed
                _moveDirection.Speed = _moveDirection.Speed * -1;

                // 2) activate a bool on the player (this bool will slowly increase the speed up until the original level speed)
                _moveDirection.BouncedBack = true;
            }
            else if (collision.contacts[0].normal.normalized.y <= -0.2f) // bounce down with rigidbody force 
            {
                _moveDirection.BouncedVertically = true;                
                _moveDirection.Speed = 2;

                StartCoroutine(LostControl(_immunityTime / 4f));

                _followFinger.GetComponent<Rigidbody2D>().AddForce(-Vector2.up * 25);
            }
            else if (collision.contacts[0].normal.normalized.y >= 0.2f) // bounce up with rigidbody force 
            {
                _moveDirection.BouncedVertically = true;
                _moveDirection.Speed = 2;

                StartCoroutine(LostControl(_immunityTime / 4f));

                _followFinger.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 25);
            }
        }
    }

    private void LoseControl()
    {
        // checks what control scheme is used and disables it
        if (TryGetComponent(out FollowFinger followFingerScript))
        {
            followFingerScript.enabled = false;
        }
        if (TryGetComponent(out FollowFingerXY followFingerScriptXY))
        {
            followFingerScriptXY.enabled = false;
        }
        if (GetComponentInParent<BimControllerFloating>() != null)
        {
            GetComponentInParent<BimControllerFloating>().enabled = false;
        }
    }

    IEnumerator GainImmunity(float immuneTimer)
    {
        _isImmune = true;

        yield return new WaitForSeconds (immuneTimer);

        _isImmune = false;
    }

    IEnumerator LostControl(float lostControlTimer)
    {
        _followFinger.enabled = false;

        yield return new WaitForSeconds(lostControlTimer);

        _followFinger.enabled = true;
    }
}
