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


    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CircleCollider2D>();
        _audioSource = GetComponent<AudioSource>();

        _gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
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
            // add it to the list
            _gameManager.CollectedFeathers.Add(this);
            // update actual score
            _gameManager.PickedUpFeather();

            // play sound effect
            if (_possibleSounds.Count > 0) 
            {
                _audioSource.PlayOneShot(_possibleSounds[Random.Range(0, _possibleSounds.Count - 1)]);
            }
            // instantiate particle
            Instantiate(_particleSystemPickup, transform.position, Quaternion.identity);

        }
    }
}