using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Navigation : MonoBehaviour {

    //Waypoint Layer Class
    [System.Serializable]
    public class wayPointLayer
    {
        public int wayPointLayerID;
        public wayPoint[] wayPoint;

    }

    //Waypoint Class
    [System.Serializable]
    public class wayPoint
    {
        public int wayPointId;
        public GameObject wayPointObject;
        public GameObject[] validNextWaypoint;

    }

    public wayPointLayer[] wayPointLayerArray;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
