using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour {

	public void OnButtonStarGameDown()
    {
        SceneManager.LoadScene(1);
    }
    public void OnButtonEndlessDown()
    {
        SceneManager.LoadScene(2);
    }
    public void OnButtonExitGameDown()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
#endif
        Application.Quit();
    }
}
