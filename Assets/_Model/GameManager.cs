using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    #region Variable
    private static GameManager m_Instance;
    [SerializeField] private int m_GameScore;
    [SerializeField] private int m_PausedToggle;
    #endregion

    #region Getter and Setter
    public static GameManager Instance { get { return m_Instance; } }
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

    private void Awake()
    {
        if (m_Instance != null && m_Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            m_Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        EventManager.StartListening(E_EventName.Enemy_Death, IncreaseScore);
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        EventManager.StopListening(E_EventName.Enemy_Death, IncreaseScore);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    //Called when a scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        EventManager.TriggerEvent(E_EventName.Start_Level);
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
            //EventManager.TriggerEvent();

        }



    }

}
