using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCannonBall : MonoBehaviour
{
    [SerializeField] private GameObject _projectile;
    [SerializeField] private float _loopTime;
    [SerializeField] private float _speed;

    [SerializeField]
    private GameObject _canonBallPrefab, _featherPickupPrefab;
    private GameObject _chosenProjectile;

    [SerializeField]
    private Transform _shootPosition;

    private enum ProjectileType { Cannonball, Feather, Random }
    [SerializeField] private ProjectileType _projectileType;

    private float _timer = 0;


    private void Start()
    {
        _timer = _loopTime;
    }


    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _loopTime)
        {
            if (_projectileType == ProjectileType.Cannonball) // canonballs only
            {
                _chosenProjectile = Instantiate(_canonBallPrefab, _shootPosition.position, Quaternion.identity);
            }
            else if (_projectileType == ProjectileType.Feather) // feathers only
            {
                _chosenProjectile = Instantiate(_featherPickupPrefab, _shootPosition.position, Quaternion.identity);               
            }
            else // randomly shoots both
            {
                var randomValue = UnityEngine.Random.Range(0, 2);
                if(randomValue == 0)
                {
                    _chosenProjectile = Instantiate(_canonBallPrefab, _shootPosition.position, Quaternion.identity);
                }
                else
                    _chosenProjectile = Instantiate(_featherPickupPrefab, _shootPosition.position, Quaternion.identity);
            }

            var shootDir = _shootPosition.right.normalized;
            if (transform.parent.localScale.x < 0) // if i mirrored the cannon, reverse shoot direction
            {
                shootDir *= -1;
            }

            _chosenProjectile.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            _chosenProjectile.GetComponent<Rigidbody2D>().velocity = _speed * Time.fixedDeltaTime * shootDir; // adds the velocity to the spawned object

            _timer = 0;

            //GameObject projectile = Instantiate(_projectile, gameObject.transform.position, Quaternion.Euler(transform.eulerAngles.z * -1, 90, 0));
            //projectile.GetComponent<Projectile>().speed = _speed;

            //if (_projectileType == ProjectileType.Feather)
            //{
            //    projectile.GetComponent<Projectile>().feather = true;
            //}
            //else if (_projectileType == ProjectileType.Random)
            //{
            //    int randomNumber = Random.Range(0, 2);

            //    if (randomNumber > 0)
            //    {
            //        projectile.GetComponent<Projectile>().feather = true;
            //    }
            //    else
            //    {
            //        projectile.GetComponent<Projectile>().feather = false;
            //    }
            //}

            //timer = 0;
        }       
    }


    //private void Shoot()
    //{
    //    _chosenProjectile.GetComponent<Rigidbody2D>().velocity = Vector3.forward * Time.deltaTime * _speed; // adds the velocity to the spawned object
    //}
}
