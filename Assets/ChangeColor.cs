using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    private float colorVal = 0;

    void Update()
    {
        GetComponent<Camera>().backgroundColor = Color.Lerp(new Color32(49, 159, 205, 253), new Color32(152, 43, 36, 253), colorVal);

        if (Score.score > 80)
        {
            colorVal +=0.01f;
        }
    }
}
