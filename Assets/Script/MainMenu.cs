using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour {

    public EventSystem eventSystem;
    public GameObject selectedObject;

    private bool buttonSelected;



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxisRaw("Vertical") != 0 && buttonSelected == false) 
        {
            eventSystem.SetSelectedGameObject(selectedObject);
            buttonSelected = true;
        }
    }

    private void OnDisable()
    {
        buttonSelected = false;
    }


    public void PlayGame(int sceneIndex) {
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
        Button[] buttons = GameObject.Find("Main Menu Panel").GetComponentsInChildren<Button>();

        foreach (Button value in buttons) {
            value.interactable = !value.interactable;

        }

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
