using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUIController : MonoBehaviour
{
    #region Variable
    [SerializeField] private GameObject m_UIPanel;
    #endregion

    #region Getter and Setter
    public GameObject UIPanel
    {
        get
        {
            return m_UIPanel;
        }

        set
        {
            m_UIPanel = value;
        }
    }
    #endregion

    private void OnEnable()
    {
        EventManager.StartListening(E_EventName.Pause_Game, PauseGame);
        EventManager.StartListening(E_EventName.Resume_Game, ResumeGame);
    }
    private void OnDisable()
    {
        EventManager.StopListening(E_EventName.Pause_Game, PauseGame);
        EventManager.StopListening(E_EventName.Resume_Game, ResumeGame);
    }

    private void ResumeGame(EventParam obj)
    {
        Time.timeScale = 1;
        m_UIPanel.SetActive(false);
    }

    private void PauseGame(EventParam obj)
    {
        Time.timeScale = 0;
        m_UIPanel.SetActive(true);
    }

}
