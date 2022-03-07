using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using System;

public class FollowFinger : MonoBehaviour
{
    // PRIVATE

    private Animator _animator;
    private GameObject bim;
    private static Rigidbody2D rigidBody;
    private Vector3 mouseWorldPosition;
    private bool bounce = false;
    private float velocityMultiplier = 0;

    private float _lostControlTimer;
    private bool _onClickableSurface;

    // Move to destination
    private bool moveTo = false;
    private float t;
    private Vector3 startPosition;
    private Vector3 target;
    private float timeToReachTarget;


    // PUBLIC
    
    public static bool controlCharacter = true;
    public float velocity = 0;


    // follow path
    private bool follow = false;
    private PathCreator pathCreator;
    private float distanceTravelled;
    private float pathSpeed;
    private Vector3 endPosition;
    public float travelTime;

    private HitObstacle _hitObstacle;
    [HideInInspector]
    public float MegaBimTimeLimit;
    [HideInInspector]
    public bool MegaBimActive;
    private float _megaBimTimer;

    public bool HoldingDown;
    public Vector2 TargetPosition;
    private Vector2 _movementDirection;
    private float _timerT;

    [SerializeField]
    private GameObject _getHitParticlePrefab;
    private bool _tookDamage, _recoveringToNormal;
    private float _damagedTimer;

    private LineAssistant _lineAssitsant;
    private Transform _parentObject;


    void Start()
    {
        _hitObstacle = GetComponent<HitObstacle>();
        bim = transform.GetChild(0).gameObject;
        _animator = bim.GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        controlCharacter = true;

        _parentObject = GetComponentInParent<MoveDirection>().transform;
        _lineAssitsant = _parentObject.GetComponentInChildren<LineAssistant>();
    }

    void FixedUpdate()
    {
        /////////
        // Calculate Velocity

        if (HoldingDown == true)
        {
            mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            TargetPosition = mouseWorldPosition;
        }


        if (mouseWorldPosition.y < transform.position.y - 0.1f)
        {
            velocity = (Mathf.Pow(2, velocityMultiplier)) * -1;  // creates negative velocity
            if (velocity > -7)
            {
                velocityMultiplier += 0.5f;
            }
            //velocity = -8;
        }
        else if (mouseWorldPosition.y > transform.position.y + 0.1f)
        {
            velocity = Mathf.Pow(2, velocityMultiplier);
            if (velocity < 7)
            {
                velocityMultiplier += 0.5f;
            }
            //velocity = 8;
        }
        else
        {
            velocityMultiplier = 0;
            velocity = 0;
        }

        // update the velocity calculater to be more efficient //
        //if (TargetPosition.y < transform.position.y - 0.25f)
        //{
        //    velocity = -6;
        //}
        //else if (TargetPosition.y > transform.position.y + 0.25f)
        //{
        //    velocity = 6;
        //}
        //else
        //{
        //    velocityMultiplier = 0;
        //    velocity = 0;
        //}
        //////////

        //// Move to
        //if(moveTo)
        //{
        //    t += Time.deltaTime / travelTime;
        //    transform.position = Vector3.Lerp(startPosition, target, t);

        //    if(transform.position == target)
        //    {
        //        follow = true;
        //        moveTo = false;
        //    }
        //}
    }

