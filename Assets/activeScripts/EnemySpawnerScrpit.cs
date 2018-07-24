using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScrpit : MonoBehaviour {

    public GameObject Enemy;
    public float spawnRate = 5f;
    private Vector2 whereToSpawn;
    private float nextSpawn = 0.0f;
    private float enemyBottomEdge = 0;
    private bool spaceOccupied;

    // Use this for initialization
    void Start () {
        enemyBottomEdge = Enemy.GetComponent<BoxCollider2D>().size.y / 2;
        spaceOccupied = false;
    }
	
	// Update is called once per frame
	void Update () {
		if(Time.time > nextSpawn && spaceOccupied == false )
        {
            nextSpawn = Time.time + spawnRate;
            whereToSpawn = new Vector2(transform.position.x, transform.position.y + enemyBottomEdge);
            Instantiate(Enemy, whereToSpawn, Quaternion.identity);
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        spaceOccupied = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        spaceOccupied = false;
    }
}
