using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global_Enum;

public class Player_Controller : MonoBehaviour
{

    Rigidbody2D playerRB;
    public int playerSpeed = 8;

    private bool isGrounded = true;
    private Direction playerDirection;
    public float jumpVelocity = 10f;
    public float knockBackForce = 3f;
    public float knockBackDuration = .5f;
    public float knockBackCount = 0f;

    private bool playerDeath;
    private float moveX;
    private float moveY;

    private bool canMove;
    private bool isAttacking;
    public float attackForce = 100f;
    public float attackDuration = .5f;
    private float attackCountdownDuration;
    private bool attackOnCooldown;
    public float attackCooldownDuration = 2f;
    private float attackCooldownCountdown;

    public Collider2D lanceHitBox;


    // Use this for initialization
    void Start()
    {
        PlayerRB = gameObject.GetComponent<Rigidbody2D>();
        PlayerDirection = Direction.RIGHT;

        CanMove = true;
        AttackOnCooldown = false;
        IsAttacking = false;
        MoveX = 0;


        //Set player to be alive at the start of game
        playerDeath = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (KnockBackCount <= 0)
        {
            if (CanMove)
            {
                PlayerMove();
            }
            Jump();
            Attack();
            checkColliderCollision();
        }
        else
        {
            KnockBackCount -= Time.deltaTime;
        }
    }

    void PlayerMove()
    {
        //Controls
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");

        PlayerRB.drag = 1f;

        if (moveY == 0)
        {
            PlayerRB.velocity = new Vector2(moveX * playerSpeed, PlayerRB.velocity.y);
        }
        else if (moveY > 0)
        {
            PlayerRB.drag = 10f;
            PlayerRB.velocity = new Vector2(moveX * (playerSpeed / 2), PlayerRB.velocity.y);

        }

        //Animations

        Vector2 localScale = this.transform.localScale;
        //Player Directions
        if (moveX < 0.0f)
        {
            localScale.x = -1;
            transform.localScale = localScale;
            playerDirection = Direction.LEFT;
        }
        else if (moveX > 0.0f)
        {
            localScale.x = 1;
            transform.localScale = localScale;
            playerDirection = Direction.RIGHT;
        }


    }

    private IEnumerator HitScan(Collider2D col)
    {
        while (true)
        {
            Debug.Log("Attacking");


            ContactFilter2D colFilter = new ContactFilter2D();
            colFilter.SetLayerMask(LayerMask.GetMask("Hitbox"));
            Collider2D[] result = new Collider2D[10];

            int count = Physics2D.OverlapCollider(lanceHitBox, colFilter, result);

            foreach (Collider2D r in result)
            {
                if (r != null)
                {
                    if (r.name.Equals("Enemy_Hit_Box"))
                    {
                        r.gameObject.GetComponentInParent<Enemy_Controller>().EnemyState = EnemyState.DEATH;
                    }
                    Debug.Log(r.name);
                }

            }

            yield return null;
        }
    }


