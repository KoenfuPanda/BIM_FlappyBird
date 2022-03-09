using System.Collections;
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
   

    public List<EggElixir> CollectedEggs = new List<EggElixir>();

    public List<Feathers> CollectedFeathers = new List<Feathers>();
    public List<Feathers> CollectedSavedFeathers = new List<Feathers>();
    public Text EggScoreText;
    public Text FeatherScoreText;
    private int FeatherScore;
    private int SavedFeatherScore;

    public List<GameObject> CollectedPowerups = new List<GameObject>();

    private List<SetAnimationSpeed> _rotatingPillars = new List<SetAnimationSpeed>();
    private List<SpawnCannonBall> _cannonShooters = new List<SpawnCannonBall>();
    private List<CanonBall_Projectile> _balls = new List<CanonBall_Projectile>();

    public List<CheckPoint> CheckPoints = new List<CheckPoint>();
    private CheckPoint _currentCheckpoint;

    public List<GameObject> HeartSprites = new List<GameObject>();
    [SerializeField]
    private Animator _healthAnimator;
    const string _loseHealth_1 = "Health_1_Lost";
    const string _loseHealth_2 = "Health_2_Lost";
    const string _loseHealth_3 = "Health_3_Lost";

    [SerializeField]
    private GameObject _playerPrefab;
    private GameObject _currentPlayer;

    [SerializeField]
    private GameObject _eggElixir;
    [SerializeField]
    private List<Transform> _eggSpawnPoints = new List<Transform>();
    [SerializeField]
    private int _levelIndex;

    private Vector3 _originalPlayerOffset;
    private Vector3 _instantiateExtraOffset;

    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip[] _audioClips = new AudioClip[2];

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

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

        _rotatingPillars = FindObjectsOfType<SetAnimationSpeed>().ToList();
        _cannonShooters = FindObjectsOfType<SpawnCannonBall>().ToList();

        //SaveFeathersCollectedSoFar();

        SpawnEggs();
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
            _healthAnimator.Play(_loseHealth_1);     
        }
        else if (HealthBiM == 1)
        {
            _healthAnimator.Play(_loseHealth_2);       
        }
        else
        {
            _healthAnimator.Play(_loseHealth_3);
        }
    }
    public void RefillHealth(bool respawned, bool isLevelStart)
    {
        HealthBiM = 3;

        if(respawned)
        {
            if (isLevelStart == false)
            {
                _audioSource.PlayOneShot(_audioClips[1]);
            }           
            _healthAnimator.Play("Health_Full_State");
        }
        else
        {
            _audioSource.PlayOneShot(_audioClips[0]);
            _healthAnimator.Play("Health_Refilled_Fully");
        }     
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
        UpdateFeatherScoreHud();
    }
    private void ResetFeatherCount()
    {
        FeatherScore = SavedFeatherScore;
        UpdateFeatherScoreHud();
    }
    private void UpdateFeatherScoreHud()
    {
        FeatherScoreText.text = FeatherScore.ToString();
    }
    public void UpdateEggScoreHud()
    {
        EggScoreText.text = CollectedEggs.Count().ToString();
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

        // disable the cannons and rotating pillars
        foreach (var obj in _rotatingPillars)
        {
            obj.gameObject.SetActive(false);
        }
        foreach (var obj in _cannonShooters)
        {
            obj.enabled = false;
        }

        // remove excess cannonballs in game
        _balls = FindObjectsOfType<CanonBall_Projectile>().ToList();
        foreach (var ball in _balls)
        {
            Destroy(ball);
        }



        // respawn player //
        foreach (CheckPoint checkPoint in CheckPoints) 
        {
            if (checkPoint.IsActive == true)
            {
                _currentCheckpoint = checkPoint;
            }
        }
        RefillHealth(true, false);
        if(_playerPrefab != null) 
        {

            //var newPlayer = Instantiate(_playerPrefab, _currentCheckpoint.SpawnPoint.position + _instantiateExtraOffset, Quaternion.identity);
            var newPlayer = Instantiate(_playerPrefab, new Vector3(_currentCheckpoint.SpawnPoint.position.x - (-7), 1.51f, -40), Quaternion.identity);
            newPlayer.GetComponentInChildren<Rigidbody2D>().transform.localPosition = new Vector3(-8, _currentCheckpoint.SpawnPoint.position.y - 1, 40);

            // destroy the player that previously died
            Destroy(_currentPlayer);
            // assign new player as current
            _currentPlayer = newPlayer;  
        }        
    }

    private void SpawnEggs()
    {
        for(int index = 0; index < 3; index++)
        {
            if (!GameInstance.CollectedEggs[_levelIndex, index])
            {
                GameObject EggTemp = Instantiate(_eggElixir, _eggSpawnPoints[index]);
                EggTemp.GetComponent<EggElixir>().EggIndex = index;
                EggTemp.GetComponent<EggElixir>().LevelIndex = _levelIndex;
            }
        }
    }
}
