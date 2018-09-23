using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Global_Enum {

    public enum Direction { LEFT, RIGHT};
    public enum CollideDirection { UP, DOWN, LEFT, RIGHT };
    public enum EnemyState { SPAWNED, TRAVELING, DEATH};
    public enum SpawnManagerState { SPAWNING, WAITING, COUNTING };
    public enum SpawnPointStatus { AVAILABLE, UNAVAILABLE };
    public enum Action {Attack, Waiting,  }

    //Enemy Movement
    public enum EnemyMovementType{SAILING, KNOCKBACK, TOPEDGE, BOTTOMEDGE};
    public enum EnemyMovementTypeVertical {UPWARD,  NONE,  DOWNWARD}

    public enum GameStatus { PLAYING, WAITING, RESUME, PAUSED, DEATH};
    public enum PlayerNumber { ONE, TWO}
    public enum PlayerStatus { DEAD, ALIVE };

}
