using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Global_Enum {

    public enum Direction { LEFT, RIGHT };
    public enum EnemyState { SPAWNED, TRAVELING, ARRIVED, INTERRUPTED, DEATH, EDGE };
    public enum WaypointType { NORMAL, EDGE };
    public enum SpawnManagerState { SPAWNING, WAITING, COUNTING };

    /* Soaring - Fly Up
     * Sailiing - Fly Straight
     * Gliding- Fly Down
     * Flapping - Sharp Fly Up
     * Diving - Sharp Fly Down
     * Correction - Fly Toward Waypoint
     */
    public enum MovementType{SOARING, SAILING, GLIDING, FLAPPING, DIVING, CORRECTION };

}
