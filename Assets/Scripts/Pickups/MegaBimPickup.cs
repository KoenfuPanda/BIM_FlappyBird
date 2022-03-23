using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaBimPickup : MonoBehaviour
{
    [SerializeField]
    private float _upgradeTime;

    [SerializeField]
    private AudioSource _audioSource;
    private Collider2D _trigger;

    [SerializeField]
    private GameObject _animatingParent;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _trigger = GetComponent<Collider2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out FollowFinger followFinger))
        {
            followFinger.BimGrow(_upgradeTime);

            _audioSource.Play();
            _animatingParent.SetActive(false);
            _trigger.enabled = false;
        }
    }

    // upon picking me up
    // 1) set a boolean on HitObstacle to true 
    // ---> this bool will make it so that collisions with terrain will not damage you, but it will send the terrain flying
    // 2) activate a timer on HitObstacle that will count down according to a limit decided in here
    // 3) when this timer runs out, set the bool to false, set bims scale back to normal
}
