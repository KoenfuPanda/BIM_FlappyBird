using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    private GameObject playerRoot;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        playerRoot = collider.gameObject;
        playerRoot.transform.parent.gameObject.GetComponent<ChangeDirection>().speed = 12;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        StartCoroutine(SlowDown());
    }

    IEnumerator SlowDown()
    {
        playerRoot.transform.parent.gameObject.GetComponent<ChangeDirection>().speed = 10;

        yield return new WaitForSeconds(0.5f);
        playerRoot.transform.parent.gameObject.GetComponent<ChangeDirection>().speed = 8;
        
        yield return new WaitForSeconds(0.2f);
        playerRoot.transform.parent.gameObject.GetComponent<ChangeDirection>().speed = 7;
    }
}
