using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherScore : MonoBehaviour
{
    public static int Feathers = 0;

    void Start()
    {
        Feathers = 0;
    }

    //void Update()
    //{
    //    GetComponent<UnityEngine.UI.Text>().text = Feathers.ToString();
    //}
}
