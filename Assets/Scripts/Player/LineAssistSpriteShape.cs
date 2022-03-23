using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class LineAssistSpriteShape : MonoBehaviour
{
    private SpriteShapeController _shaper;

    [SerializeField]
    private FollowFinger _followFinger;

    [SerializeField]
    private float _startOffset;

    [SerializeField]
    private float _lineDistanceLimit;
    private float _lineDistance;

    [HideInInspector]
    public float TransparencyValue;

    private float frameRate = 0;
    private Vector3 prevBim;
    private Vector3 newBim;
    private float interpolateRatio;
    private Vector3 bim;

    private void Start()
    {
        _shaper = GetComponent<SpriteShapeController>();
    }

    private void Update()
    {
        // Calculate transparancy of line
        if (_followFinger.HoldingDown == true)
        {
            TransparencyValue = 1;
        }
        else
        {
            TransparencyValue -= Time.deltaTime * 1.2f;
            if (TransparencyValue <= 0)
            {
                TransparencyValue = 0;
            }
        }

        // if target position.x is smaller/equal to Bim.x  OR  control character is disabled  =>  set transparency to 0  (check for mirrored bim!)
        if (_followFinger.transform.localScale.x > 0 && _followFinger.TargetPosition.x <= _followFinger.transform.position.x || FollowFinger.controlCharacter == false)
        {
            TransparencyValue = 0;
        }
        else if (_followFinger.transform.localScale.x < 0 && _followFinger.TargetPosition.x >= _followFinger.transform.position.x)
        {
            TransparencyValue = 0;
        }


        // Calculate position of line
        bim = new Vector3(_followFinger.transform.position.x + _startOffset, _followFinger.transform.position.y, 0);
        Vector3 target = new Vector3(_followFinger.TargetPosition.x, _followFinger.TargetPosition.y, 0);

        _lineDistance = (target - bim).magnitude;

        // if line distance is greater than x, change target position to being a target at y (y = target pos - excess limit)
        if (_lineDistance >= _lineDistanceLimit)
        {
            float extraDist = _lineDistance - _lineDistanceLimit;

            Vector3 targetV3 = _followFinger.TargetPosition;
            target = targetV3 - ((targetV3 - bim).normalized * extraDist);
        }

        // Set positions of line

        //_shaper.spline.SetPosition(0, bim);
        //_shaper.spline.SetPosition(1, target);

        _shaper.spline.SetPosition(0, Vector3.Lerp(prevBim, newBim, interpolateRatio));
        _shaper.spline.SetPosition(1, _shaper.spline.GetPosition(0) + new Vector3(5, 0, 0));

        if (interpolateRatio < 1)
        {
            interpolateRatio += 0.05f;
        }
        else
        {
            prevBim = newBim;
            newBim = bim;
            interpolateRatio = 0;
        }



        // Set transparancy of line
        GetComponent<SpriteShapeRenderer>().color = new Color(1, 1, 1, TransparencyValue);
    }

    private void FixedUpdate()
    {
        //if (frameRate < 1)
        //{
        //    frameRate++;
        //}
        //else
        //{
        //    prevBim = newBim;
        //    newBim = bim;
        //    interpolateRatio = 0;
        //    frameRate = 0;
        //}
    }
}
