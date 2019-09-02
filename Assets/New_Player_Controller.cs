using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global_Enum;
using UnityEngine.Events;

public class New_Player_Controller : MonoBehaviour {

    [SerializeField] private float m_MoveX;
    [SerializeField] private float m_MoveY;
    [SerializeField] private Direction m_SpriteDirection;

    [SerializeField] private float m_HorizontalForce;
    [SerializeField] private float m_VerticalForce;

    [SerializeField] private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private Rigidbody2D m_RigidBody2D;

    public UnityEvent test;



    // Use this for initialization
    void Awake () {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_RigidBody2D = GetComponent<Rigidbody2D>();

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update () {
        m_MoveX = Input.GetAxis("Horizontal");
        m_MoveY = Input.GetAxis("Vertical");
        Move();

        Vector3 vel = m_RigidBody2D.velocity;
        vel.y = Mathf.Clamp(vel.y, 0, 10);
        m_RigidBody2D.velocity = vel;
        Debug.Log(vel + " " + m_MoveX);
    }

    private void FixedUpdate()
    {
        
        UpdateSpriteDirection();
    }

    void Move()
    {
        //Right Movement
        if (m_MoveX > 0)
        {
            
            m_SpriteDirection = Direction.RIGHT; 
        }

        //Left Movement
        if (m_MoveX < 0)
        {
            m_SpriteDirection = Direction.LEFT;
        }

        //No Movement
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        m_RigidBody2D.AddForce(Vector2.right * m_MoveX * m_HorizontalForce);
        


    }

    void Jump()
    {
        Debug.Log(Vector2.up);
        m_RigidBody2D.AddForce(Vector2.up * m_VerticalForce);

        
        
    }

    void UpdateSpriteDirection() {
        switch (m_SpriteDirection) {
            case Direction.LEFT:
                m_SpriteRenderer.flipX = true;
                break;
            case Direction.RIGHT:
                m_SpriteRenderer.flipX = false;
                break;
        }

    }

    




}
