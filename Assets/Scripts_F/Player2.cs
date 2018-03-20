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
    private bool attack;

    private static string ptag = "player tag";

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
        Vector2 velocity = rb.velocity;
        velocity.x = movement;
        rb.velocity = velocity;

        isJumping = Input.GetKeyDown("w");
        attack = Input.GetKeyDown(KeyCode.Space);

        if (isJumping)
        {

            //Jump(velocity);
            Vector2 jumps = rb.velocity;
            jumps.y = jump;
            rb.velocity = jumps;
            Debug.Log("jump");
        } 
        if (attack)
        {
            Debug.Log("Space has been pressed");
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