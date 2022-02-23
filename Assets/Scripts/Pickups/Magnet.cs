using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    // PUBLIC

    public Collider2D playerCollider;
    public Collider2D featherCollider;

    // PRIVATE

    private bool magnetActive = false;
    private GameObject player;
    [SerializeField] private float _timeActive = 15;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(!magnetActive)
        {
            player = collider.gameObject;
            transform.parent = collider.gameObject.transform;
            transform.localPosition = new Vector3(0, 0, 0);
            GetComponent<SpriteRenderer>().enabled = false;
            playerCollider.enabled = false;
            featherCollider.enabled = true;
            StartCoroutine(DestroyMagnet());
            magnetActive = true;
        }

        if(collider.gameObject.tag == "Feather")
        {
            //collider.gameObject.GetComponent<Feathers>().FollowPlayer(player);

            collider.gameObject.AddComponent<Magnetizer>();
        }
    }

    IEnumerator DestroyMagnet()
    {
        yield return new WaitForSeconds(_timeActive);
        Destroy(gameObject);
    }
}