using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObstacle : MonoBehaviour
{
    public GameManager gameManager;

    private Rigidbody2D _rigidbody;
    private Collider2D _collider;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Block")
        {
            _collider.enabled = false;


            // checks what control scheme is used and disables it
            if(TryGetComponent(out FollowFinger followFingerScript))
            {
                followFingerScript.enabled = false;
            }
            if(GetComponentInParent<BimControllerFloating>() != null)
            {
                GetComponentInParent<BimControllerFloating>().enabled = false;
            }


            _rigidbody.gravityScale = 1;
            _rigidbody.constraints = RigidbodyConstraints2D.None;


            gameManager.GameOver();
        }
    }
}
