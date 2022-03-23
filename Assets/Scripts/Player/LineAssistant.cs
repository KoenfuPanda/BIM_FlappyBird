using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineAssistant : MonoBehaviour
{
    [SerializeField]
    private FollowFinger _followFinger;

    private LineRenderer _lineRenderer;


    private Color _colorStart;
    private Color _colorEnd;

    [HideInInspector]
    public float TransparencyValue;

    private float _lineDistance;
    private float _lineWidth, _lineMaxWidth;

    [SerializeField]
    private float _lineDistanceLimit;


    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        TransparencyValue = 0;

        _lineRenderer.numCapVertices = 3;
        _lineMaxWidth = 0.2f;
    }

    void Update()
    {
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

        // (when tapped, draw line with goal transparency and then slowly lose transparency)
        // (done in FollowFinger code)

        // if target position.x is smaller/equal to Bim.x  OR  control character is disabled  =>  set transparency to 0  (check for mirrored bim!)
        if (_followFinger.transform.localScale.x > 0 && _followFinger.TargetPosition.x <= _followFinger.transform.position.x || FollowFinger.controlCharacter == false)
        {
            TransparencyValue = 0;
        }
        else if (_followFinger.transform.localScale.x < 0 && _followFinger.TargetPosition.x >= _followFinger.transform.position.x)
        {
            TransparencyValue = 0;
        }


        _colorStart = new Color(1, 0, 0, TransparencyValue);
        _colorEnd = new Color(0, 0, 1, TransparencyValue);
        _lineRenderer.startColor = _colorStart;
        _lineRenderer.endColor = _colorEnd;

        Vector3 bim = _followFinger.transform.position;
        Vector3 target = _followFinger.TargetPosition;
   
        _lineRenderer.positionCount = 2;

        _lineDistance = (target - bim).magnitude;


        // if line distance is greater than x, change target position to being a target at y (y = target pos - excess limit)
        if (_lineDistance >= _lineDistanceLimit)
        {
            float extraDist = _lineDistance - _lineDistanceLimit;

            Vector3 targetV3 = _followFinger.TargetPosition;
            target = targetV3 - ((targetV3 - bim).normalized * extraDist);
        }

        _lineRenderer.SetPosition(0, bim);
        _lineRenderer.SetPosition(1, target);

       



        // the lower my distance, the thicker the line. 
        // if the line distance is smaller than 4 , max thichkness
        // if the line distance is greater than 4, start reducing thicness
        // check for very small widths that should not be allowed (cuzz it's ugly)

        if (_lineDistance <= 4)
        {
            _lineWidth = _lineMaxWidth;
        }
        else if (_lineDistance > 4)
        {
            _lineWidth = _lineMaxWidth - ((_lineDistance ) / 65);

            if (_lineWidth <= 0.07f)
            {
                _lineWidth = 0.07f;
            }
        }

        _lineRenderer.startWidth = _lineWidth;
        _lineRenderer.endWidth = _lineWidth;
    }    
}
