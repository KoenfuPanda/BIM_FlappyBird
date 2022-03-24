using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAdjuster : MonoBehaviour
{
    private AudioSource _audioSource;

    private float _startPitch;
    private int _startVolumeTimes100;


    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _startPitch = _audioSource.pitch;
        _startVolumeTimes100 = (int)(_audioSource.volume * 100);
    }




    // kill/reduce the volume of the music with this method, called end level ?
    public IEnumerator FizzleOutMusic()
    {
        for (int i = _startVolumeTimes100; i > 0; i--)
        {
            _audioSource.volume -= (float)((float)i/100f);
        }

        yield return null;
    }


    // method called from the mega/clara powerup
    public void PitchShift(float pitchShiftAmount)
    {
        _audioSource.pitch += pitchShiftAmount;
    }
    // method called when they run out
    public void PitchToNormal()
    {
        _audioSource.pitch = _startPitch;
    }
}
