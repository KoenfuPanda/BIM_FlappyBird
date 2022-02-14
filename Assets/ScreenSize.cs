using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSize : MonoBehaviour
{
    public GameObject text;
    private float aspectRatio;
    private float camSize;
    void Start()
    {
        aspectRatio = (Screen.height * 1.0f) / (Screen.width * 1.0f);
        camSize = Mathf.Lerp(3.78f, 5.43f, (aspectRatio- 0.75f) * 1.19f);
        GetComponent<Camera>().orthographicSize = camSize;
    }
}
