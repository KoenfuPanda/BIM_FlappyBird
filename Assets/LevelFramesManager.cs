using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFramesManager : MonoBehaviour
{
    public int FrameNumber = 1;
    public List<LevelFrame> Frames = new List<LevelFrame>();

    private void Start()
    {
        StartCoroutine(DelayStart());
    }

    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(0.05f);

        for(int levelIndex = 0; levelIndex < 6; levelIndex++)
        {
            //Feathers
            if(GameInstance.CollectedFeathers[levelIndex] > 0)
            {
                Frames[levelIndex].EggScore.text = GameInstance.CollectedFeathers[levelIndex].ToString() + "/" + GameInstance.MaxFeathers[levelIndex].ToString();
                Frames[levelIndex].Filling.padding = new Vector4(0, 0, 0, 80 - ((float)GameInstance.CollectedFeathers[levelIndex] / (float)GameInstance.MaxFeathers[levelIndex] * 80));
            }

            // Emblems
            for (int EmblemIndex = 0; EmblemIndex < 3; EmblemIndex++)
            {
                if (GameInstance.CollectedEggs[levelIndex, EmblemIndex])
                {
                    Frames[levelIndex].EmblemPieces[EmblemIndex].enabled = true;
                }
            }

            if(GameInstance.CollectedFeathers[levelIndex] > 0)
            {
                Frames[0].EggScore.text = GameInstance.CollectedFeathers[0].ToString() + "/" + GameInstance.MaxFeathers[0].ToString();
            }
        }

        UnlockFrames();
    }
    private void UnlockFrames()
    {
        for(int index = 1; index < GameInstance.GameState + 1; index++)
        {
            if(index < 6)
            {
                Frames[index].Locked.SetActive(false);
                Frames[index].Unlocked.SetActive(true);
            }
        }
    }

    public void LoadSelectedLevel()
    {
        SceneManager.LoadScene("Alpha_level_0" + FrameNumber.ToString());
    }

    public void FrameToRight()
    {
        if(FrameNumber < 6)
        {
            FrameNumber++;
            GetComponent<Animator>().SetTrigger(FrameNumber.ToString());
        }
    }

    public void FrameToLeft()
    {
        if (FrameNumber > 1)
        {
            FrameNumber--;
            GetComponent<Animator>().SetTrigger(FrameNumber.ToString());
        }
    }
}
