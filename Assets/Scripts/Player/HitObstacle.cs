using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObstacle : MonoBehaviour
{
    private GameManager _gameManager;

    private Rigidbody2D _rigidbody;
    private Collider2D _collider;

    private FollowFinger _followFinger;
    private BimControllerFloating _bimFloating;

    private MoveDirection _moveDirection;
    private Vector3 _bimLocalScale;

    [HideInInspector]
    public bool IsImmune;

    [SerializeField]
    private float _immunityTime;
    //[SerializeField]
    //private AudioClip[] _soundEffectsGiantBim;

    private AudioSource _audioSource;
    private Animator _animator;

    [SerializeField]
    private GameObject _particlePrefabDestruction, _particlePrefabHitCannonball;


    private void Start()
    {
        if (GetComponent<FollowFinger>() != null)
        {
            _followFinger = GetComponent<FollowFinger>();
        }
        else
        {
            _bimFloating = GetComponent<BimControllerFloating>();
        }

        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();

        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _moveDirection = GetComponentInParent<MoveDirection>();

        _gameManager = FindObjectOfType<GameManager>();

        _bimLocalScale = _rigidbody.transform.localScale;

        IsImmune = false;
    }

    // check for tags with projectile, and rotating obstacle (rotating needs to phase through, so make that a trigger, same for the cannon)

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Block")
        {
            if (_followFinger != null) // if using followfinger ...
            {
                if (_followFinger.MegaBimActive == true && collision.gameObject.tag != "DestructibleObstacle") // IF GIGANTIC && not a destroyable obstacle (obstacles that evaporate on hit)
                {
                    // disable the collider(s) on the object
                    var colls = collision.gameObject.GetComponents<Collider2D>();
                    foreach (var col in colls)
                    {
                        col.enabled = false;
                    }
                    //collision.collider.enabled = false;
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
                        if (collision.transform.GetComponentInChildren<SpawnCannonBall>() != null) // if it has the script...
                        {
                            Destroy(collision.transform.GetComponentInChildren<SpawnCannonBall>()); // destroy it
                        }
                        
                    }

                    // !! always have a checkpoint right after he turns small (otherwise i need code to respawn the level pieces) !!
                    // add lifespan to the pieces (they get destroyed after 3 seconds or so)
                    Destroy(collision.gameObject, 3f);

                    // play a sound effect effect ( ideally probably an object pool of instantiated particles with their own hit sounds, but that would be for later //
                    //var random = UnityEngine.Random.Range(0,_soundEffectsGiantBim.Length);
                    //_audioSource.PlayOneShot(_soundEffectsGiantBim[random]);
                    if (collision.gameObject.GetComponent<CanonBall_Projectile>() != null)
                    {
                        // add cannonball bounce particle
                        Instantiate(_particlePrefabHitCannonball, collision.contacts[0].point, Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(_particlePrefabDestruction, collision.contacts[0].point, Quaternion.identity);
                    }
                    


                }
                else // IF NOT GIGANTIC
                {
                    if (IsImmune == false)  // only take damage when not immune and when the collision is forward
                    {
                        //Debug.Log(collision.gameObject.name + " is the hit obstacle");

                        if (transform.localScale.x > 0 && collision.contacts[0].normal.normalized.x <= -0.78f ||
                            transform.localScale.x < 0 && collision.contacts[0].normal.normalized.x >= 0.78f)
                        {
                            // check bounce direction
                            CheckReboundDirectionFollowFinger(collision);
                            // become immune
                            StartCoroutine(GainImmunity(_immunityTime));
                            // instantiate object with sound effect and particle
                            _followFinger.HitWallFeedback(collision.contacts[0].point);
                            // lose health
                            _gameManager.HealthBiM -= 1;
                            // update sprites
                            _gameManager.UpdateHUDHealth();
                            // if health == 0  -> checkpoint
                            if (_gameManager.HealthBiM <= 0)
                            {
                                _animator.Play("BimDies");

                                _followFinger.enabled = false;

                                _collider.enabled = false;
                                _rigidbody.gravityScale = 1;
                                _rigidbody.constraints = RigidbodyConstraints2D.None;

                                _gameManager.StartCoroutine(_gameManager.RespawnLatestPoint());
                            }
                        }

                    }


                    // logic for bouncing of when colliding with terrain
                    //  check for the normal of the collision ... (right,left,down,up) //
                    //CheckReboundDirectionFollowFinger(collision);
                }
            }
            else // else if using floaty ...
            {
                CheckReboundDirectionFloating(collision);
            }       
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //Debug.Log(collision.contacts.Length + " number of contacts");
        //Debug.Log(collision.contacts[0].normal.normalized + " normal");

        if (collision.gameObject.tag != "Block")
        {
            if (_followFinger != null) // if using followfinger ...
            {
                if (_followFinger.MegaBimActive == false)
                {
                    if (IsImmune == false)  // only take damage when not immune and when the collision is forward
                    {
                        //Debug.Log(collision.gameObject.name + " is the hit obstacle");

                        if (transform.localScale.x > 0 && collision.contacts[0].normal.normalized.x <= -0.78f ||
                            transform.localScale.x < 0 && collision.contacts[0].normal.normalized.x >= 0.78f)
                        {
                            // check bounce direction
                            CheckReboundDirectionFollowFinger(collision);
                            // become immune
                            StartCoroutine(GainImmunity(_immunityTime));
                            // instantiate object with sound effect and particle
                            _followFinger.HitWallFeedback(collision.contacts[0].point);
                            // lose health
                            _gameManager.HealthBiM -= 1;
                            // update sprites
                            _gameManager.UpdateHUDHealth();
                            // if health == 0  -> checkpoint
                            if (_gameManager.HealthBiM <= 0)
                            {
                                _animator.Play("BimDies");

                                _followFinger.enabled = false;

                                _collider.enabled = false;
                                _rigidbody.gravityScale = 1;
                                _rigidbody.constraints = RigidbodyConstraints2D.None;

                                _gameManager.StartCoroutine(_gameManager.RespawnLatestPoint());
                            }
                        }
                    }
                }
            }
        }
    }






    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11 || collision.gameObject.layer == 10)  // if I hit a rotating obstacle or projectile (using layer index)
        {
            if (IsImmune == false)  // only take damage when not immune and when the collision is forward
            {
                //Debug.Log(collision.name + " is the hit obstacle");
                // become immune
                StartCoroutine(GainImmunity(_immunityTime));
                // instantiate object with sound effect and particle
                _followFinger.HitWallFeedback(transform.position);
                // lose health
                _gameManager.HealthBiM -= 1;


                //// rotate
                //if (_bimLocalScale.x >= 0f)
                //{
                //    StartCoroutine(RotateBim(_immunityTime, true));
                //}
                //else
                //{
                //    StartCoroutine(RotateBim(_immunityTime, false));
                //}
                _animator.Play("BimHitWall");                

                // update sprites
                _gameManager.UpdateHUDHealth();
                // if health == 0  -> checkpoint
                if (_gameManager.HealthBiM <= 0)
                {
                    _animator.Play("BimDies");

                    _followFinger.enabled = false;

                    _collider.enabled = false;
                    _rigidbody.gravityScale = 1;
                    _rigidbody.constraints = RigidbodyConstraints2D.None;

                    _gameManager.StartCoroutine(_gameManager.RespawnLatestPoint());
                }               
            }
        }
    }





    private void CheckReboundDirectionFollowFinger(Collision2D collision)
    {
        //Debug.Log(collision.contacts[0].normal.normalized.x + " is the normal X normalized");
        //Debug.Log(collision.contacts[0].normal.normalized.y + " is the normal Y normalized");

        if (collision.contacts[0].normal.normalized.x <= -0.3f)  // bounce backwards with moveDirection script
        {
            // 0) lose control (maybe not this)
            _followFinger.TurnOffControl(_immunityTime / 4f, true, false, false);

            // 1) activate a bool on the player (this bool will slowly increase the speed up until the original level speed)
            // check for bims local scale to figure out bounce direction
            _bimLocalScale = _rigidbody.transform.localScale;
            if (_bimLocalScale.x > 0)
            {
                _moveDirection.BouncedBack = true;
            }
            else
            {
                _moveDirection.BouncedForward = true;
            }

            ////rotate
            //if (_gameManager.HealthBiM <= 0)
            //{
            //    StartCoroutine(RotateBimDead(_immunityTime, true));
            //}
            //else
            //{
            //    StartCoroutine(RotateBim(_immunityTime, true));
            //}
            _animator.Play("BimHitWall");

            // 2) reverse the speed
            _moveDirection.Speed = _moveDirection.Speed * -1;
        }
        else if (collision.contacts[0].normal.normalized.x >= 0.3f)    // bounce forwards 
        {
            _followFinger.TurnOffControl(_immunityTime / 4f, true, false, false);

            _bimLocalScale = _rigidbody.transform.localScale;
            if (_bimLocalScale.x > 0)
            {
                _moveDirection.BouncedBack = true;
            }
            else
            {
                _moveDirection.BouncedForward = true;
            }

            ////rotate           
            //if (_gameManager.HealthBiM <= 0)
            //{
            //    StartCoroutine(RotateBimDead(_immunityTime, false));
            //}
            //else
            //{
            //    StartCoroutine(RotateBim(_immunityTime, false));
            //}
            _animator.Play("BimHitWall");


            _moveDirection.Speed = _moveDirection.Speed * -1;
        }
        else if (collision.contacts[0].normal.normalized.y <= -0.2f) // bounce down with rigidbody force      // DATED CODE BELOW AS THIS IS NOT RELEVANT ANYMORE //
        {
            _moveDirection.BounceTimer = 0;
            _moveDirection.BouncedVertically = true;

            _bimLocalScale = _rigidbody.transform.localScale;
            if (_bimLocalScale.x > 0) // if BiM was moving right, push him right (it just a slowdown)
            {
                _moveDirection.Speed = 2;
            }
            else
            {
                _moveDirection.Speed = -2;
            }

            //StartCoroutine(LostControl(_immunityTime / 4f));
            _followFinger.TurnOffControl(_immunityTime / 4f, true, false, false);

            //_followFinger.GetComponent<Rigidbody2D>().AddForce(-Vector2.up * 25);
            _followFinger.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            _followFinger.GetComponent<Rigidbody2D>().velocity = -Vector2.up * 2;
        }
        else if (collision.contacts[0].normal.normalized.y >= 0.2f) // bounce up with rigidbody force 
        {
            _moveDirection.BounceTimer = 0;
            _moveDirection.BouncedVertically = true;

            _bimLocalScale = _rigidbody.transform.localScale;
            if (_bimLocalScale.x > 0) // if BiM was moving right, push him right (it just a slowdown)
            {
                _moveDirection.Speed = 2;
            }
            else
            {
                _moveDirection.Speed = -2;
            }

            //StartCoroutine(LostControl(_immunityTime / 4f));
            _followFinger.TurnOffControl(_immunityTime / 4f, true, false, false);

            //_followFinger.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 25);
            _followFinger.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            _followFinger.GetComponent<Rigidbody2D>().velocity = Vector2.up * 2;
        }
    }
    private void CheckReboundDirectionFloating(Collision2D collision)
    {
        //Debug.Log(collision.contacts[0].normal.normalized.x + " is the normal X normalized");
        //Debug.Log(collision.contacts[0].normal.normalized.y + " is the normal Y normalized");

        if (collision.gameObject.tag == "Floor" || collision.contacts[0].normal.normalized.y >= 0.2f)
        {
            _moveDirection.BounceTimer = 0;
            _moveDirection.BouncedVertically = true;

            _bimLocalScale = _rigidbody.transform.localScale;
            if (_bimLocalScale.x > 0) // if BiM was moving right, push him right (it just a slowdown)
            {
                _moveDirection.Speed = 2;
            }
            else
            {
                _moveDirection.Speed = -2;
            }

            //StartCoroutine(LostControl(_immunityTime / 4f));
            _bimFloating.TurnOffControl(_immunityTime / 4f, true, false);

            //_followFinger.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 25);
            _bimFloating.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            _bimFloating.GetComponent<Rigidbody2D>().velocity = Vector2.up * 5;
        }
        else if (collision.gameObject.tag == "Ceiling" || collision.contacts[0].normal.normalized.y <= -0.2f) // bounce down with rigidbody force 
        {
            //_moveDirection.BounceTimer = 0;
            //_moveDirection.BouncedVertically = true;

            //_bimLocalScale = _rigidbody.transform.localScale;
            //if (_bimLocalScale.x > 0) // if BiM was moving right, push him right (it just a slowdown)
            //{
            //    _moveDirection.Speed = 2;
            //}
            //else
            //{
            //    _moveDirection.Speed = -2;
            //}

            ////StartCoroutine(LostControl(_immunityTime / 4f));
            //_bimFloating.TurnOffControl(_immunityTime / 4f, true, false);

            ////_followFinger.GetComponent<Rigidbody2D>().AddForce(-Vector2.up * 25);
            //_bimFloating.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            //_bimFloating.GetComponent<Rigidbody2D>().velocity = -Vector2.up * 2;
        }
        else if (collision.contacts[0].normal.normalized.x <= -0.3f)  // bounce backwards with moveDirection script
        {
            // 0) lose control (maybe not this)
            _bimFloating.TurnOffControl(_immunityTime / 4f, true, false);

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
            _bimFloating.TurnOffControl(_immunityTime / 4f, true, false);

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




    IEnumerator RotateBim(float lostControlTimer, bool movingForward)
    {
        _rigidbody.freezeRotation = false;

        if (movingForward)
        {
            _rigidbody.AddTorque(12);
        }
        else
        {
            _rigidbody.AddTorque(-12);
        }

        yield return new WaitForSeconds(lostControlTimer/2f);

        _rigidbody.rotation = 0;
        _rigidbody.freezeRotation = true;
    }

    IEnumerator RotateBimDead(float lostControlTimer, bool movingForward)
    {
        _rigidbody.freezeRotation = false;

        if (movingForward)
        {
            _rigidbody.AddTorque(18);
        }
        else
        {
            _rigidbody.AddTorque(-18);
        }
        

        yield return new WaitForSeconds(lostControlTimer);

        _rigidbody.rotation = 0;
        _rigidbody.freezeRotation = true;
    }
}
