using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody2D rb;
    private float movement;
    public float speed;
    public bool isJumping;
    public bool isGrounded;
    public float jump;

    // Use this for initialization
    void Start()
    {

        rb = this.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        movement = Input.GetAxis("Horizontal") * speed;

    }


    void FixedUpdate()
    {
        Vector2 velocity = rb.velocity;
        velocity.x = movement;
        rb.velocity = velocity;

        isJumping = Input.GetKeyDown("w");

        if (isJumping)
        {

            //Jump(velocity);
            Vector2 jumps = rb.velocity;
            jumps.y = 7f;
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

    }
    void OnCollisionExit2D(Collision2D theCollision)
    {
        if (theCollision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }
}