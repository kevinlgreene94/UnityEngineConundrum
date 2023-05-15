using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]
public class Wander : MonoBehaviour
{
    CircleCollider2D circleCollider;
    public float pursuitSpeed;
    public float wanderSpeed;
    float currentSpeed;
    public float directionChangeInterval;
    public bool followPlayer;
    Coroutine moveCoroutine;
    Rigidbody2D rb2d;
    Animator animator;
    Transform targetTransform = null;
    Vector3 endPosition;
    string animationState = "AnimationState";
    float currentAngle = 0;
    public float rangeOne;
    public float rangeTwo;
    bool facingLeft = false;
    bool isAttacking = false;

    enum CharStates
    {
        walkRight = 1,
        walkLeft = 2,
        attackRight = 3,
        attackLeft = 4,
        idleRight = 5,
        idleLeft = 6,
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentSpeed = wanderSpeed;
        rb2d = GetComponent<Rigidbody2D>();
        StartCoroutine(WanderRoutine());
        circleCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(rb2d.position, endPosition, Color.red); 
    }
    public IEnumerator WanderRoutine()
    {
        while (true)
        {
            ChooseNewEndpoint();
            if(moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(Move(rb2d, currentSpeed));
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }
    public IEnumerator Move(Rigidbody2D rigidBodyToMove, float speed)
    {
        float remainingDistance = (transform.position - endPosition).sqrMagnitude;
        while(remainingDistance > float.Epsilon)
        {
            if(targetTransform != null)
            {
                endPosition = targetTransform.position;
            }
            if(rigidBodyToMove != null)
            {              
                Vector3 newPosition = Vector3.MoveTowards(rigidBodyToMove.position, endPosition, speed * Time.deltaTime);
                rb2d.MovePosition(newPosition);
                remainingDistance = (transform.position - endPosition).sqrMagnitude;
            }
            if (endPosition.x > rb2d.position.x && isAttacking == false)
            {
                animator.SetInteger(animationState, (int)CharStates.walkRight);               
            }
            else if(endPosition.x < rb2d.position.x && isAttacking == false)
            {
                animator.SetInteger(animationState, (int)CharStates.walkLeft);
            }
            if (endPosition.x < rb2d.position.x)
            {
                facingLeft = true;
            }
            else
            {
                facingLeft = false;
            }
            yield return new WaitForFixedUpdate();
        }
        
    }
    void ChooseNewEndpoint()
    {
        currentAngle += Random.Range(0, 360);
        //endPosition += Vector3FromAngle(currentAngle);
        endPosition += new Vector3(Random.Range(rangeOne, rangeTwo), rb2d.position.y, 0);
    }
   /* Vector3 Vector3FromAngle(float inputAngleDegrees)
    {
        float inputAngleRadians = inputAngleDegrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(inputAngleRadians), Mathf.Sin(inputAngleRadians), 0);
    }*/
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && followPlayer)
        {
            isAttacking = true;
            currentSpeed = pursuitSpeed;
            targetTransform = collision.gameObject.transform;
            if(moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(Move(rb2d, currentSpeed));
            if (facingLeft)
            {
                animator.SetInteger(animationState, (int)CharStates.attackLeft);
            }
            else
            {
                animator.SetInteger(animationState, (int)CharStates.attackRight);
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isAttacking = false;
            currentSpeed = wanderSpeed;
            if(moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            targetTransform = null;
        }
    }
    void OnDrawGizmos()
    {
        if(circleCollider != null)
        {
            Gizmos.DrawWireSphere(transform.position, circleCollider.radius);
        }
    }
}
