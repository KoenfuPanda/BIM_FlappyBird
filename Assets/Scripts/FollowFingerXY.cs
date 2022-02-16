using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowFingerXY : MonoBehaviour
{
    // PRIVATE

    private GameObject bim;
    private static Rigidbody2D rigidBody;
    private Vector3 mouseWorldPosition;
    private Vector3 _adjustedMousePosition, _fingerOffset;
    private bool bounce = false;
    private float velocityMultiplierY = 0;
    private float velocityMultiplierX = 0;
    private float _velocityMultiplier = 0;



    // Move to destination
    private bool moveTo = false;
    private float t;
    private Vector3 startPosition;
    private Vector3 target;
    private float timeToReachTarget;


    // PUBLIC

    public static bool controlCharacter = true;
    public float velocityY = 0;
    public float velocityX = 0;
    public Vector2 VelocityXY = Vector2.zero;


    // follow path
    private bool follow = false;
    private float distanceTravelled;
    private float pathSpeed;
    private Vector3 endPosition;
    public float travelTime;

    void Start()
    {
        bim = transform.GetChild(0).gameObject;
        rigidBody = GetComponent<Rigidbody2D>();
        controlCharacter = true;

        _fingerOffset = Vector3.right * 0.45f;
    }

    void FixedUpdate()
    {
        // Calculate Velocity

        mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _adjustedMousePosition = mouseWorldPosition - _fingerOffset;    // small offset to the left so that finger does not block vision

        // y
        if (_adjustedMousePosition.y < transform.position.y - 0.1f)
        {
            velocityY = (Mathf.Pow(2, velocityMultiplierY)) * -1;
            if (velocityY > -7)
            {
                velocityMultiplierY += 0.2f;
            }
        }
        else if (_adjustedMousePosition.y > transform.position.y + 0.1f)
        {
            velocityY = Mathf.Pow(2, velocityMultiplierY);
            if (velocityY < 7)
            {
                velocityMultiplierY += 0.2f;
            }
        }
        else
        {
            velocityMultiplierY = 0;
            velocityY = 0;
        }

        // x  ...use magnitude instead ?
        if (_adjustedMousePosition.x < transform.position.x - 0.1f)
        {
            velocityX = (Mathf.Pow(2, velocityMultiplierX)) * -1;
            if (velocityX > -7)
            {
                velocityMultiplierX += 0.2f;
            }
        }
        else if (_adjustedMousePosition.x > transform.position.x + 0.1f)
        {
            velocityX = Mathf.Pow(2, velocityMultiplierX);
            if (velocityX < 7)
            {
                velocityMultiplierX += 0.2f;
            }
        }
        else
        {
            velocityMultiplierX = 0;
            velocityX = 0;
        }

        //// Move to
        //if (moveTo)
        //{
        //    t += Time.deltaTime / travelTime;
        //    transform.position = Vector3.Lerp(startPosition, target, t);

        //    if (transform.position == target)
        //    {
        //        follow = true;
        //        moveTo = false;
        //    }
        //}
    }

    void Update()
    {
        // Mouse button down

        if (Input.GetMouseButton(0) && controlCharacter)
        {
            rigidBody.velocity = Vector2.up * velocityY + Vector2.right * velocityX;
        }
        else if (!bounce)
        {
            velocityY = 0;
            rigidBody.velocity = Vector2.zero;
        }

        // Give direction


        // When flying up or down
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

    public void TurnOnControl()
    {
        bounce = false;
        controlCharacter = true;
        rigidBody.gravityScale = 0;
    }
    public void TurnOffControl(float timeLostControl)
    {
        bounce = true;
        controlCharacter = false;
        rigidBody.gravityScale = 1;

        // start coroutine to regain control
        StartCoroutine(RegainControl(timeLostControl));
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

    //public void FollowPath(PathCreator pathCreator_, float speed_, Vector3 endPosition_)
    //{
    //    controlCharacter = false;
    //    pathCreator = pathCreator_;
    //    pathSpeed = speed_;
    //    endPosition = endPosition_;
    //    t = 0;
    //    startPosition = transform.position;
    //    target = pathCreator_.transform.position;
    //    travelTime = Vector3.Distance(startPosition, target) / 10;

    //    moveTo = true;
    //}
}
