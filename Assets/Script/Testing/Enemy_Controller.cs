using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global_Enum;
using System.Linq;

public class Enemy_Controller : MonoBehaviour
{

    private GameObject spawnerManager;
    private Spawner_Manager spawnerManagerScript;

    private EnemyState enemyState;
    private EnemyMovementType enemyMovementType;
    private EnemyMovementTypeVertical enemyMovementTypeVertical;

    private Rigidbody2D enemyRB;
    public int enemyID;
    private Direction enemyDirection;
    private bool enemyGrounded;
    public LayerMask groundLayer;

    private float moveX = 0;
    private float moveY = 0;
    private float movementActionCountdown;

    public float horizontalSpeed = 0f;
    public float horizontalSpeedMax = 8f;
    public float verticalSpeed = 0f;
    public float verticalSpeedMax = 2f;
    private bool adjustVertical = false;

    public float knockBackLength = .5f;
    public float knockBackForce = 2f;
    private CollideDirection knockBackDirection;
    private bool isKnockBack;

    System.Random ran = new System.Random();


    private void OnDisable()
    {
        if (SpawnerManager != null)
            SpawnerManager.GetComponent<Spawner_Manager>().enemyAliveList.Remove(EnemyID);
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

        if (this.GetComponent<Rigidbody2D>() != null)
            EnemyRB = this.GetComponent<Rigidbody2D>();
        else
            Debug.LogError("Enemy Gameobject does not have Rigidbody attached to it");

        float randomNum = (float)GetRandomNumber(3, 8);
        horizontalSpeedMax = randomNum;
        EnemyGrounded = false;

        int randomNumDirection = ran.Next(0, 1);
        switch (randomNumDirection) {
            case 0:
                EnemyDirection = Direction.LEFT;
                break;
            case 1:
                EnemyDirection = Direction.RIGHT;
                break;
        }


        EnemyState = EnemyState.TRAVELING;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        EnemyGrounded = Physics2D.OverlapPoint(transform.GetChild(0).position, groundLayer);
        checkColliderCollision();


        switch (enemyState)
        {
            //State when Enemy Spawn
            case EnemyState.SPAWNED:
                //Check if the enemy is grounded else travel
                break;

            //State when enemy is traveling to a waypoint
            case EnemyState.TRAVELING:
                Movement();
                break;

            //State when a enemy is dead
            case EnemyState.DEATH:
                Destroy(this.gameObject);
                break;

        }
    }

    private void Movement()
    {
        //Auto update the direction movement of the enemy velocity
        SetEnemyDirectionalMovement();

        Vector2 upperRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 lowerLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        
        if (movementActionCountdown <= 0 & isKnockBack == false)
        {
            /* Stop all current Coroutines and reset the Vertical Speed
             * Adjust the action duration and begin countdown
             * Call SetRandomMovementVertical to get a random Vertical movement
             * Execute the ModifyMovementValue to adjust movement based on the random Vertical Movement
             */
            StopAllCoroutines();
            VerticalSpeed = 0;

            float randomNum = (float) GetRandomNumber(.5,2);
            movementActionCountdown = randomNum;

            float distanceBetweenTopBorder = Mathf.Sqrt(Mathf.Pow((upperRight.x - upperRight.x), 2) + Mathf.Pow((upperRight.y - this.transform.position.y), 2));
            float distanceBetweenBottomBorder = Mathf.Sqrt(Mathf.Pow((lowerLeft.x - lowerLeft.x), 2) + Mathf.Pow((lowerLeft.y - this.transform.position.y), 2));

            if (EnemyGrounded == true) {
                SetRandomMovementVertical(33, 66);
            }
            else if (distanceBetweenTopBorder <= 2f)
            {
                SetRandomMovementVertical(67, 100);
            }
            else if (distanceBetweenBottomBorder <= 2f)
            {
                SetRandomMovementVertical(0, 32);
            }
            else {
                SetRandomMovementVertical(0, 100);
            }

            StartCoroutine("ModifyMovementValue");
            
        }
        else
        {

            movementActionCountdown -= Time.deltaTime;

            if (HorizontalSpeed <= HorizontalSpeedMax)
                HorizontalSpeed += 1 * Time.deltaTime;

            if (VerticalSpeed <= VerticalSpeedMax && adjustVertical == true)
                VerticalSpeed += 3 * Time.deltaTime;

            if (isKnockBack == false)
                EnemyRB.velocity = new Vector2(MoveX * HorizontalSpeed, MoveY * VerticalSpeed);

        }
    }

