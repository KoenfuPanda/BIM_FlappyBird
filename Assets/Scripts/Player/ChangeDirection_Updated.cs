using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDirection_Updated : MonoBehaviour
{
    // PRIVATE


    private GameObject bimPivot;
    private GameObject mainCamera;
    public float speed;

    private bool leftDirection = false;
    private bool rightDirection = false;

    private float timeElapsed;
    private float lerpDuration = 1;

    private Vector3 startCam;
    private Vector3 endCam;


    void Start()
    {
        speed = GetComponent<MoveDirection>().Speed;

        bimPivot = GetComponentInChildren<Rigidbody2D>().gameObject;
        mainCamera = GetComponentInChildren<Camera>().gameObject;

        startCam = mainCamera.transform.localPosition;
        endCam = new Vector3(mainCamera.transform.localPosition.x - 9, mainCamera.transform.localPosition.y, mainCamera.transform.localPosition.z);
    }

    void Update()
    {
        GetComponent<MoveDirection>().Speed = speed;

        if (timeElapsed < lerpDuration && leftDirection)
        {
            mainCamera.transform.localPosition = Vector3.Lerp(startCam, endCam, timeElapsed / lerpDuration);

            timeElapsed += Time.deltaTime;
        }
        else if (leftDirection)
        {
            timeElapsed = 0;
            leftDirection = false;
        }

        if (timeElapsed < lerpDuration && rightDirection)
        {
            mainCamera.transform.localPosition = Vector3.Lerp(endCam, startCam, timeElapsed / lerpDuration);

            timeElapsed += Time.deltaTime;
        }
        else if (rightDirection)
        {
            timeElapsed = 0;
            rightDirection = false;
        }
    }

    public void GoRight()
    {
        timeElapsed = 0;
        leftDirection = false;

        rightDirection = true;
        speed = 6;
        bimPivot.transform.localScale = new Vector3(1, 1, 1);
    }

    public void GoLeft()
    {
        timeElapsed = 0;
        rightDirection = false;

        leftDirection = true;
        speed = -6;
        bimPivot.transform.localScale = new Vector3(-1, 1, 1);

    }
}
