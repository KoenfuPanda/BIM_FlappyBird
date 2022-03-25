using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAdjuster : MonoBehaviour
{
    private AudioSource _audioSource;

    private float _startPitch;
    private float _startVolume, _targetVolume;
    private float _durationFizzle;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        _startPitch = _audioSource.pitch;
        _startVolume = _audioSource.volume;

        _durationFizzle = 3f;
        _targetVolume = 0.2f;
    }




    // kill/reduce the volume of the music with this method, called end level ?
    public IEnumerator FizzleOutMusic()
    {
        yield return new WaitForSeconds(1.3f);

        float currentTime = 0;
        float start = _audioSource.volume;

        while (currentTime < _durationFizzle)
        {
            currentTime += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(start, _targetVolume, currentTime / _durationFizzle);
            yield return null;
        }
        yield break;

 
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
