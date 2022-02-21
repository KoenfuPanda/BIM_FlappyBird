using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string levelName;
    public GameObject gameOverCanvas;

    [HideInInspector]
    public int HealthBiM;

    //public List<Feathers> LevelPickups = new List<Feathers>();
    //public List<Feathers> _savedFeathers = new List<Feathers>();

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
        //LevelPickups = FindObjectsOfType<Feathers>().ToList();
        //LevelPickups.AddRange(FindObjectsOfType<Feathers>());

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


    public void SaveFeathersCollectedSoFar()
    {
        //_savedFeathers.AddRange(LevelPickups);
    }

    public IEnumerator RespawnLatestPoint()
    {
        yield return new WaitForSeconds(2);

        // respawn feathers //
        // optional, might not be needed as the game would be quite casual


        // respawn player //
        foreach (CheckPoint checkPoint in CheckPoints) 
        {
            if (checkPoint.IsActive == true)
            {
                _currentCheckpoint = checkPoint;
            }
        }

        RefillHealth();

        if(_playerPrefab != null) // null check
        {
            var newPlayer = Instantiate(_playerPrefab, _currentCheckpoint.SpawnPoint.position + _instantiateExtraOffset, Quaternion.identity);

            // destroy the current player
            Destroy(_currentPlayer);

            _currentPlayer = newPlayer;
        }        
    }
}
