using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Controller : MonoBehaviour {

    private int enemyID;
    private GameObject spawnerManager;

    public int EnemyID
    {
        get { return enemyID; }
        set
        {
            enemyID = value; this.gameObject.name = "Enemy Clone " + value;
        }
    }

    public GameObject SpawnerManager
    {
        get { return spawnerManager; }
        set { spawnerManager = value; }
    }


    private void OnDisable()
    {
        Debug.Log("Deleted");
        spawnerManager.GetComponent<Spawner_Manager>().enemyAliveList.Remove(enemyID);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    
}
