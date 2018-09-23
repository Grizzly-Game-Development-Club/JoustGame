using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu_Navigation : MonoBehaviour
{

    public EventSystem eventSystem;
    public GameObject currentSelectedGameObject;
    public GameObject lastSelectedGameObject;

    private bool buttonSelected;



    // Update is called once per frame
    void Update()
    {
        if (lastSelectedGameObject == null)
        {
            currentSelectedGameObject.transform.GetChild(0).gameObject.SetActive(true);
            currentSelectedGameObject.transform.GetChild(1).gameObject.SetActive(true);
        }

        if (Input.GetAxisRaw("Vertical") != 0 && buttonSelected == false)
        {
            EventSystem.SetSelectedGameObject(currentSelectedGameObject);
            buttonSelected = true;
        }
        GetLastGameObjectSelected();
    }

    private void GetLastGameObjectSelected()
    {
        if (eventSystem.currentSelectedGameObject != currentSelectedGameObject)
        {
            lastSelectedGameObject = currentSelectedGameObject;
            LastSelectedGameObject.transform.GetChild(0).gameObject.SetActive(false);
            LastSelectedGameObject.transform.GetChild(1).gameObject.SetActive(false);
            currentSelectedGameObject = eventSystem.currentSelectedGameObject;
            currentSelectedGameObject.transform.GetChild(0).gameObject.SetActive(true);
            currentSelectedGameObject.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        Debug.Log(currentSelectedGameObject.name);
        buttonSelected = false;
    }

    public EventSystem EventSystem
    {
        get { return eventSystem; }
        set { eventSystem = value; }
    }

    public GameObject LastSelectedGameObject
    {
        get { return lastSelectedGameObject; }
        set { lastSelectedGameObject = value; }
    }
}
