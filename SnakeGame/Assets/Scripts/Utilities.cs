using UnityEngine;

public class Utilities
{
    public enum GamePlayState
    {
        Play,
        Pause,
        GameOver,
    }
    
    public static void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}

