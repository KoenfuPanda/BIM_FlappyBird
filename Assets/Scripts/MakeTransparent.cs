using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D;

public class MakeTransparent : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.5f);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);
    }
}
