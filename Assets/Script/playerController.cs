using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {

    public int playerSpeed = 8;
    private bool facingRight = false;
    private float moveX;
    private float jumpVelocity = 4f;
    public bool isGrounded = true;
    Rigidbody2D playerRB;
    

    public bool isAttacking;
    public int attackCounter;

    //Use for Transform Player Position if Touching Edge
    float leftEdge;
    float rightEdge;

    //Use to check if player is dead
    private bool playerDeath;

    //Reference to Game Manager Script
    JoustGameManager gameManager;

    //Unused
    //public bool isPressingJump = false;
    //public bool inAir = true;
    //public int tapJumpMultiplier = 1f;

    // Use this for initialization
    void Start() {
        Camera main_cam = Camera.main;

        

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
        gameManager = GameObject.Find("Game Manager").GetComponent<JoustGameManager>();
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
        //Jumping code
        if (Input.GetButtonDown("Jump") ) {
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpVelocity);
        }
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
                    Death();
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


    //On Player Death
    void Death() {
        Destroy(this);
        gameManager.setPlayerDeath(true);
        gameManager.changeGameStatus("gameOverStatus", true);
    }

}
