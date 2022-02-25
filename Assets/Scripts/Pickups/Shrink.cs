using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrink : MonoBehaviour
{
    [SerializeField] private float _timeActive;

    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out HitObstacle hitObstacle))
        {
            GetComponent<SpriteRenderer>().enabled = false;

            hitObstacle.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

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
        character.transform.localScale = new Vector3(1, 1, 1);
        //Destroy(gameObject);
    }
}
