using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnContact : MonoBehaviour
{
    private Collider2D _collider;
    private GameObject _visuals;

    [SerializeField]
    private GameObject _particleToShowPrefab;

    [SerializeField]
    private List<Transform> _particleLocations = new List<Transform>();

    void Start()
    {
        _collider = GetComponent<Collider2D>();
        _visuals = transform.GetChild(0).gameObject;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<FollowFinger>())
        {
            if (collision.gameObject.GetComponent<FollowFinger>().MegaBimActive == true || collision.gameObject.GetComponentInParent<FollowFinger>().MegaBimActive == true)
            {
                _collider.enabled = false;
                _visuals.SetActive(false);

                foreach (var location in _particleLocations)
                {
                    Instantiate(_particleToShowPrefab, location.position, Quaternion.identity);
                }
            }
        }        
    }
}
