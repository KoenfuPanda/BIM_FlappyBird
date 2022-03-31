using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // PRIVATE

    [SerializeField]
    private AudioClip _intro;
    [SerializeField]
    private AudioClip[] _verses;
    [SerializeField]
    private AudioClip[] _bridges;
    private AudioSource _audioSource;
    private int _state = 0;

    // PUBLIC

    public int verseIndex;
    public int BridgeIndex;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_audioSource.clip.length >= _audioSource.time)
        {
            if (_state == 0 || _state == 2)
            {
                _audioSource.clip = _verses[verseIndex];
                _audioSource.Play();
                _state = 1;
            }
            else if (_state == 1)
            {
                _audioSource.clip = _bridges[BridgeIndex];
                _audioSource.Play();
                _state = 2;
            }
        }
    }


}
