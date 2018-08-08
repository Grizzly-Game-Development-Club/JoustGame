using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global_Enum;


public class Enemy_Controller : MonoBehaviour {

    

    private GameObject spawnerManager;
    public GameObject waypointManager;
    private Spawner_Manager spawnerMangerScript;
    private Waypoint_Manager waypointManagerScript;

    private int enemyID;
    public float enemySpeed = 3f;
    public float flapVelocity = 1f;
    public float linearDrag = 40f;
    public float soarDuration = 1f;
    public float sailDuration = 1f;
    public float glideDuration = 1f;
    private float movementActionCountdown;
    private Direction enemyDirection;

    private float speedHolder;
    private float dragHolder;
    private float gravityHolder;

    private Rigidbody2D enemyRB;
    private bool enemyGrounded;

    private EnemyState enemyState;
    private MovementType enemyMovementType;
    public GameObject nextWaypoint;
    public GameObject currentWaypoint;

    private void OnDisable()
    {
        if (spawnerManager != null)
            spawnerManager.GetComponent<Spawner_Manager>().enemyAliveList.Remove(enemyID);
        else
            Debug.Log("No Spawn Manager Detected");
    }

    void Start() {
        EnemyRB = this.GetComponent<Rigidbody2D>();
        waypointManagerScript = waypointManager.GetComponent<Waypoint_Manager>();

        dragHolder = LinearDrag;
        speedHolder = EnemySpeed;
        gravityHolder = EnemyRB.gravityScale;

        enemyGrounded = false;
        enemyDirection = Direction.RIGHT;
        enemyState = EnemyState.SPAWNED;
    }

    // Update is called once per frame
    void FixedUpdate () {
        switch (enemyState) {
            case EnemyState.SPAWNED:
                nextWaypoint = waypointManagerScript.findNearestWaypoint(this.GetComponent<Transform>(), enemyDirection);
                enemyState = EnemyState.TRAVELING;           
                break;
            case EnemyState.TRAVELING:
                MoveTowardsWaypoint();
                break; 
            case EnemyState.ARRIVED:
                nextWaypoint = waypointManagerScript.findNextWaypoint(CurrentWaypoint, enemyDirection);
                CurrentWaypoint = null;
                enemyState = EnemyState.TRAVELING;
                break;
            case EnemyState.INTERRUPTED:
                break;
            case EnemyState.DEATH:
                break;
        }
	}

    

    private void MoveTowardsWaypoint()
    {

        float moveX = 0;
        switch (EnemyDirection) {
            case Direction.LEFT:
                moveX = -1;
                break;
            case Direction.RIGHT:
                moveX = 1;
                break;
            default:
                moveX = 0;
                Debug.Log("Direction Not Set Error");
                break;
        }

        EnemyRB.velocity = new Vector2(moveX * enemySpeed, EnemyRB.velocity.y);

        Vector2 start = this.transform.position;
        Vector2 end = nextWaypoint.transform.position;

        float distanceBetweenStartAndEnd = Mathf.Sqrt(Mathf.Pow((end.x - start.x), 2) + Mathf.Pow((end.y - start.y), 2));

        if (distanceBetweenStartAndEnd <= 3f) {
            resetSetting();

            StopAllCoroutines();
            EnemyRB.gravityScale = 1;
            transform.position = Vector2.MoveTowards(transform.position, nextWaypoint.transform.position, enemySpeed * Time.deltaTime);
        }
        else if (movementActionCountdown <= 0)
        {
            resetSetting();
            StopAllCoroutines();
            setRandomMovement();
            switch (EnemyMovementType) {
                case MovementType.SOARING:
                    movementActionCountdown = soarDuration;
                    StartCoroutine("ExecuteSoaring");
                    break;
                case MovementType.SAILING:
                    Debug.Log("Start");
                    movementActionCountdown = soarDuration;
                    StartCoroutine("ExecuteSailing");
                    break;
                case MovementType.GLIDING:
                    movementActionCountdown = glideDuration;
                    StartCoroutine("ExecuteGliding");
                    break;
                case MovementType.FLAPPING:
                    Debug.Log("Flapping");
                    movementActionCountdown = .5f;
                    StartCoroutine("ExecuteFlapping");
                    break;
                case MovementType.DIVING:
                    Debug.Log("Diving");
                    movementActionCountdown = .3f;
                    StartCoroutine("ExecuteDiving");
                    break;
                default:
                    Debug.Log("Movement Type not configured");
                    break;

            }
        }
        else
        {
            //Debug.Log("Current Action is " + System.Enum.GetName(typeof(MovementType),EnemyMovementType));
            movementActionCountdown -= Time.deltaTime;
        }
    }

