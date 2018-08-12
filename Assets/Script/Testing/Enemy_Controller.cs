using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global_Enum;
using System.Linq;
using System;

public class Enemy_Controller : MonoBehaviour
{

    private GameObject spawnerManager;
    private GameObject waypointManager;
    private Spawner_Manager spawnerManagerScript;
    private Waypoint_Manager waypointManagerScript;
    private EnemyState enemyState;
    public EnemyMovementType enemyMovementType;
    private EnemyMovementTypeVertical enemyMovementTypeVertical;
    private GameObject storedWaypoint;
    private Rigidbody2D enemyRB;
    

    private int enemyID;
    private Direction enemyDirection;
    private bool enemyGrounded;

    private float moveX = 0;
    private float moveY = 0;
    public float movementActionCountdown;
    private float acceleration;

    public GameObject nextWaypoint;
    public float distanceToWaypoint;

    public float horizontalSpeed = 0f;
    public float horizontalSpeedMax = 3f;
    public float verticalSpeed = 0f;
    public float verticalSpeedMax = 2f;
    private bool adjustVertical = false;
    [Range(1, 5)]
    public int correctionFrequency = 30;
    [Range(.5f, 3f)]
    public float correctionDuration = 1f;
    [Range(.5f, 3f)]
    public float sailingDuration = 1f;
    public float knockBackLength = 2f;
    public float knockBackForce = 5f;
    private Direction knockBackDirection;
    public float knockBackCount;

    System.Random ran = new System.Random();

    private void OnDisable()
    {
        if (spawnerManager != null)
            spawnerManager.GetComponent<Spawner_Manager>().enemyAliveList.Remove(enemyID);
        else
            Debug.LogError("Enemy was not spawned by a spawner");
    }

    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Spawner Manager") != null)
            SpawnerManager = GameObject.FindGameObjectWithTag("Spawner Manager");
        else
            Debug.LogError("Cannot Find Spawner Manager, please create one and if you do have one " +
                "make sure your Spawner Manager has (Spawner Manager) Tag attached to it.");

        if (GameObject.FindGameObjectWithTag("Waypoint Manager") != null)
            WaypointManager = GameObject.FindGameObjectWithTag("Waypoint Manager");
        else
            Debug.LogError("Cannot Find Waypoint Manager, please create one and if you do have one " +
                "make sure your Waypoint Manager has (Waypoint Manager) Tag attached to it.");

        if (this.GetComponent<Rigidbody2D>() != null)
            EnemyRB = this.GetComponent<Rigidbody2D>();
        else
            Debug.LogError("Enemy Gameobject does not have Rigidbody attached to it");


        EnemyGrounded = false;
        EnemyDirection = Direction.RIGHT;
        EnemyState = EnemyState.SPAWNED;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        checkColliderCollision();

