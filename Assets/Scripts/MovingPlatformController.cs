using UnityEngine;

enum MovementAxis
{
    X, 
    Y
}

public class MovingPlatformController : MonoBehaviour
{
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float minOffset = 2.0f;
    [SerializeField] private float maxOffset = 2.0f;
    [SerializeField] private MovementAxis movementAxis = MovementAxis.X;
    private Vector3 initialPosition;
    private bool movingTowardsPositive = false;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    { 
        float currentPosition = movementAxis == MovementAxis.X ? transform.position.x : transform.position.y;
        float initialPositionValue = movementAxis == MovementAxis.X ? initialPosition.x : initialPosition.y;

        if(currentPosition < initialPositionValue - minOffset)
        {
            movingTowardsPositive = true;
        }
        else if (currentPosition > initialPositionValue + maxOffset)
        {
            movingTowardsPositive = false;
        }

        Vector3 movement;

        if (movementAxis == MovementAxis.X)
        {
            movement = new Vector3(speed * Time.deltaTime, 0.0f, 0.0f);
        }
        else
        {
            movement = new Vector3(0.0f, speed * Time.deltaTime, 0.0f);
        }

        if(!movingTowardsPositive)
        {
            movement.x = movement.x * -1;
            movement.y = movement.y * -1;
        }

        transform.Translate(movement, Space.World);
    }
}
