using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // Start is called before the first frame update
    public LayerMask groundLayer;

    void Start()
    {
        Destroy(gameObject, 10.0f);
    }

    private void Update()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 225.0f);

        if(Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer.value) 
            || Physics2D.Raycast(transform.position, Vector2.up, 0.1f, groundLayer.value)
            || Physics2D.Raycast(transform.position, Vector2.left, 0.1f, groundLayer.value)
            || Physics2D.Raycast(transform.position, Vector2.right, 0.1f, groundLayer.value)
            )
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer.Equals(groundLayer.value))
        {
            Destroy(gameObject);
        }
    }
}
