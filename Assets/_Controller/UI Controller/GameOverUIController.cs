using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUIController : MonoBehaviour
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
        EventManager.StartListening(E_EventName.Game_Over, GameOver);
    }
    private void OnDisable()
    {
        EventManager.StopListening(E_EventName.Game_Over, GameOver);
    }

    private void GameOver(EventParam obj)
    {
        Time.timeScale = 0;
        m_UIPanel.SetActive(true);
    }


}
