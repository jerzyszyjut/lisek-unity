using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement parameters")]
    [Range(0.01f, 20.0f)] [SerializeField] private float moveSpeed = 0.1f; // move speed
    [Range(0.01f, 20.0f)][SerializeField] private float jumpForce = 6.0f;
    private readonly float rayLength = 1.0f;
    public LayerMask groundLayer;
    //public Vector3 theScale; //MF III 13 a)
    private Rigidbody2D rigidBody;
    private Animator animator; //MF
    private bool isWalking = false; //MF
    //posta� zawr�cena w prawo = true, w lewo = false
    private bool isFacingRight = true; //MF
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        isWalking = false; //MF
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            //MF
            if (!isFacingRight)
            {
                isFacingRight = true;
                Debug.Log("Going Right");
            }
            isWalking = true; //MF
            transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            //MF
            if (isFacingRight)
            {
                isFacingRight = false;
                Debug.Log("Going Left");
            }
            isWalking = true; //MF
            transform.Translate(-1 * moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        }
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        Debug.DrawRay(transform.position, rayLength * Vector3.down, Color.white, 1, false);
        animator.SetBool("isGrounded", IsGrounded()); //MF
        animator.SetBool("isWalking", isWalking); //MF

    }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); //MF 
    }

    //MF - ca�a metoda
    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale; //???
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    void Jump()
    {
        if (IsGrounded())
        {
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            Debug.Log("Jumping!");
        }
    }

    bool IsGrounded()
    {
        return Physics2D.Raycast(this.transform.position, Vector2.down, rayLength, groundLayer.value);
    }
}
