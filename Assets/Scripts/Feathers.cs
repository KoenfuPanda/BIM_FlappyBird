using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feathers : MonoBehaviour
{
    // move to destination
    private bool followPlayer = false;
    private float t;
    private Vector3 startPosition;
    private Vector3 target;
    private float timeToReachTarget = 0.5f;
    private GameObject player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            FeatherScore.feathers++;
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (followPlayer)
        {
            target = player.transform.position;
            t += Time.deltaTime / timeToReachTarget;
            transform.position = Vector3.Lerp(startPosition, target, t);
        }
    }

    public void FollowPlayer(GameObject player_)
    {
        player = player_;
        startPosition = transform.position;
        followPlayer = true;
    }
}