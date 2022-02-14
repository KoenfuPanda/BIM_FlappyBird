using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrink : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        GetComponent<SpriteRenderer>().enabled = false;
        collider.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        StartCoroutine(SetNormalSize(collider.gameObject));
    }

    IEnumerator SetNormalSize(GameObject character)
    {
        yield return new WaitForSeconds(17);
        character.transform.localScale = new Vector3(1, 1, 1);
        Destroy(gameObject);
    }
}
