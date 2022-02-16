using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAdjuster : MonoBehaviour
{
    public enum SendDir { SendsDown, SendStraight }
    public SendDir SendDirection;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out MoveDirection moveDirection))
        {
            if(SendDirection.ToString() == "SendStraight")
            {
                moveDirection.GoDiagonalDown = false;
            }
            else
            {
                moveDirection.GoDiagonalDown = true;
            }          
        }

    }
}
