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
    private float _transparencyValue;


    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        _transparencyValue = 0;
    }

    void Update()
    {
        // when holding down, draw line between bim and finger (ramp up the transparency up to goal)
        if (_followFinger.HoldingDown == true)
        {
            _transparencyValue += Time.deltaTime * 0.7f;
            if (_transparencyValue >= 1)
            {
                _transparencyValue = 1;
            }
        }
        else
        {            
            _transparencyValue -= Time.deltaTime * 1.2f;
            if (_transparencyValue <= 0)
            {
                _transparencyValue = 0;
            }
        }
        // (when tapped, draw line with goal transparency and then slowly lose transparency)


        // if target position.x is smaller/equal to Bim.x  OR  control character is disabled  =>  set transparency to 0
        if (_followFinger.TargetPosition.x <= _followFinger.transform.position.x || FollowFinger.controlCharacter == false)
        {
            _transparencyValue = 0;
        }

        _colorStart = new Color(1, 0, 0, _transparencyValue);
        _colorEnd = new Color(0, 0, 1, _transparencyValue);
        _lineRenderer.startColor = _colorStart;
        _lineRenderer.endColor = _colorEnd;

        Vector3 bim = _followFinger.transform.position;
        Vector3 target = _followFinger.TargetPosition;

        _lineRenderer.positionCount = 2;

        _lineRenderer.SetPosition(0, bim);
        _lineRenderer.SetPosition(1, target);


        Debug.Log(_transparencyValue);
    }    
}
