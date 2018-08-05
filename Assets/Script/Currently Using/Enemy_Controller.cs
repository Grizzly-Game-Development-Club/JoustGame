using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global_Enum;


public class Enemy_Controller : MonoBehaviour {

    

    private GameObject spawnerManager;
    public GameObject waypointManager;
    private Spawner_Manager spawnerMangerScript;
    private Waypoint_Manager waypointManagerScript;

    private int enemyID;
    private DIRECTION enemyDirection;
    private STATE enemyState;
    public GameObject nextWaypoint;
    
    

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
        set { spawnerManager = value;
              spawnerMangerScript = value.GetComponent<Spawner_Manager>(); }
    }

    public GameObject WaypointManager
    {
        get { return waypointManager; }
        set { waypointManager = value;
             waypointManagerScript = value.GetComponent<Waypoint_Manager>(); }
    }

    public STATE EnemyState
    {
        get { return enemyState; }
        set { enemyState = value; }
    }

    private void OnDisable()
    {
        if (spawnerManager != null)
        {
            spawnerManager.GetComponent<Spawner_Manager>().enemyAliveList.Remove(enemyID);
        }
        else {
            Debug.Log("No Spawn Manager Detected");
        }
    }

    void GetClosestWaypoint() {

        //this.gameObject.transform;
    }

    // Use this for initialization
    void Start () {
        waypointManagerScript = waypointManager.GetComponent<Waypoint_Manager>();
        enemyState = STATE.SPAWNED;
        
    }

    public float speed = 3f;

    private void MoveTowardsWaypoint()
    {
        // Get the moving objects current position
        Vector3 currentPosition = this.transform.position;

        // Get the target waypoints position
        Vector3 targetPosition = nextWaypoint.GetComponent<Transform>().position;

        // If the moving object isn't that close to the waypoint
        if (Vector3.Distance(currentPosition, targetPosition) > .1f)
        {

            // Get the direction and normalize
            Vector3 directionOfTravel = targetPosition - currentPosition;
            directionOfTravel.Normalize();

            //scale the movement on each axis by the directionOfTravel vector components
            this.transform.Translate(
                targetPosition.x * speed * Time.deltaTime,
                targetPosition.y * speed * Time.deltaTime,
                targetPosition.z * speed * Time.deltaTime,
                Space.World
            );
        }

        //EnemyState = STATE.ARRIVED;
    }

    // Update is called once per frame
    void Update () {
        switch (enemyState) {
            case STATE.SPAWNED:
                nextWaypoint = waypointManagerScript.findNearestWaypoint(this.GetComponent<Transform>(), enemyDirection);
                MoveTowardsWaypoint();
                break;
            case STATE.TRAVELING:
                break; 
            case STATE.ARRIVED:
                break;
            case STATE.INTERRUPTED:
                break;
            case STATE.DEATH:
                break;
        }
	}

    



}
