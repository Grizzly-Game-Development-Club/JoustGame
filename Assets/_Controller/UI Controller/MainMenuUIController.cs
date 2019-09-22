using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuUIController : MonoBehaviour
{
    [SerializeField] private EventSystem m_EventSystemReference;
    [SerializeField] private GameObject m_CurrentSelectedGameObject;
    [SerializeField] private GameObject m_LastSelectedGameObject;
    private bool m_ButtonSelected;

    // Update is called once per frame
    void Update()
    {
        if (LastSelectedGameObject == null)
        {
            CurrentSelectedGameObject.transform.GetChild(0).gameObject.SetActive(true);
            CurrentSelectedGameObject.transform.GetChild(1).gameObject.SetActive(true);
        }

        if (Input.GetAxisRaw("Vertical") != 0 && ButtonSelected == false)
        {
            EventSystemReference.SetSelectedGameObject(CurrentSelectedGameObject);
            ButtonSelected = true;
        }
        GetLastGameObjectSelected();
    }

    private void GetLastGameObjectSelected()
    {
        if (EventSystemReference.currentSelectedGameObject != CurrentSelectedGameObject)
        {
            LastSelectedGameObject = CurrentSelectedGameObject;
            LastSelectedGameObject.transform.GetChild(0).gameObject.SetActive(false);
            LastSelectedGameObject.transform.GetChild(1).gameObject.SetActive(false);

            CurrentSelectedGameObject = EventSystemReference.currentSelectedGameObject;
            CurrentSelectedGameObject.transform.GetChild(0).gameObject.SetActive(true);
            CurrentSelectedGameObject.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void PlayGame(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void ExitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit ();
    #endif
    }

    public EventSystem EventSystemReference
    {
        get
        {
            return m_EventSystemReference;
        }

        set
        {
            m_EventSystemReference = value;
        }
    }
    public GameObject CurrentSelectedGameObject
    {
        get
        {
            return m_CurrentSelectedGameObject;
        }

        set
        {
            m_CurrentSelectedGameObject = value;
        }
    }
    public GameObject LastSelectedGameObject
    {
        get
        {
            return m_LastSelectedGameObject;
        }

        set
        {
            m_LastSelectedGameObject = value;
        }
    }
    public bool ButtonSelected
    {
        get
        {
            return m_ButtonSelected;
        }

        set
        {
            m_ButtonSelected = value;
        }
    }
}
