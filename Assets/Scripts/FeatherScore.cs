using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherScore : MonoBehaviour
{
    public static int feathers = 0;

    void Start()
    {
        feathers = 0;
    }

    void Update()
    {
        GetComponent<UnityEngine.UI.Text>().text = feathers.ToString();
    }
}
