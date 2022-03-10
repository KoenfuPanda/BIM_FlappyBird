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

    private float _currentFeatherCountDuringRecount;
    private float _currentFillAmount;
    private float _threshHold1, _threshHold2, _threshHold3;
    private bool _threshHold1Reached, _threshHold2Reached, _threshHold3Reached;
    private bool _pausedCount;

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

    // sound effect things
    [SerializeField]
    private SoundEffectMaster _audioMaster;
    


    private void Start()
    {
        _gameManager = GetComponentInParent<GameManager>();

        _HUDScoreElementToMove.GetComponent<Animator>().enabled = false; // just making sure this is disabled at start
        _engravingsParentAnimator = _engravingParent.GetComponent<Animator>();
        _engravingsParentAnimator.enabled = false; // idem ditto

        // 20, 50 and 90 %
        _threshHold1 = _gameManager.AllFeathers.Count / 5;
        _threshHold2 = _gameManager.AllFeathers.Count / 2;
        _threshHold3 = _gameManager.AllFeathers.Count * (9f / 10f);
        Debug.Log(_threshHold1 + " T " + _threshHold2 + " T " + _threshHold3);

        this.enabled = false;  // disables update method untill it is needed. (enable it when the _hudScoreElement is in place next to the panel)
    }

    private void Update()
    {
        if (_finishedEggCount == false) // if we have yet to check all collected eggs...
        {
            //  increase the _currentFeatherCountDuringRecount by 1 every 0.08s, untill it has reached the _gameManager.collectedFeathers

            if (_currentFeatherCountDuringRecount <= _gameManager.CollectedFeathers.Count)
            {
                if (_pausedCount == false)
                {
                    _timer += Time.deltaTime;
                    if (_timer >= 0.08f)
                    {
                        _currentFeatherCountDuringRecount += 1;
                        _timer = 0;
                    }
                }
            }
            else if (_currentFeatherCountDuringRecount >= _gameManager.AllFeathers.Count) // if all eggs are collected...
            {
                _collectedAllEggs = true;
                _finishedEggCount = true;
                _egg100Percent.SetActive(true);
            }
            else
            {
                _finishedEggCount = true;
            }


            // checking threshHolds
            if (_currentFeatherCountDuringRecount >= _threshHold1 && _threshHold1Reached == false)
            {
                _star1.SetActive(true);
                _threshHoldsPanel.GetComponent<Animator>().Play(_stringThreshHold1);

                _threshHold1Reached = true;
                _pausedCount = true;
            }
            else if (_currentFeatherCountDuringRecount >= _threshHold2 && _threshHold2Reached == false)
            {
                _star2.SetActive(true);
                _threshHoldsPanel.GetComponent<Animator>().Play(_stringThreshHold2);

                _threshHold2Reached = true;
                _pausedCount = true;
            }
            else if (_currentFeatherCountDuringRecount >= _threshHold3 && _threshHold3Reached == false)
            {
                _star3.SetActive(true);
                _threshHoldsPanel.GetComponent<Animator>().Play(_stringThreshHold3);

                _threshHold3Reached = true;
                _pausedCount = true;
            }

            // stop the count for 1 second when it reaches a threshHold, then continue counting
            // once the count starts, play a oneshot of the charging sound effect (pitch increase)
            if (_pausedCount == true)
            {
                _timer += Time.deltaTime;
                if (_timer >= 0.7f)
                {
                    _pausedCount = false;
                    _timer = 0;
                }
            }

            // update fill image
            _currentFillAmount = _currentFeatherCountDuringRecount / _gameManager.AllFeathers.Count;
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
            if (_timer2 >= 1f)
            {
                _buttonPanel.SetActive(true);
                _timer2 = 0;
                this.enabled = false; // disable the update once here              
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
                _gameInstance.SetGameState(_levelNumber);
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
        this.enabled = true;
    }

    public void PopOpenScore()
    {
        _endScreenCanvas.SetActive(true);
    }

}
