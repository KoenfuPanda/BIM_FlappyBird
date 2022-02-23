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

    private void Start()
    {
        // Calculate border
        GameObject border = transform.Find("Border").gameObject;
        borderSprite = border.transform.GetComponent<SpriteRenderer>();

        leftBorder = borderSprite.transform.TransformPoint(borderSprite.sprite.bounds.min).x;
        rightBorder = borderSprite.transform.TransformPoint(borderSprite.sprite.bounds.max).x;
        topBorder = borderSprite.transform.TransformPoint(borderSprite.sprite.bounds.max).y;
        bottomBorder = borderSprite.transform.TransformPoint(borderSprite.sprite.bounds.min).y;

        Destroy(border);
        
    }

    void OnMouseDown()
    {
        FollowFinger.controlCharacter = false;

        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }
    
    void OnMouseUp()
    {
        FollowFinger.controlCharacter = true;
    }

    void OnMouseDrag()
    {

        // Calculate mouse & obstacle position

        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

        // Dragging directions

        if (direction.ToString() == "Left") // Left dragging
        {
            previousValue = transform.position.x;

            if (previousValue > curPosition.x && leftBorder < transform.TransformPoint(GetComponent<SpriteRenderer>().sprite.bounds.max).x)
            {
                transform.position = new Vector3(curPosition.x, transform.position.y, transform.position.z);
            }
        }
        else if (direction.ToString() == "Top") // Top dragging
        {
            previousValue = transform.position.y;

            if (previousValue < curPosition.y && topBorder > transform.TransformPoint(GetComponent<SpriteRenderer>().sprite.bounds.min).y)
            {
                transform.position = new Vector3(transform.position.x, curPosition.y, transform.position.z);
            }
        }
        else if (direction.ToString() == "Right") // Right dragging
        {
            previousValue = transform.position.x;

            if (previousValue < curPosition.x && rightBorder > transform.TransformPoint(GetComponent<SpriteRenderer>().sprite.bounds.min).x)
            {
                transform.position = new Vector3(curPosition.x, transform.position.y, transform.position.z);
            }
        }
        else if (direction.ToString() == "Bottom") // Bottom dragging
        {
            previousValue = transform.position.y;

            if (previousValue > curPosition.y && bottomBorder < transform.TransformPoint(GetComponent<SpriteRenderer>().sprite.bounds.max).y)
            {
                transform.position = new Vector3(transform.position.x, curPosition.y, transform.position.z);
            }
        }
    }
}