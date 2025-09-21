using System.Collections;
using UnityEngine;

public class FloorAttack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    Transform playerTransform;
    void Start()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(3f);
        {
            // transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
