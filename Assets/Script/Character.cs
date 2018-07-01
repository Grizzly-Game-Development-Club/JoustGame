using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
	
	public int speed = 0;
	private bool isFacingRight = false;
	public float jumpVelocity = 0f;
	public bool isGrounded = false;
	Rigidbody2D characterRB;

	//State variable
	public bool isAttacking = false;
	bool isJumping = false;
	bool isWalking = false;

	public int attackCounter = 0;

	//Use for Transform Player Position if Touching Edge
	float leftEdge;
	float rightEdge;

	float dirX = 0;
	Vector2 vel;
	//Use to check if player is dead
	private bool isAlive;

	//Reference to Game Manager Script
	//JoustGameManager gameManager;

	// Use this for initialization
	public void Start() {
		characterRB = gameObject.GetComponent<Rigidbody2D>();
		isAlive =true;
		vel = new Vector2 (0, 0);
		//Set Left and Right Border X Position
		leftEdge = GameObject.Find("Edge Collider/Left").transform.position.x;
		rightEdge = GameObject.Find("Edge Collider/Right").transform.position.x;
		//Set Game Manager
		//gameManager = GameObject.Find("Game Manager").GetComponent<JoustGameManager>();
	}

	// Update is called once per frame
	void Update() {

	}
	public void Move() {
		//set temp velocity value to RB velocity
		vel = characterRB.velocity;

		//Player Directions
		if (dirX < 0.0f && !isFacingRight) {
			FlipPlayer();
		}
		else if (dirX > 0.0f && isFacingRight) {
			FlipPlayer();
		}
		//Character Moves
		if (isWalking) {
			vel.x = dirX * speed;
		} 
		if (isJumping)  
		{
			vel.y = jumpVelocity;
		}
		if (isAttacking)
		{
			if (isFacingRight)
				dirX = -1;
			else
				dirX = 1;
			Vector2 force = new Vector2(dirX * 500, 0);
			characterRB.AddForce(force);
			attackCounter--;
			if (attackCounter <= 0)
			{
				attackCounter = 20;
				isAttacking = false;
				dirX = 0;
			}
		}
		isJumping = false;
		isWalking = false;
		//print ("in here");
		characterRB.velocity = vel;
	}
	public void Jump()
	{
		isJumping = true;
	}

	public void Attack()
	{
		if (!isAttacking)
		{
			isAttacking = true;                        
		}

	}
	public void Walk(float dir)
	{
		dirX = dir;
		isWalking = true;
	}
	void FlipPlayer() 
	{
		isFacingRight = !isFacingRight;
		Vector2 localScale = gameObject.transform.localScale;
		print (localScale.x);
		localScale.x *= -1;
		transform.localScale = localScale;
	}
	void OnCollisionEnter2D(Collision2D theCollision)
	{

		//Edge Reset
		if (theCollision.gameObject.tag == "Edge")
			switch (theCollision.gameObject.name) {
		case "Left":                
			this.transform.position = new Vector3(rightEdge - 1.2f, this.transform.position.y, this.transform.position.z);
			break;
		case "Right":      
			this.transform.position = new Vector3(leftEdge + 1.2f, this.transform.position.y, this.transform.position.z);
			break;
		case "Bottom":
			//End();
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
	void End() {
		Destroy(this);
		//En.notify("CharacterDeath|id="+ID);
		gameManager.setPlayerDeath(true);
		gameManager.changeGameStatus("gameOverStatus", true);
	}
    */
}
