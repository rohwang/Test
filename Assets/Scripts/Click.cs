using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class Click : MonoBehaviour
{
    public Image image;
    public void Awake()
    {
        image = GetComponent<Image>();
    }
    public void DoClick()
    {
        StartCoroutine(WhenClick());
    }
    public IEnumerator WhenClick()
    {
        image.tintColor = Color.black;
        yield return new WaitForSeconds(0.5f);
        image.tintColor = Color.white;
    }
}
