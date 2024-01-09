using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _193064
{

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
        float angleZ = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg, angleY = 0.0f;
        if(angleZ >= 90.0f && angleZ <= 270.0f)
        {
            angleZ -= 180.0f;
            angleY = 180.0f;
        }
        transform.rotation = Quaternion.Euler(0.0f, angleY, angleZ);

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}

}
