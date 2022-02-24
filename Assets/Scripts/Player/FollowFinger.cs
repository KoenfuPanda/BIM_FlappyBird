using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using System;

public class FollowFinger : MonoBehaviour
{
    // PRIVATE

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



    void Start()
    {
        _hitObstacle = GetComponent<HitObstacle>();
        bim = transform.GetChild(0).gameObject;
        rigidBody = GetComponent<Rigidbody2D>();
        controlCharacter = true;
    }

    void FixedUpdate()
    {
        // Calculate Velocity

        mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mouseWorldPosition.y < transform.position.y - 0.1f)
        {
            velocity = (Mathf.Pow(2, velocityMultiplier))*-1;
            if (velocity > -7)
            {
                velocityMultiplier += 0.2f;
            }
        }
        else if (mouseWorldPosition.y > transform.position.y + 0.1f)
        {
            velocity = Mathf.Pow(2, velocityMultiplier);
            if (velocity < 7)
            {
                velocityMultiplier += 0.2f;
            }
        }
        else
        {
            velocityMultiplier = 0;
            velocity = 0;
        }

        // Move to
        if(moveTo)
        {
            t += Time.deltaTime / travelTime;
            transform.position = Vector3.Lerp(startPosition, target, t);

            if(transform.position == target)
            {
                follow = true;
                moveTo = false;
            }
        }
    }

    void Update()
    {
        if (controlCharacter == false)
        {
            _lostControlTimer -= Time.deltaTime;

            if (_lostControlTimer <= 0)
            {
                TurnOnControl();
                _lostControlTimer = 0;
            }
        }


        // Mouse button down
        if (Input.GetMouseButton(0) && controlCharacter)
        {
            rigidBody.velocity = Vector2.up * velocity;
        }
        else if (!bounce)
        {
            velocity = 0;
            rigidBody.velocity = Vector2.zero;
        }


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
        rigidBody.gravityScale = 0;
        _lostControlTimer = 0;
    }
    public void TurnOffControl(float timeLostControl, bool shouldOverrideTimerNoMatterWhat, bool wantsGravity)
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