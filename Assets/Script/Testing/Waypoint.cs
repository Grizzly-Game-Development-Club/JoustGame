using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

    public int WaypointID;
    public WaypointType wayPointType;
    public validWaypoint[] validWaypointConnection;

    public enum Direction {LEFT,RIGHT};
    public enum WaypointType { NORMAL, EDGE };

    [System.Serializable]
    public class validWaypoint
    {
        public GameObject waypointObject;
        public Direction direction;
    }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
