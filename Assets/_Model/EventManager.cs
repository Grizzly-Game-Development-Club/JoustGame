using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_EventName
{
    Pause_Game, Resume_Game, Game_Over,
    Start_Level, Set_Initial, Set_Wave_Value, Spawner_Started, Spawner_Stopped, Wave_Complete, Wave_Setup, End_Level,
    Enemy_Spawned, Enemy_Death, Add_Spawner, Spawner_Available, Spawner_Unavailable
};

public enum E_ValueIdentifer
{
    WaveInfo_Array_Int,
    Countdown_Toggle_Bool,
    Time_Left_Int,
    Spawner_GameObject,
    Enemy_GameObject
}

public class EventManager : MonoBehaviour
{
    #region Variable
    private static EventManager m_EventManager;
    private Dictionary<E_EventName, Action<EventParam>> m_EventDictionary;
    private bool m_EventDebugMode;
    #endregion

    #region Getter & Setter
    public static EventManager Instance
    {
        get
        {
            if (!m_EventManager)
            {
                m_EventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!m_EventManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    m_EventManager.Init();
                }
            }

            return m_EventManager;
        }
        set
        {
            m_EventManager = value;
        }
    }
    public static EventObject EventObject
    {
        get
        {
            return m_EventObject;
        }

        set
        {
            m_EventObject = value;
        }
    }

    public Dictionary<E_EventName, Action<EventParam>> EventDictionary
    {
        get
        {
            return m_EventDictionary;
        }

        set
        {
            m_EventDictionary = value;
        }
    }
    public bool EventDebugMode
    {
        get
        {
            return m_EventDebugMode;
        }

        set
        {
            m_EventDebugMode = value;
        }
    }
    #endregion

    void Awake()
    {
        Instance = Instance;
    }

    void Init()
    {
        if (m_EventDictionary == null)
        {
            m_EventDictionary = new Dictionary<E_EventName, Action<EventParam>>();
        }
    }

    public static void StartListening(E_EventName eventName, Action<EventParam> listener)
    {
        Action<EventParam> thisEvent;
        if (Instance.EventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent += listener;
            Instance.EventDictionary[eventName] = thisEvent;
            EventDebugLog(String.Format("Event Name Registered: {0} \n Event Method Registered: {1}", eventName, listener.Method.Name));
        }
        else
        {
            thisEvent += listener;
            Instance.m_EventDictionary.Add(eventName, thisEvent);
            EventDebugLog(String.Format("Event Name Registered: {0} \n Event Method Registered: {1}", eventName, listener.Method.Name));
        }

    }

    public static void StopListening(E_EventName eventName, Action<EventParam> listener)
    {
        if (m_EventManager == null) return;
        Action<EventParam> thisEvent;
        if (Instance.EventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;
            Instance.EventDictionary[eventName] = thisEvent;
            EventDebugLog(String.Format("Event Name Unregistered: {0} \n Event Method Unregistered: {1}", eventName, listener.Method.Name));
        }
    }

    public static void TriggerEvent(E_EventName eventName)
    {
        Action<EventParam> thisEvent;

        if (Instance.EventDictionary.TryGetValue(eventName, out thisEvent))
        {
            EventDebugLog(String.Format("Event Started: {0}", eventName.ToString()));
            thisEvent.Invoke(new EventParam(eventName));
            EventDebugLog(String.Format("Event Ended: {0}", eventName.ToString()));
        }
    }

    public static void TriggerEvent(E_EventName eventName, Dictionary<E_ValueIdentifer, object> eventObject)
    {
        
        Action<EventParam> thisEvent;
        if (Instance.EventDictionary.TryGetValue(eventName, out thisEvent))
        {
            EventDebugLog(String.Format("Event Started: {0}", eventName));
            thisEvent.Invoke(new EventParam(eventName, eventObject));
            EventDebugLog(String.Format("Event Ended: {0}", eventName));
        }
    }

    public static void FinishEvent(E_EventName eventName)
    {
        EventDebugLog(String.Format("Event Finished: {0}", eventName));
    }

    public static void EventDebugLog(String eventDebugMessage)
    {
        if (Instance.m_EventDebugMode)
        {
            Debug.Log(String.Format("Event Log: \n {0} \n", eventDebugMessage));
        }
    }

}


public class Variable
{
    public object Value { get; set; }
}

[Serializable]
public class EventParam
{
    #region Variable
    private E_EventName m_EventName;
    private Dictionary<E_ValueIdentifer, object> m_EventObject;
    #endregion

    #region Constructor
    public EventParam(E_EventName m_EventName)
    {
        this.m_EventName = m_EventName;
        this.EventObject = new Dictionary<E_ValueIdentifer, object>();
    }

    public EventParam(E_EventName m_EventName, Dictionary<E_ValueIdentifer, object> m_EventObject)
    {
        this.m_EventName = m_EventName;
        this.m_EventObject = m_EventObject;
    }
    #endregion

    #region Getter & Setter
    public E_EventName EventName
    {
        get
        {
            return m_EventName;
        }

        set
        {
            m_EventName = value;
        }
    }
    public EventObject EventObject
    {
        get
        {
            return m_EventObject;
        }

        set
        {
            m_EventObject = value;
        }
    }
    #endregion
}
