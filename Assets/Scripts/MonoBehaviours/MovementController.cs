using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovementController : MonoBehaviour
{
    //1
    public float movementSpeed = 3.0f;
    public float jumpforce = 8f;
    public float climbSpeed = 8f;
    private float direction = 0f;
    private bool facingLeft = false;
    private float vertical;
    private float speed = 8f;
    private bool isLadder;
    private bool isClimbing;
    private bool isDead = false;
    Animator animator;
    string animationState = "AnimationState";
    //2
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isTouchingGround;
    // Start is called before the first frame update
    //3
    
    Rigidbody2D rb2D;
    Vector2 movement = new Vector2();

    enum CharStates
    {
        walkRight = 1,
        walkLeft = 2,
        jumpRight = 3,
        jumpLeft = 4,
        idleRight = 5,
        idleLeft = 6,
        climbingUp = 7,
        death = 8,
    }
    private void Start()
    {
        //4
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
     private void Update()
    {
        UpdateState();
        vertical = Input.GetAxis("Vertical");
        if (isLadder && Mathf.Abs(vertical) > 0f)
        {
            isClimbing = true;
        }
        
    }

    //5
    void FixedUpdate()
    {
         MoveCharacter();
    }
    
    private void MoveCharacter()
    {
        isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        direction = Input.GetAxis("Horizontal");
        

        if (direction > 0f)
        {
            rb2D.velocity = new Vector2(direction * movementSpeed, rb2D.velocity.y);

        }
        else if (direction < 0f)
        {
            rb2D.velocity = new Vector2(direction * movementSpeed, rb2D.velocity.y);

        }
        else if (isDead == true)
        {
            direction = 0;
        }
        else
        {
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
        }

        if (Input.GetKey(KeyCode.UpArrow) && isTouchingGround)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpforce);

        }
        if (isClimbing)
        {
            rb2D.gravityScale = 0f;
            rb2D.velocity = new Vector2(rb2D.velocity.x, vertical * speed);
        }
        else
        {
            rb2D.gravityScale = 7;
        }
        
            movement.Normalize();
    }
    
    private void UpdateState()
    {
        // 8
        if (direction > 0f && isTouchingGround)
        {
            animator.SetInteger(animationState, (int)CharStates.walkRight);
            facingLeft = false;
        }
        else if (direction < 0f && isTouchingGround)
        {
            animator.SetInteger(animationState, (int)CharStates.walkLeft);
            facingLeft = true;
        }
        else if (!isTouchingGround && facingLeft == false && !isClimbing)
        {
            animator.SetInteger(animationState, (int)CharStates.jumpRight);
        }
        else if (!isTouchingGround && facingLeft == true && !isClimbing)
        {
            animator.SetInteger(animationState, (int)CharStates.jumpLeft);
        }
        else if (facingLeft == true)
        {
            animator.SetInteger(animationState, (int)CharStates.idleLeft);
        }
        else if (isClimbing)
        {
            animator.SetInteger(animationState, (int)CharStates.climbingUp);
        }
        else if (isDead == true)
        {
            animator.SetInteger(animationState, (int)CharStates.death);
        }
        else
        {
            animator.SetInteger(animationState, (int)CharStates.idleRight);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
        if (collision.CompareTag("Damage"))
        {
            isDead = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
        }
    }
}
