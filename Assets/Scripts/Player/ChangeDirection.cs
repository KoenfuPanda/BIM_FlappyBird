using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDirection : MonoBehaviour
{
    // PRIVATE


    private GameObject bimPivot;
    private GameObject mainCamera;
    public float speed;

    private bool leftDirection = false;
    private bool rightDirection = false;

    private float timeElapsed;
    private float lerpDuration = 0.5f;

    private Vector3 startCam;
    private Vector3 endCam;


    void Start()
    {
        speed = GetComponent<MoveDirection>().Speed;

        bimPivot = transform.Find("Bim_Pivot").gameObject;
        mainCamera = transform.Find("Main Camera").gameObject;

        startCam = mainCamera.transform.localPosition;
        endCam = new Vector3(mainCamera.transform.localPosition.x - 9, mainCamera.transform.localPosition.y, mainCamera.transform.localPosition.z);
    }

    void Update()
    {
        //GetComponent<MoveDirection>().speed = speed;

        if (timeElapsed < lerpDuration && leftDirection)
        {
            mainCamera.transform.localPosition = Vector3.Lerp(startCam, endCam, timeElapsed / lerpDuration);

            timeElapsed += Time.deltaTime;
        }
        else if(leftDirection)
        {
            timeElapsed = 0;
            leftDirection = false;
        }

        if (timeElapsed < lerpDuration && rightDirection)
        {
            mainCamera.transform.localPosition = Vector3.Lerp(endCam, startCam, timeElapsed / lerpDuration);

            timeElapsed += Time.deltaTime;
        }
        else if(rightDirection)
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
        //speed = 6;
        GetComponent<MoveDirection>().BouncedBack = false;
        GetComponent<MoveDirection>().BouncedForward = false;
        GetComponent<MoveDirection>().Speed = GetComponent<MoveDirection>().IntendedLevelSpeed;

        bimPivot.transform.localScale = new Vector3(-bimPivot.transform.localScale.x, bimPivot.transform.localScale.y, bimPivot.transform.localScale.z);
    }

    public void GoLeft()
    {
        timeElapsed = 0;
        rightDirection = false;

        leftDirection = true;
        //speed = -6;
        GetComponent<MoveDirection>().BouncedBack = false;
        GetComponent<MoveDirection>().BouncedForward = false;
        GetComponent<MoveDirection>().Speed = -GetComponent<MoveDirection>().IntendedLevelSpeed;
        bimPivot.transform.localScale = new Vector3(-bimPivot.transform.localScale.x, bimPivot.transform.localScale.y, bimPivot.transform.localScale.z);

    }
}