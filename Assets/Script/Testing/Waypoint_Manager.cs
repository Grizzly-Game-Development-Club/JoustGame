using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global_Enum;

public class Waypoint_Manager : MonoBehaviour {


    private GameObject[] waypointList;

    // Use this for initialization
    void Start () {

        if (GameObject.FindGameObjectsWithTag("Waypoint").Length != 0)
            waypointList = GameObject.FindGameObjectsWithTag("Waypoint");
        else
            Debug.LogError("No Waypoint has been detected. Make sure that your waypoint object is tag with (Waypoint)");
        
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
    
    //Find the Next Waypoint from the current waypoint using the enemy direction
    public GameObject findNextWaypoint(GameObject currentWaypoint, Direction enemyDirection) {

        System.Random ran = new System.Random();
        //Filter out the Valid Waypoint that doesn't have the enemy direction
        List<Valid_Waypoint> validWaypointList = filterValidWaypoint(currentWaypoint.GetComponent<Waypoint>().validWaypointConnection, enemyDirection);

        int randomSpawnerNumber = ran.Next(0, validWaypointList.Count);
        return validWaypointList[randomSpawnerNumber].WaypointObject;
    }
    


    List<Valid_Waypoint> filterValidWaypoint(List<Valid_Waypoint> validWaypointList, Direction enemyDirection) {
        List<Valid_Waypoint> copyList = new List<Valid_Waypoint>(validWaypointList);

        //Search through the list of validwaypoint of the waypoint
        for (int counter = 0; counter <= copyList.Count-1; counter++)
        {

            Valid_Waypoint validWaypoint = copyList[counter];
            if (!validWaypoint.WaypointDirection.Equals(enemyDirection))
            {
                copyList.Remove(validWaypoint);
            }

        }
        return copyList;
    }

    private Vector2 AsVector2(Transform transform)
    {
        return new Vector2(transform.position.x, transform.position.y);
    }


    // Update is called once per frame
    void Update () {
		
	}
}
