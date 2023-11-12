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
    [SerializeField] private int lives = 3;
    [SerializeField] private int keysNumber = 3;
    private int keysFound = 0;
    private readonly float rayLength = 1.0f;
    public LayerMask groundLayer;
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
            GameManager.instance.AddPoints(1);
        }
        if (other.CompareTag("Finish"))
        {
            GameManager.instance.LevelCompleted();
        }
        if (other.CompareTag("Enemy"))
        {
            if (transform.position.y > other.gameObject.transform.position.y)
            {
                GameManager.instance.AddPoints(1);
            }
            else
            {
                Die();
            }
        }
        if (other.CompareTag("Key"))
        { 
            other.gameObject.SetActive(false);
            GameManager.instance.AddKey(other.gameObject.GetComponent<SpriteRenderer>().color);
        }        
        if (other.CompareTag("Lifes"))
        {
            other.gameObject.SetActive(false);
            lives += 1;
            GameManager.instance.SetLives(lives);
            Debug.Log($"Found one up! Current lives: {lives}");
        }
        if (other.CompareTag("Fall level"))
        {
            Die();
            Debug.Log($"You have fallen and died!!! Current lives: {lives}");
        }
        if (other.CompareTag("MovingPlatform"))
        {
            transform.SetParent(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MovingPlatform"))
        {
            transform.SetParent(null);
        }
    }

    void Update()
    {
        isWalking = false;

        if (GameManager.instance.currentGameState == GameState.GS_GAME)
        {
            if (lives > 0)
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

            
        }

        if (rigidBody.velocity.y < -0.5f)
        {
            isFalling = true;
        }
        else
        {
            isFalling = false;
        }

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
        lives -= 1;
        GameManager.instance.SetLives(lives);
        if(lives <= 0)
        {
            Debug.Log("Game over!");
        }
    }
}
