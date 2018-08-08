using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Global_Enum {

    public enum Direction { LEFT, RIGHT };
    public enum EnemyState { SPAWNED, TRAVELING, ARRIVED, INTERRUPTED, DEATH };
    public enum WaypointType { NORMAL, EDGE };
    public enum SpawnManagerState { SPAWNING, WAITING, COUNTING };
    public enum MovementType{SOARING, SAILING, GLIDING, FLAPPING, DIVING, CORRECTION };

}
