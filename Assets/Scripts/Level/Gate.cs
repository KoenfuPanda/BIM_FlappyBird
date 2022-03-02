using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private GameObject triggerRoot;
    private GameObject gateRoot;

    private FollowFinger _followFinger;

    [SerializeField]
    private bool _onlyAllowedFinger, _onlyAllowedBim;

    [SerializeField]
    private Collider2D _triggerCollider, _fingerCollider;

    [SerializeField]
    private bool _isTutorial;
    [HideInInspector]
    public bool IsActive;
    [SerializeField]
    private TutorialLogic _requiredTutorial;


    // to make this logic work, make sure to have the trigger collider be greater in size than finger collider !

    private void Start()
    {
        triggerRoot = transform.Find("TriggerRoot").gameObject;
        gateRoot = transform.Find("GateRoot").gameObject;

        _followFinger = FindObjectOfType<FollowFinger>();

        if (_isTutorial == true)
        {
            IsActive = false;
        }
        else
        {
            IsActive = true;
        }

        if (_onlyAllowedFinger == true)
        {
            _triggerCollider.enabled = false;
        }
        if(_onlyAllowedBim == true)
        {
            _fingerCollider.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (IsActive == true)
        {
            _fingerCollider.enabled = false;

            triggerRoot.transform.Rotate(0, 0, 50);
            gateRoot.GetComponent<Animator>().enabled = true;

            if (_isTutorial == true)
            {
                _requiredTutorial.ContinueTheGame();
            }

            Destroy(this, 1f);
        }    
    }


    private void OnMouseDown()
    {
        if (IsActive == true && _onlyAllowedFinger)
        {
            _followFinger.EnteredClickableSurface();

            _triggerCollider.enabled = false;

            triggerRoot.transform.Rotate(0, 0, 50);
            gateRoot.GetComponent<Animator>().enabled = true;

            if (_isTutorial == true)
            {
                _requiredTutorial.ContinueTheGame();
            }

            Destroy(this, 1f);
        }

    }

    private void OnMouseUp()
    {
        if (IsActive == true)
        {
            _followFinger.ExitedClickableSurface();
        }
    }
}
