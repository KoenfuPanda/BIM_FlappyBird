using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    // Direction of dragging
    public enum Direction { Left, Right, Top, Bottom }
    public Direction direction;
    
    // Mouse position
    private Vector3 screenPoint;
    private Vector3 offset;

    // Borders draggable object
    private SpriteRenderer borderSprite;

    private float leftBorder;
    private float rightBorder;
    private float topBorder;
    private float bottomBorder;

    private float previousValue;

    private FollowFinger _followFinger;

    [SerializeField]
    private bool _isTutorial;
    [HideInInspector]
    public bool IsActive;
    [SerializeField]
    private TutorialLogic _requiredTutorial;


    [SerializeField]
    private GameObject _borderObject;

    // tapping functionality
    [SerializeField]
    private bool _tapFuncionalityOn;
    private bool _activatedByTap;
    [SerializeField]
    private Animator _animator;
    private Collider2D _collider;


    private void Start()
    {
        // Calculate border
        //GameObject border = transform.Find("Border").gameObject;
        borderSprite = _borderObject.transform.GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();

        _followFinger = FindObjectOfType<FollowFinger>();

        if (_isTutorial == true)
        {
            IsActive = false;
        }
        else
        {
            IsActive = true;
        }

        leftBorder = borderSprite.transform.TransformPoint(borderSprite.sprite.bounds.min).x;
        rightBorder = borderSprite.transform.TransformPoint(borderSprite.sprite.bounds.max).x;
        topBorder = borderSprite.transform.TransformPoint(borderSprite.sprite.bounds.max).y;
        bottomBorder = borderSprite.transform.TransformPoint(borderSprite.sprite.bounds.min).y;

        Destroy(_borderObject);
        
    }

    void OnMouseDown()
    {
        if (IsActive == true && _activatedByTap == false)
        {
            //FollowFinger.controlCharacter = false;
            _followFinger.EnteredClickableSurface();

            screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

            if (_tapFuncionalityOn == true)
            {
                // activate animation
                _animator.Play("DraggableMove");
                _collider.enabled = false;

                if (_isTutorial) // if lifted high enough => call continue game
                {
                    _requiredTutorial.ContinueTheGame();
                }

                _activatedByTap = true;
            }
        }
    }
    
    void OnMouseUp()
    {
        _followFinger.ExitedClickableSurface();
        //FollowFinger.controlCharacter = true;       
    }

    void OnMouseDrag()
    {
        if (IsActive == true && _tapFuncionalityOn == false)
        {
            // Calculate mouse & obstacle position

            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

            // Dragging directions

            if (direction == Direction.Left) // Left dragging
            {
                previousValue = transform.position.x;

                if (previousValue > curPosition.x && leftBorder < transform.TransformPoint(GetComponent<SpriteRenderer>().sprite.bounds.max).x)
                {
                    transform.position = new Vector3(curPosition.x, transform.position.y, transform.position.z);
                }
            }
            else if (direction == Direction.Top) // Top dragging
            {
                previousValue = transform.position.y;

                if (previousValue < curPosition.y && topBorder > transform.TransformPoint(GetComponent<SpriteRenderer>().sprite.bounds.min).y)
                {
                    transform.position = new Vector3(transform.position.x, curPosition.y, transform.position.z);
                }
            }
            else if (direction == Direction.Right) // Right dragging
            {
                previousValue = transform.position.x;

                if (previousValue < curPosition.x && rightBorder > transform.TransformPoint(GetComponent<SpriteRenderer>().sprite.bounds.min).x)
                {
                    transform.position = new Vector3(curPosition.x, transform.position.y, transform.position.z);
                }
            }
            else if (direction == Direction.Bottom) // Bottom dragging
            {
                previousValue = transform.position.y;

                if (previousValue > curPosition.y && bottomBorder < transform.TransformPoint(GetComponent<SpriteRenderer>().sprite.bounds.max).y)
                {
                    transform.position = new Vector3(transform.position.x, curPosition.y, transform.position.z);
                }
            }

            if (_isTutorial && transform.position.y >= 1.4f) // if lifted high enough => call continue game
            {
                _requiredTutorial.ContinueTheGame();
            }
        }

    }


}