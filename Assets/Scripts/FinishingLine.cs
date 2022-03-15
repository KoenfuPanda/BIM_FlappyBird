using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishingLine : MonoBehaviour
{
    private MoveDirection _moveDirection;
    private FollowFinger _followFinger;
    private Rigidbody2D _rigidBim;

    public GameObject FX;

    [SerializeField]
    private GameManager _gameManager;

    [SerializeField] private int _levelNumber;

    private GameInstance _gameInstance;


    // score measurement objects
    [SerializeField]
    private GameObject _endScreenCanvas, _HUDScoreElementToMove;
    [SerializeField]
    private GameObject _engraving1, _engraving2, _engraving3, _engravingParent;
    [SerializeField]
    private GameObject _egg100Percent;
    [SerializeField]
    private GameObject _threshHoldsPanel;
    [SerializeField]
    private GameObject _star1, _star2, _star3;
    [SerializeField]
    private GameObject _buttonPanel;
    // filling bar
    [SerializeField]
    private Image _fillingBar;


    // etc
    private float _timer;
    private float _timer2;

    private float _timePer1Egg = 3.2f;
    private float _timerLimit;

    private float _currentFeatherCountDuringRecount;
    private float _currentFillAmount;
    private float _threshHold1, _threshHold2, _threshHold3;
    private bool _threshHold1Reached, _threshHold2Reached, _threshHold3Reached;
    private bool _pausedCount;
    private bool _playedChargingSound;

    const string _stringThreshHold1 = "ThreshHold_1_Reached";
    const string _stringThreshHold2 = "ThreshHold_2_Reached";
    const string _stringThreshHold3 = "ThreshHold_3_Reached";

    private bool _collectedSpecial1, _collectedSpecial2, _collectedSpecial3;
    private bool _foundOneSpecial;

    private bool _finishedEggCount, _finishedSpecialsCount;
    private bool _collectedAllEggs, _collectedAllSpecials;
    private bool _needsToCountPieces;

    private Animator _engravingsParentAnimator;
    private float _numberFoundSpecials, _foundSpecialCounter;

    [HideInInspector]
    public bool IsEnabled;


    // sound effect things
    [SerializeField]
    private SoundEffectMaster _audioMaster;
    


    private void Start()
    {
        _gameManager = GetComponentInParent<GameManager>();

        _HUDScoreElementToMove.GetComponent<Animator>().enabled = false; // just making sure this is disabled at start
        _engravingsParentAnimator = _engravingParent.GetComponent<Animator>();
        _engravingsParentAnimator.enabled = false; // idem ditto

        // 20, 50 and 80 %
        _threshHold1 = _gameManager.AllFeathers.Count / 5;
        _threshHold2 = _gameManager.AllFeathers.Count / 2;
        _threshHold3 = _gameManager.AllFeathers.Count * (8f / 10f);

        _timerLimit = _timePer1Egg / (float)_gameManager.AllFeathers.Count;

        Debug.Log(_gameManager.AllFeathers.Count + " all feathers in the level" );
        Debug.Log(_threshHold1 + " T " + _threshHold2 + " T " + _threshHold3);

        for (int i = 0; i < 3; i++) // checking whether the pieces have been picked up
        {
            if (GameInstance.CollectedEggs[_levelNumber, 0] == true)
            {
                // enable engarving piece, disable animator
            }
            
        }


        //this.enabled = false;  // disables update method untill it is needed. (enable it when the _hudScoreElement is in place next to the panel) (does not work ?)
    }

    private void Update()
    {
        if(IsEnabled == true)
        {
            if (_finishedEggCount == false) // if we have yet to check all collected eggs...
            {
                //  increase the _currentFeatherCountDuringRecount by 1 every x s, untill it has reached the _gameManager.saved feathers
                if (_currentFeatherCountDuringRecount <= _gameManager.SavedFeatherScore - 1)
                {
                    if (_pausedCount == false)
                    {
                        // play a oneshot of charging meter
                        PlayChargingSound(0.1f);

                        _timer += Time.deltaTime;
                        if (_threshHold1Reached == true && _threshHold3Reached == false) // speed up a bit more than normal
                        {
                            if (_timer >= _timerLimit / 2f)
                            {
                                _currentFeatherCountDuringRecount += 1;
                                _timer = 0;
                            }
                        }
                        else
                        {
                            if (_timer >= _timerLimit)
                            {
                                _currentFeatherCountDuringRecount += 1;
                                _timer = 0;
                            }
                        }


                    }
                }
                else if (_currentFeatherCountDuringRecount >= _gameManager.AllFeathers.Count) // if all eggs are collected...
                {
                    _collectedAllEggs = true;
                    _finishedEggCount = true;
                    _egg100Percent.SetActive(true);
                    _audioMaster.NormalizePitch();
                }
                else
                {
                    _audioMaster.GetComponent<AudioSource>().Stop(); // stop the charging sound once it's finished counting
                    Debug.Log(" teeeeeeee");
                    _audioMaster.NormalizePitch();
                    _finishedEggCount = true;
                }


                // checking threshHolds
                if (_currentFeatherCountDuringRecount >= _threshHold1 && _threshHold1Reached == false)
                {
                    _playedChargingSound = false;
                    _audioMaster.NormalizePitch();

                    _star1.SetActive(true);
                    _threshHoldsPanel.GetComponent<Animator>().Play(_stringThreshHold1);

                    _threshHold1Reached = true;
                    _pausedCount = true;
                }
                else if (_currentFeatherCountDuringRecount >= _threshHold2 && _threshHold2Reached == false)
                {
                    _playedChargingSound = false;

                    _star2.SetActive(true);
                    _threshHoldsPanel.GetComponent<Animator>().Play(_stringThreshHold2);

                    _threshHold2Reached = true;
                    _pausedCount = true;

                    //_audioMaster.IncreasePitch(0.1f);
                }
                else if (_currentFeatherCountDuringRecount >= _threshHold3 && _threshHold3Reached == false)
                {
                    _playedChargingSound = false;

                    _star3.SetActive(true);
                    _threshHoldsPanel.GetComponent<Animator>().Play(_stringThreshHold3);

                    _threshHold3Reached = true;
                    _pausedCount = true;

                    //_audioMaster.IncreasePitch(0.1f);
                }

                // stop the count for 1 second when it reaches a threshHold, then continue counting
                // once the count starts, play a oneshot of the charging sound effect (pitch increase)
                if (_pausedCount == true)
                {
                    _timer += Time.deltaTime;
                    if (_timer >= 0.9f)
                    {
                        _pausedCount = false;
                        _timer = 0;
                    }
                }

                // update fill image
                _currentFillAmount = _currentFeatherCountDuringRecount / _gameManager.AllFeathers.Count;
                //Debug.Log(_currentFeatherCountDuringRecount + " recounter2");
                //Debug.Log(_currentFillAmount + " fills");
                _fillingBar.fillAmount = _currentFillAmount;
            }
            else if (_finishedSpecialsCount == false) // else if we have not counted all the specials collected...
            {
                _timer += Time.deltaTime;

                if (_timer >= 0.75f)
                {
                    // for each elixer collected, check types
                    foreach (var elixer in _gameManager.CollectedEggs)
                    {
                        if (_foundOneSpecial == false)
                        {
                            if (elixer.ElixerTypePiece == EggElixir.ElixerType.Left && _collectedSpecial1 == false)
                            {
                                _engraving1.SetActive(true);
                                _collectedSpecial1 = true;
                                _foundOneSpecial = true;
                                _foundSpecialCounter += 1;
                            }
                            if (elixer.ElixerTypePiece == EggElixir.ElixerType.Middle && _collectedSpecial2 == false)
                            {
                                _engraving2.SetActive(true);
                                _collectedSpecial2 = true;
                                _foundOneSpecial = true;
                                _foundSpecialCounter += 1;

                                if (_collectedSpecial1 == true)
                                {
                                    _audioMaster.IncreasePitch(0.2f);
                                }
                            }
                            if (elixer.ElixerTypePiece == EggElixir.ElixerType.Right && _collectedSpecial3 == false)
                            {
                                _engraving3.SetActive(true);
                                _collectedSpecial3 = true;
                                _foundOneSpecial = true;
                                _foundSpecialCounter += 1;

                                if (_collectedSpecial1 == true || _collectedSpecial2 == true)
                                {
                                    _audioMaster.IncreasePitch(0.2f);
                                }
                            }
                        }
                    }

                    _foundOneSpecial = false; // reset bool to check for each again after timeframe has passed
                    _timer = 0;
                }


                // activate special animation for when all 3 pieces have been collected
                if (_collectedSpecial1 && _collectedSpecial2 && _collectedSpecial3 && _engravingsParentAnimator.enabled == false)
                {
                    _timer2 += Time.deltaTime;
                    if (_timer2 >= 0.75f)
                    {
                        _audioMaster.NormalizePitch();
                        _engravingsParentAnimator.enabled = true;
                        _timer2 = 0;
                    }
                }
                else if (_foundSpecialCounter >= _numberFoundSpecials)
                {
                    _audioMaster.NormalizePitch();
                    _finishedSpecialsCount = true;
                }
            }
            else // finally, show the buttons becoming available
            {
                _timer2 += Time.deltaTime;
                if (_timer2 >= 1.2f)
                {
                    _buttonPanel.SetActive(true);
                    _timer2 = 0;
                    this.enabled = false; // disable the update once here              
                }
            }
        }
    }



    private void PlayChargingSound(float pitchIncrease)
    {
        if (_playedChargingSound == false)
        {
            _audioMaster.PlaySpecificSoundEffect(1);
            _playedChargingSound = true;

            if (_threshHold1Reached == true)
            {
                _audioMaster.IncreasePitch(pitchIncrease);
            }           
        }
    }







    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out FollowFinger followFinger)) // if it's the player...
        {
            _followFinger = followFinger;
            _rigidBim = _followFinger.GetComponent<Rigidbody2D>();
            _moveDirection = _followFinger.GetComponentInParent<MoveDirection>();

            // disable controls
            _followFinger.TurnOffControl(1000, true, false, false);
            // set the movedirection speed to 0
            //_moveDirection.Speed = 0;
            StartCoroutine(SlowdownSpeed());
            // unfreeze bim x, freeze other
            _rigidBim.constraints = RigidbodyConstraints2D.None;
            _rigidBim.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            // add velocity to x
            _rigidBim.velocity = Vector2.right * _moveDirection.IntendedLevelSpeed;

            FX.SetActive(true);
            GetComponent<AudioSource>().Play();

            // save the feathers for one last time
            _gameManager.SaveFeathersCollectedSoFar(); 

            // 1)   set active of end level screen to true, this plays inherent popup animation
            // 1.1) this end level screen has some visualization of the feather score

            // first activate the moving hud element, then pop open the score
            //_endScreenCanvas.SetActive(true);
            _HUDScoreElementToMove.SetActive(true);
            // 2)   after the popup animation is complete, move the feather counter closer to the panel, transfer the score from the HUD to the score panel egg
            // 2.1) at 3 treshholds, each adding a 'star' when reached (maybe pause the count during the activation of a star)(maybe slowdown the count the farther it gets, for suspense)

            _HUDScoreElementToMove.GetComponent<Animator>().enabled = true;
            // x)   set the fill amount value 1 EQUAL to all feathers in the level
            // y)   set the 3 threshholds to 20%, 50%, 90% (start)
            // z)   when the counting feathers reach a treshhold, play "string" animation           

            // 2.2) the adding of a star would be a SetActive(true), inherently playing animation and sound effect

            // 3)   after the count is finished, SetActive(true) sillhouette of 3 rare collectibles
            // 3.1) for each collected, wait x seconds and SetActive(true) playing yet again an animation and sound effect/particle
            // 3.2) if all 3 are collected, merge them with an additional animation and particle effect into the complete engraving (SetActive(true))

            _numberFoundSpecials = _gameManager.CollectedEggs.Count;
            // 4)   if all of the above have ended, wait x seconds and show the buttons: retry, (menu,) next

            // 5)   store the required save data information in a script

            if (GameObject.Find("GameInstance(Clone)").GetComponent<GameInstance>() != null)
            {
                _gameInstance = GameObject.Find("GameInstance(Clone)").GetComponent<GameInstance>();

                // eggs collected in this level level x = _gamemanager.saved feathers

                _gameInstance.SetGameState(_levelNumber);
            }

            // Amount collected feathers in level

            if(GameInstance.MaxFeathers[_levelNumber - 1] == 0)
            {
                GameInstance.MaxFeathers[_levelNumber-1] = _gameManager.AllFeathers.Count;
            }

            if(GameInstance.CollectedFeathers[_levelNumber - 1] < _gameManager.FeatherScore)
            {
                GameInstance.CollectedFeathers[_levelNumber-1] = _gameManager.FeatherScore;
            }
        }

    }


    private IEnumerator SlowdownSpeed()
    {
        yield return new WaitForSeconds(0);
        _moveDirection.Speed = 4;

        yield return new WaitForSeconds(0.5f);
        _moveDirection.Speed = 3;

        yield return new WaitForSeconds(0.5f);
        _moveDirection.Speed = 2;

        yield return new WaitForSeconds(0.5f);
        _moveDirection.Speed = 1;

        yield return new WaitForSeconds(0.5f);
        _moveDirection.Speed = 0;

    }



    // call this when the everything is ready to begin (score screen animation being done)
    public void EnableThisUpdate()
    {
        //this.enabled = true;
        IsEnabled = true;
    }

    public void PopOpenScore()
    {
        _endScreenCanvas.SetActive(true);
    }

}
