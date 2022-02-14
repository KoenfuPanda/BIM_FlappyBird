using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fly : MonoBehaviour
{
    public GameManager gameManager;
    public float velocity = 0.8f;
    private Rigidbody2D rb;
    private float rotateOffset = 0;
    private bool falling = true;
    private bool playing = true;

    public ParticleSystem fethersParticle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        gameObject.transform.eulerAngles = new Vector3(0, 0, -18 + rotateOffset);

        if (playing)
        {
            if (falling)
            {
                if (rotateOffset < -4)
                {
                    rotateOffset -= 1.5f;
                }
                else if (rotateOffset < -2)
                {
                    rotateOffset -= 0.9f;
                }
                else
                {
                    rotateOffset -= 0.5f;
                }

            }
            else
            {
                rotateOffset += 15.0f;
                if (rotateOffset > 20f)
                {
                    rotateOffset = 20f;
                    falling = true;
                }
            }
        }       
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && playing)
        {
            rb.velocity = Vector2.up * velocity;
            GetComponent<Animator>().SetTrigger("Fly");
            ParticleSystem fethers = Instantiate(fethersParticle);
            fethers.transform.position = new Vector3(transform.position.x- 0.5f, transform.position.y + 1, transform.position.z);
            falling = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameManager.GameOver();
        playing = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
    }
}
