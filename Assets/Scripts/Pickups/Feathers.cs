using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feathers : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _collider;

    private AudioSource _audioSource;

    [SerializeField]
    private GameObject _particleSystemPickup, _particleSystemGlow;

    [SerializeField]
    private List<AudioClip> _possibleSounds;

    private GameManager _gameManager;

    [HideInInspector]
    public Vector3 StartingPosition;

    [SerializeField]
    private bool _imAProjectile;

    private void Start()
    {
        //_spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        _collider = GetComponent<CircleCollider2D>();
        _audioSource = GetComponent<AudioSource>();

        _gameManager = FindObjectOfType<GameManager>();

        StartingPosition = transform.position;

        if (_imAProjectile)
        {
            Destroy(this.gameObject, 15f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponentInChildren<Magnet>() == null && collision.gameObject.GetComponentInParent<FollowFinger>() != null || collision.gameObject.GetComponent<FollowFinger>() != null)
        {
            //FeatherScore.Feathers++;

            // destroy logic replaced (due to feather respawn system)
            //Destroy(_particleSystemGlow);
            //Destroy(_collider);
            //Destroy(_spriteRenderer);

            _particleSystemGlow.SetActive(false);
            _collider.enabled = false;
            _spriteRenderer.enabled = false;

            // remove magnetizer if present
            if (TryGetComponent(out Magnetizer magnetizer))
                Destroy(magnetizer);
            // add it to the list (only if it's part of the stage)
            if (_imAProjectile == false)
            {
                _gameManager.CollectedFeathers.Add(this);
            }           
            // update actual score
            _gameManager.PickedUpFeather();


            // play sound effect // replacement logic present in particle
            //if (_possibleSounds.Count > 0) 
            //{
            //    _audioSource.PlayOneShot(_possibleSounds[Random.Range(0, _possibleSounds.Count - 1)]);
            //}

            // instantiate particle //
            //Instantiate(_particleSystemPickup, transform.position, Quaternion.identity);


            // replacement for instantiate logic
            _particleSystemPickup.GetComponent<AudioSource>().pitch += (float)(_gameManager.EggQuickPickupCount / 20f);
            _particleSystemPickup.SetActive(true);



            if (_imAProjectile)
            {
                Destroy(this.gameObject, 2f);
            }

        }
    }
}