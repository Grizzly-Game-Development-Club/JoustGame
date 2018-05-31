using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public enum SpawnState { SPAWNING, WAITING, COUNTING };

    [System.Serializable]
    public class wave
    {
        public string name;
        public Transform enemy;
        public int count;
        public float rate;

    }

    public wave[] waves;
    private int nextWave = 0;

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f;
    public float waveCountDown;

    private float searchCountDown = 1f;

    private SpawnState state = SpawnState.COUNTING;


	// Use this for initialization
	void Start () {

        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No Spawn Points Referenced");
        }

        waveCountDown = timeBetweenWaves;

}
	
	// Update is called once per frame
	void Update ()
    {
        if(state == SpawnState.WAITING)
        {
            //Check if enemies are still alive
            if (!EnemyIsAlive())
            {
                //Begin new round
                WaveCompleted();
               

            }
            else return;
        }

		if (waveCountDown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave ( waves[nextWave] ) );
            }
        }
        else
        {
            waveCountDown -= Time.deltaTime;
        }
	}

    void WaveCompleted()
    {

        Debug.Log("wave completed");

        state = SpawnState.COUNTING;
        waveCountDown = timeBetweenWaves;
        if (nextWave + 1 > waves.Length - 1)
        {
            //Can enter end screen or a different event here
            nextWave = 0;
            Debug.Log("All waves Complete. Looping...");
        }
        else
        {
            nextWave++;
        }
 
    }

    bool EnemyIsAlive()
    {
        searchCountDown -= Time.deltaTime;
        if (searchCountDown <= 0f)
        {
            searchCountDown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave(wave _wave)
    {
        Debug.Log("Spawning wave:" + _wave.name);
        state = SpawnState.SPAWNING;

        for(int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        state = SpawnState.WAITING;

        yield break;
    }

    void SpawnEnemy(Transform _enemy)
    {
        //spawn enemy
        Debug.Log("Spawning Enemy" + _enemy.name);



        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation);
        
    }
}
