using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour {

    public void PlayGame(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void HowToPlay()
    {


    }

    public void Highscore()
    {


    }

    public void Settings()
    {
        /*
        Button[] buttons = GameObject.Find("Main Menu Panel").GetComponentsInChildren<Button>();

        foreach (Button value in buttons)
        {
            value.interactable = !value.interactable;

        }
        */
    }

    public void Credits()
    {


    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }
}
