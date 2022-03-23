using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSelfDestruct : MonoBehaviour
{
    void Start()
    {
        //Destroy(this.gameObject, 2f);

        Invoke("SetToFalse", 2f);
    }

    private void SetToFalse()
    {
        this.gameObject.SetActive(false);
    }
}
