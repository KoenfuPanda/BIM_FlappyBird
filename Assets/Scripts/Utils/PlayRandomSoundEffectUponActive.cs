using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomSoundEffectUponActive : MonoBehaviour
{
    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip[] _soundEffectsGiantBim;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        var random = UnityEngine.Random.Range(0, _soundEffectsGiantBim.Length);
        _audioSource.PlayOneShot(_soundEffectsGiantBim[random]);
    }
}