    void Update()
    {
        //if (mouseWorldPosition.y < transform.position.y - 0.1f)
        //{
        //    velocity = (Mathf.Pow(2, velocityMultiplier)) * -1;  // creates negative velocity
        //    if (velocity > -7)
        //    {
        //        velocityMultiplier += 0.2f;
        //    }
        //    //velocity = -8;
        //}
        //else if (mouseWorldPosition.y > transform.position.y + 0.1f)
        //{
        //    velocity = Mathf.Pow(2, velocityMultiplier);
        //    if (velocity < 7)
        //    {
        //        velocityMultiplier += 0.2f;
        //    }
        //    //velocity = 8;
        //}
        //else
        //{
        //    velocityMultiplier = 0;
        //    velocity = 0;
        //}

     
        if (controlCharacter == false)
        {
            _lostControlTimer -= Time.deltaTime;

            if (_lostControlTimer <= 0)
            {
                TurnOnControl();
                _lostControlTimer = 0;
            }
        }
        // if I can control BiM
        if (controlCharacter == true)  
        {
            // --- Mouse button tap ---
            if (Input.GetMouseButtonDown(0) && HoldingDown == false) // tapping
            {
                mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                TargetPosition = mouseWorldPosition;
               
                // set line transparency to 1
                _lineAssitsant.TransparencyValue = 1;

                // Get the target position  
                Vector2 dir = new Vector2(rigidBody.position.x, mouseWorldPosition.y) - rigidBody.position;
                // Get the velocity required to reach the target in the next frame
                dir /= Time.fixedDeltaTime;

                //Debug.Log(dir);

                // Clamp that to the max speed
                if (dir.y < 0) // if direction was downwards
                {
                    dir = -Vector2.ClampMagnitude(dir, -8);
                }
                else
                {
                    dir = Vector2.ClampMagnitude(dir, 8);
                }
                // Apply that to the rigidbody
                _movementDirection = dir;
                rigidBody.velocity = _movementDirection;

                //Debug.Log(_movementDirection);
            }

            // --- Mouse button down ---
            if (Input.GetMouseButton(0)) // finger held down
            {
                _timerT += Time.deltaTime;
                if (_timerT >= 0.15f) // held down with intent.
                {
                    HoldingDown = true;
                }
            }

            if (HoldingDown == true)
            {
                rigidBody.velocity = Vector2.up * velocity;

                //Debug.Log(rigidBody.velocity + " is the velocity");
            }
            // if I am not holding down, && at the desired Y, stop moving
            if (HoldingDown == false)
            {
                if (_movementDirection.y < 0 && rigidBody.position.y <= mouseWorldPosition.y) // if direction was downwards
                {
                    rigidBody.velocity = Vector2.zero;
                }
                else if (_movementDirection.y > 0 && rigidBody.position.y >= mouseWorldPosition.y) // if direction was downwards
                {
                    rigidBody.velocity = Vector2.zero;
                }
            }

            if (Input.GetMouseButtonUp(0)) // if let go
            {
                _timerT = 0;
                HoldingDown = false;
            }
        }
        //else if (bounce == false)
        //{
        //    velocity = 0;
        //    rigidBody.velocity = Vector2.zero;
        //}

        //// Mouse button down
        //if (Input.GetMouseButton(0) && controlCharacter) // finger held down
        //{
        //    _timerT += Time.deltaTime;
        //    if (_timerT >= 0.5f) // held down with intent.
        //    {
        //        _holdingDown = true;
        //    }
        //}
        //if (_holdingDown == true && controlCharacter == true)
        //{
        //    rigidBody.velocity = Vector2.up * velocity;
        //}
        //else if (!bounce)
        //{
        //    velocity = 0;
        //    rigidBody.velocity = Vector2.zero;
        //}

        //// Mouse button tap
        //if (Input.GetMouseButtonDown(0) && controlCharacter == true && _holdingDown == false) // tapping
        //{
        //    mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //    // Get the target position  
        //    Vector2 dir = new Vector2(rigidBody.position.x, mouseWorldPosition.y) - rigidBody.position;           
        //    // Get the velocity required to reach the target in the next frame
        //    dir /= Time.fixedDeltaTime;
        //    Debug.Log(dir);
        //    // Clamp that to the max speed
        //    if (dir.y < 0) // if direction was downwards
        //    {
        //        dir = -Vector2.ClampMagnitude(dir, -8);
        //    }
        //    else
        //    {
        //        dir = Vector2.ClampMagnitude(dir, 8);
        //    }
        //    // Apply that to the rigidbody
        //    dire = dir;
        //    Debug.Log(dire + " clamped");
        //    rigidBody.velocity = dire;
        //}

        //// if I am not holding down, && at the desired Y, stop moving
        //if(_holdingDown == false)
        //{
        //    if (dire.y < 0 && rigidBody.position.y <= mouseWorldPosition.y) // if direction was downwards
        //    {
        //        rigidBody.velocity = Vector2.zero;
        //    }
        //    else if (dire.y > 0 && rigidBody.position.y >= mouseWorldPosition.y) // if direction was downwards
        //    {
        //        rigidBody.velocity = Vector2.zero;
        //    }
        //}

        //if (_holdingDown == true && Input.GetMouseButtonUp(0)) // if let go
        //{
        //    _timerT = 0;
        //    _holdingDown = false;
        //}







        // When flying up or down
        BimRotationLogic();

        if (MegaBimActive == true)
        {
            _megaBimTimer += Time.deltaTime;

            if (_megaBimTimer >= MegaBimTimeLimit)
            {
                ReturnBimToNormal();
            }
        }

        if (_tookDamage == true)
        {
            _damagedTimer += Time.deltaTime;
            if(_damagedTimer > 0.75f)
            {
                _recoveringToNormal = true;
                _tookDamage = false;
                _damagedTimer = 0;
            }
        }
        else if (_recoveringToNormal == true)
        {
            _animator.speed += Time.deltaTime * 0.5f;
            if(_animator.speed >= 1)
            {
                _animator.speed = 1;
                _recoveringToNormal = false;
            }
        }



        // TEST
        //if (follow)
        //{
        //    distanceTravelled += pathSpeed * Time.deltaTime;
        //    transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);

        //    if (Mathf.Abs(transform.position.x - endPosition.x) < 0.2 && Mathf.Abs(transform.position.y - endPosition.y) < 0.2)
        //    {
        //        controlCharacter = true;
        //        follow = false;
        //    }
        //}
    }

