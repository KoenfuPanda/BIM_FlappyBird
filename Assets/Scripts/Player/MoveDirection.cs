using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDirection : MonoBehaviour
{
    public float Speed = 0;

    public float IntendedLevelSpeed;

    public bool GoDiagonalDown = false;

    [SerializeField]
    private Transform _startPoint, _endPoint;

    private Vector3 _calculatedDirection;

    public float ReboundSpeed;
    [HideInInspector]
    public bool BouncedBack, BouncedForward; // for horizontally made collisions
    [HideInInspector]
    public bool BouncedVertically; // for vertically made collisions (crashing into floor/ceiling)

    private float _bounceTimer;
    [SerializeField]
    private float _bounceTmeSpeedFrozen;

    private bool _returnToNormalSpeed;
    private float _returnSpeedModifier = 2;

    [HideInInspector]
    public float BoostedTimeLimit, BoostedTimer;
    private bool _boostedSpeed;

    void Start()
    {
        //StartCoroutine(DelayStart());

        if (_endPoint != null && _startPoint != null)
        {
            _calculatedDirection = _endPoint.position - _startPoint.position;
        }

        if (IntendedLevelSpeed != 0)
        {
            Speed = IntendedLevelSpeed;
        }
        else
        {
            Speed = 5;
        }
    }

    void Update()
    {
        if (_boostedSpeed == true)
        {
            BoostedTimer += Time.deltaTime;
            //BoostedTimer += Time.fixedDeltaTime;

            if (BoostedTimer >= BoostedTimeLimit)
            {
                // activate slowdown
                ReturnToNormalSpeed();
            }
        }

        HitWall();
        // Logic that makes it so that the game slows down when a vertical rebound/hit is made against terrain //
        HitCeilingOrFloor();

        // Move root to the right
        transform.Translate(new Vector3(Speed * Time.deltaTime, 0, 0));
    }


    private void HitWall()
    {
        if (BouncedBack == true)
        {
            Speed += Time.deltaTime * ReboundSpeed;

            if (Speed >= IntendedLevelSpeed)
            {
                Speed = IntendedLevelSpeed;
                BouncedBack = false;
            }

        }
        else if (BouncedForward == true)
        {
            Speed -= Time.deltaTime * ReboundSpeed;
            if (Speed <= -IntendedLevelSpeed)
            {
                Speed = -IntendedLevelSpeed;
                BouncedForward = false;
            }
        }
    }

    private void HitCeilingOrFloor()
    {
        if (BouncedVertically == true)
        {
            _bounceTimer += Time.deltaTime;

            if (_bounceTimer >= _bounceTmeSpeedFrozen) // 1 for example
            {
                _returnToNormalSpeed = true;
                _bounceTimer = 0;

                BouncedVertically = false;
            }
        }
        if (_returnToNormalSpeed == true)
        {
            Speed += Time.deltaTime * _returnSpeedModifier;
            if (Speed >= IntendedLevelSpeed)
            {
                Speed = IntendedLevelSpeed;
                _returnToNormalSpeed = false;
            }
        }
    }


    public void VerticalBounceMatrass()
    {
        BouncedVertically = false;
        _returnToNormalSpeed = false;
        _bounceTimer = 0;

        Speed = IntendedLevelSpeed;
    }
    public void VertcalBounceBoost(float bonusSpeed, float timeBoosted)
    {
        BouncedVertically = false;
        _returnToNormalSpeed = false;
        _bounceTimer = 0;

        _boostedSpeed = true;
        BoostedTimer = 0;
        BoostedTimeLimit = timeBoosted;

        Speed = IntendedLevelSpeed + bonusSpeed;
    }
    private void ReturnToNormalSpeed()
    {
        Speed -= Time.deltaTime * _returnSpeedModifier;

        if (Speed <= IntendedLevelSpeed)
        {
            Speed = IntendedLevelSpeed;
            _boostedSpeed = false;
            BoostedTimer = 0;
        }
    }
    public void HorizontalBoostMatrass()
    {
        BouncedVertically = false;
        _returnToNormalSpeed = false;
        _bounceTimer = 0;
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

        if (IntendedLevelSpeed != 0)
        {
            Speed = IntendedLevelSpeed;
        }
        else
        {
            Speed = 6;
        }
        
    }
}
