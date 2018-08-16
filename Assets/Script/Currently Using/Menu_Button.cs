using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Menu_Button : MonoBehaviour
{

    public void PlayGame(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
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

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }
}
