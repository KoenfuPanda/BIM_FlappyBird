using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBall_Projectile : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out FollowFinger followfinger))  // if it is the player...
        {
            if (followfinger.MegaBimActive == true)   // if bim is gigantic..
            {
                // nothing
            }
            else
                Destroy(this.gameObject); // use fancier logic here for sound, particle, etc
        }
        else
        {
            Destroy(this.gameObject); 
        }
       
    }
}
