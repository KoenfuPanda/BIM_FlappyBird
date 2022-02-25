using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObstacle : MonoBehaviour
{
    private GameManager _gameManager;

    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private FollowFinger _followFinger;
    private MoveDirection _moveDirection;
    private Vector3 _bimLocalScale;

    [HideInInspector]
    public bool IsImmune;

    [SerializeField]
    private float _immunityTime;


    private void Start()
    {
        _followFinger = GetComponent<FollowFinger>();

        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _moveDirection = GetComponentInParent<MoveDirection>();

        _gameManager = FindObjectOfType<GameManager>();

        _bimLocalScale = _rigidbody.transform.localScale;

        IsImmune = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Block")
        {
            if (_followFinger.MegaBimActive == true)
            {
                // disable the collider on the object
                collision.collider.enabled = false;
                // add force forward/up to the object
                Vector2 randomForce = new Vector2(Random.Range(7, 13), Random.Range(6, 13));

                if (collision.gameObject.TryGetComponent(out Rigidbody2D rigidObject)) // if it has a rigidbody...
                {
                    rigidObject.gravityScale = 1;
                    rigidObject.velocity = randomForce;
                    rigidObject.AddTorque(Random.Range(-20, 20));
                }
                else
                {
                    var rigidObj = collision.gameObject.AddComponent<Rigidbody2D>(); // else, add a rigidbody
                    rigidObj.velocity = randomForce;
                    rigidObj.AddTorque(Random.Range(-20, 20));
                }

                if (collision.gameObject.layer == 9) // if it's a cannon
                {
                    Destroy(collision.transform.GetComponentInChildren<SpawnCannonBall>());
                }
                                             
                // !! always have a checkpoint right after he turns small (otherwise i need code to respawn the level pieces) !!
                // add lifespan to the pieces (they get destroyed after 3 seconds or so)
                Destroy(collision.gameObject, 3f);

            }
            else // if not Mega
            {
                if (IsImmune == false)
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

                //Debug.Log(collision.contacts[0].normal.normalized.x + " is the normal X normalized");
                //Debug.Log(collision.contacts[0].normal.normalized.y + " is the normal Y normalized");


                //  check for the normal of the collision ... (right,left,down,up) //
                if (collision.contacts[0].normal.normalized.x <= -0.3f)            // bounce backwards with moveDirection script
                {
                    // 0) lose control (maybe not this)
                    _followFinger.TurnOffControl(_immunityTime / 4f, true, false);

                    // 1) activate a bool on the player (this bool will slowly increase the speed up until the original level speed)
                    // check for bims local scale to figure out bounce direction
                    if (_bimLocalScale.x > 0)
                    {
                        _moveDirection.BouncedBack = true;
                    }
                    else
                    {
                        _moveDirection.BouncedForward = true;
                    }

                    // 2) reverse the speed
                    _moveDirection.Speed = _moveDirection.Speed * -1;
                }
                else if (collision.contacts[0].normal.normalized.x >= 0.3f)    // bounce forwards 
                {
                    _followFinger.TurnOffControl(_immunityTime / 4f, true, false);

                    if (_bimLocalScale.x > 0)
                    {
                        _moveDirection.BouncedBack = true;
                    }
                    else
                    {
                        _moveDirection.BouncedForward = true;
                    }

                    _moveDirection.Speed = _moveDirection.Speed * -1;
                }
                else if (collision.contacts[0].normal.normalized.y <= -0.2f) // bounce down with rigidbody force 
                {
                    _moveDirection.BouncedVertically = true;

                    if (_bimLocalScale.x > 0) // if BiM was moving left
                    {
                        _moveDirection.Speed = -2;
                    }
                    else
                    {
                        _moveDirection.Speed = 2;
                    }
                    
                    //StartCoroutine(LostControl(_immunityTime / 4f));
                    _followFinger.TurnOffControl(_immunityTime / 4f, true, false);

                    _followFinger.GetComponent<Rigidbody2D>().AddForce(-Vector2.up * 25);
                }
                else if (collision.contacts[0].normal.normalized.y >= 0.2f) // bounce up with rigidbody force 
                {
                    _moveDirection.BouncedVertically = true;

                    if (_bimLocalScale.x > 0) // if BiM was moving left
                    {
                        _moveDirection.Speed = -2;
                    }
                    else
                    {
                        _moveDirection.Speed = 2;
                    }

                    //StartCoroutine(LostControl(_immunityTime / 4f));
                    _followFinger.TurnOffControl(_immunityTime / 4f, true, false);

                    _followFinger.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 25);
                }
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
        IsImmune = true;

        yield return new WaitForSeconds (immuneTimer);

        IsImmune = false;
    }

    public void ImmuneToggled()
    {
        IsImmune = !IsImmune;
    }

    IEnumerator LostControl(float lostControlTimer)
    {
        _followFinger.enabled = false;

        yield return new WaitForSeconds(lostControlTimer);

        _followFinger.enabled = true;
    }
}
