using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global_Enum;

public class Waypoint_Manager : MonoBehaviour {

    //

    public GameObject[] horizontalLane;

    private GameObject[] waypointList;

    // Use this for initialization
    void Start () {
        if (waypointList == null) {
            
            waypointList = GameObject.FindGameObjectsWithTag("Waypoint");
        }
	}

    public void test() {
        Debug.Log("test");
    }

    public GameObject findNearestWaypoint(Transform enemyPosition, DIRECTION enemyDirection) {
        List<GameObject> validTransformList = new List<GameObject>();
        float leastDistance = float.MaxValue;
        GameObject nearestWaypoint = null;

        

        foreach (GameObject waypointGameobject in waypointList) {
            Transform waypointPosition = waypointGameobject.GetComponent<Transform>();

            if (waypointPosition.position.x < enemyPosition.position.x && enemyDirection.Equals(DIRECTION.LEFT)) {
                validTransformList.Add(waypointGameobject);
            }
            else if (enemyPosition.position.x < waypointPosition.position.x && enemyDirection.Equals(DIRECTION.RIGHT)) {
                validTransformList.Add(waypointGameobject);
            }
        }
        Debug.Log(waypointList.Length);

        foreach (GameObject validWaypoint in validTransformList)
        {
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

    List<Valid_Waypoint> filterValidWaypoint(DIRECTION enemyDirection, List<Valid_Waypoint> validWaypointList) {
        foreach (Valid_Waypoint validWaypoint in validWaypointList) {
            if (!validWaypoint.WaypointDirection.Equals(enemyDirection)) {
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
