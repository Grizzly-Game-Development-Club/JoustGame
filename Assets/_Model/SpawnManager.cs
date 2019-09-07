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
    private List<String> m_WaveInfo;
    #endregion

    #region Getter & Setter
    public List<GameObject> SpawnPoints
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
    public List<GameObject> AvailableSpawnPoints
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
    public List<string> WaveInfo
    {
        get
        {
            return m_WaveInfo;
        }

        set
        {
            m_WaveInfo = value;
        }
    }
    #endregion

    private void Awake()
    {
        WaveInfo = new List<string>(){m_SpawnWaveList.get};
        SpawnPoints = new List<GameObject>();
        SpawnWaveList = new List<SpawnWave>();
    }

    private void OnEnable()
    {
        EventManager.StartListening(E_EventName.Add_Spawner, AddSpawner);
        EventManager.StartListening(E_EventName.Spawner_Available, AddAvailableSpawner);
        EventManager.StartListening(E_EventName.Spawner_Unavailable, RemoveUnavaliableSpawner);

        EventManager.StartListening(E_EventName.Enemy_Spawned, AddEnemy);
        EventManager.StartListening(E_EventName.Enemy_Death, RemoveEnemy);

        EventManager.StartListening(E_EventName.Start_Level, StartSpawner);
    }

    private void RemoveEnemy(EventParam obj)
    {
        try
        {
            Dictionary<E_ValueIdentifer, object> eo = obj.EventObject;

            object enemyReference;
            if (eo.TryGetValue(E_ValueIdentifer.Enemy_GameObject, out enemyReference))
            {
                EnemyAlive.Remove((GameObject)enemyReference);
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

    private void AddEnemy(EventParam obj)
    {
        try
        {
            Dictionary<E_ValueIdentifer, object> eo = obj.EventObject;

            object enemyReference;
            if (eo.TryGetValue(E_ValueIdentifer.Enemy_GameObject, out enemyReference))
            {
                EnemyAlive.Add((GameObject)enemyReference);
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
        foreach (SpawnWave currentSpawnWave in SpawnWaveList)
        {
            //Wave Information
            int[] waveInfo = { SpawnWaveList.IndexOf(currentSpawnWave), SpawnWaveList.Count };
            int timeLeft = currentSpawnWave.TimeDuration;

            //Pass Wave Information to the UI
            Dictionary<E_ValueIdentifer, object> WaveValueEventObject = new Dictionary<E_ValueIdentifer, object>();
            WaveValueEventObject.Add(E_ValueIdentifer.WaveInfo_Array_Int, waveInfo);
            WaveValueEventObject.Add(E_ValueIdentifer.Time_Left_Int, timeLeft);
            EventManager.TriggerEvent(E_EventName.Set_Wave_Value, WaveValueEventObject);

            //Pause Before Wave Spawn
            yield return new WaitForSeconds(currentSpawnWave.PauseBeforeWave);

            //Pass Value to Toggle Countdown
            Dictionary<E_ValueIdentifer, object> SpawnerStartedEventObject = new Dictionary<E_ValueIdentifer, object>();
            SpawnerStartedEventObject.Add(E_ValueIdentifer.Countdown_Toggle_Bool, true);
            EventManager.TriggerEvent(E_EventName.Spawner_Started, SpawnerStartedEventObject);

            //Spawn All Enemy in Wave
            for (int count = 0; count <= currentSpawnWave.EnemyCount; count++)
            {
                //Select Random Spawn Point and spawn monster
                int randomSpawnNum = UnityEngine.Random.Range(0, AvailableSpawnPoints.Count);
                AvailableSpawnPoints[randomSpawnNum].GetComponent<Spawner>().Spawn(currentSpawnWave.EnemyPrefab);

                //Wait depending on spawn rate
                yield return new WaitForSeconds(1 * currentSpawnWave.SpawnRate);
            }

            //Wait until all the enemy are dead
            yield return new WaitUntil(() => EnemyAlive.Count == 0);

            //Pass Value to Toggle Countdown
            Dictionary<E_ValueIdentifer, object> WaveCompleteEventObject = new Dictionary<E_ValueIdentifer, object>();
            WaveCompleteEventObject.Add(E_ValueIdentifer.Countdown_Toggle_Bool, true);
            EventManager.TriggerEvent(E_EventName.Wave_Complete, WaveCompleteEventObject);

        }

    }

}

[System.Serializable]
public class SpawnWave
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
    #endregion
}

