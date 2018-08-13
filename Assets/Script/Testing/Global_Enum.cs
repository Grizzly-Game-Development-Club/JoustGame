using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Global_Enum {

    public enum Direction { LEFT, RIGHT};
    public enum CollideDirection { UP, DOWN, LEFT, RIGHT };
    public enum EnemyState { SPAWNED, TRAVELING, INTERRUPTED, DEATH};
    public enum SpawnManagerState { SPAWNING, WAITING, COUNTING };

    //Enemy Movement
    public enum EnemyMovementType{SAILING, KNOCKBACK, TOPEDGE, BOTTOMEDGE};
    public enum EnemyMovementTypeVertical {UPWARD,  NONE,  DOWNWARD}

}
