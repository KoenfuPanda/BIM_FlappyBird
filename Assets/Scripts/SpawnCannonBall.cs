using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCannonBall : MonoBehaviour
{
    [SerializeField] private GameObject cannonBall;

    void Start()
    {
        Instantiate(cannonBall, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
    }
}
