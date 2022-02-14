using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnControl : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            collider.gameObject.GetComponent<FollowFinger>().TurnOnControl();
        }
    }
}