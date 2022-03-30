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
    private float _timer = 0;
    private int _state = 0;

    // PUBLIC

    public int verseIndex;
    public int BridgeIndex;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        //StartCoroutine(SetIntro());
    }

    private void Update()
    {
        print(_audioSource.time + " /  " + _audioSource.clip.length);

        if (_audioSource.clip.length == _audioSource.time)
        {
            if (_state == 0 || _state == 2)
            {
                _audioSource.clip = _verses[0];
                _audioSource.Play();
                _state = 1;
            }
            else if (_state == 1)
            {
                _audioSource.clip = _bridges[0];
                _audioSource.Play();
                _state = 2;
            }
        }

        //print(_timer);
        //print(_audioSource.clip.length);
        //_timer +=Time.deltaTime;

        //if (_timer > _audioSource.clip.length)
        //{
        //    if(_state == 0)
        //    {
        //        _audioSource.clip = _intro;
        //        _audioSource.Play();
        //        _state = 1;
        //        _timer = 0;
        //    }
        //    else if(_state == 1)
        //    {
        //        _audioSource.clip = _verses[0];
        //        _audioSource.Play();
        //        _state = 2;
        //        _timer = 0;
        //    }
        //    else if(_state == 2)
        //    {
        //        _audioSource.clip = _bridges[0];
        //        _audioSource.Play();
        //        _state = 1;
        //        _timer = 0;
        //    }
        //}
    }

    IEnumerator SetIntro()
    {
        yield return new WaitForSeconds(_audioSource.clip.length-0.5f);
        _audioSource.clip = _verses[verseIndex];
        _audioSource.Play();
        StartCoroutine(SetBridge());
    }

    IEnumerator SetVerse()
    {
        yield return new WaitForSeconds(_audioSource.clip.length);
        _audioSource.clip = _verses[verseIndex];
        _audioSource.Play();
        StartCoroutine(SetBridge());
    }

    IEnumerator SetBridge()
    {
        yield return new WaitForSeconds(_audioSource.clip.length);
        _audioSource.clip = _bridges[BridgeIndex];
        _audioSource.Play();
        StartCoroutine(SetVerse());
    }
}
