using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {

    public int playerSpeed = 8;
    private bool facingRight = false;
    private float moveX;
    private float jumpVelocity = 4f;
    public bool isGrounded = true;
    //public bool isPressingJump = false;

    public int leftBorder;
    public int rightBorder;

    //public bool inAir = true;
    //public int tapJumpMultiplier = 1f;

    // Use this for initialization
    void Start() {
        leftBorder = -8;
        rightBorder = 8;

    }

    // Update is called once per frame
    void Update() {
        PlayerMove();
        Jump();
        EdgeReset();
    }
    void PlayerMove() {
        //Controls
        moveX = Input.GetAxis("Horizontal");

        //Animations

        //Player Directions
        if (moveX < 0.0f && facingRight == false) {
            FlipPlayer();
        }
        else if (moveX > 0.0f && facingRight == true) {
            FlipPlayer();
        }

        //Physics
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(moveX * playerSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);

    }
    void Jump()
    {
        //Jumping code
        if (Input.GetButtonDown("Jump") ) {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpVelocity);
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
