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
    private readonly float rayLength = 1.0f;
    public LayerMask groundLayer;
    public int score = 0;
    private Rigidbody2D rigidBody;
    private Animator animator;
    private bool isWalking = false;
    private bool isFalling = false;
    private FacingDirection facingDirection = FacingDirection.Right;

    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bonus"))
        {
            other.gameObject.SetActive(false);
            score += 1;
        }
    }

    void Update()
    {
        isWalking = false;

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
        return Physics2D.Raycast(this.transform.position, Vector2.down, rayLength, groundLayer.value);
    }
}
