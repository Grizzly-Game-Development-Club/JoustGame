using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    private Rigidbody2D erb;
    private float movementX;
    public float movementY;
    public float movementSpeedX;
    public float movementSpeedY;
    private int directionX;
    private int directionY;
    private bool grounded = true;
    private int check = 0;

    public int leftBorder;
    public int rightBorder;


    // Use this for initialization
    void Start () {
        erb = this.GetComponent<Rigidbody2D>();
        do
        {
            directionX = Random.Range(-1, 3);
            Debug.Log(directionX);
        } while (directionX == 0);
	}
	
	// Update is called once per frame
	void Update () {
        movementX = directionX * movementSpeedX * Time.deltaTime;
        if (!grounded)
        {
            movementY = directionY * movementSpeedY * Time.deltaTime;
        }

        EdgeReset();
 
    }

    void FixedUpdate()
    {
        Vector2 velocity = erb.velocity;
        velocity.x = movementX;
        velocity.y = movementY;
        erb.velocity = velocity;
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = false;
            Debug.Log("hehe");

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
            var normal = collision.contacts[0].normal;
            if (normal.y > 0)
            { //if the bottom side hit something 
                directionY *= -1;
                Debug.Log("You Hit the floor");
                do
                {
                    directionY = Random.Range(-1, 3);
                } while (directionY == 0);
                Debug.Log(directionY);

            }

        }

        if (collision.gameObject.tag == "Enemy")
        {
            directionX *= -1;
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            var normal = collision.contacts[0].normal;
            if (normal.y > 0)
            { //if the bottom side hit something 
                directionY *= -1;
                Debug.Log("You Hit the floor");
            }
            if (normal.y < 0)
            { //if the top side hit something
                directionY *= -1;
                Debug.Log("You Hit the roof");
            }
            if (normal.x > 0)
            {
                //if the right side hit something
                directionX *= -1;
                Debug.Log("You hit the right");
            }
            if (normal.x < 0)
            {
                //if the left side hit something
                directionX *= -1;
                Debug.Log("You hit the left");
            }
        }

        if (collision.gameObject.tag == "Top")
        {
            directionY *= -1;
        }


        //if (collision.gameObject.tag == "Floor")
        //{
        //    if (erb.velocity.y < 0)
        //    {
        //        directionY *= -2;

        //    }
            
        //}

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

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Ground")
    //    {
    //        Debug.Log("roof roof");
    //        directionY *= 1;

    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Floor")
        {

             directionY *= -2;
        }
    }
}
