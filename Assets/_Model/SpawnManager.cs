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
    #endregion

    private void OnEnable()
    {
        EventManager.StartListening(E_EventName.Enemy_Spawned, AddEnemy);
        EventManager.StartListening(E_EventName.Enemy_Death, RemoveEnemy);

        EventManager.StartListening(E_EventName.Add_Spawner, AddSpawner);
        EventManager.StartListening(E_EventName.Add_Spawner, AddAvailableSpawner);
    }

    private void AddEnemy(EventParam obj)
    {
        try
        {
            Dictionary<E_ValueIdentifer, object> eo = obj.EventObject;

            object enemyReference;
            if (eo.TryGetValue(E_ValueIdentifer.GameObject_Enemy, out enemyReference))
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
    private void RemoveEnemy(EventParam obj)
    {
        try
        {
            Dictionary<E_ValueIdentifer, object> eo = obj.EventObject;

            object enemyReference;
            if (eo.TryGetValue(E_ValueIdentifer.GameObject_Enemy, out enemyReference))
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

    private void AddAvailableSpawner(EventParam obj)
    {
        try
        {
            Dictionary<E_ValueIdentifer, object> eo = obj.EventObject;

            object spawnerReference;
            if (eo.TryGetValue(E_ValueIdentifer.GameObject_Spawner, out spawnerReference))
            {
                AvailableSpawnPoints.Add((GameObject)spawnerReference);
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
            if (eo.TryGetValue(E_ValueIdentifer.GameObject_Spawner, out spawnerReference))
            {
                AvailableSpawnPoints.Remove((GameObject)spawnerReference);
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
            if (eo.TryGetValue(E_ValueIdentifer.GameObject_Spawner, out spawnerReference))
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
            //Set up spawner list on first run through
            if (m_SpawnPoints.Count == 0)
            {
                EventManager.TriggerEvent(E_EventName.Setup_Spawner_List);
            }

            //Set Wave Information
            int currentWave = SpawnWaveList.IndexOf(currentSpawnWave);
            int totalWave = SpawnWaveList.Count;
            int waveDuration = currentSpawnWave.TimeDuration;
            int[] waveInfo = new int[3]{ currentWave, totalWave, waveDuration };

            //Pass Wave Information to the UI
            Dictionary<E_ValueIdentifer, object> waveInfoEO = new Dictionary<E_ValueIdentifer, object>();
            waveInfoEO.Add(E_ValueIdentifer.IntArray_WaveInfo, waveInfo);
            EventManager.TriggerEvent(E_EventName.Setup_UI_Wave_Info, waveInfoEO);

            //Pause Before Wave Spawn
            yield return new WaitForSeconds(currentSpawnWave.PauseBeforeWave);

            //Toggle Time Countdown on
            Dictionary<E_ValueIdentifer, object> spawnStartedEO = new Dictionary<E_ValueIdentifer, object>();
            spawnStartedEO.Add(E_ValueIdentifer.Bool_CountdownToggle, true);
            EventManager.TriggerEvent(E_EventName.Countdown_Toggle, spawnStartedEO);

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

            //Wave has been completed
            Dictionary<E_ValueIdentifer, object> waveCompleteEO = new Dictionary<E_ValueIdentifer, object>();
            waveCompleteEO.Add(E_ValueIdentifer.Bool_CountdownToggle, false);
            waveCompleteEO.Add(E_ValueIdentifer.Wave_Victory_Score, currentSpawnWave.WaveVictoryScore);
            EventManager.TriggerEvent(E_EventName.Wave_Complete, waveCompleteEO);
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
    private int m_WaveVictoryScore;
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
    public int WaveVictoryScore
    {
        get
        {
            return m_WaveVictoryScore;
        }

        set
        {
            m_WaveVictoryScore = value;
        }
    }
    #endregion
}

