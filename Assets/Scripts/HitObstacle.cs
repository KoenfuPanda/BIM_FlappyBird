using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObstacle : MonoBehaviour
{
    public GameManager gameManager;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Block")
        {
            GetComponent<Collider2D>().enabled = false;
            GetComponent<FollowFinger>().enabled = false;
            GetComponent<Rigidbody2D>().gravityScale = 1;
            gameManager.GameOver();
        }
    }
}
