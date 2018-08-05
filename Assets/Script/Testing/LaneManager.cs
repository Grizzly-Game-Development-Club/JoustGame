using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneManager : MonoBehaviour {

    private int laneID;
    private GameObject[] waypointInLane;

    public int LaneID
    {
        get { return laneID; }
        set { laneID = value; }
    }

    public GameObject[] WaypointInLane
    {
        get { return waypointInLane; }
        set { waypointInLane = value; }
    }

    void IsFromDifferentLane(GameObject validWaypointConnection)
    {

    }

    void LaneSwitchAvailable()
    {

    }

    void ExecuteLaneSwitch()
    {

    }
}
