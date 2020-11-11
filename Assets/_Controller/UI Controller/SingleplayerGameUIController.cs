using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SingleplayerGameUIController : MonoBehaviour
{

    #region Variable
    [SerializeField] private GameObject m_UIPanel;
    [SerializeField] private TextMeshProUGUI m_ScoreText;
    [SerializeField] private TextMeshProUGUI m_TimeDurationText;
    [SerializeField] private TextMeshProUGUI m_WaveText;

    private int m_TimeLeft = 0;
    private int[] m_Wave = { 0, 0 };
    private bool m_CountdownToggle = false;
    #endregion

    #region Getter & Setter
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
    public TextMeshProUGUI ScoreText
    {
        get
        {
            return m_ScoreText;
        }

        set
        {
            m_ScoreText = value;
        }
    }
    public TextMeshProUGUI TimeDurationText
    {
        get
        {
            return m_TimeDurationText;
        }

        set
        {
            m_TimeDurationText = value;
        }
    }
    public TextMeshProUGUI WaveText
    {
        get
        {
            return m_WaveText;
        }

        set
        {
            m_WaveText = value;
        }
    }
    public int TimeLeft
    {
        get
        {
            return m_TimeLeft;
        }

        set
        {
            m_TimeLeft = value;
        }
    }
    public int[] Wave
    {
        get
        {
            return m_Wave;
        }

        set
        {
            m_Wave = value;
        }
    }
    public bool CountdownToggle
    {
        get
        {
            return m_CountdownToggle;
        }

        set
        {
            m_CountdownToggle = value;
        }
    }
    #endregion

    public void OnEnable()
    {
        EventManager.StartListening(E_EventName.Start_Level, StartCountdown);
        EventManager.StartListening(E_EventName.Setup_UI_Wave_Info, SetupUIValue);
        EventManager.StartListening(E_EventName.Countdown_Toggle, ToggleCountdown);
        EventManager.StartListening(E_EventName.Wave_Complete, ToggleCountdown);
    }

    private void ToggleCountdown(EventParam obj)
    {
        try
        {
            Dictionary<E_ValueIdentifer, object> eo = obj.EventObject;

            object toggleValue;
            if (eo.TryGetValue(E_ValueIdentifer.Bool_CountdownToggle, out toggleValue))
            {
                m_CountdownToggle = (bool)toggleValue;
            }
            else
            {
                EventManager.EventDebugLog("Value does not exist");
            }

            
        }
        catch (Exception e)
        {
            EventManager.EventDebugLog(e.ToString());
        }
    }

    private void SetupUIValue(EventParam obj)
    {
        try
        {
            Dictionary<E_ValueIdentifer, object> eo = obj.EventObject;

            object waveInfoValue;
            if (eo.TryGetValue(E_ValueIdentifer.IntArray_WaveInfo, out waveInfoValue))
            {
                int[] waveInfo = (int[])waveInfoValue;
                Wave[0] = waveInfo[0];
                Wave[1] = waveInfo[1];
                TimeLeft = waveInfo[2];
            }
            else
            {
                EventManager.EventDebugLog("Value does not exist");
            }

        }
        catch (Exception e)
        {
            EventManager.EventDebugLog(e.ToString());
        }
    }

    public void OnDisable()
    {

    }
    


    //Start the Coroutine to countdown
    private void StartCountdown(EventParam obj)
    {
        StopAllCoroutines();
        StartCoroutine("Countdown_Coroutine");
    }

    //Set the UI Text
    void SetUIText()
    {
        int minutes = TimeLeft / 60;
        int seconds = TimeLeft % 60;
        TimeDurationText.SetText(String.Format("Time: {0}:{1}", minutes, seconds));
        WaveText.SetText(String.Format("Wave: {0}/{1}", Wave[0], Wave[1]));
    }

    //Count down the time duration of wave if toggled on
    IEnumerator Countdown_Coroutine()
    {
        while (CountdownToggle)
        {
            SetUIText();

            if (TimeLeft == 0)
            {
                EventManager.TriggerEvent(E_EventName.Game_Over);
            }
            yield return new WaitForSeconds(1f);
            TimeLeft--;
        }
    }

}
