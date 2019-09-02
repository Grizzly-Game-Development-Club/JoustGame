using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager m_SpawnManager;

    #region Variable
    private List<GameObject> m_SpawnPoints;
    private List<GameObject> m_AvailableSpawnPoints;
    private List<SpawnWave> m_SpawnWaveList;
    private static SpawnWave m_CurrentWave;
    private bool m_CountdownToggle;
    #endregion

    #region Getter & Setter
    public static List<GameObject> SpawnPoints
    {
        get
        {
            return m_SpawnPoints;
        }

        set
        {
            m_SpawnPoints = value;
        }
    }
    public static List<GameObject> AvailableSpawnPoints
    {
        get
        {
            return m_AvailableSpawnPoints;
        }

        set
        {
            m_AvailableSpawnPoints = value;
        }
    }
    public List<SpawnWave> SpawnWaveList
    {
        get
        {
            return m_SpawnWaveList;
        }

        set
        {
            m_SpawnWaveList = value;
        }
    }
    public static SpawnWave CurrentWave
    {
        get
        {
            return m_CurrentWave;
        }

        set
        {
            m_CurrentWave = value;
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


    private void Awake()
    {
        SpawnPoints = new List<GameObject>();
        SpawnWaveList = new List<SpawnWave>();
        CountdownToggle = false;
    }

    private void OnEnable()
    {
        EventManager.StartListening(E_EventName.Spawner_Available, AddSpawner);
        EventManager.StartListening(E_EventName.Spawner_Unavailable, RemoveSpawner);
        EventManager.StartListening(E_EventName.Set_Level, SetValue);
        EventManager.StartListening(E_EventName.Start_Level, StartSpawner);
    }

    private void AddSpawner(EventParam obj)
    {
        
        throw new NotImplementedException();
    }

    private void RemoveSpawner(EventParam obj)
    {
        throw new NotImplementedException();
    }

    private void SetValue(EventParam obj)
    {
        CurrentWave = (SpawnWave)SpawnWaveList[0].Clone();
    }

    private void StartSpawner(EventParam obj)
    {
        StopAllCoroutines();
        StartCoroutine("CountDownTime");
        StartCoroutine("Spawn");
    }

    

    private void Update()
    {
        
    }

    public IEnumerator Spawn()
    {
        SpawnWave currentWave = CurrentWave;
        while (currentWave.EnemyCount != 0)
        {


            yield return new WaitForSeconds(currentWave.SpawnRate);
        }
    }

    IEnumerator CountDownTime()
    {
        while (CountdownToggle)
        {
            int timeLeft = CurrentWave.TimeDuration;

            if (timeLeft <= 0)
            {
                EventManager.TriggerEvent(E_EventName.Game_Over);
            }

            yield return new WaitForSeconds(1);
            timeLeft--;

            CurrentWave.TimeDuration = timeLeft;
        }
    }

[System.Serializable]
public class SpawnWave : ICloneable
{
    #region Variable
    private int m_WaveID;
    private GameObject m_EnemyPrefab;
    private int m_EnemyCount;
    private float m_SpawnRate;
    private int m_TimeDuration;
    #endregion

    #region Getter & Setter
    public int WaveID
    {
        get
        {
            return m_WaveID;
        }

        set
        {
            m_WaveID = value;
        }
    }
    public GameObject EnemyPrefab
    {
        get
        {
            return m_EnemyPrefab;
        }

        set
        {
            m_EnemyPrefab = value;
        }
    }
    public int EnemyCount
    {
        get
        {
            return m_EnemyCount;
        }

        set
        {
            m_EnemyCount = value;
        }
    }
    public float SpawnRate
    {
        get
        {
            return m_SpawnRate;
        }

        set
        {
            m_SpawnRate = value;
        }
    }
    public int TimeDuration
    {
        get
        {
            return m_TimeDuration;
        }

        set
        {
            m_TimeDuration = value;
        }
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
    #endregion
}
}
