using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetDarker : MonoBehaviour
{
    private float alpha = 0;

    void FixedUpdate()
    {

        GetComponent<Image>().color = new Color32(12, 10, 58, (byte)alpha);

        
        if (alpha < 80 && Score.score > 80)
        {
            alpha += 0.1f;
        }
    }
}
