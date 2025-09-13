using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Mab B");
    }

    public void Quit()
    {
        //Application.Quit();

        UnityEditor.EditorApplication.isPlaying = false;
    }
}
