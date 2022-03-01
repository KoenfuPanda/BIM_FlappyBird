using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishingLine : MonoBehaviour
{
    public GameObject EndScreen;
    public GameObject FX;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        EndScreen.SetActive(true);
        FX.SetActive(true);
    }
}
