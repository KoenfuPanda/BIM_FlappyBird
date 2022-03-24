//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomSoundEffectUponActive : MonoBehaviour
{
    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip[] _soundEffects;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        int random = UnityEngine.Random.Range(0, _soundEffects.Length);
        float randomBonusPitch = UnityEngine.Random.Range(-0.2f, 0.2f);

        _audioSource.pitch += randomBonusPitch;
        _audioSource.PlayOneShot(_soundEffects[random]);
    }
}
