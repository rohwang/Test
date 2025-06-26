using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;


public class PlayCinematic : MonoBehaviour
{
    public Collider2D col;
    public CinemachineCamera cinecamera;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("�÷��̾� ����");
            StartCoroutine(PlayCinematicAnimation());
        }
    }
    public IEnumerator PlayCinematicAnimation()
    {
        Debug.Log("�Լ� �ߵ�");
        cinecamera.Priority = 9;
        yield return new WaitForSeconds(4f);
        cinecamera.Priority = 0;
        cinecamera.gameObject.SetActive(false);
    }
}
