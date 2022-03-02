using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject ObjectToSpawn;
    public void SpawnTheObject()
    {
        GameObject theObject = Instantiate(ObjectToSpawn);
    }
}
