using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{

    private Rigidbody2D rb;
    private float movement;
    public float speed;
    public bool isJumping;
    public bool isGrounded;
    public float jump;

    private bool facingRight = false;

    public int leftBorder;
    public int rightBorder;

    // Use this for initialization
    void Start()
    {

        rb = this.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        movement = Input.GetAxis("Horizontal") * speed;
        EdgeReset();

    }


    void FixedUpdate()
    {
        if (movement < 0.0f && facingRight == false)
        {
            FlipPlayer();
        }
        else if (movement > 0.0f && facingRight == true)
        {
            FlipPlayer();
        }

        Vector2 velocity = rb.velocity;
        velocity.x = movement;
        rb.velocity = velocity;

        isJumping = Input.GetKeyDown("w");

        if (isJumping)
        {

            //Jump(velocity);
            Vector2 jumps = rb.velocity;
            jumps.y = jump;
            rb.velocity = jumps;
            Debug.Log("jump");
        } 

    }

    void Jump(Vector2 velocity)
    {
        velocity.y = velocity.y + jump * 2;
        rb.AddForce(velocity);
    }

    void OnCollisionEnter2D(Collision2D theCollision)
    {
        if (theCollision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }

        if (theCollision.gameObject.tag == "Enemy")
        {
            Destroy(theCollision.gameObject);
        }

        //if (theCollision.gameObject.tag == "Ground")
        //{
        //    var normal = theCollision.contacts[0].normal;
        //    if (normal.y > 0)
        //    { //if the bottom side hit something 
        //        Debug.Log("you hit the floor");
        //    }
        //    if (normal.y < 0)
        //    { //if the top side hit something
        //        Debug.Log("you hit the roof");
        //    }
        //    if (normal.x > 0)
        //    {
        //        //if the right side hit something
        //        Debug.Log("you hit the right");
        //    }
        //    if (normal.x < 0)
        //    {
        //        //if the left side hit something
        //        Debug.Log("you hit the left");
        //    }
        //}

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

    void FlipPlayer()
    {
        facingRight = !facingRight;
        Vector2 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}