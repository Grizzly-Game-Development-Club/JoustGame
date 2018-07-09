using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour {

    enum SpawnPointStatus { AVAILABLE, UNAVAILABLE };

    private int spawnerID;
    private GameObject enemy;
    private GameObject spawnerManager;
    private SpawnPointStatus availability;

    private Vector2 spawnVector2;

    public int SpawnerID
    {
        get { return spawnerID; }
        set {
            spawnerID = value; this.gameObject.name = "Enemy Spawner Point " + value;
        }
    }

    public GameObject Enemy
    {
        get { return enemy; }
        set { enemy = value;
            Vector2 enemySize = value.GetComponent<BoxCollider2D>().size;
            this.gameObject.GetComponent<BoxCollider2D>().size = enemySize;
            this.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, enemySize.y/2);
        }
    }

    public GameObject SpawnerManager
    {
        get { return spawnerManager; }
        set { spawnerManager = value; }
    }

    public bool Availability
    {
        get {
            switch (availability)
            {
                case SpawnPointStatus.AVAILABLE:
                    return true;
                case SpawnPointStatus.UNAVAILABLE:
                    return false;
                default:
                    Debug.Log("Error, SpawnPointState has not been set");
                    return false;
            }
        }
        set {
            if (value == true)
            {
                this.availability = SpawnPointStatus.AVAILABLE;
            }
            else if (value == false)
            {
                this.availability = SpawnPointStatus.AVAILABLE;
            }
            else {
                Debug.Log("Error: Not Valid Input");
            }
        }
    }

    public Vector2 SpawnVector2
    {
        get
        {
            if (enemy != null)
            {
                float enemyBottomEdge = enemy.GetComponent<BoxCollider2D>().size.y / 2;
                spawnVector2 = new Vector2(transform.position.x, transform.position.y + enemyBottomEdge);
                return spawnVector2;
            }
            else {
                Debug.Log("Enemy has not been set");
                return spawnVector2;
            }
        }

        set
        {
            spawnVector2 = value;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        availability = SpawnPointStatus.UNAVAILABLE;
        if (spawnerManager.GetComponent<Spawner_Manager>().availableSpawnPoints.Contains(this.spawnerID))
        {
            spawnerManager.GetComponent<Spawner_Manager>().availableSpawnPoints.Remove(this.spawnerID);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        availability = SpawnPointStatus.AVAILABLE;
        if (!spawnerManager.GetComponent<Spawner_Manager>().availableSpawnPoints.Contains(this.spawnerID)) {
            spawnerManager.GetComponent<Spawner_Manager>().availableSpawnPoints.Add(this.spawnerID);
        }
    }
}
