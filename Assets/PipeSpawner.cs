using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    public float maxTime = 1;
    private float timer = 0;
    public GameObject pipe;
    public GameObject altar;
    public float height;

    public GameObject bottomTile;

    private float scaleTime = 1;
    private float scaleSpeed = 1;
    private bool excellarate = true;

    public float offset = 0;

    private bool spawnPipes = true;
    private bool eggPillar = false;
    private int numberPipes = 3;
    private int maxPipes = 4;
    private int maxPipesScale = 1;

    public GameObject[] flowers = new GameObject[10];

    public int maxRandom = 80;

    void Start()
    {
        timer = 0;
        GameObject newpipe = Instantiate(pipe);
        GameObject newbottom = Instantiate(bottomTile);
        newpipe.transform.position = transform.position + new Vector3(0, Random.Range(-height, height), 0);
        newbottom.transform.parent = newpipe.transform;
    }

    private void Update()
    {
        print("scaleSpeed: " + scaleSpeed);
        print("scaleTime: " + scaleTime);
        print("maxTime: " + maxTime);
        print("height: " + height);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(numberPipes > maxPipes)
        {
            spawnPipes = false;
            eggPillar = true;
        }

        if (spawnPipes)
        {
            if (timer > maxTime)
            {
                numberPipes++;
                GameObject newpipe = Instantiate(pipe);
                GameObject newbottom = Instantiate(bottomTile);
                newpipe.transform.position = transform.position + new Vector3(0, Random.Range(-height, height), 0);
                newpipe.GetComponent<move>().speed *= scaleSpeed;

                if (offset < 0.8)
                {
                    offset += 0.05f;
                }
                newpipe.GetComponent<move>().offset = offset;
                newbottom.transform.parent = newpipe.transform;

                // flowers
                int randomNumber = Random.Range(0, maxRandom);

                if ((float)randomNumber % 2 == 0)
                {
                    GameObject newFlower = Instantiate(flowers[Random.Range(0, flowers.Length)]);
                    newFlower.transform.parent = newbottom.transform;
                }
                else
                {
                    maxRandom--;
                }

                Destroy(newpipe, 15);
                timer = 0;
            }

            timer += Time.deltaTime;

            if (excellarate)
            {
                StartCoroutine(Faster());
            }
        }
        
        if(eggPillar)
        {
            maxPipesScale++;
            maxPipes += (maxPipesScale * 5);
            StartCoroutine(SpawnAltar());
            eggPillar = false;
        }
    }

    IEnumerator Faster()
    {
        excellarate = false;
        yield return new WaitForSeconds(3 * scaleTime);
        
        if(scaleTime < 2.7)
        {
            maxTime -= 0.1f;
        }

        scaleTime += 0.1f;
        scaleSpeed += 0.04f;
        height -= 0.05f;
        excellarate = true;
    }

    IEnumerator SpawnAltar()
    {
        yield return new WaitForSeconds(5);
        GameObject newAltar = Instantiate(altar);
        newAltar.GetComponent<move>().speed *= scaleSpeed;
        yield return new WaitForSeconds(4);
        spawnPipes = true;
    }
}
