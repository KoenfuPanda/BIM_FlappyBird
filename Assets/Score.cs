using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static int score = 0;
    public static int eggs = 0;

    public bool showEggs = false;

    private void Start()
    {
        score = 0;
        eggs = 0;
    }

    private void Update()
    {
        if(showEggs)
        {
            GetComponent<UnityEngine.UI.Text>().text = eggs.ToString();
        }
        else
        {
            GetComponent<UnityEngine.UI.Text>().text = score.ToString();
        }
    }
}
