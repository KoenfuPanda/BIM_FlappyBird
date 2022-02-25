using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    // PUBLIC

    public Collider2D PlayerCollider;
    public Collider2D FeatherCollider;

    [HideInInspector]
    public bool MagnetActive = false;

    [HideInInspector]
    public Vector3 StartPosition, StartSize;

    // PRIVATE

    private GameObject player;
    [SerializeField] private float _timeActive = 15;

    private GameManager _gameManager;


    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();

        StartPosition = transform.position;
        StartSize = transform.localScale;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(!MagnetActive)
        {
            _gameManager.CollectedPowerups.Add(this.gameObject);

            player = collider.gameObject;
            transform.parent = collider.gameObject.transform;

            // if my local scale y is smaller than 1, double my size
            if (collider.transform.localScale.y < 1)
            {
                transform.localScale = Vector3.one;
            }

            transform.localPosition = new Vector3(0, 0, 0);
            GetComponent<SpriteRenderer>().enabled = false;
            PlayerCollider.enabled = false;
            FeatherCollider.enabled = true;
            //StartCoroutine(DestroyMagnet());
            StartCoroutine(DisableMagnet());
            MagnetActive = true;
        }

        if(collider.gameObject.tag == "Feather")
        {
            //collider.gameObject.GetComponent<Feathers>().FollowPlayer(player);

            collider.gameObject.AddComponent<Magnetizer>();
        }
    }

    IEnumerator DisableMagnet()
    {
        yield return new WaitForSeconds(_timeActive);
        FeatherCollider.enabled = false;
    }

    IEnumerator DestroyMagnet()
    {
        yield return new WaitForSeconds(_timeActive);
        Destroy(gameObject);
    }
}