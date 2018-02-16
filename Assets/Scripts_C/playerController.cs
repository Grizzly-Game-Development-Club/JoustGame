using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {

    public int playerSpeed = 8;
    private bool facingRight = false;
    private float moveX;
    public float jumpVelocity = 8f;
    public bool grounded = false;
    //public bool inAir = true;
    //public int tapJumpMultiplier = 1f;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        PlayerMove();
        Jump();
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
        if (Input.GetButton("Jump") && grounded) {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpVelocity);
        }
        /*else if (Input.GetButton("Jump") && inAir) {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, tapJumpMultiplier);
        }*/
    }
    void FlipPlayer() {
        facingRight = !facingRight;
        Vector2 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
    void OnTriggerEnter2D()
    {
        grounded = true;
        //inAir = false;
    }
    void OnTriggerExit2D()
    {
        grounded = false;
        //inAir = true;
    }
}
