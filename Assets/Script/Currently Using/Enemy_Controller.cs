using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global_Enum;
using System.Linq;

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
    public float correctionDuration = 1f;
    [Range(1, 99)]
    public int correctionFrequency = 30;
    private float movementActionCountdown;
    private Direction enemyDirection;

    private float moveX = 0;
    private float speedHolder;
    private float dragHolder;
    private float gravityHolder;

    private Rigidbody2D enemyRB;
    private bool enemyGrounded;

    System.Random ran = new System.Random();

    private int soaringNumber = 0;
    private int sailingNumber = 0;
    private int glidingNumber = 0;
    private int flappingNumber = 0;
    private int divingNumber = 0;
    private int correctionNumber = 0;

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
        waypointManager = GameObject.FindGameObjectWithTag("Waypoint Manager");
        waypointManagerScript = waypointManager.GetComponent<Waypoint_Manager>();

        dragHolder = LinearDrag;
        speedHolder = EnemySpeed;
        gravityHolder = EnemyRB.gravityScale;

        enemyGrounded = false;
        enemyDirection = Direction.RIGHT;
        enemyState = EnemyState.SPAWNED;
    }

    // Update is called once per frame
    void FixedUpdate() {

        checkColliderCollision();


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
                if (nextWaypoint.GetComponent<Waypoint>().WayPointType.Equals(WaypointType.EDGE) &&
                    CurrentWaypoint.GetComponent<Waypoint>().WayPointType.Equals(WaypointType.EDGE))
                {
                    Debug.Log("Test");
                    EnemyRB.velocity = new Vector2(moveX * enemySpeed, EnemyRB.velocity.y);
                }
                else
                {
                    CurrentWaypoint = null;
                    enemyState = EnemyState.TRAVELING;
                }
                break;
            case EnemyState.INTERRUPTED:
                break;
            case EnemyState.DEATH:
                break;
            case EnemyState.EDGE:
                CurrentWaypoint = null;
                break;
        }
    }

    private void checkColliderCollision() {

        //Corner locations in world coordinates
        Vector2 upperRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 lowerLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));

        //If Past Left Side of Screen
        if (this.transform.position.x  < lowerLeft.x) {
            
            //Appear on right
            this.transform.position = new Vector2(upperRight.x - 0.7f, this.transform.position.y);
            this.EnemyDirection = Direction.LEFT;
            EnemyState = EnemyState.ARRIVED;
        }
        //If Past Right Side of Screen
        if (this.transform.position.x > upperRight.x)
        {
            this.transform.position = new Vector2(lowerLeft.x + 0.7f, this.transform.position.y);
            this.EnemyDirection = Direction.RIGHT;
            EnemyState = EnemyState.ARRIVED;
        }
        //If Reach Top Side of Screen
        if (this.transform.position.y > upperRight.y)
        {
            EnemyState = EnemyState.ARRIVED;
        }
        //If Reach Bottom Side of Screen
        if (this.transform.position.y < lowerLeft.y + 0.7f)
        {
            EnemyState = EnemyState.DEATH;
        }

    }

    private void MoveTowardsWaypoint()
    {

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
            EnemyRB.velocity = new Vector2(0,0);
            transform.position = Vector2.MoveTowards(transform.position, nextWaypoint.transform.position, enemySpeed * Time.deltaTime);
        }
        else if (movementActionCountdown <= 0)
        {
            /*
            string s = System.String.Format("Soaring: {0},Sailing: {1},Gliding: {2}," +
                "Flapping: {3},Diving: {4},Correction: {5},",
                soaringNumber, sailingNumber, glidingNumber, flappingNumber, divingNumber, correctionNumber);
            Debug.Log(s);
            */
            StopAllCoroutines();
            setRandomMovement();
            switch (EnemyMovementType) {
                case MovementType.SOARING:
                    movementActionCountdown = soarDuration;
                    StartCoroutine("ExecuteSoaring");
                    soaringNumber++;
                    break;
                case MovementType.SAILING:
                    movementActionCountdown = soarDuration;
                    StartCoroutine("ExecuteSailing");
                    sailingNumber++;
                    break;
                case MovementType.GLIDING:
                    movementActionCountdown = glideDuration;
                    StartCoroutine("ExecuteGliding");
                    glidingNumber++;
                    break;
                case MovementType.FLAPPING:
                    movementActionCountdown = .5f;
                    StartCoroutine("ExecuteFlapping");
                    flappingNumber++;
                    break;
                case MovementType.DIVING:
                    movementActionCountdown = .3f;
                    StartCoroutine("ExecuteDiving");
                    divingNumber++;
                    break;
                case MovementType.CORRECTION:
                    movementActionCountdown = correctionDuration;
                    correctionNumber++;
                    break;
                default:
                    Debug.Log("Movement Type not configured");
                    break;

            }
        }
        
        else if (!(distanceBetweenStartAndEnd <= 3f) && Mathf.RoundToInt(Mathf.Abs((this.transform.position.x+1000)-(nextWaypoint.transform.position.x+1000))) <= 3f)
        {
            EnemyRB.velocity = new Vector2(0, 0);
            transform.position = Vector2.MoveTowards(transform.position, nextWaypoint.transform.position, enemySpeed * Time.deltaTime);
        }
        else
        {
            movementActionCountdown -= Time.deltaTime;
            if (EnemyMovementType.Equals(MovementType.CORRECTION)){
                EnemyRB.velocity = new Vector2(0, 0);
                transform.position = Vector2.MoveTowards(transform.position, nextWaypoint.transform.position, enemySpeed * Time.deltaTime);
            }
        }
    }

    void setRandomMovement()
    {
        resetSetting();
        int randomNum = ran.Next(0, 100);

        if (Enumerable.Range(0, correctionFrequency).Contains(randomNum)) {
            Debug.Log("Start Correction");
            EnemyMovementType = MovementType.CORRECTION;
        }
        else if (Enumerable.Range(correctionFrequency+1, 100).Contains(randomNum)) {
            int randomNum2 = ran.Next(0, 100);

            if (Enumerable.Range(0, 28).Contains(randomNum2))
            {
                //Debug.Log("Start SAILING");
                EnemyMovementType = MovementType.SAILING;

            }
            if (Enumerable.Range(29, 46).Contains(randomNum2)) {
                //Debug.Log("Start SOARING");
                EnemyMovementType = MovementType.SOARING;

            }
            if (Enumerable.Range(47, 64).Contains(randomNum2))
            {
                //Debug.Log("Start GLIDING");
                EnemyMovementType = MovementType.GLIDING;

            }
            if (Enumerable.Range(65, 82).Contains(randomNum2))
            {
                //Debug.Log("Start FLAPPING");
                EnemyMovementType = MovementType.FLAPPING;

            }
            if (Enumerable.Range(83, 100).Contains(randomNum2))
            {
                //Debug.Log("Start DIVING");
                EnemyMovementType = MovementType.DIVING;

            }

        }

    }

    IEnumerator ExecuteSailing()
    {
        EnemySpeed = EnemySpeed + .5f;

        while (movementActionCountdown > 0) {

            EnemyRB.gravityScale = -EnemyRB.gravityScale;
            yield return new WaitForSeconds(sailDuration / 2);
        }

        yield break;
    }

    IEnumerator ExecuteSoaring() {

        EnemyRB.drag = LinearDrag;
        enemyRB.gravityScale = -1;
        yield return new WaitUntil(() => movementActionCountdown <= .25f);
        EnemyRB.gravityScale = 1;

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
        EnemyRB.velocity = new Vector2(EnemyRB.velocity.x + 1f, FlapVelocity);
        yield break;
    }

    IEnumerator ExecuteDiving()
    {
        EnemyRB.velocity = new Vector2(EnemyRB.velocity.x + 1f, -FlapVelocity / 2);
        yield break;
    }

    IEnumerator ExecuteCorrection()
    {
        EnemyRB.drag = 0;
        EnemyRB.gravityScale = 0;
        
        while (movementActionCountdown > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, nextWaypoint.transform.position, enemySpeed * Time.deltaTime);
            yield return new WaitForSeconds(.000001f);
        }
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

    public float CorrectionDuration
    {
        get { return correctionDuration; }
        set { correctionDuration = value; }
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
