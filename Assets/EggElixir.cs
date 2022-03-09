using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggElixir : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private CapsuleCollider2D _collider;

    private AudioSource _audioSource;

    [SerializeField]
    private GameObject _particleSystemPickup, _particleSystemGlow;

    [SerializeField]
    private List<AudioClip> _possibleSounds;

    private GameManager _gameManager;

    public int LevelIndex = 0;
    public int EggIndex = 0;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CapsuleCollider2D>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            _particleSystemGlow.SetActive(false);
            _gameManager.CollectedEggs.Add(this);
            _gameManager.UpdateEggScoreHud();
            _spriteRenderer.enabled = false;
            _collider.enabled = false;

            // play sound effect
            if (_possibleSounds.Count > 0)
            {
                _audioSource.PlayOneShot(_possibleSounds[Random.Range(0, _possibleSounds.Count - 1)]);
            }

            // instantiate particle
            Instantiate(_particleSystemPickup, transform.position, Quaternion.identity);


            GameInstance.CollectedEggs[LevelIndex, EggIndex] = true;
        }
    }
}