using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global_Enum;

public class Waypoint_Manager : MonoBehaviour {


    private GameObject[] waypointList;

    // Use this for initialization
    void Start () {
        if (waypointList == null) {
            
            waypointList = GameObject.FindGameObjectsWithTag("Waypoint");
        }
	}


    public GameObject findNearestWaypoint(Transform enemyPosition, Direction enemyDirection) {


        List<GameObject> validTransformList = new List<GameObject>();
        float leastDistance = float.MaxValue;
        GameObject nearestWaypoint = null;

        //Filter out Valid Waypoint Depending on Direction
        for (int counter = 0; counter <= waypointList.Length - 1; counter++)
        {
            GameObject waypointGameobject = waypointList[counter];
            Transform waypointPosition = waypointGameobject.GetComponent<Transform>();

            if (waypointPosition.position.x < enemyPosition.position.x && enemyDirection.Equals(Direction.LEFT)) {
                validTransformList.Add(waypointGameobject);
            }
            else if (enemyPosition.position.x < waypointPosition.position.x && enemyDirection.Equals(Direction.RIGHT)) {
                validTransformList.Add(waypointGameobject);
            }
        }

        //Find the Nearest Waypoint of all the Valid Waypoint
        for (int counter = 0; counter <= validTransformList.Count - 1; counter++)
        {
            GameObject validWaypoint = validTransformList[counter];
            Vector2 start = AsVector2(enemyPosition);
            Vector2 end = AsVector2(validWaypoint.GetComponent<Transform>());

            float distanceBetweenStartAndEnd = Mathf.Sqrt(Mathf.Pow((end.x - start.x), 2) + Mathf.Pow((end.y - start.y), 2));

            if (distanceBetweenStartAndEnd < leastDistance)
            {
                leastDistance = distanceBetweenStartAndEnd;
                nearestWaypoint = validWaypoint;
            }
            
        }
        return nearestWaypoint;
    }
    

    public GameObject findNextWaypoint(GameObject currentWaypoint, Direction enemyDirection) {
        System.Random ran = new System.Random();
        List<Valid_Waypoint> validWaypointList = filterValidWaypoint(currentWaypoint.GetComponent<Waypoint>().validWaypointConnection, enemyDirection);
        int randomSpawnerNumber = ran.Next(0, validWaypointList.Count);
        return validWaypointList[randomSpawnerNumber].WaypointObject;
    }
    


    List<Valid_Waypoint> filterValidWaypoint(List<Valid_Waypoint> validWaypointList, Direction enemyDirection) {
        for (int counter = 0; counter <= validWaypointList.Count-1; counter++)
        {
            Valid_Waypoint validWaypoint = validWaypointList[counter];
            if (!validWaypoint.WaypointDirection.Equals(enemyDirection))
            {
                validWaypointList.Remove(validWaypoint);
            }

        }
        return validWaypointList;
    }

    private Vector2 AsVector2(Transform transform)
    {
        return new Vector2(transform.position.x, transform.position.y);
    }


    // Update is called once per frame
    void Update () {
		
	}
}
