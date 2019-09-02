using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Single_Mode_UI_Controller : MonoBehaviour
{

    #region Variable
    [SerializeField] private GameObject m_UIPanel;
    [SerializeField] private TextMeshProUGUI m_ScoreText;
    [SerializeField] private TextMeshProUGUI m_TimeDurationText;
    [SerializeField] private TextMeshProUGUI m_WaveText;


    private int[] m_TimeLeft;
    private int[] m_Wave;
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
    #endregion

    private void Awake()
    {
        m_TimeLeft = new int[2];
        m_Wave = new int[2];
    }

    public void OnEnable()
    {
        EventManager.StartListening(E_EventName.Set_Level, SetLevel);
        EventManager.StartListening(E_EventName.Set_Level, StartCountdown);
        EventManager.StartListening(E_EventName.Pause_Game, StopCountdown);
    }

    private void StopCountdown(EventParam obj)
    {
        throw new NotImplementedException();
    }

    private void StartCountdown(EventParam obj)
    {
        Start
    }

    //When the Level is loaded, set up the wave num, and duration
    public void SetLevel(EventParam obj)
    {
        E_EventName en = obj.EventName;
        EventObject eo = obj.EventObject;

        foreach (String es in eo.TypeString)
        {
            string[] esArray = es.Split(':');

            switch (esArray[0])
            {
                case "Time Left Minute":
                    m_TimeLeft[0] = int.Parse(esArray[1]);
                    break;
                case "Time Left Second":
                    m_TimeLeft[1] = int.Parse(esArray[1]);
                    break;
                case "Wave Current":
                    m_Wave[0] = int.Parse(esArray[1]);
                    break;
                case "Wave Total":
                    m_Wave[1] = int.Parse(esArray[1]);
                    break;
            }
        }

        EventManager.FinishEvent(en);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        TimeDurationText.SetText(String.Format("Time: {0}:{1}", m_TimeLeft[0], m_TimeLeft[1]));
        WaveText.SetText(String.Format("Wave: {0}/{1}", m_Wave[0], m_Wave[1]));   
    }

    void StartCountdown()
    {


    }

    IEnumerator CountDownTime()
    {
        while (true)
        {
            int minute = m_TimeLeft[0];
            int second = m_TimeLeft[1];

            if (minute <= 0 && second <= 0)
            {
                EventManager.TriggerEvent(E_EventName.Game_Over);
            }
            if (second <= 0)
            {
                minute--;
                second = 60;
            }
            
            yield return new WaitForSeconds(1);
            second--;

            m_TimeLeft[0] = minute;
            m_TimeLeft[1] = second;
        }
    }

}
