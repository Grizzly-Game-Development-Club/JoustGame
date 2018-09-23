using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global_Enum;

public class Spawner_Manager : MonoBehaviour {

    //Wave Class
    [System.Serializable]
    public class wave
    {
        public string name;
        public GameObject enemy;
        public int count;
        public float rate;
    }

    System.Random ran = new System.Random();

    public List<wave> waves;

    public GameObject[] spawnPoints;
    public List<int> availableSpawnPoints;

    private int enemyIDAssigner = 0;
    public List<int> enemyAliveList;
    private float searchCountDown = 1f;
    public int maxEnemyAllowed;

    public float timeBetweenWaves = 5f;
    public float waveCountDown;
    private int nextWave = 0;

    private SpawnManagerState state = SpawnManagerState.COUNTING;


	// Use this for initialization
	void Start () {

        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No Spawn Points Referenced");
        }
        createSpawnPoints();

        waveCountDown = timeBetweenWaves;
        InvokeRepeating("IncreaseMax", 10, 10);
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
            if (waves.Count != 0) {
                waveCountDown -= Time.deltaTime;
            }
        }

        
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
        


    }

    void IncreaseMax() {
        maxEnemyAllowed += 7;
    }

    void createSpawnPoints()
    {
        for (int counter = 0; counter <= spawnPoints.Length-1; counter++)
        {
            Debug.Log(waves[0].enemy.name);
            spawnPoints[counter].GetComponent<Enemy_Spawner>().SpawnerID = counter;
            spawnPoints[counter].GetComponent<Enemy_Spawner>().Enemy = waves[0].enemy;
            spawnPoints[counter].GetComponent<Enemy_Spawner>().SpawnerManager = this.gameObject;
            spawnPoints[counter].GetComponent<Enemy_Spawner>().Availability = true;
            availableSpawnPoints.Add(counter);   
        }
    }

    IEnumerator SpawnWave(wave _wave)
    {
        state = SpawnManagerState.SPAWNING;

        if(_wave.count != 0){
            //Finds a random spawner that is not occupied
            if (availableSpawnPoints.Count != 0 || enemyAliveList.Count >= maxEnemyAllowed) {
                int randomSpawnerNumber = ran.Next(0, availableSpawnPoints.Count-1);
                int randomSpawnID = availableSpawnPoints[randomSpawnerNumber];

                SpawnEnemy(_wave.enemy, spawnPoints[randomSpawnID].GetComponent<Enemy_Spawner>().SpawnVector2);
                _wave.count--;
                yield return new WaitForSeconds(1f / _wave.rate);
            }
            else {
                Debug.Log("No Available SpawnPoint");
                yield return new WaitUntil(() => availableSpawnPoints.Count != 0);
            }  
        }

        state = SpawnManagerState.WAITING;

        yield break;
    }

    
    void WaveCompleted()
    {

        Debug.Log("wave completed");

        state = SpawnManagerState.COUNTING;
        waveCountDown = timeBetweenWaves;
    }
    

    
    bool EnemyIsAlive()
    {
        searchCountDown -= Time.deltaTime;
        if (searchCountDown <= 0f)
        {
            searchCountDown = 1f;
            if (enemyAliveList.Count == 0)
            {
                return false;
            }
        }
        return true;
    }
    


    void SpawnEnemy(GameObject enemy, Vector2 spawnPoint)
    {

        GameObject enemyReference = Instantiate(enemy, spawnPoint, Quaternion.identity) as GameObject;

        enemyReference.GetComponent<Enemy_Controller>().EnemyID = enemyIDAssigner;
        enemyReference.GetComponent<Enemy_Controller>().SpawnerManager = this.gameObject;
        enemyAliveList.Add(enemyIDAssigner);
        enemyIDAssigner++;


    }
}
