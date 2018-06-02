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
    //public bool isPressingJump = false;

    public float leftBorder;
    public float rightBorder;
    public bool isAttacking;
    public int attackCounter;

    //public bool inAir = true;
    //public int tapJumpMultiplier = 1f;

    // Use this for initialization
    void Start() {
        Camera main_cam = Camera.main;
		float height = 2f * main_cam.orthographicSize;
		float width = height * main_cam.aspect;
		leftBorder = 0- width / 2; //-16;//(int)main_cam.transform.position.x;
		rightBorder = width / 2;//(int)main_cam.pixelWidth;
        attackCounter = 10;
        playerRB = gameObject.GetComponent<Rigidbody2D>();
        isAttacking = false;


        moveX = 0;

    }

    // Update is called once per frame
    void Update() {
        PlayerMove();
        Jump();
        Attack();
        EdgeReset();
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


    void EdgeReset()
    {
        if (this.transform.position.x < leftBorder)
        {
            this.transform.position = new Vector3(rightBorder - 0.1f, this.transform.position.y, this.transform.position.z);

        }

        if (this.transform.position.x > rightBorder)
        {
            this.transform.position = new Vector3(leftBorder + 0.1f, this.transform.position.y, this.transform.position.z);
        }
    }
}
