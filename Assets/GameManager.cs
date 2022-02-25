﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public string levelName;
    public GameObject gameOverCanvas;

    [HideInInspector]
    public int HealthBiM;

    
    public List<Feathers> CollectedFeathers = new List<Feathers>();
    public List<Feathers> CollectedSavedFeathers = new List<Feathers>();
    public Text FeatherScoreText;
    private int FeatherScore;
    private int SavedFeatherScore;

    public List<GameObject> CollectedPowerups = new List<GameObject>();

    public List<CheckPoint> CheckPoints = new List<CheckPoint>();
    private CheckPoint _currentCheckpoint;

    public List<GameObject> HeartSprites = new List<GameObject>();

    [SerializeField]
    private GameObject _playerPrefab;
    private GameObject _currentPlayer;

    private Vector3 _originalPlayerOffset;
    private Vector3 _instantiateExtraOffset;

    private void Start()
    {
        //Application.targetFrameRate = 60;

        Time.timeScale = 1;
        HealthBiM = 3;
      
        CheckPoints = FindObjectsOfType<CheckPoint>().ToList();
        foreach (CheckPoint checkPoint in CheckPoints)
        {
            if (checkPoint.IsActive == true)
            {
                _currentCheckpoint = checkPoint;
            }
        }

        _currentPlayer = FindObjectOfType<MoveDirection>().gameObject;
        _originalPlayerOffset = _currentPlayer.transform.position;
        _instantiateExtraOffset = new Vector3(5, _originalPlayerOffset.y, -40);

        //SaveFeathersCollectedSoFar();
    }

    public void GameOver()
    {
        gameOverCanvas.SetActive(true);
    }

    public void Replay()
    {
        SceneManager.LoadScene(levelName);
    }



    public void UpdateHUDHealth()
    {
        if(HealthBiM == 2)
        {
            HeartSprites[2].gameObject.SetActive(false);
        }
        else if (HealthBiM == 1)
        {
            HeartSprites[2].gameObject.SetActive(false);
            HeartSprites[1].gameObject.SetActive(false);
        }
        else
        {
            HeartSprites[2].gameObject.SetActive(false);
            HeartSprites[1].gameObject.SetActive(false);
            HeartSprites[0].gameObject.SetActive(false);
        }
    }
    public void RefillHealth()
    {
        HealthBiM = 3;

        HeartSprites[2].gameObject.SetActive(true);
        HeartSprites[1].gameObject.SetActive(true);
        HeartSprites[0].gameObject.SetActive(true);
    }


    public void SaveFeathersCollectedSoFar()  // logic for when a checkpoint is reached
    {
        SavedFeatherScore = FeatherScore;

        CollectedSavedFeathers.Clear(); // clear it first

        for (int i = 0; i < CollectedFeathers.Count; i++)
        {
            // copy of the list that holds same references
            CollectedSavedFeathers.Add(CollectedFeathers[i]);
        }

        CollectedFeathers.Clear();

        foreach (Feathers feather in CollectedSavedFeathers)  
        {
            Destroy(feather.gameObject);
        }

        CollectedSavedFeathers.Clear(); // clear it first
    }
    public void PickedUpFeather()
    {
        FeatherScore += 1;
        UpdateScoreHud();
    }
    private void ResetFeatherCount()
    {
        FeatherScore = SavedFeatherScore;
        UpdateScoreHud();
    }
    private void UpdateScoreHud()
    {
        FeatherScoreText.text = FeatherScore.ToString();
    }




    public IEnumerator RespawnLatestPoint()
    {
        yield return new WaitForSeconds(2);

        // respawn feathers //
        foreach (Feathers pickedFeather in CollectedFeathers)
        {
            pickedFeather.GetComponent<Collider2D>().enabled = true;
            pickedFeather.GetComponent<SpriteRenderer>().enabled = true;
            pickedFeather.transform.GetChild(0).gameObject.SetActive(true); // get component didnt seem to do the trick to get the particle ..
            
            // check for feathers with magnetizer on them, -> enable = false ,reset their position to start, remove the script -- players could die when feathers are being magnetized, hence this logic.
            if (pickedFeather.TryGetComponent(out Magnetizer magnetizer))
            {
                magnetizer.enabled = false;
                Destroy(magnetizer);
            }

            pickedFeather.transform.position = pickedFeather.StartingPosition;
        }
        // reset score //
        ResetFeatherCount();

        // respawn magnets and shrinks
        foreach (GameObject upgrade in CollectedPowerups)
        {
            if (upgrade.TryGetComponent(out Magnet magnet))
            {
                magnet.transform.SetParent(null);
                magnet.transform.position = magnet.StartPosition;
                magnet.transform.localScale = magnet.StartSize;
                magnet.PlayerCollider.enabled = true;
                magnet.FeatherCollider.enabled = false;
                magnet.MagnetActive = false;
                magnet.GetComponent<SpriteRenderer>().enabled = true;
                magnet.transform.GetChild(0).gameObject.SetActive(true); // get component didnt seem to do the trick to get the particle ..
            }
            else
            {
                upgrade.GetComponent<Collider2D>().enabled = true;
                upgrade.GetComponent<SpriteRenderer>().enabled = true;
                upgrade.transform.GetChild(0).gameObject.SetActive(true); 
            }
        }

        // respawn player //
        foreach (CheckPoint checkPoint in CheckPoints) 
        {
            if (checkPoint.IsActive == true)
            {
                _currentCheckpoint = checkPoint;
            }
        }
        RefillHealth();
        if(_playerPrefab != null) 
        {

            //var newPlayer = Instantiate(_playerPrefab, _currentCheckpoint.SpawnPoint.position + _instantiateExtraOffset, Quaternion.identity);
            var newPlayer = Instantiate(_playerPrefab, new Vector3(_currentCheckpoint.SpawnPoint.position.x - (-7), 1.51f, -40), Quaternion.identity);
            newPlayer.GetComponentInChildren<Rigidbody2D>().transform.localPosition = new Vector3(-8, _currentCheckpoint.SpawnPoint.position.y - 1, 42);

            // destroy the player that previously died
            Destroy(_currentPlayer);
            // assign new player as current
            _currentPlayer = newPlayer;  
        }        
    }
}
