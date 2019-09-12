using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{


    #region Variable
    [SerializeField] private int m_GameScore;
    [SerializeField] private int m_PausedToggle;
    #endregion

    #region Getter and Setter
    public int GameScore
    {
        get
        {
            return m_GameScore;
        }

        set
        {
            m_GameScore = value;
        }
    }
    public int PausedToggle
    {
        get
        {
            return m_PausedToggle;
        }

        set
        {
            m_PausedToggle = value;
        }
    }
    #endregion

    private void OnEnable()
    {
        EventManager.StartListening(E_EventName.Enemy_Death, IncreaseScore);
    }
    private void OnDisable()
    {
        EventManager.StopListening(E_EventName.Enemy_Death, IncreaseScore);
    }

    private void IncreaseScore(EventParam obj)
    {
        try
        {
            Dictionary<E_ValueIdentifer, object> eo = obj.EventObject;

            object enemyReference;
            if (eo.TryGetValue(E_ValueIdentifer.GameObject_Enemy, out enemyReference))
            {
                GameObject enemeyObject = (GameObject)enemyReference;

                m_GameScore += enemeyObject.GetComponent<EnemyController>().EnemyScore;
            }
            else
            {
                EventManager.EventDebugLog("Value does not exist");
            }

            EventManager.FinishEvent(obj.EventName);
        }
        catch (Exception e)
        {
            EventManager.EventDebugLog(e.ToString());
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EventManager.TriggerEvent();

        }



    }

}
