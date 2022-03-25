using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    private Collider2D _collider;
    private SpriteRenderer _spriteRen;

    [SerializeField]
    private GameObject _particlePrefab;

    
    public bool IsBarrel;


    private void Start()
    {
        if (GetComponent<Collider2D>() == null)
        {
            _collider = GetComponentInChildren<Collider2D>();
        }
        else
        {
            _collider = GetComponent<Collider2D>();
        }


        if (GetComponent<SpriteRenderer>() == null)
        {
            _spriteRen = GetComponentInChildren<SpriteRenderer>();
        }
        else
        {
            _spriteRen = GetComponent<SpriteRenderer>();
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if im a barrel, only crash when colliding with destructible wall... barrels cannot destroy barrels  && if im not the player
        if (collision.gameObject.TryGetComponent(out DestroyOnCollision destroyScript) && collision.gameObject.layer != 8)
        {
            if (IsBarrel == true && destroyScript.IsBarrel == false)
            {
                // disable sprite
                _spriteRen.enabled = false;
                // disable collider
                _collider.enabled = false;
                // set particle active to true
                _particlePrefab.SetActive(true);
            }
            else if (IsBarrel == false)
            {
                // disable sprite
                _spriteRen.enabled = false;
                // disable collider
                _collider.enabled = false;
                // set particle active to true
                _particlePrefab.SetActive(true);
            }
        }



        //if (collision.gameObject.layer != 8) 
        //{
        //    //Destroy(gameObject);

        //    // disable sprite
        //    _spriteRen.enabled = false;
        //    // disable collider
        //    _collider.enabled = false;
        //    // set particle active to true
        //    _particlePrefab.SetActive(true);
        //}
    }
}
