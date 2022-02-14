using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMatras : MonoBehaviour
{
    public enum Direction { Left, Right }
    public Direction direction;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        GetComponent<Animator>().SetTrigger("Bounce");
        if (direction.ToString() == "Right")
        {
            collider.gameObject.transform.parent.gameObject.transform.GetComponent<ChangeDirection>().GoRight();
        }

        if (direction.ToString() == "Left")
        {
            collider.gameObject.transform.parent.gameObject.transform.GetComponent<ChangeDirection>().GoLeft();
        }
    }
}
