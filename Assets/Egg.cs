using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Score.eggs++;
        //Time.timeScale = 0.75f;
        Destroy(gameObject);
    }
}