    //Auto Correct enemy movement to Waypoint
    IEnumerator ExecuteKnockback()
    {
        AdjustVertical = true;
        VerticalSpeed = 0;
        
        float storeTime = movementActionCountdown;
        

        if (KnockBackDirection.Equals(CollideDirection.RIGHT) || KnockBackDirection.Equals(CollideDirection.LEFT)) {
            while (movementActionCountdown >= storeTime / 2)
            {
                if (KnockBackDirection.Equals(CollideDirection.RIGHT))
                {
                    EnemyRB.velocity = new Vector2(knockBackForce * 1.5f, VerticalSpeed * KnockBackForce);
                }
                else if (KnockBackDirection.Equals(CollideDirection.LEFT))
                {
                    EnemyRB.velocity = new Vector2(-knockBackForce * 1.5f, VerticalSpeed * KnockBackForce);
                }
                else if (KnockBackDirection.Equals(CollideDirection.UP))
                {
                    EnemyRB.velocity = new Vector2(EnemyRB.velocity.x, -VerticalSpeed * KnockBackForce);
                }
                else if (KnockBackDirection.Equals(CollideDirection.DOWN))
                {
                    EnemyRB.velocity = new Vector2(EnemyRB.velocity.x, VerticalSpeed * KnockBackForce);
                }
                yield return new WaitForFixedUpdate();
            }

            VerticalSpeed = 0;

            //Debug.Log(movementActionCountdown);

            while (movementActionCountdown >= 0)
            {
                if (KnockBackDirection.Equals(CollideDirection.RIGHT))
                {
                    EnemyRB.velocity = new Vector2(knockBackForce * 1.5f, -VerticalSpeed * KnockBackForce);
                }
                else if (KnockBackDirection.Equals(CollideDirection.LEFT))
                {
                    EnemyRB.velocity = new Vector2(-knockBackForce * 1.5f, -VerticalSpeed * KnockBackForce);
                }
                else if (KnockBackDirection.Equals(CollideDirection.UP))
                {
                    EnemyRB.velocity = new Vector2(EnemyRB.velocity.x, -VerticalSpeed * KnockBackForce);
                }
                else if (KnockBackDirection.Equals(CollideDirection.DOWN))
                {
                    EnemyRB.velocity = new Vector2(EnemyRB.velocity.x, VerticalSpeed * KnockBackForce);
                }
                yield return new WaitForFixedUpdate();
            }
        }
        else if (KnockBackDirection.Equals(CollideDirection.UP) || KnockBackDirection.Equals(CollideDirection.DOWN)) {
            while (movementActionCountdown >= 0)
            {
                if (KnockBackDirection.Equals(CollideDirection.UP))
                {
                    EnemyRB.velocity = new Vector2(EnemyRB.velocity.x, -VerticalSpeed * KnockBackForce);
                }
                else if (KnockBackDirection.Equals(CollideDirection.DOWN))
                {
                    EnemyRB.velocity = new Vector2(EnemyRB.velocity.x, VerticalSpeed * KnockBackForce);
                }
                yield return new WaitForFixedUpdate();
            }


        }
        
        

        AdjustVertical = false;
        VerticalSpeed = 0;
        isKnockBack = false;

        if (!EnemyDirection.Equals(KnockBackDirection))
        {
            if (KnockBackDirection.Equals(CollideDirection.LEFT))
            {
                EnemyDirection = Direction.LEFT;
            }
            if (KnockBackDirection.Equals(CollideDirection.RIGHT))
            {
                EnemyDirection = Direction.RIGHT;
            }
        }
        

        yield break;
    }