    void Jump()
    {
        //Jumping code
        if (Input.GetButtonDown("Jump"))
        {
            PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, JumpVelocity);

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
                    IsAttacking = true;
                    AttackDurationCountdown = attackDuration;
                    StartCoroutine("HitScan", lanceHitBox);
                }
            }
            else
            {
                AttackCooldownCountdown -= Time.deltaTime;
                if (AttackCooldownCountdown <= 0)
                {
                    AttackOnCooldown = false;
                }
            }
        }


        //If Player Attacking
        if (isAttacking)
        {
            int dirX = 0;
            attackCountdownDuration -= Time.deltaTime;
            if (attackCountdownDuration >= 0)
            {
                PlayerRB.drag = 1f;
                PlayerRB.gravityScale = .3f;
                CanMove = false;

                if (playerDirection.Equals(Direction.LEFT))
                    dirX = -1;
                else
                    dirX = 1;

                Vector2 force = new Vector2(dirX * attackForce, 0);
                PlayerRB.AddForce(force);
            }
            else
            {
                PlayerRB.gravityScale = 1f;
                IsAttacking = false;
                AttackOnCooldown = true;
                AttackCooldownCountdown = attackCooldownDuration;
                StopCoroutine("HitScan");
                CanMove = true;
            }


        }

    }

    private void checkColliderCollision()
    {

        //Corner locations in world coordinates
        Vector2 upperRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 lowerLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));

        //If Past Left Side of Screen
        if (this.transform.position.x < lowerLeft.x)
        {

            //Appear on right
            this.transform.position = new Vector2(upperRight.x - 0.7f, this.transform.position.y);
            this.playerDirection = Direction.LEFT;
        }
        //If Past Right Side of Screen
        if (this.transform.position.x > upperRight.x)
        {
            this.transform.position = new Vector2(lowerLeft.x + 0.7f, this.transform.position.y);
            this.playerDirection = Direction.RIGHT;
        }
        //If Reach Top Side of Screen
        if (this.transform.position.y > upperRight.y - 0.7f)
        {
            this.transform.position = new Vector2(transform.position.x, upperRight.y - 0.7f);
        }
        //If Reach Bottom Side of Screen
        if (this.transform.position.y > upperRight.y)
        {
            //playerDirection = EnemyMovementType.BOTTOMEDGE;
        }


    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        foreach (ContactPoint2D hitPos in collision.contacts)
        {
            if (hitPos.normal.x < 0)
            {
                //Left
                KnockBackCount = KnockBackDuation;
                AttackDurationCountdown = 0;
                StartCoroutine("PlayerKnockBack", new Vector2(-KnockBackForce * 1.5f, PlayerRB.velocity.y * KnockBackForce));
            }
            else if (hitPos.normal.x > 0)
            {
                //Right
                KnockBackCount = KnockBackDuation;
                AttackDurationCountdown = 0;
                StartCoroutine("PlayerKnockBack", new Vector2(KnockBackForce * 1.5f, PlayerRB.velocity.y * KnockBackForce));
            }

        }

        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    IEnumerator PlayerKnockBack(Vector2 knockback)
    {

        PlayerRB.velocity = knockback;

        yield return new WaitUntil(() => knockBackCount <= 0);

        yield return null;
    }


    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            IsGrounded = false;
        }
    }

    public Rigidbody2D PlayerRB
    {
        get { return playerRB; }
        set { playerRB = value; }
    }

    public int PlayerSpeed
    {
        get { return playerSpeed; }
        set { playerSpeed = value; }
    }

    public bool IsGrounded
    {
        get { return isGrounded; }
        set { isGrounded = value; }
    }

    public Direction PlayerDirection
    {
        get { return playerDirection; }

        set { playerDirection = value; }
    }

    public float JumpVelocity
    {
        get { return jumpVelocity; }
        set { jumpVelocity = value; }
    }

    public float KnockBackForce
    {
        get { return knockBackForce; }
        set { knockBackForce = value; }
    }

    public float KnockBackDuation
    {
        get { return knockBackDuration; }
        set { knockBackDuration = value; }
    }

    public float KnockBackCount
    {
        get { return knockBackCount; }
        set { knockBackCount = value; }
    }

    public bool PlayerDeath
    {
        get { return playerDeath; }
        set { playerDeath = value; }
    }

    public float MoveX
    {
        get { return moveX; }
        set { moveX = value; }
    }

    public float MoveY
    {
        get { return moveY; }
        set { moveY = value; }
    }

    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }

    public bool IsAttacking
    {
        get { return isAttacking; }
        set { isAttacking = value; }
    }

    public float AttackForce
    {
        get { return attackForce; }
        set { attackForce = value; }
    }

    public float AttackDuration
    {
        get { return attackDuration; }
        set { attackDuration = value; }
    }

    public float AttackDurationCountdown
    {
        get { return attackCountdownDuration; }
        set { attackCountdownDuration = value; }
    }

    public bool AttackOnCooldown
    {
        get { return attackOnCooldown; }
        set { attackOnCooldown = value; }
    }

    public float AttackCooldown
    {
        get { return attackCooldownDuration; }
        set { attackCooldownDuration = value; }
    }

    public float AttackCooldownCountdown
    {
        get { return attackCooldownCountdown; }
        set { attackCooldownCountdown = value; }
    }

    public Collider2D LanceHitBox
    {
        get { return lanceHitBox; }
        set { lanceHitBox = value; }
    }
}
