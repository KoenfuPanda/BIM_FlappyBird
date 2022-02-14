using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private GameObject triggerRoot;
    private GameObject gateRoot;

    private void Start()
    {
        triggerRoot = transform.Find("TriggerRoot").gameObject;
        gateRoot = transform.Find("GateRoot").gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        triggerRoot.transform.Rotate(0, 0, 50);
        gateRoot.GetComponent<Animator>().enabled = true;
    }
}
