using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void Exit()
    {
        // This quits the game when playing the built application (.exe)
        Application.Quit();

        // This stops the game when testing inside the Unity Editor
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #endif
    }
}