using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovementController : MonoBehaviour
{
    //1
    public float movementSpeed = 3.0f;
    public float jumpforce = 8f;
    private float direction = 0f;
    private bool facingLeft = false;
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
        else
        {
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
        }

        if (Input.GetKey(KeyCode.UpArrow) && isTouchingGround)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpforce);

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
        else if (!isTouchingGround && facingLeft == false)
        {
            animator.SetInteger(animationState, (int)CharStates.jumpRight); 
        }
        else if (!isTouchingGround && facingLeft == true)
        {
            animator.SetInteger(animationState, (int)CharStates.jumpLeft);
        }
        else if (facingLeft == true)
        {
            animator.SetInteger(animationState, (int)CharStates.idleLeft);
        }
        else
        {
            animator.SetInteger(animationState, (int)CharStates.idleRight);
        }
    }
}
