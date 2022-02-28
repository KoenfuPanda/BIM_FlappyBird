using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToNormalSizeTrigger : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<FollowFinger>() != null)
        {
            if (collision.transform.localScale.x > 0)
            {
                collision.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                collision.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }
}
