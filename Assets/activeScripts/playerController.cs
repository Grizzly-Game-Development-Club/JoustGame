using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {

    //Player Speed
    public int playerSpeed = 8;

    //Player Sprite Direction
    private bool facingRight = false;


    //Player Jump Velocity
    private float jumpVelocity = 4f;

    //Store Horizontal Movement
    private float moveX;

    //Check if player is touching the ground
    public bool isGrounded = true;

    //Variable to hold the player Rigidbody
    Rigidbody2D playerRB;

    //Check if player is dead
    private bool playerDeath;

    public bool isAttacking;
    public int attackCounter;

    //Use for Transform Player Position if Touching Edge
    float leftEdge;
    float rightEdge;

    

    //Reference to Game Manager Script
    //JoustGameManager gameManager;

    //Unused
    //public bool isPressingJump = false;
    //public bool inAir = true;
    //public int tapJumpMultiplier = 1f;

    // Use this for initialization
    void Start() {
        Camera main_cam = Camera.main;
/*<<<<<<< HEAD
		float height = 2f * main_cam.orthographicSize;
		float width = height * main_cam.aspect;
		leftBorder = 0- width / 2; //-16;//(int)main_cam.transform.position.x;
		rightBorder = width / 2;//(int)main_cam.pixelWidth;
=======*/

        

//>>>>>>> Hieu-Changes
        attackCounter = 10;
        playerRB = gameObject.GetComponent<Rigidbody2D>();
        isAttacking = false;


        moveX = 0;

        //Set Left and Right Border X Position
        leftEdge = GameObject.Find("Edge Collider/Left").transform.position.x;
        rightEdge = GameObject.Find("Edge Collider/Right").transform.position.x;

        //Set player to be alive at the start of game
        playerDeath = false;

        //Set Game Manager
        //gameManager = GameObject.Find("Game Manager").GetComponent<JoustGameManager>();




    }

    // Update is called once per frame
    void Update() {
        PlayerMove();
        Jump();
        Attack();
    }
    void PlayerMove() {
        //Controls
        moveX = Input.GetAxis("Horizontal");

        //Animations

        //Player Directions
        if (moveX < 0.0f && !facingRight) {
            FlipPlayer();
        }
        else if (moveX > 0.0f && facingRight) {
            FlipPlayer();
        }
        int dirX = 0;
       
        if (isAttacking)
        {
            
            if (facingRight)
                dirX = -1;
            else
                dirX = 1;

            Vector2 force = new Vector2(dirX * 500, 0);
            playerRB.AddForce(force);
            attackCounter--;
            if (attackCounter <= 0)
            {
                attackCounter = 10;
                isAttacking = false;
                moveX = 0;
            }
        }
        //Physics
      
        playerRB.velocity = new Vector2(moveX * playerSpeed, playerRB.velocity.y);
        

    }
    void Jump()
    {
        /*
        //Jumping code
        if (Input.GetButtonDown("Jump") ) {
            if (totalStamina >= actionCost) // check if there is enough stamina to cause
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpVelocity);
                totalStamina -= actionCost;
            }
            else
            {
                Debug.Log("Out of Stamina");
            }
        }
        */
       /* else
        {
            Vector2 vel = GetComponent<Rigidbody2D>().velocity;
            vel = new Vector2(GetComponent<Rigidbody2D>().velocity.x, vel.y-jumpVelocity);
        }*/
        /*else if (Input.GetButton("Jump") && inAir) {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, tapJumpMultiplier);
        }*/
        
    }

    void Attack()
    {
        if (!isAttacking)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                isAttacking = true;                        
            }
        }
        
    }
    void FlipPlayer() {
        facingRight = !facingRight;
        Vector2 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }/*
    void OnTriggerEnter2D()
    {
        grounded = true;
        //inAir = false;
    }
    void OnTriggerExit2D()
    {
        grounded = false;
        //inAir = true;
    }*/

    void OnCollisionEnter2D(Collision2D theCollision)
    {

        //Edge Reset
        if (theCollision.gameObject.tag == "Edge")
            switch (theCollision.gameObject.name) {
                case "Left":                
                    this.transform.position = new Vector3(rightEdge - 0.7f, this.transform.position.y, this.transform.position.z);
                    break;
                case "Right":      
                    this.transform.position = new Vector3(leftEdge + 0.7f, this.transform.position.y, this.transform.position.z);
                    break;
                case "Bottom":
                    //Death();
                    break;
            }

        if (theCollision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D theCollision)
    {
        if (theCollision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }

    /*
    //On Player Death
    void Death() {
        Destroy(this);
        gameManager.setPlayerDeath(true);
        gameManager.changeGameStatus("gameOverStatus", true);
    }
    */
}
