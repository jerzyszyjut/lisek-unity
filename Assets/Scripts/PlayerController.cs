using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum FacingDirection
{
    Left,
    Right
}

public class PlayerController : MonoBehaviour
{
    [Header("Movement parameters")]
    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 0.1f;
    [Range(0.01f, 20.0f)][SerializeField] private float jumpForce = 6.0f;
    [SerializeField] private int lifes = 3;
    [SerializeField] private int keysNumber = 3;
    private int keysFound = 0;
    private readonly float rayLength = 1.0f;
    public LayerMask groundLayer;
    public int score = 0;
    private Rigidbody2D rigidBody;
    private Animator animator;
    private bool isWalking = false;
    private bool isFalling = false;
    private FacingDirection facingDirection = FacingDirection.Right;
    Vector3 initialPosition;

    void Start()
    { 
        initialPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bonus"))
        {
            other.gameObject.SetActive(false);
            score += 1;
            Debug.Log("Picked up cherry! Current score: " + score);
        }
        if (other.CompareTag("Finish line"))
        {
            if(keysFound >= keysNumber)
            {
                other.gameObject.SetActive(false);
                Debug.Log("Player reached the end of the level!");
            }
            else
            {
                Debug.Log($"You need to find {keysNumber - keysFound} more keys!");
            }
        }
        if (other.CompareTag("Enemy"))
        {
            if (transform.position.y > other.gameObject.transform.position.y)
            {
                score += 1;
                Debug.Log("Killed an enemy! Current score: " + score);
            }
            else
            {
                Die();
                Debug.Log("Died from enemy! Current lives: " + lifes);
            }
        }
        if (other.CompareTag("Key"))
        {
            other.gameObject.SetActive(false);
            keysFound += 1;
            Debug.Log($"Found key! Current number of keys found {keysFound}");
        }        
        if (other.CompareTag("Lifes"))
        {
            other.gameObject.SetActive(false);
            lifes += 1;
            Debug.Log($"Found one up! Current lives: {lifes}");
        }
        if (other.CompareTag("Fall level"))
        {
            Die();
            Debug.Log($"You have fallen and died!!! Current lives: {lifes}");
        }
    }

    void Update()
    {
        isWalking = false;

        if(lifes > 0)
        {
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                if (facingDirection == FacingDirection.Left)
                {
                    Flip();
                }
                isWalking = true;
                transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            }

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                if (facingDirection == FacingDirection.Right)
                {
                    Flip();
                }
                isWalking = true;
                transform.Translate(-1 * moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            }

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }

        if(rigidBody.velocity.y < 0.0f)
        {
            isFalling = true;
        } else
        {
            isFalling = false;
        }

        //Debug.DrawRay(transform.position, rayLength * Vector3.down, Color.white, 1, false);
        animator.SetBool("isGrounded", IsGrounded());
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isFalling", isFalling);
    }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Flip()
    {
        if (facingDirection != FacingDirection.Left)
        {
            facingDirection = FacingDirection.Left;
        }
        else
        {
            facingDirection = FacingDirection.Right;
        }

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void Jump()
    {
        if (IsGrounded())
        {
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, rayLength, groundLayer.value);
    }

    void Die()
    {
        transform.position = initialPosition;
        rigidBody.velocity = Vector3.zero;
        lifes -= 1;
        if(lifes <= 0)
        {
            Debug.Log("Game over!");
        }

    }
}
