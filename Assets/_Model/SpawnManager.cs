using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager m_SpawnManager;

    #region Variable
    private List<GameObject> m_SpawnPoints = new List<GameObject>();
    private List<GameObject> m_AvailableSpawnPoints = new List<GameObject>();
    private List<GameObject> m_EnemyAlive = new List<GameObject>();
    private List<SpawnWave> m_SpawnWaveList = new List<SpawnWave>();

    private static SpawnWave m_CurrentWave;
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
    public List<GameObject> EnemyAlive
    {
        get
        {
            return m_EnemyAlive;
        }

        set
        {
            m_EnemyAlive = value;
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
    #endregion

    private void Awake()
    {
        SpawnPoints = new List<GameObject>();
        SpawnWaveList = new List<SpawnWave>();
    }

    private void OnEnable()
    {
        EventManager.StartListening(E_EventName.Add_Spawner, AddSpawner);
        EventManager.StartListening(E_EventName.Spawner_Available, AddAvailableSpawner);
        EventManager.StartListening(E_EventName.Spawner_Unavailable, RemoveUnavaliableSpawner);

        EventManager.StartListening(E_EventName.Start_Level, StartSpawner);
    }

    private void RemoveUnavaliableSpawner(EventParam obj)
    {
        try
        {
            Dictionary<E_ValueIdentifer, object> eo = obj.EventObject;

            object spawnerReference;
            if (eo.TryGetValue(E_ValueIdentifer.Spawner_GameObject, out spawnerReference))
            {
                SpawnPoints.Add((GameObject)spawnerReference);
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

    private void AddAvailableSpawner(EventParam obj)
    {
        try
        {
            Dictionary<E_ValueIdentifer, object> eo = obj.EventObject;

            object spawnerReference;
            if (eo.TryGetValue(E_ValueIdentifer.Spawner_GameObject, out spawnerReference))
            {
                SpawnPoints.Add((GameObject)spawnerReference);
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

    private void AddSpawner(EventParam obj)
    {
        try
        {
            Dictionary<E_ValueIdentifer, object> eo = obj.EventObject;

            object spawnerReference;
            if (eo.TryGetValue(E_ValueIdentifer.Spawner_GameObject, out spawnerReference))
            {
                SpawnPoints.Add((GameObject)spawnerReference);
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


    private void StartSpawner(EventParam obj)
    {
        StartCoroutine("Spawner_Couroutine");
    }

    IEnumerator Spawner_Couroutine()
    {
        foreach (SpawnWave spawnWave in SpawnWaveList)
        {
            CurrentWave = (SpawnWave)spawnWave.Clone();
            int[] waveInfo = { SpawnWaveList.IndexOf(spawnWave), SpawnWaveList.Count };
            int timeLeft = spawnWave.TimeDuration;


            Dictionary<E_ValueIdentifer, object> eventObject = new Dictionary<E_ValueIdentifer, object>();
            eventObject.Add(E_ValueIdentifer.WaveInfo_Array_Int, waveInfo);
            eventObject.Add(E_ValueIdentifer.Time_Left_Int, timeLeft);
            EventManager.TriggerEvent(E_EventName.Set_Wave_Value, eventObject);

            yield return WaitForSeconds()


        }

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
    private float m_PauseBeforeWave;
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
    public float PauseBeforeWave
    {
        get
        {
            return m_PauseBeforeWave;
        }

        set
        {
            m_PauseBeforeWave = value;
        }
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
    #endregion
}

