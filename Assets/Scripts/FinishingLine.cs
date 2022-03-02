using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishingLine : MonoBehaviour
{
    public GameObject EndScreen;
    public GameObject FX;

    [SerializeField] private int _levelNumber;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        EndScreen.SetActive(true);
        FX.SetActive(true);
        GetComponent<AudioSource>().Play();

        GameObject.Find("GameInstance(Clone)").GetComponent<GameInstance>().SetGameState(_levelNumber);
    }
}
