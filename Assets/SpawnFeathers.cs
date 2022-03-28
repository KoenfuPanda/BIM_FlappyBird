using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFeathers : MonoBehaviour
{
    [SerializeField]
    private GameObject _featherPrefab;

    [SerializeField]
    private Transform[] _spawnPoints;

    void Start()
    {
        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            Instantiate(_featherPrefab, _spawnPoints[i].position, Quaternion.identity);
        }
    }
}
