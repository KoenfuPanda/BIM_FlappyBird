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

    [HideInInspector]
    public float BounceTimer;
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
                ReturnToNormalSpeedAfterSpeedBoost();
            }
        }

        HitWall();
        // Logic that makes it so that the game slows down when a vertical rebound/hit is made against terrain //
        HitCeilingOrFloor();

        // Move root to the right
        if (GoDiagonalDown)
        {
            // Move root according to direction
            transform.Translate(Speed * _calculatedDirection.normalized * Time.deltaTime);
        }
        else
        {
            transform.Translate(new Vector3(Speed * Time.deltaTime, 0, 0));
        }


        
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
        // needs a _bounceTimer reset here --> present in hitobstacle

        if (BouncedVertically == true)
        {
            BounceTimer += Time.deltaTime;

            if (BounceTimer >= _bounceTmeSpeedFrozen) // time limit with which time stays frozen for a bit after hitting floor or ceiling
            {
                _returnToNormalSpeed = true;
                BounceTimer = 0;

                BouncedVertically = false;
            }
        }
        if (_returnToNormalSpeed == true)
        {
            // for moving right
            if (transform.GetComponentInChildren<Rigidbody2D>().transform.localScale.x > 0)
            {
                Speed += Time.deltaTime * _returnSpeedModifier;
                if (Speed >= IntendedLevelSpeed)
                {
                    Speed = IntendedLevelSpeed;
                    _returnToNormalSpeed = false;
                }
            }
            else // for moving left
            {        
                Speed -= Time.deltaTime * _returnSpeedModifier;
                if (Speed <= -IntendedLevelSpeed)
                {
                    Speed = -IntendedLevelSpeed;
                    _returnToNormalSpeed = false;
                }
            }
        }
    }


    public void VerticalBounceMatrass()
    {
        BouncedVertically = false;
        _returnToNormalSpeed = false;
        BounceTimer = 0;

        Speed = IntendedLevelSpeed;
    }
    public void VertcalBounceBoost(float bonusSpeed, float timeBoosted)
    {
        BouncedVertically = false;
        _returnToNormalSpeed = false;
        BounceTimer = 0;

        _boostedSpeed = true;
        BoostedTimer = 0;
        BoostedTimeLimit = timeBoosted;

        Speed = IntendedLevelSpeed + bonusSpeed;
    }
    private void ReturnToNormalSpeedAfterSpeedBoost()
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
        BounceTimer = 0;
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