    void setRandomMovement()
    {
        System.Random ran = new System.Random();
        int randomNum = ran.Next(0, 100);

        if (System.Linq.Enumerable.Range(1, 100).Contains(randomNum)) {

        }
        switch (randomSpawnerNumber) {
            case 0-25:
                Debug.Log("Start SOARING");
                EnemyMovementType = MovementType.SOARING;
                break;    
            case 1:
                Debug.Log("Start SAILING");
                EnemyMovementType = MovementType.SAILING;
                break;
            case 2:
                Debug.Log("Start GLIDING");
                EnemyMovementType = MovementType.GLIDING;     
                break;
            case 3:
                Debug.Log("Start FLAPPING");
                EnemyMovementType = MovementType.FLAPPING;
                break;
            case 4:
                Debug.Log("Start DIVING");
                EnemyMovementType = MovementType.DIVING;
                break;

            default:
                Debug.Log("Movement Type not configured");
                break;

        }

    }

    IEnumerator ExecuteSailing()
    {

        EnemySpeed = EnemySpeed + .5f;

        while (movementActionCountdown > 0) {
            
            EnemyRB.gravityScale = -EnemyRB.gravityScale;
            yield return new WaitForSeconds(sailDuration/2);
        }

        yield break;
    }

    IEnumerator ExecuteSoaring() {       

        EnemyRB.drag = LinearDrag;
        enemyRB.gravityScale = -1;
        yield return new WaitUntil(() => movementActionCountdown <= .25f);

        yield break;
    }

    IEnumerator ExecuteGliding()
    {
        EnemySpeed = EnemySpeed + .5f;
        EnemyRB.drag = LinearDrag;
        yield return new WaitUntil(() => movementActionCountdown <= 0);

        yield break;
    }

    IEnumerator ExecuteFlapping()
    {
        EnemyRB.velocity = new Vector2(EnemyRB.velocity.x, FlapVelocity);
        yield break;
    }

    IEnumerator ExecuteDiving()
    {
        //enemyRB.gravityScale = -1;
        EnemyRB.velocity = new Vector2(EnemyRB.velocity.x, -FlapVelocity/2);
        yield break;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        FlipEnemy();
        nextWaypoint = waypointManagerScript.findNearestWaypoint(this.GetComponent<Transform>(), enemyDirection);
        EnemyState = EnemyState.TRAVELING;
    }

    void resetSetting() {
        EnemySpeed = speedHolder;
        LinearDrag = dragHolder;
        EnemyRB.gravityScale = gravityHolder;
    }

    void FlipEnemy()
    {
        Vector2 localScale = this.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    /* Getter and Setter Method 
     */

    public GameObject SpawnerManager
    {
        get { return spawnerManager; }
        set
        {
            spawnerManager = value;
            spawnerMangerScript = value.GetComponent<Spawner_Manager>();
        }
    }

    public GameObject WaypointManager
    {
        get { return waypointManager; }
        set
        {
            waypointManager = value;
            waypointManagerScript = value.GetComponent<Waypoint_Manager>();
        }
    }

    public int EnemyID
    {
        get { return enemyID; }
        set { enemyID = value; this.gameObject.name = "Enemy Clone " + value; }
    }

    public float EnemySpeed
    {
        get { return enemySpeed; }
        set { enemySpeed = value; }
    }

    public float FlapVelocity
    {
        get { return flapVelocity; }
        set { flapVelocity = value; }
    }

    public float LinearDrag
    {
        get { return linearDrag; }
        set { linearDrag = value; }
    }

    public float SoarDuration
    {
        get { return soarDuration; }
        set { soarDuration = value; }
    }

    public float SailDuration
    {
        get { return sailDuration; }
        set { sailDuration = value; }
    }

    public float GlideDuration
    {
        get { return glideDuration; }
        set { glideDuration = value; }
    }

    public float MovementActionCountdown
    {
        get { return movementActionCountdown; }
        set { movementActionCountdown = value; }
    }

    public Direction EnemyDirection
    {
        get { return enemyDirection; }
        set { enemyDirection = value; }
    }

    public Rigidbody2D EnemyRB
    {
        get { return enemyRB; }
        set { enemyRB = value; }
    }

    public bool EnemyGrounded
    {
        get { return enemyGrounded; }
        set { enemyGrounded = value; }
    }

    public EnemyState EnemyState
    {
        get { return enemyState; }
        set { enemyState = value; }
    }

    public MovementType EnemyMovementType
    {
        get { return enemyMovementType; }
        set { enemyMovementType = value; }
    }

    public GameObject NextWaypoint
    {
        get { return nextWaypoint; }
        set { nextWaypoint = value; }
    }

    public GameObject CurrentWaypoint
    {
        get { return currentWaypoint; }
        set { currentWaypoint = value; }
    }
}
