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
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip bonusSound;
    [SerializeField] private AudioClip killEnemySound;
    [SerializeField] private AudioClip keySound;
    [SerializeField] private float shootingCooldown = 0.5f;
    private AudioSource source;
    private readonly float rayLength = 1.0f;
    public LayerMask groundLayer;
    private Rigidbody2D rigidBody;
    private Animator animator;
    private bool isWalking = false;
    private bool isFalling = false;
    private FacingDirection facingDirection = FacingDirection.Right;
    Vector3 initialPosition;
    private float currentShootingCooldown = 0.0f;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

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
            source.PlayOneShot(bonusSound, AudioListener.volume);
        }
        if (other.CompareTag("Exit"))
        {
            GameManager.instance.AddPoints(GameManager.instance.GetLives() * 100);
            GameManager.instance.LevelCompleted();
        }
        if (other.CompareTag("Enemy"))
        {
            if (transform.position.y > other.gameObject.transform.position.y)
            {
                GameManager.instance.IncrementEnemiesDefeated();
                source.PlayOneShot(killEnemySound, AudioListener.volume);
            }
            else
            {
                if (other.GetComponent<EnemyController>().currentHitpoints > 0.0f)
                {
                    Die();
                }
            }
        }
        if (other.CompareTag("Key"))
        { 
            other.gameObject.SetActive(false);
            GameManager.instance.AddKey(other.gameObject.GetComponent<SpriteRenderer>().sprite);
            source.PlayOneShot(keySound, AudioListener.volume);
        }        
        if (other.CompareTag("Lifes"))
        {
            other.gameObject.SetActive(false);
            GameManager.instance.IncrementLives();
        }
        if (other.CompareTag("Fall level"))
        {
            Die();
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

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
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

        currentShootingCooldown -= Time.deltaTime;
        Debug.Log(currentShootingCooldown);
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
        //Debug.DrawRay(transform.position - new Vector3(0.3f, 0.0f, 0.0f), transform.TransformDirection(Vector2.down) * rayLength);
        //Debug.DrawRay(transform.position, transform.TransformDirection(Vector2.down) * rayLength);
        //Debug.DrawRay(transform.position + new Vector3(0.3f, 0.0f, 0.0f), transform.TransformDirection(Vector2.down) * rayLength);
        return Physics2D.Raycast(transform.position, Vector2.down, rayLength, groundLayer.value) ||
            Physics2D.Raycast(transform.position - new Vector3(0.3f, 0.0f, 0.0f), Vector2.down, rayLength, groundLayer.value) ||
            Physics2D.Raycast(transform.position + new Vector3(0.3f, 0.0f, 0.0f), Vector2.down, rayLength, groundLayer.value);
    }

    void Shoot()
    {
        if (currentShootingCooldown > 0.0f) return;

        currentShootingCooldown = shootingCooldown;
        Debug.Log(currentShootingCooldown);
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        Vector3 cursorWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 shootVector = (cursorWorldPosition - transform.position).normalized;
        bulletRb.AddForce(shootVector * 20.0f, ForceMode2D.Impulse);
        source.PlayOneShot(shootSound, AudioListener.volume);
    }

    void Die()
    {
        transform.position = initialPosition;
        rigidBody.velocity = Vector3.zero;
        GameManager.instance.DecrementLives();
    }
}
