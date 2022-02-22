using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutorialDraggable : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    // Direction of dragging
    public enum Direction { Left, Right, Top, Bottom }
    public Direction direction;

    // Mouse position
    private Vector3 screenPoint;
    private Vector3 offset;

    // Borders draggable object
    //private SpriteRenderer borderSprite;
    private Image image;

    private float leftBorder;
    private float rightBorder;
    private float topBorder;
    private float bottomBorder;

    private Vector3[] _imageCorners = new Vector3[4];
    private float counter;

    private float previousValue;

    private void Start()
    {
        // Calculate border
        //GameObject border = transform.Find("Border").gameObject;
        //borderSprite = border.transform.GetComponent<SpriteRenderer>();

        image = GetComponent<Image>();

        //leftBorder = borderSprite.transform.TransformPoint(borderSprite.sprite.bounds.min).x;
        //rightBorder = borderSprite.transform.TransformPoint(borderSprite.sprite.bounds.max).x;
        //topBorder = borderSprite.transform.TransformPoint(borderSprite.sprite.bounds.max).y;
        //bottomBorder = borderSprite.transform.TransformPoint(borderSprite.sprite.bounds.min).y;


        image.rectTransform.GetWorldCorners(_imageCorners); // this might need to get updated as it gets dragged down

        foreach (var corner in _imageCorners)
        {
            Debug.Log("number " + counter + " for x of " + corner.x + " + y of " + corner.y);
            counter++;
        }

        //Destroy(border);

    }


    public void OnPointerClick(PointerEventData data)
    {
        FollowFinger.controlCharacter = false;

        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        //Debug.Log("MOUSE WENT DOWN BABY");
        Debug.Log(Input.mousePosition.x + " + " + Input.mousePosition.y + " is me clicking") ;
        //Debug.Log(data.selectedObject.name + " IS MY NAME");
    }

    public void OnPointerUp(PointerEventData data)
    {
        FollowFinger.controlCharacter = true;

        //Debug.Log("mouse up pogachamp");
    }

    public void OnPointerDown(PointerEventData data)
    {

        // Calculate mouse & obstacle position

        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

        // Dragging directions

        //if (direction.ToString() == "Left") // Left dragging
        //{
        //    previousValue = transform.position.x;

        //    if (previousValue > curPosition.x && leftBorder < _imageCorners[2].x) // max x
        //    {
        //        transform.position = new Vector3(curPosition.x, transform.position.y, transform.position.z);
        //    }
        //}
        //else if (direction.ToString() == "Top") // Top dragging
        //{
        //    previousValue = transform.position.y;

        //    if (previousValue < curPosition.y && topBorder < _imageCorners[2].y) // max y
        //    {
        //        transform.position = new Vector3(transform.position.x, curPosition.y, transform.position.z);
        //    }
        //}
        //else if (direction.ToString() == "Right") // Right dragging
        //{
        //    previousValue = transform.position.x;

        //    if (previousValue < curPosition.x && rightBorder > _imageCorners[2].x - _imageCorners[1].x) // min x
        //    {
        //        transform.position = new Vector3(curPosition.x, transform.position.y, transform.position.z);
        //    }
        //}
        if (direction.ToString() == "Bottom") // Bottom dragging
        {
            Debug.Log("trueee");

            previousValue = transform.position.y;

            if (previousValue > curPosition.y && bottomBorder > _imageCorners[2].y - _imageCorners[0].y) // min y
            {
                transform.position = new Vector3(transform.position.x, curPosition.y, transform.position.z);
            }
        }
    }

}
