using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFramesManager : MonoBehaviour
{
    public List<LevelFrame> Frames = new List<LevelFrame>();

    void Start()
    {
        StartCoroutine(DelayStart());
    }

    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(1);

        if(GameInstance.CollectedEggs[0,0])
        {
            Frames[0].EmblemPieces[0].enabled = true;
        }
    }
}
