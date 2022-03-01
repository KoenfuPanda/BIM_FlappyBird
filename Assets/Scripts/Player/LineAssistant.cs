using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineAssistant : MonoBehaviour
{
    [SerializeField]
    private FollowFinger _followFinger;

    private LineRenderer _lineRenderer;

    private float _transparancyTimer;


    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    void LateUpdate()
    {
        // when holding down, draw line between bim and finger (ramp up the transparency up to goal)
        //if (_followFinger.HoldingDown == true)
        //{

        //}

        // (when tapped, draw line with goal transparency and then slowly lose transparency)


        // if target position.x is smaller/equal to Bim.x, set transparency to 0




        Vector3 bim = _followFinger.transform.position;
        Vector3 target = _followFinger.TargetPosition;

        _lineRenderer.positionCount = 2;

        _lineRenderer.SetPosition(0, new Vector3(bim.x,bim.y, 1));
        _lineRenderer.SetPosition(1, new Vector3(target.x, target.y, 1));
    }
}
