using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    // source -> ht"t"ps://www.raywenderlich.com/847-object-pooling-in-unity


    public static ObjectPooler SharedInstance;


    public List<GameObject> PooledObjects;
    public GameObject ObjectToPool;
    public int NumberToPool;

    public Transform ParentTransform;


    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        PooledObjects = new List<GameObject>();

        for (int i = 0; i < NumberToPool; i++)
        {
            //GameObject obj = (GameObject)Instantiate(ObjectToPool,ParentTransform); // normally you don't give a parent, but it might make things better here
            GameObject obj = Instantiate(ObjectToPool);
            obj.SetActive(false);
            PooledObjects.Add(obj);
        }
    }




    public GameObject GetPooledObject()
    {
        //1
        for (int i = 0; i < PooledObjects.Count; i++)
        {
            //2
            if (!PooledObjects[i].activeInHierarchy)
            {
                return PooledObjects[i];
            }
        }
        //3   
        return null;
    }
}
