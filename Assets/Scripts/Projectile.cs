using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Sprite _featherImage;
    [HideInInspector] public float speed;
    [HideInInspector] public bool feather = false;

    private void Start()
    {
        if (feather)
        {
            GetComponent<Collider2D>().isTrigger = true;
            transform.GetChild(0).transform.GetComponent<SpriteRenderer>().sprite = _featherImage;
        }
    }
    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out FollowFinger followFinger))
        {
            if (followFinger.MegaBimActive == true) // if bim is mega, do not destroy
            {
                // do nothing special
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
            Destroy(gameObject);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            FeatherScore.Feathers++;
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "EndZone")
        {
            Destroy(gameObject);
        }
    }
}