    //Auto update the direction in which the enemy is moving
    void SetEnemyDirectionalMovement()
    {
        Vector2 localScale = this.transform.localScale;
        switch (EnemyDirection)
        {

            case Direction.LEFT:
                localScale.x = -1;
                transform.localScale = localScale;
                MoveX = -1;
                break;
            case Direction.RIGHT:
                localScale.x = 1;
                transform.localScale = localScale;
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
        }
        //If Past Right Side of Screen
        if (this.transform.position.x > upperRight.x)
        {
            this.transform.position = new Vector2(lowerLeft.x + 0.7f, this.transform.position.y);
            this.EnemyDirection = Direction.RIGHT;
        }
        //If Reach Top Side of Screen
        if (this.transform.position.y > upperRight.y)
        {
            EnemyMovementType = EnemyMovementType.TOPEDGE;
        }
        //If Reach Bottom Side of Screen
        if (this.transform.position.y > upperRight.y)
        {
            EnemyMovementType = EnemyMovementType.BOTTOMEDGE;
        }


    }

    private int UPWARD;
    private int NONE;
    private int DOWNWARD;

    //Pick a random vertical movement;
    void SetRandomMovementVertical(int minRange, int Maxrange)
    {
        //Set Enemy Movement Type
        int randomNum = ran.Next(minRange, Maxrange);


        if (Enumerable.Range(0, 32).Contains(randomNum))
        {
            EnemyMovementTypeVertical = EnemyMovementTypeVertical.UPWARD;
            UPWARD++;
           
        }
        if (Enumerable.Range(33, 33).Contains(randomNum))
        {
            EnemyMovementTypeVertical = EnemyMovementTypeVertical.NONE;
            NONE++;
        }
        if (Enumerable.Range(67, 33).Contains(randomNum))
        {
            EnemyMovementTypeVertical = EnemyMovementTypeVertical.DOWNWARD;
            DOWNWARD++;
        }

        //Debug.Log("Upward: " + UPWARD + " Downard: " + DOWNWARD + " None: " + NONE);
    }

    //Auto Correct enemy movement to Waypoint
    IEnumerator ModifyMovementValue()
    {
        while (MovementActionCountdown > 0)
        {
            if (EnemyMovementTypeVertical.Equals(EnemyMovementTypeVertical.NONE))
            {
                VerticalSpeed = 0;
            }
            else if (EnemyMovementTypeVertical.Equals(EnemyMovementTypeVertical.UPWARD) || EnemyMovementTypeVertical.Equals(EnemyMovementTypeVertical.DOWNWARD))
            {
                AdjustVertical = true;
            }
            yield return new WaitForFixedUpdate();
        }
        AdjustVertical = false;

        yield break;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (EnemyGrounded == false || (EnemyGrounded == true && collision.gameObject.CompareTag("Enemy"))) {
            foreach (ContactPoint2D hitPos in collision.contacts)
            {

                if (hitPos.normal.x < 0)
                {

                    KnockBackDirection = CollideDirection.LEFT;
                }
                else if (hitPos.normal.x > 0)
                {
                    KnockBackDirection = CollideDirection.RIGHT;
                }
                else if (hitPos.normal.y < 0)
                {
                    KnockBackDirection = CollideDirection.UP;
                }
                else if (hitPos.normal.y > 0)
                {
                    KnockBackDirection = CollideDirection.DOWN;
                }

            }

            MovementActionCountdown = KnockBackLength;
            isKnockBack = true;
            StopAllCoroutines();
            StartCoroutine("ExecuteKnockback");
        }
    }

    void SwitchEnemyDirection()
    {
        switch (EnemyDirection) {
            case Direction.LEFT:
                EnemyDirection = Direction.RIGHT;
                break;
            case Direction.RIGHT:
                enemyDirection = Direction.LEFT;
                break;
        }
    }

    public double GetRandomNumber(double minimum, double maximum)
    {
        return ran.NextDouble() * (maximum - minimum) + minimum;
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

    public CollideDirection KnockBackDirection
    {
        get { return knockBackDirection; }
        set { knockBackDirection = value; }
    }

    public bool KnockBackBool
    {
        get { return isKnockBack; }
        set { isKnockBack = value; }
    }
    
}
