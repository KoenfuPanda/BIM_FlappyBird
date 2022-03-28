using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBall_Projectile : MonoBehaviour
{
    //[SerializeField]
    //private Collider2D _triggerCollider, _collisionCollider;

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.TryGetComponent(out FollowFinger followfinger))  // if it is the player...
    //    {
    //        if (followfinger.MegaBimActive == true)   // if bim is gigantic..
    //        {
    //            // nothing
    //        }
    //        else
    //            Destroy(this.gameObject); // use fancier logic here for sound, particle, etc
    //    }
    //    else
    //    {
    //        Destroy(this.gameObject); 
    //    }
       
    //}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out FollowFinger followfinger))  // if it is the player...
        {
            if (followfinger.MegaBimActive == true)   // if bim is gigantic..
            {

            }
            else
            {
                //Destroy(this.gameObject);
                this.gameObject.SetActive(false);
            }           
        }
        else if (collision.gameObject.GetComponentInParent<FollowFinger>() != null) // if the parent is the player...
        {
            if (collision.gameObject.GetComponentInParent<FollowFinger>().MegaBimActive == true)   // if bim is gigantic..
            {

            }
            else
            {
                //Destroy(this.gameObject);
                this.gameObject.SetActive(false);
            }               
        }
        else if (collision.gameObject.GetComponent<Feathers>() == null && collision.gameObject.GetComponent<EggElixir>() == null && collision.gameObject.tag != "ImmuneToProjectile") // if it's not a feather,elixer or special terrain
        {
            //Destroy(this.gameObject);
            this.gameObject.SetActive(false);
        }
    }
}
