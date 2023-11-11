using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UIElements;

public class WaypointFollowingPlatformController : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private GameObject[] waypoints;
    private int currentWaypointIndex = 0;

    void Update()
    {
        Vector2 nextWaypointPosition = waypoints[currentWaypointIndex].transform.position;
        float distanceToNextWaypoint = Vector2.Distance(nextWaypointPosition, transform.position);
        if (distanceToNextWaypoint < 0.1f)
        {
            if (currentWaypointIndex < waypoints.Length - 1)
            {
                currentWaypointIndex += 1;
            }
            else
            {
                currentWaypointIndex = 0;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, nextWaypointPosition, speed * Time.deltaTime);
    }
}
