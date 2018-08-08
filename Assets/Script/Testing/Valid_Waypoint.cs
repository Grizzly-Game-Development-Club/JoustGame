using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global_Enum;

[System.Serializable]
public class Valid_Waypoint : System.Object {

    public GameObject waypointObject;
    public Direction waypointDirection;

    public GameObject WaypointObject
    {
        get { return waypointObject; }
        set { waypointObject = value; }
    }

    public Direction WaypointDirection
    {
        get { return waypointDirection; }
        set { waypointDirection = value; }
    }

    public Valid_Waypoint(GameObject waypointObject, Direction direction)
    {
        this.WaypointObject = waypointObject;
        this.WaypointDirection = direction;
    }



}
