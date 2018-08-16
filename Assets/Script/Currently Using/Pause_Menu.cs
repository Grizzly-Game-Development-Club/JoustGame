using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Pause_Menu : MonoBehaviour
{

    public EventSystem eventSystem;
    public GameObject selectedObject;

    private bool buttonSelected;


    // Use this for initialization
    void Start () {
        if (Input.GetAxisRaw("Vertical") != 0 && buttonSelected == false)
        {
            eventSystem.SetSelectedGameObject(selectedObject);
            buttonSelected = true;
        }

        
    }

    public void resume() {
        
    }

}
