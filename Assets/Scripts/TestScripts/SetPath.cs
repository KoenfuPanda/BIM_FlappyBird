using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class SetPath : MonoBehaviour
{
    public float pathSpeed;

    void OnTriggerEnter2D(Collider2D collider)
    {
        PathCreator pathCreator = GetComponent<PathCreator>();
        Vector3 endPoint = transform.position + (pathCreator.bezierPath[pathCreator.bezierPath.NumPoints - 1]);
        collider.gameObject.GetComponent<FollowFinger>().FollowPath(pathCreator, pathSpeed, endPoint);
    }
}
