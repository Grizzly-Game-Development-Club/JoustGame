using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class DeathMenu : MonoBehaviour
{

    public EventSystem eventSystem;
    public GameObject selectedObject;

    private bool buttonSelected;

    //Reference to Game Manager Script
    JoustGameManager gameManager;

    // Use this for initialization
    void Start()
    {
        if (Input.GetAxisRaw("Vertical") != 0 && buttonSelected == false)
        {
            eventSystem.SetSelectedGameObject(selectedObject);
            buttonSelected = true;
        }

        //Set Game Manager
        gameManager = GameObject.Find("Game Manager").GetComponent<JoustGameManager>();
    }

    public void playAgain()
    {
        Scene loadedLevel = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadedLevel.buildIndex);
        Time.timeScale = 1;
    }
}
