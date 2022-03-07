using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObstacles : MonoBehaviour
{
    public List<GameObject> RotatingObstacles = new List<GameObject>();
    public List<GameObject> CannonObstacles = new List<GameObject>();

    [SerializeField]
    private BoxCollider2D _collider;

    private Collider2D[] _obstacleColliders;

    [SerializeField]
    private LayerMask _checkedLayers;

    private Vector3 _boxOffset;
    private Vector3 _boxSize;

    //[SerializeField]
    //private GameObject _example;

    private void Start()
    {
        _boxOffset = _collider.offset;
        _boxSize = _collider.size;

        _obstacleColliders = Physics2D.OverlapBoxAll(transform.position + _boxOffset, _collider.size, 0f, _checkedLayers);

        //Instantiate(_example, transform.position + _boxOffset , Quaternion.identity);


        foreach (var obj in _obstacleColliders)
        {
            if (obj.TryGetComponent(out SetAnimationSpeed rotatingObstacle))
            {
                RotatingObstacles.Add(obj.gameObject);
                obj.gameObject.SetActive(false);
            }
            else if (obj.GetComponentInChildren<SpawnCannonBall>() != null)
            {
                CannonObstacles.Add(obj.gameObject);
                obj.GetComponentInChildren<SpawnCannonBall>().enabled = false;
            }
        }

        Destroy(_collider);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<HitObstacle>() != null) // if the player
        {
            foreach (var obstacle in RotatingObstacles)
            {
                obstacle.SetActive(true);
            }
            foreach (var cannon in CannonObstacles)
            {
                cannon.GetComponentInChildren<SpawnCannonBall>().enabled = true;
            }
        }
    }


    //void OnDrawGizmos()
    //{
    //    Color prevColor = Gizmos.color;

    //    Matrix4x4 prevMatrix = Gizmos.matrix;

    //    Gizmos.color = Color.red;
    //    Gizmos.matrix = transform.localToWorldMatrix;

    //    //Vector3 boxPosition = transform.position;

    //    // convert from world position to local position 
    //    Vector3 boxPosition = transform.InverseTransformPoint(transform.position);
    //    Vector3 pos = new Vector3(_boxSize.x / 2, _boxOffset.y / 2, 0);

    //    Gizmos.DrawWireCube(boxPosition + _boxOffset, _collider.size);

    //    // restore previous Gizmos settings
    //    Gizmos.color = prevColor;
    //    Gizmos.matrix = prevMatrix;
    //}
}
