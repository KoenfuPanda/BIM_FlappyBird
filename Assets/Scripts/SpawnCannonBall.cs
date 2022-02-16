using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCannonBall : MonoBehaviour
{
    [SerializeField] private GameObject _projectile;
    [SerializeField] private float _loopTime;
    [SerializeField] private float _speed;
    private enum ProjectileType { Cannonball, Feather, Random }
    [SerializeField] private ProjectileType _projectileType;

    private float timer = 0;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > _loopTime)
        {
            GameObject projectile = Instantiate(_projectile, gameObject.transform.position, Quaternion.Euler(transform.eulerAngles.z * -1, 90, 0));
            projectile.GetComponent<Projectile>().speed = _speed;

            if (_projectileType.ToString() == "Feather")
            {
                projectile.GetComponent<Projectile>().feather = true;
            }
            else if (_projectileType.ToString() == "Random")
            {
                int randomNumber = Random.Range(0, 2);

                if (randomNumber > 0)
                {
                    projectile.GetComponent<Projectile>().feather = true;
                }
                else
                {
                    projectile.GetComponent<Projectile>().feather = false;
                }
            }

            timer = 0;
        }       
    }
}
