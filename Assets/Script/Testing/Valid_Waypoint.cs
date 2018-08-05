using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global_Enum;

public class Valid_Waypoint : MonoBehaviour {


    private GameObject waypointObject;
    private DIRECTION waypointDirection;

    public GameObject WaypointObject
    {
        get { return waypointObject; }
        set { waypointObject = value; }
    }

    public DIRECTION WaypointDirection
    {
        get { return waypointDirection; }
        set { waypointDirection = value; }
    }

    public Valid_Waypoint(GameObject waypointObject, DIRECTION direction)
    {
        this.WaypointObject = waypointObject;
        this.WaypointDirection = direction;
    }



}
