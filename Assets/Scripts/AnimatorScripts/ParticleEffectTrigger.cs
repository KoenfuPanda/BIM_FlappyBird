using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject _requiredParticleParent;

    public void SetParticleParentActive()
    {
        _requiredParticleParent.SetActive(true);
    }
}
