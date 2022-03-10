using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectTrigger : MonoBehaviour
{
    [SerializeField]
    private SoundEffectMaster _audioMaster;


    private void Start()
    {
        if (GetComponent<SoundEffectMaster>() == null)
        {
            _audioMaster = GetComponentInParent<SoundEffectMaster>();
        }
        else
        {
            _audioMaster = GetComponent<SoundEffectMaster>();
        }       
    }

    public void PlaySoundEffect(int index)
    {
        _audioMaster.PlaySpecificSoundEffect(index);
    }
}
