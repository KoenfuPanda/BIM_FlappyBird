using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDirection : MonoBehaviour
{
    public float speed = 0;

    void Start()
    {
        StartCoroutine(DelayStart());
    }

    void Update()
    {
        // Move root to the right

        transform.Translate(new Vector3( speed * Time.deltaTime, 0 ,0 ));
    }

    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(2);

        if(TryGetComponent(out ChangeDirection changeDirection))
        {
            changeDirection.enabled = true;
        }
        if (TryGetComponent(out ChangeDirection_Updated changeDirectionU))
        {
            changeDirectionU.enabled = true;
        }

        speed = 6;
    }
}
