using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement parameters")]
    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 0.1f;
    [SerializeField] private float moveRange = 1.0f;
    [SerializeField] private float hitpoints = 100.0f;
    [SerializeField] HealthBarController healthbar;
    private float currentHitpoints = 100.0f;
    private Animator animator;
    private FacingDirection facingDirection = FacingDirection.Left;
    private float startPositionX;

    void Start()
    {
        startPositionX = transform.position.x;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float currentPositionX = transform.position.x;
        if (currentPositionX < startPositionX - moveRange && facingDirection == FacingDirection.Left)
        {
            Flip();
        }
        else if (currentPositionX > startPositionX + moveRange && facingDirection == FacingDirection.Right)
        {
            Flip();
        }

        Vector3 movement;

        if (facingDirection == FacingDirection.Left)
        {
            movement = new Vector3(-1 * moveSpeed * Time.deltaTime, 0.0f, 0.0f);
        }
        else
        {
            movement = new Vector3(moveSpeed * Time.deltaTime, 0.0f, 0.0f);
        }

        transform.Translate(movement, Space.World);
    }

    private bool isDead = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isDead)
        {
            if (other.CompareTag("Player"))
            {
                if (transform.position.y < other.gameObject.transform.position.y)
                {
                    isDead = true;
                    animator.SetBool("isDead", true);
                    StartCoroutine(KillOnAnimationEnd());
                }
            }
            if (other.CompareTag("Bullet"))
            {
                currentHitpoints -= 40.0f;
                if (currentHitpoints <= 0.0f)
                {
                    isDead = true;
                    animator.SetBool("isDead", true);
                    StartCoroutine(KillOnAnimationEnd());
                    GameManager.instance.IncrementEnemiesDefeated();
                    healthbar.UpdateHealthBar(0.0f);
                }
                else
                {
                    healthbar.UpdateHealthBar(currentHitpoints / hitpoints);
                }
                Destroy(other);
            }
        }
    }

    IEnumerator KillOnAnimationEnd()
    {
        yield return new WaitForSeconds(0.8f);
        gameObject.SetActive(false);
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
        Vector3 healthbarScale = healthbar.slider.transform.localScale;
        healthbarScale.x *= -1;
        healthbar.slider.transform.localScale = healthbarScale;
    }
}
