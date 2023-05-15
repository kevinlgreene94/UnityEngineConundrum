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
    //private bool facingLeft = false;
    private float vertical;
    private float speed = 8f;
    private bool isLadder;
    private bool isClimbing;
    Animator animator;
    string animationState = "AnimationState";
    //2
    public AudioSource audioSource;
    public AudioClip[] audioClipArray;
    // Start is called before the first frame update
    //3
    public Player player;
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
        attackRight = 9,
        attackLeft = 10,
        shootRight = 11,
        shootLeft = 12,
        blockRight = 13,
        blockLeft = 14,
    }
    private void Start()
    {
        //4
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
     private void Update()
    {
        UpdateState();
        UpdateAudio();
        vertical = Input.GetAxis("Vertical");
        if (isLadder && Input.GetKey(KeyCode.W))
        {
            isClimbing = true;
        }
        else
        {
            isClimbing = false;
        }
        

    }

    //5
    void FixedUpdate()
    {
         MoveCharacter();
    }
    
    private void MoveCharacter()
    {
        
        direction = Input.GetAxis("Horizontal");
        

        if (direction > 0f && player.isAttacking == false && player.isShooting == false && player.isDead == false)
        {
            rb2D.velocity = new Vector2(direction * movementSpeed, rb2D.velocity.y);

        }
        else if (direction < 0f && player.isAttacking == false && player.isShooting == false && player.isDead == false)
        {
            rb2D.velocity = new Vector2(direction * movementSpeed, rb2D.velocity.y);

        }
        else
        {
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
        }

        if (Input.GetKey(KeyCode.W) && player.isTouchingGround && player.isAttacking == false)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpforce);

        }
        
        if (isClimbing && player.isDead == false)
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
        if (direction > 0f && player.isTouchingGround && player.isAttacking == false && player.isShooting == false && player.isDead == false)
        {
            animator.SetInteger(animationState, (int)CharStates.walkRight);
            player.facingLeft = false;
        }
        else if (direction < 0f && player.isTouchingGround && player.isAttacking == false && player.isShooting == false && player.isDead == false)
        {
            animator.SetInteger(animationState, (int)CharStates.walkLeft);
            player.facingLeft = true;
        }
        else if (!player.isTouchingGround && player.facingLeft == false && !isClimbing && player.isAttacking == false && player.isShooting == false && player.isDead == false)
        {
            animator.SetInteger(animationState, (int)CharStates.jumpRight);
        }
        else if (!player.isTouchingGround && player.facingLeft == true && !isClimbing && player.isAttacking == false && player.isShooting == false && player.isDead == false)
        {
            animator.SetInteger(animationState, (int)CharStates.jumpLeft);
        }
        else if (player.facingLeft == true && player.isAttacking == false && player.isShooting == false && player.isDead == false)
        {
            animator.SetInteger(animationState, (int)CharStates.idleLeft);
        }
        else if (isClimbing && player.isDead == false)
        {
            animator.SetInteger(animationState, (int)CharStates.climbingUp);
        }
        else if (player.isAttacking == true && player.facingLeft == false && player.isShooting == false && player.isDead == false)
        {
            animator.SetInteger(animationState, (int)CharStates.attackRight);
        }
        else if (player.isAttacking == true && player.facingLeft == true && player.isShooting == false && player.isDead == false)
        {
            animator.SetInteger(animationState, (int)CharStates.attackLeft);
        }
        else if (player.isShooting == true && player.facingLeft == true && player.isDead == false)
        {
            animator.SetInteger(animationState, (int)CharStates.shootLeft);
            
        }
        else if (player.isShooting == true && player.facingLeft == false && player.isDead == false)
        {
            animator.SetInteger(animationState, (int)CharStates.shootRight);
            
        }
        else if ( player.isDead == true)
        {
            animator.SetInteger(animationState, (int)CharStates.death);
        }
        else if (player.isBlocking)
        {
            animator.SetInteger(animationState, (int)CharStates.blockRight);
        }
        else
        {
            animator.SetInteger(animationState, (int)CharStates.idleRight);
        }
    }
    private void UpdateAudio()
    {
        if (Input.GetMouseButtonDown(0) && player.isTouchingGround)
        {
            audioSource.PlayOneShot(audioClipArray[0]);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            audioSource.PlayOneShot(audioClipArray[1]);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audioSource.PlayOneShot(audioClipArray[2]);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
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
