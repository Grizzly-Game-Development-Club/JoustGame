using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Global_Enum {

    public enum Direction { LEFT, RIGHT, UP, DOWN };
    public enum EnemyState { SPAWNED, FIX,TRAVELING, ARRIVED, INTERRUPTED, DEATH, EDGE };
    public enum SpawnManagerState { SPAWNING, WAITING, COUNTING };

    //Enemy Movement
    public enum EnemyMovementType{SAILING, CORRECTION, KNOCKBACK };
    public enum EnemyMovementTypeVertical {UPWARD,  NONE,  DOWNWARD}

}
