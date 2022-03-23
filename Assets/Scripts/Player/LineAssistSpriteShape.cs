using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class LineAssistSpriteShape : MonoBehaviour
{
    private SpriteShapeController _shaper;

    [SerializeField]
    private FollowFinger _followFinger;

    private void Start()
    {
        _shaper = GetComponent<SpriteShapeController>();
    }

    private void Update()
    {
        //Vector3 bim = new Vector3(_followFinger.transform.position.x, _followFinger.transform.position.y, -4);
        Vector3 target = new Vector3(_followFinger.TargetPosition.x + 8, _followFinger.TargetPosition.y, -4);

        print(_followFinger.TargetPosition.x);

        //_shaper.spline.SetPosition(0, bim);
        _shaper.spline.SetPosition(1, target);
    }
}
