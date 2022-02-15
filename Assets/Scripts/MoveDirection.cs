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

        // -- FOR FOLLOW BIM EVERYWHERE --
        // 1) make it so that the logic in here is only applied to the camera, not Bim
        // 2) make followfinger logic also be applied to x-axis mouse position
        // NOTES
        // - make Bim turn 180 degrees depending on the movement 5 not needed, but perhaps)
        // - proppelor ?!
        // - vertical matrass adjustment needed, add a constant speed forward (equal to cam moving speed)

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