        switch (enemyState)
        {
            //State when Enemy Spawn
            case EnemyState.SPAWNED:
                enemyState = EnemyState.FIX;
                break;

            //State when enemy need a waypoint, but not at a waypoint
            case EnemyState.FIX:
                nextWaypoint = waypointManagerScript.findNearestWaypoint(this.GetComponent<Transform>(), enemyDirection);
                enemyState = EnemyState.TRAVELING;
                break;

            //State when enemy is traveling to a waypoint
            case EnemyState.TRAVELING:
                MoveTowardsWaypoint();
                break;

            //State when enemy is arrived to a waypoint
            case EnemyState.ARRIVED:
                nextWaypoint = waypointManagerScript.findNextWaypoint(nextWaypoint, enemyDirection);
                enemyState = EnemyState.TRAVELING;
                break;

            //State when a enemy is dead
            case EnemyState.DEATH:
                break;

            //State when enemy is at the edge of waypoint
            case EnemyState.EDGE:
                storedWaypoint = null;
                break;
        }
    }

    private void MoveTowardsWaypoint()
    {

        SetEnemyDirectionalMovement();
        UpdateDistanceToWaypoint();

        Vector2 lowerLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));

        //If Enemy is within a 3f radius of the waypoint then auto correct to waypoint
        if (DistanceToWaypoint <= 3f)
        {
            EnemyRB.velocity = new Vector2(0, 0);
            StopAllCoroutines();
            MovementActionCountdown = 0;
            transform.position = Vector2.MoveTowards(transform.position, nextWaypoint.transform.position, horizontalSpeedMax * Time.deltaTime);
        }
        //If Enemy is not within 3f radius and exceed 3f difference between their current position and the next waypoint, then auto correct to waypoint
        else if (!(DistanceToWaypoint <= 3f) && Mathf.RoundToInt(Mathf.Abs((this.transform.position.x + 1000) - (nextWaypoint.transform.position.x + 1000))) <= 3f)
        {
            EnemyRB.velocity = new Vector2(0, 0);
            StopAllCoroutines();
            MovementActionCountdown = 0;
            transform.position = Vector2.MoveTowards(transform.position, nextWaypoint.transform.position, horizontalSpeedMax * Time.deltaTime);
        }//If Reach Bottom Side of Screen
        else if (Mathf.RoundToInt(Mathf.Abs((this.transform.position.y + 1000) - (lowerLeft.y + 1000))) <= 2f)
        {
            EnemyRB.velocity = new Vector2(0, 0);
            StopAllCoroutines();
            MovementActionCountdown = 0;
            transform.position = Vector2.MoveTowards(transform.position, nextWaypoint.transform.position, horizontalSpeedMax * Time.deltaTime);
        }
        else if (EnemyMovementType.Equals(EnemyMovementType.KNOCKBACK))
        {
            horizontalSpeed = 0f;
            StartCoroutine("ExecuteKnockback");
            movementActionCountdown = KnockBackLength;
        }
        else if (movementActionCountdown <= 0)
        {
            StopAllCoroutines();
            SetRandomMovement();
            VerticalSpeed = 0;

            switch (EnemyMovementType)
            {
                case EnemyMovementType.SAILING:
                    movementActionCountdown = sailingDuration;
                    StartCoroutine("ExecuteSailing");
                    break;
                case EnemyMovementType.CORRECTION:
                    movementActionCountdown = correctionDuration;
                    break;
                
                    
                default:
                    Debug.LogError("Movement Type not configured");
                    break;

            }
        }
        else if(movementActionCountdown > 0)
        {
            movementActionCountdown -= Time.deltaTime;
            if (HorizontalSpeed <= HorizontalSpeedMax)
            {
                HorizontalSpeed += 1 * Time.deltaTime;
            }

            if (VerticalSpeed <= VerticalSpeedMax && adjustVertical == true)
            {
                VerticalSpeed += 3 * Time.deltaTime;
            }

            
            else
            {
                
                if (EnemyMovementType.Equals(EnemyMovementType.CORRECTION))
                {
                    EnemyRB.velocity = new Vector2(0, 0);
                    transform.position = Vector2.MoveTowards(transform.position, NextWaypoint.transform.position, horizontalSpeedMax * Time.deltaTime);
                }
                else
                {

                    EnemyRB.velocity = new Vector2(MoveX * HorizontalSpeed, moveY * VerticalSpeed);
                }
            }
        }
    }

    //Check whenever enemy collide on the edge of a camera
    private void checkColliderCollision()
    {

        //Corner locations in world coordinates
        Vector2 upperRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 lowerLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));

        //If Past Left Side of Screen
        if (this.transform.position.x < lowerLeft.x)
        {

            //Appear on right
            this.transform.position = new Vector2(upperRight.x - 0.7f, this.transform.position.y);
            this.EnemyDirection = Direction.LEFT;
            EnemyState = EnemyState.FIX;
        }
        //If Past Right Side of Screen
        if (this.transform.position.x > upperRight.x)
        {
            this.transform.position = new Vector2(lowerLeft.x + 0.7f, this.transform.position.y);
            this.EnemyDirection = Direction.RIGHT;
            EnemyState = EnemyState.FIX;
        }
        //If Reach Top Side of Screen
        if (this.transform.position.y > upperRight.y)
        {
            EnemyState = EnemyState.FIX;
        }
        

    }

    //Set enemy moveX variable depending on direction
    void SetEnemyDirectionalMovement()
    {
        switch (EnemyDirection)
        {
            case Direction.LEFT:
                MoveX = -1;
                break;
            case Direction.RIGHT:
                MoveX = 1;
                break;
            default:
                MoveX = 0;
                Debug.Log("Direction Not Set Error");
                break;
        }

        switch (EnemyMovementTypeVertical)
        {
            case EnemyMovementTypeVertical.UPWARD:
                MoveY = 1;
                break;
            case EnemyMovementTypeVertical.NONE:
                MoveY = 0;
                break;
            case EnemyMovementTypeVertical.DOWNWARD:
                MoveY = -1;
                break;
        }
    }

    //Update the distance to waypoint
    void UpdateDistanceToWaypoint()
    {
        Vector2 start = this.transform.position;
        Vector2 end = nextWaypoint.transform.position;
        DistanceToWaypoint = Mathf.Sqrt(Mathf.Pow((end.x - start.x), 2) + Mathf.Pow((end.y - start.y), 2));
    }

    void SetRandomMovement()
    {
        //Set Enemy Movement Type
        int randomNum = ran.Next(0, 100);

        //Correction
        if (Enumerable.Range(0, correctionFrequency).Contains(randomNum))
        {
            EnemyMovementType = EnemyMovementType.CORRECTION;
        }
        //Flapping Or Sailing
        else if (Enumerable.Range(correctionFrequency + 1, 100).Contains(randomNum))
        {
            EnemyMovementType = EnemyMovementType.SAILING;
        }

    }

    

    //Auto Correct enemy movement to Waypoint
    IEnumerator ExecuteSailing()
    {
        SetRandomMovementVertical(33);
        while (MovementActionCountdown > 0) {
            if (EnemyMovementTypeVertical.Equals(EnemyMovementTypeVertical.NONE))
            {
                VerticalSpeed = 0;
            }
            else if(EnemyMovementTypeVertical.Equals(EnemyMovementTypeVertical.UPWARD) || EnemyMovementTypeVertical.Equals(EnemyMovementTypeVertical.DOWNWARD))
            {
                AdjustVertical = true;
            }
            yield return new WaitForEndOfFrame();
        }
        AdjustVertical = false;

        yield break;
    }

    //Auto Correct enemy movement to Waypoint
    IEnumerator ExecuteCorrection()
    {
        EnemyRB.drag = 0;
        EnemyRB.gravityScale = 0;

        while (movementActionCountdown > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, nextWaypoint.transform.position, HorizontalSpeed * Time.deltaTime);
            yield return new WaitForSeconds(.000001f);
        }
        yield break;
    }

    //Auto Correct enemy movement to Waypoint
    IEnumerator ExecuteKnockback()
    {
        AdjustVertical = true;
        VerticalSpeed = 0;

        float storeTime = movementActionCountdown;
        Debug.Log("Test");
        while (movementActionCountdown >= storeTime / 2)
        {
            if (KnockBackDirection.Equals(Direction.RIGHT))
            {
                EnemyRB.velocity = new Vector2(knockBackForce*1.5f, VerticalSpeed * KnockBackForce);
            }
            else if (KnockBackDirection.Equals(Direction.LEFT))
            {
                EnemyRB.velocity = new Vector2(-knockBackForce*1.5f, VerticalSpeed * KnockBackForce);
            }
            else if (KnockBackDirection.Equals(Direction.UP))
            {
                EnemyRB.velocity = new Vector2(EnemyRB.velocity.x, -VerticalSpeed * KnockBackForce);
            }
            else if (KnockBackDirection.Equals(Direction.DOWN))
            {
                EnemyRB.velocity = new Vector2(EnemyRB.velocity.x, VerticalSpeed * KnockBackForce);
            }
            yield return new WaitForEndOfFrame();
        }

        VerticalSpeed = 0;

        while (movementActionCountdown > 0) {
            if (KnockBackDirection.Equals(Direction.RIGHT))
            {
                EnemyRB.velocity = new Vector2(knockBackForce * 1.5f, -VerticalSpeed * KnockBackForce);
            }
            else if (KnockBackDirection.Equals(Direction.LEFT))
            {
                EnemyRB.velocity = new Vector2(-knockBackForce * 1.5f, -VerticalSpeed * KnockBackForce);
            }
            else if (KnockBackDirection.Equals(Direction.UP))
            {
                EnemyRB.velocity = new Vector2(EnemyRB.velocity.x, -VerticalSpeed * KnockBackForce);
            }
            else if (KnockBackDirection.Equals(Direction.DOWN))
            {
                EnemyRB.velocity = new Vector2(EnemyRB.velocity.x, VerticalSpeed * KnockBackForce);
            }
            yield return new WaitForEndOfFrame();
        }

        AdjustVertical = false;
        VerticalSpeed = 0;
        SetRandomMovement();


        if (!EnemyDirection.Equals(KnockBackDirection)) {
            if (KnockBackDirection.Equals(Direction.LEFT)) {
                EnemyDirection = Direction.LEFT;
            }
            if (KnockBackDirection.Equals(Direction.RIGHT))
            {
                EnemyDirection = Direction.RIGHT;
            }

            FlipEnemy();
        }

        EnemyState = EnemyState.FIX;

        yield break;
    }

    //When enemy collide with something flip the enemy and fix waypoint
    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D hitPos in collision.contacts)
        {
            
            if (hitPos.normal.x < 0)
            {

                KnockBackDirection = Direction.LEFT;
            }
            else if (hitPos.normal.x > 0)
            {
                KnockBackDirection = Direction.RIGHT;
            }
            else if (hitPos.normal.y > 0)
            {
                KnockBackDirection = Direction.UP;
            }
            else if (hitPos.normal.y < 0)
            {
                KnockBackDirection = Direction.DOWN;
            }

        }

        StopAllCoroutines();
        EnemyMovementType = EnemyMovementType.KNOCKBACK;
        
    }

    void SetRandomMovementVertical(int range)
    {
        //Set Enemy Movement Type
        int randomNum = ran.Next(0, range);

        if (Enumerable.Range(67, 100).Contains(randomNum))
        {
            EnemyMovementTypeVertical = EnemyMovementTypeVertical.UPWARD;
        }
        else if (Enumerable.Range(34, 66).Contains(randomNum))
        {
            EnemyMovementTypeVertical = EnemyMovementTypeVertical.DOWNWARD;
        }
        else if (Enumerable.Range(0, 33).Contains(randomNum))
        {
            EnemyMovementTypeVertical = EnemyMovementTypeVertical.NONE;

        }
    }

    //Flip the enemy object
    void FlipEnemy()
    {
        
        Vector2 localScale = this.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    /* Getter and Setter Method
     * ----------------------*/

    public GameObject SpawnerManager
    {
        get { return spawnerManager; }
        set
        {
            spawnerManager = value;

            if (value.GetComponent<Spawner_Manager>() != null)
                spawnerManagerScript = value.GetComponent<Spawner_Manager>();
            else
                Debug.LogError("Spawn Manager does not have (Spawner_Manager) script attached to it");
        }
    }

    public GameObject WaypointManager
    {
        get { return waypointManager; }
        set
        {
            waypointManager = value;

            if (value.GetComponent<Waypoint_Manager>() != null)
                waypointManagerScript = value.GetComponent<Waypoint_Manager>();
            else
                Debug.LogError("Waypoint Manager does not have (Waypoint_Manager) script attached to it");
        }
    }

    public EnemyState EnemyState
    {
        get { return enemyState; }
        set { enemyState = value; }
    }

    public EnemyMovementType EnemyMovementType
    {
        get { return enemyMovementType; }
        set { enemyMovementType = value; }
    }

    public EnemyMovementTypeVertical EnemyMovementTypeVertical
    {
        get { return enemyMovementTypeVertical; }
        set { enemyMovementTypeVertical = value; }
    }

    public GameObject StoredWaypoint
    {
        get { return storedWaypoint; }
        set { storedWaypoint = value; }
    }

    public Rigidbody2D EnemyRB
    {
        get { return enemyRB; }
        set { enemyRB = value; }
    }

    public int EnemyID
    {
        get { return enemyID; }
        set { enemyID = value; this.gameObject.name = "Enemy Clone " + value; }
    }

    public Direction EnemyDirection
    {
        get { return enemyDirection; }
        set { enemyDirection = value; }
    }

    public bool EnemyGrounded
    {
        get { return enemyGrounded; }
        set { enemyGrounded = value; }
    }

    public float MoveX
    {
        get { return moveX; }
        set { moveX = value; }
    }

    public float MoveY
    {
        get { return moveY; }
        set { moveY = value; }
    }

    public float MovementActionCountdown
    {
        get { return movementActionCountdown; }
        set { movementActionCountdown = value; }
    }

    public GameObject NextWaypoint
    {
        get { return nextWaypoint; }
        set { nextWaypoint = value; }
    }

    public float DistanceToWaypoint
    {
        get { return distanceToWaypoint; }
        set { distanceToWaypoint = value; }
    }

    public float HorizontalSpeed
    {
        get { return horizontalSpeed; }
        set { horizontalSpeed = value; }
    }

    public float HorizontalSpeedMax
    {
        get { return horizontalSpeedMax; }
        set { horizontalSpeedMax = value; }
    }

    public float VerticalSpeed
    {
        get { return verticalSpeed; }
        set { verticalSpeed = value; }
    }

    public float VerticalSpeedMax
    {
        get { return verticalSpeedMax; }
        set { verticalSpeedMax = value; }
    }

    public bool AdjustVertical
    {
        get { return adjustVertical; }
        set { adjustVertical = value; }
    }

    public float KnockBackLength
    {
        get { return knockBackLength; }
        set { knockBackLength = value; }
    }

    public float KnockBackForce
    {
        get { return knockBackForce; }
        set { knockBackForce = value; }
    }

    public Direction KnockBackDirection
    {
        get { return knockBackDirection; }
        set { knockBackDirection = value; }
    }

    public float KnockBackCount
    {
        get { return knockBackCount; }
        set { knockBackCount = value; }
    }
















}
