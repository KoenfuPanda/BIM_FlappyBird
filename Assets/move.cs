using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public GameObject pillarUp;
    public GameObject pillarDown;

    public float speed;

    public float offset = 0;

    void Start()
    {
        if(pillarUp)
        {
            StartCoroutine(PositionPillars());        
        }
    }

    void FixedUpdate()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;        
    }

    IEnumerator PositionPillars()
    {
        yield return new WaitForSeconds(0.5f);
        pillarUp.transform.position = new Vector3(pillarUp.transform.position.x, pillarUp.transform.position.y - offset, pillarUp.transform.position.z);
        pillarDown.transform.position = new Vector3(pillarDown.transform.position.x, pillarDown.transform.position.y + offset, pillarDown.transform.position.z);

        yield return new WaitForSeconds(0.5f);

        if (Score.score > 45)
        {
            GetComponent<Animator>().enabled = true;
        }

        if(Score.score > 85)
        {
            float animationSpeed = 2;

            if (Score.score > 130)
            {
                animationSpeed = Random.Range(1.0f, 2.5f);
            }

            GetComponent<Animator>().speed = animationSpeed;

        }
    }
}