using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectTrigger : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> _soundEffects = new List<AudioClip>();

    private AudioSource _audioSource;


    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }


    private void PlaySoundEffect(int index)
    {
        _audioSource.PlayOneShot(_soundEffects.ToArray()[index]);
    }
}
