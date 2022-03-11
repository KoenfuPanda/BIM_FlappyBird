using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelFrame : MonoBehaviour
{
    public List<Image> Stars = new List<Image>();
    public List<Image> EmblemPieces = new List<Image>();
    public TextMeshProUGUI EggScore;
    public RectMask2D Filling;

    public GameObject Locked;
    public GameObject Unlocked;
}
