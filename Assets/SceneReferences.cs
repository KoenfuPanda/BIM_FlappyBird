using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneReferences : MonoBehaviour
{
    public List<GameObject> SceneObjects = new List<GameObject>();

    private void Start()
    {
        if (GameObject.Find("GameInstance(Clone)"))
        {
            GameObject reference = GameObject.Find("GameInstance(Clone)");
            reference.GetComponent<GameInstance>().SetLevelButtons();
        }
    }
}
