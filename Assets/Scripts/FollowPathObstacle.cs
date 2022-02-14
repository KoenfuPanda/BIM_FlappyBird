using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPathObstacle : MonoBehaviour
{
    // Speed
    public float speed = 10;
    private float velocity = 1;

    // Direction of path
    public enum Orientation { Vertical, Horizontal }
    public Orientation orientation;

    public enum StartDirection { Forwards, Backwards }
    public StartDirection startDirection;

    private bool start = true;

    // Borders draggable object
    private SpriteRenderer pathSprite;

    private float leftBorder;
    private float rightBorder;
    private float topBorder;
    private float bottomBorder;

    private void Start()
    {
        // Calculate path  
        GameObject path = transform.Find("Path").gameObject;
        pathSprite = path.transform.GetComponent<SpriteRenderer>();

        leftBorder = pathSprite.transform.TransformPoint(pathSprite.sprite.bounds.min).x;
        rightBorder = pathSprite.transform.TransformPoint(pathSprite.sprite.bounds.max).x;
        topBorder = pathSprite.transform.TransformPoint(pathSprite.sprite.bounds.max).y;
        bottomBorder = pathSprite.transform.TransformPoint(pathSprite.sprite.bounds.min).y;

        Destroy(path);
    }

    private void Update()
    {
        // Setting orientation
        if (orientation.ToString() == "Horizontal") // Horizontal
        {
            // Start direction
            if (startDirection.ToString() == "Backwards" && start) // Start backwards
            {
                velocity = -1;
                transform.localScale = new Vector3(-1, 1, 1);
                start = false;
            }
            else if (start) // Start forwards
            {
                start = false;
            }

            // Change direction
            if (velocity < 0 && transform.position.x < leftBorder) // Go backwards
            {
                velocity = 1;
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (velocity > 0 && transform.position.x > rightBorder) // Go forwards
            {
                velocity = -1;
                transform.localScale = new Vector3(-1, 1, 1);
            }

            // Set translation
            transform.Translate(new Vector3(speed * velocity * Time.deltaTime, 0, 0));
        }
        else // Vertical
        {
            // Start direction
            if (startDirection.ToString() == "Backwards" && start) // Start backwards
            {
                velocity = 1;
                transform.localScale = new Vector3(1, -1, 1);
                start = false;
            }
            else if (start) // Start forwards
            {
                velocity = -1;
                transform.localScale = new Vector3(1, 1, 1);
                start = false;
            }

            // Change direction
            if (velocity < 0 && transform.position.y < bottomBorder) // Go backwards
            {
                velocity = 1;
                transform.localScale = new Vector3(1, -1, 1);
            }
            else if (velocity > 0 && transform.position.y > topBorder) // Go forwards
            {
                velocity = -1;
                transform.localScale = new Vector3(1, 1, 1);
            }

            // Set translation
            transform.Translate(new Vector3(0, speed * velocity * Time.deltaTime, 0));             
        }
    }
}
