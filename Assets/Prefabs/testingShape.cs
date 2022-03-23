using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class testingShape : MonoBehaviour
{
    public SpriteShapeController _shaper;

    private void Start()
    {
        _shaper = GetComponent<SpriteShapeController>();

    }

    private void Update()
    {
        _shaper.spline.GetPosition(0);
        _shaper.spline.GetPosition(1);


    }
}
