using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSelfDestruct : MonoBehaviour
{
    [SerializeField]
    private float _timeAlive;

    //void Start()
    //{
    //    //Destroy(this.gameObject, 2f);

    //    if (_timeAlive != 0)
    //    {
    //        Invoke("SetToFalse", _timeAlive);
    //    }
    //    else
    //    {
    //        Invoke("SetToFalse", 2f);
    //    }
        
    //}

    private void SetToFalse()
    {
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        //Destroy(this.gameObject, 2f);

        if (_timeAlive != 0)
        {
            Invoke("SetToFalse", _timeAlive);
        }
        else
        {
            Invoke("SetToFalse", 2f);
        }
    }
    
}
