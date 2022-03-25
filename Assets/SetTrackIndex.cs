using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTrackIndex : MonoBehaviour
{
    // PRIVATE

    [SerializeField]
    private MusicManager _musicManager;
    [SerializeField]
    private int _verseIndex;
    [SerializeField]
    private int _bridgeIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            _musicManager.verseIndex = _verseIndex;
            _musicManager.BridgeIndex = _bridgeIndex;
        }
    }
}
