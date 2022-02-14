using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFlowers : MonoBehaviour
{
    public GameObject[] flowers = new GameObject[10];

    public void Start()
    {
        /*
        GameObject newFlower = Instantiate(flowers[Random.Range(0, flowers.Length)]);
        newFlower.transform.parent = transform;*/
    }
}