using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrink : MonoBehaviour
{
    [SerializeField] private float _timeActive;

    private GameManager _gameManager;
    private HitObstacle _hitObstacle;
    [HideInInspector]
    public bool HasReturnedToNormalSize;

    private Vector3 _originalScale;

    [SerializeField]
    private List<AudioClip> _soundEffects = new List<AudioClip>();

    private MusicAdjuster _musicAdjuster;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _musicAdjuster = FindObjectOfType<MusicAdjuster>();
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out HitObstacle hitObstacle))
        {
            _hitObstacle = hitObstacle;

            GetComponent<SpriteRenderer>().enabled = false;

            // shrinking logic -> animation
            // after x seconds, setnormal size plays de-shrink
            //hitObstacle.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            _hitObstacle.GetComponent<Animator>().Play("BimShrinks");
            _hitObstacle.GetComponent<AudioSource>().PlayOneShot(_soundEffects[0]);
            _musicAdjuster.PitchShift(0.3f);

            //if i have a magnet, --> scale up the magnet object times 2
            if (collider.GetComponentInChildren<Magnet>() != null)
            {
                collider.GetComponentInChildren<Magnet>().transform.localScale = Vector3.one;
            }

            _gameManager.CollectedPowerups.Add(this.gameObject);

            StartCoroutine(SetNormalSize(collider.gameObject));
        }
    }

    IEnumerator SetNormalSize(GameObject character)
    {
        yield return new WaitForSeconds(_timeActive);
        if (character != null && character.GetComponent<FollowFinger>().MegaBimActive == false && HasReturnedToNormalSize == false)  // if Bim exists AND is not mega...
        {
            _hitObstacle.GetComponent<Animator>().Play("BimDeShrinks");
            _hitObstacle.GetComponent<AudioSource>().PlayOneShot(_soundEffects[1]);
            _musicAdjuster.PitchToNormal();

            //if (character.transform.localScale.x > 0)
            //{
            //    character.transform.localScale = new Vector3(1, 1, 1);
            //}
            //else
            //{
            //    character.transform.localScale = new Vector3(-1, 1, 1);
            //}          
        }       
        //Destroy(gameObject);
    }

    public void SetNormalSizeInstantly()
    {
        if (HasReturnedToNormalSize == false)
        {
            _hitObstacle.GetComponent<Animator>().Play("BimDeShrinks");
            _hitObstacle.GetComponent<AudioSource>().PlayOneShot(_soundEffects[1]);
            _musicAdjuster.PitchToNormal();

            HasReturnedToNormalSize = true;
        }
    }
}
