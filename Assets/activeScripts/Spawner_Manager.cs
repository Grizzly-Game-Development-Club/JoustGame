using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_Manager : MonoBehaviour {


    /* Spawning - Currently in the process of spawning a wave
     * Waiting - After a wave has been spawn, state keeps checking if all enemy has died
     * Counting - Used at starts to count how many spawn point there is
     */
    public enum SpawnManagerState { SPAWNING, WAITING, COUNTING };

    
    //Wave Class
    [System.Serializable]
    public class wave
    {
        public string name;
        public GameObject enemy;
        public int count;
        public float rate;
    }


    //Wave Array of wave class
    public wave[] waves;

    //Reference to all spawnpoint gameobject
    public GameObject[] spawnPoints;

    public List<int> availableSpawnPoints;

    //Time until next wave
    public float timeBetweenWaves = 5f;

    //Hold Countdown time for wave spawning
    public float waveCountDown;

    //Spawn State
    private SpawnManagerState state = SpawnManagerState.COUNTING;

    System.Random ran = new System.Random();

    //
    private int nextWave = 0;


    private float searchCountDown = 1f;


    
    


	// Use this for initialization
	void Start () {

        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No Spawn Points Referenced");
        }
        createSpawnPoints();

        waveCountDown = timeBetweenWaves;

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (waveCountDown <= 0)
        {
            if (state != SpawnManagerState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountDown -= Time.deltaTime;
        }

        /*
        if (state == SpawnManagerState.WAITING)
        {
            //Check if enemies are still alive
            if (!EnemyIsAlive())
            {
                //Begin new round
                WaveCompleted();
               

            }
            else return;
        }
        */


    }

    void createSpawnPoints()
    {
        for (int counter = 0; counter <= spawnPoints.Length-1; counter++)
        {
            Debug.Log("Currrent Counter:" + counter);
            spawnPoints[counter].GetComponent<Enemy_Spawner>().SpawnerID = counter;
            spawnPoints[counter].GetComponent<Enemy_Spawner>().Enemy = waves[0].enemy;
            spawnPoints[counter].GetComponent<Enemy_Spawner>().SpawnerManager = this.gameObject;
            spawnPoints[counter].GetComponent<Enemy_Spawner>().Availability = true;
            availableSpawnPoints.Add(counter);
            
        }
    }

    IEnumerator SpawnWave(wave _wave)
    {
        Debug.Log("Spawning wave:" + _wave.name);
        state = SpawnManagerState.SPAWNING;

        for (int i = 0; i < _wave.count; i++)
        {
            //Finds a random spawner that is not occupied
            if (availableSpawnPoints.Count != 0) {
                int randomSpawnerNumber = ran.Next(0, availableSpawnPoints.Count-1);
                int randomSpawnID = availableSpawnPoints[randomSpawnerNumber];

                SpawnEnemy(_wave.enemy, spawnPoints[randomSpawnID].GetComponent<Enemy_Spawner>().SpawnVector2);
                yield return new WaitForSeconds(1f / _wave.rate);
            }
            else {
                Debug.Log("No Available SpawnPoint");
                yield return new WaitForSeconds(1f);
            }


            
        }

        state = SpawnManagerState.WAITING;

        yield break;
    }

    /*
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
    */

    /*
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
    */


    void checkSpawnerOccupation() {

    }

    void SpawnEnemy(GameObject enemy, Vector2 spawnPoint)
    {
        //spawn enemy
        //Debug.Log("Spawning Enemy" + enemy.name);

        Instantiate(enemy, spawnPoint, Quaternion.identity);

    }
}
