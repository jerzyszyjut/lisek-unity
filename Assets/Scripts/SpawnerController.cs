using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnInterval = 5.0f;
    private float currentSpawnCooldown;

    void Start()
    {
        currentSpawnCooldown = spawnInterval;
    }

    void Update()
    {
        if(currentSpawnCooldown >= 0.0f)
        {
            currentSpawnCooldown -= Time.deltaTime;
        }
        else
        {
            currentSpawnCooldown = spawnInterval;
            GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            enemy.GetComponent<Pathfinding.AIDestinationSetter>().target = GameManager.instance.player.transform;
        }
    }
}
