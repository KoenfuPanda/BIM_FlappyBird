using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectMaster : MonoBehaviour
{
    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip[] _audioClips;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }




    public void PlaySpecificSoundEffect(int index)
    {
        _audioSource.PlayOneShot(_audioClips[index]);
    }


    

    public void IncreasePitch(float amount)
    {
        _audioSource.pitch += amount;
    }
    public void NormalizePitch()
    {
        _audioSource.pitch = 1;
    }
}
