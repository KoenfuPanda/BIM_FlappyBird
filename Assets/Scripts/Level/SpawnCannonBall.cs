using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCannonBall : MonoBehaviour
{
    //[SerializeField] private GameObject _projectile;
    [SerializeField] private float _loopTime;
    [SerializeField] private float _speed;
    [SerializeField] private Animator _animator;

    [SerializeField]
    private GameObject _canonBallPrefab, _featherPickupPrefab;
    private GameObject _chosenProjectile;

    [SerializeField]
    private Transform _shootPosition;
    private Vector3 _shootDirection;

    private enum ProjectileType { Cannonball, Feather, Random }
    [SerializeField] private ProjectileType _projectileType;

    private float _timer = 0;

    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _smokeParticlePrefab;

    [SerializeField]
    private ObjectPooler _cannonBallPool, _smokeParticlePool;


    private void Start()
    {
        _timer = _loopTime;

        _audioSource = GetComponent<AudioSource>();

        // calculate shoot direction, put this in update if u want to animate the cannon and shoot properly
        _shootDirection = _shootPosition.right.normalized;
        if (transform.parent.localScale.x < 0) // if i mirrored the cannon, reverse shoot direction
        {
            _shootDirection *= -1;
        }
    }


    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _loopTime)
        {
            // audio clip
            _audioSource.Play();

            // particle effect
            //Instantiate(_smokeParticlePrefab, _shootPosition.position, Quaternion.identity);
            CreateSmokeParticle();

            // animation
            _animator.SetTrigger("Shoot");

            // get pooled object
            CreateCannonballToShoot();


            // dated original logic //

            //if (_projectileType == ProjectileType.Cannonball) // canonballs only
            //{
            //    _chosenProjectile = Instantiate(_canonBallPrefab, _shootPosition.position, Quaternion.identity);
            //}
            //else if (_projectileType == ProjectileType.Feather) // feathers only
            //{
            //    _chosenProjectile = Instantiate(_featherPickupPrefab, _shootPosition.position, Quaternion.identity);               
            //}
            //else // randomly shoots both
            //{
            //    var randomValue = UnityEngine.Random.Range(0, 2);
            //    if(randomValue == 0)
            //    {
            //        _chosenProjectile = Instantiate(_canonBallPrefab, _shootPosition.position, Quaternion.identity);
            //    }
            //    else
            //        _chosenProjectile = Instantiate(_featherPickupPrefab, _shootPosition.position, Quaternion.identity);
            //}

            // velocity adding
            //_chosenProjectile.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //_chosenProjectile.GetComponent<Rigidbody2D>().velocity = _speed * Time.fixedDeltaTime * shootDir; // adds the velocity to the spawned object

            // timer reset
            _timer = 0;
        }       
    }




    private void OnEnable()
    {
        _timer = _loopTime;
    }

    public void CreateCannonballToShoot()
    {
        GameObject cannonBall = _cannonBallPool.GetPooledObject();
        if (cannonBall != null)
        {
            cannonBall.transform.SetParent(this.transform);
            cannonBall.transform.localScale = Vector3.one;
            cannonBall.transform.position = _shootPosition.position;
            cannonBall.transform.rotation = _shootPosition.rotation;
            cannonBall.SetActive(true);

            // velocity adding
            cannonBall.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            cannonBall.GetComponent<Rigidbody2D>().velocity = _speed * Time.fixedDeltaTime * _shootDirection; // adds the velocity to the spawned object
        }
    }

    public void CreateSmokeParticle()
    {
        GameObject smokeParticle = _smokeParticlePool.GetPooledObject();
        if (smokeParticle != null)
        {
            smokeParticle.SetActive(false);
            smokeParticle.transform.SetParent(this.transform);
            smokeParticle.transform.localScale = Vector3.one;
            smokeParticle.transform.position = _shootPosition.position;
            smokeParticle.transform.rotation = _shootPosition.rotation;
            smokeParticle.SetActive(true);
        }
    }
}
