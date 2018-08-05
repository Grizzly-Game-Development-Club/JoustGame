using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Global_Enum{

    public enum DIRECTION { LEFT, RIGHT };
    public enum STATE { SPAWNED, TRAVELING, ARRIVED, INTERRUPTED, DEATH };
    public enum WaypointType { NORMAL, EDGE };
    public enum SpawnManagerState { SPAWNING, WAITING, COUNTING };

}