    private void BimRotationLogic()
    {
        if (rigidBody.velocity.y > 0 && bim.transform.localRotation.z < 0.05f)
        {
            bim.transform.Rotate(0, 0, 1.5f);
        }
        else if (rigidBody.velocity.y < 0 && bim.transform.localRotation.z > -0.30f)
        {
            bim.transform.Rotate(0, 0, -1.5f);
        }
        else // When flying straight
        {
            if (bim.transform.localRotation.z > -0.15)
            {
                bim.transform.Rotate(0, 0, -1.5f);
            }
            else if (bim.transform.localRotation.z < -0.16)
            {
                bim.transform.Rotate(0, 0, +1.5f);
            }
        }
    }

    public void TurnOnControl()
    {
        bounce = false;
        controlCharacter = true;

        rigidBody.velocity = Vector3.zero;
        _movementDirection = Vector3.zero;
        HoldingDown = false;

        rigidBody.gravityScale = 0;
        _lostControlTimer = 0;
    }
    public void TurnOffControl(float timeLostControl, bool shouldOverrideTimerNoMatterWhat, bool wantsGravity, bool isFirstTutorial)
    {
        bounce = true;
        controlCharacter = false;

        if (wantsGravity == true)
        {
            rigidBody.gravityScale = 1;
        }

        // start coroutine to regain control
        //StartCoroutine(RegainControl(timeLostControl)); cgange this to update logic

        if (shouldOverrideTimerNoMatterWhat == false && timeLostControl > _lostControlTimer) // if statement for matrass bounces
        {
            _lostControlTimer = timeLostControl;
        }
        if (shouldOverrideTimerNoMatterWhat == true)
        {
            _lostControlTimer = timeLostControl;
        }

        if (isFirstTutorial == true)
        {
            rigidBody.velocity = Vector2.zero; // stop from descending too much in tutorial_1 as otherwise bim sinks too much down below
        }            
    }


    public void EnteredClickableSurface()
    {
        _lostControlTimer = 20; // just some big number ...
        controlCharacter = false;
        //_onClickableSurface = true;      
    }
    public void ExitedClickableSurface()
    {
        _lostControlTimer = 0;
        controlCharacter = true;
        //_onClickableSurface = false;
    }

    public void BimGrow(float upgradeTimeLimit)
    {
        _hitObstacle.ImmuneToggled();
        transform.localScale = new Vector3(3f, 3f, 3f); // make this gradualy increase
        MegaBimTimeLimit = upgradeTimeLimit;
        MegaBimActive = true;
    }
    public void ReturnBimToNormal()
    {
        transform.localScale = new Vector3(1f, 1f, 1f); // make this gradualy decrease
        MegaBimActive = false;
        _megaBimTimer = 0;
        _hitObstacle.ImmuneToggled();
    }





    public void HitWallFeedback(Vector3 hitPosition)
    {
        Instantiate(_getHitParticlePrefab, transform.position, Quaternion.Euler(-90,0,0));
        _animator.speed = 0.5f;

        _recoveringToNormal = false;
        _tookDamage = true;
    }






    private IEnumerator RegainControl(float timeLostControl)
    {
        yield return new WaitForSeconds(timeLostControl);

        TurnOnControl();
    }

    private void SetDestination(Vector3 destination)
    {
        t = 0;
        startPosition = transform.position;
        target = destination;
    }


    // TEST

    public void MoveToPosition(Vector3 destination, float time)
    {
        t = 0;
        startPosition = transform.position;
        target = destination;
    }

    public void FollowPath(PathCreator pathCreator_, float speed_, Vector3 endPosition_)
    {
        controlCharacter = false;
        pathCreator = pathCreator_;
        pathSpeed = speed_;
        endPosition = endPosition_;
        t = 0;
        startPosition = transform.position;
        target = pathCreator_.transform.position;
        travelTime = Vector3.Distance(startPosition, target) / 10;

        moveTo = true;
    }
}