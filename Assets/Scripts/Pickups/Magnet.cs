using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    // PUBLIC

    public Collider2D PlayerCollider;
    public Collider2D FeatherCollider;

    [HideInInspector]
    public bool MagnetActive = false;

    [HideInInspector]
    public Vector3 StartPosition, StartSize;

    // PRIVATE

    private GameObject player;
    [SerializeField] private float _timeActive = 15;

    private GameManager _gameManager;

    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _particleEffectAttract;

    [SerializeField]
    private AudioClip[] _soundEffects;


    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();

        StartPosition = transform.position;
        StartSize = transform.localScale;

        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(!MagnetActive)
        {
            _gameManager.CollectedPowerups.Add(this.gameObject);

            player = collider.gameObject;
            transform.parent = collider.gameObject.transform;

            // if my local scale y is smaller than 1, double my size
            if (collider.transform.localScale.y < 1)
            {
                transform.localScale = Vector3.one;
            }

            transform.localPosition = new Vector3(0, 0, 0);
            GetComponent<SpriteRenderer>().enabled = false;
            PlayerCollider.enabled = false;
            FeatherCollider.enabled = true;

            _audioSource.PlayOneShot(_soundEffects[0]); // pickup sound

            //StartCoroutine(DestroyMagnet());
            StartCoroutine(DisableMagnet());
            StartCoroutine(ActivateSoundRunningOut());
            _particleEffectAttract.SetActive(true);

            MagnetActive = true;
        }

        if(collider.gameObject.tag == "Feather")
        {
            //collider.gameObject.GetComponent<Feathers>().FollowPlayer(player);

            collider.gameObject.AddComponent<Magnetizer>();
        }
    }

    IEnumerator DisableMagnet()
    {
        yield return new WaitForSeconds(_timeActive);
        FeatherCollider.enabled = false;
        _particleEffectAttract.SetActive(false);
    }

    IEnumerator ActivateSoundRunningOut()
    {
        yield return new WaitForSeconds(_timeActive -2.5f);
        _audioSource.volume = 0.7f;
        _audioSource.PlayOneShot(_soundEffects[1]); // running out sound
    }

    IEnumerator DestroyMagnet()
    {
        yield return new WaitForSeconds(_timeActive);
        Destroy(gameObject);
    }
}