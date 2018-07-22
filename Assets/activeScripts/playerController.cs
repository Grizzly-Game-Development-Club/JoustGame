using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {

    Rigidbody2D playerRB;
    public int playerSpeed = 8;
    private bool playerDeath;
    public bool isGrounded = true;   
    private bool facingRight = false;
    private float jumpVelocity = 4f;

    public bool canMove;
    private float moveX;
    
    public bool isAttacking;
    public int attackForce = 500;
    public float attackDuration = 1.5f;
    private float attackDurationCountdown;
    public bool attackOnCooldown;
    public float attackCooldown = 3f;
    private float attackCooldownCountdown;

    public Collider2D lanceHitBox;

    //Use for Transform Player Position if Touching Edge
    float leftEdge;
    float rightEdge;

    // Use this for initialization
    void Start() {
        playerRB = gameObject.GetComponent<Rigidbody2D>();

        canMove = true;
        attackOnCooldown = false;
        isAttacking = false;
        moveX = 0;

        //Set Left and Right Border X Position
        //leftEdge = GameObject.Find("Edge Collider/Left").transform.position.x;
        //rightEdge = GameObject.Find("Edge Collider/Right").transform.position.x;

        //Set player to be alive at the start of game
        playerDeath = false;

    }

    // Update is called once per frame
    void Update() {
        if (canMove)
        {
            PlayerMove();
        }
        Jump();
        Attack();
    }

    void PlayerMove() {
        //Controls
        moveX = Input.GetAxis("Horizontal");
        playerRB.velocity = new Vector2(moveX * playerSpeed, playerRB.velocity.y);

        //Animations

        //Player Directions
        if (moveX < 0.0f && !facingRight) {
            FlipPlayer();
        }
        else if (moveX > 0.0f && facingRight) {
            FlipPlayer();
        }
        
       
    }

    private IEnumerator HitScan(Collider2D col) {
        while (true) {
            Debug.Log("Attacking");

            ContactFilter2D colFilter = new ContactFilter2D();
            colFilter.SetLayerMask(LayerMask.GetMask("Hitbox"));
            Collider2D[] result = new Collider2D[10];

            int count = Physics2D.OverlapCollider(lanceHitBox, colFilter, result);

            foreach (Collider2D r in result) {
                if (r != null) {
                    Debug.Log(r.name);
                }

            }
            
            yield return null;
        }
    }


    void Jump()
    {      
        //Jumping code
        if (Input.GetButtonDown("Jump") ) {
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpVelocity);
          
        }      
    }

    void Attack()
    {
        if (!isAttacking)
        {
            if (!attackOnCooldown)
            {
                if (Input.GetKeyDown(KeyCode.X))
                {
                    isAttacking = true;
                    attackDurationCountdown = attackDuration;
                    StartCoroutine("HitScan", lanceHitBox);
                }
            }
            else {
                attackCooldownCountdown -= Time.deltaTime;
                if (attackCooldownCountdown <= 0) {
                    attackOnCooldown = false;
                }
            }
        }

        
        //If Player Attacking
        if (isAttacking)
        {
            int dirX = 0;
            attackDurationCountdown -= Time.deltaTime;
            if (attackDurationCountdown >= 0)
            {
                //canMove = false;
                if (facingRight)
                    dirX = -1;
                else
                    dirX = 1;

                Vector2 force = new Vector2(dirX * attackForce, 0);
                playerRB.AddForce(force);
            }
            else {
                isAttacking = false;
                attackOnCooldown = true;
                attackCooldownCountdown = attackCooldown;
                StopCoroutine("HitScan");
                canMove = true;
            }


        }

    }



    void FlipPlayer() {
        facingRight = !facingRight;
        Vector2 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

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
}
