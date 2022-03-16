using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private GameObject triggerRoot;
    private GameObject gateRoot;

    [SerializeField]
    private Transform _lever, _wheel;
    [SerializeField]
    private Animator _gateAnimator;
    private AudioSource _audioSource;

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
        //triggerRoot = transform.Find("TriggerRoot").gameObject;
        //gateRoot = transform.Find("GateRoot").gameObject;

        _audioSource = GetComponentInChildren<AudioSource>();
        _gateAnimator = GetComponentInChildren<Animator>();

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

            //triggerRoot.transform.Rotate(0, 0, 50);
            StartCoroutine(RotateMe(_lever ,new Vector3(0, 0, 50), 1f));
            StartCoroutine(RotateMe(_wheel, new Vector3(0, 0, -50), 1.2f));
            _audioSource.Play();

            _gateAnimator.enabled = true;

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

            //triggerRoot.transform.Rotate(0, 0, 50);
            StartCoroutine(RotateMe(_lever, new Vector3(0, 0, 50), 0.7f));
            StartCoroutine(RotateMe(_wheel, new Vector3(0, 0, -50), 0.8f));
            _audioSource.Play();

            _gateAnimator.enabled = true;

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


    private IEnumerator RotateMe(Transform chosenTransform, Vector3 byAngles, float inTime)
    {
        var fromAngle = chosenTransform.rotation;
        var toAngle = Quaternion.Euler(chosenTransform.eulerAngles + byAngles);

        for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
        {
            chosenTransform.rotation = Quaternion.Lerp(fromAngle, toAngle, t);

            yield return null;
        }
    }
}
