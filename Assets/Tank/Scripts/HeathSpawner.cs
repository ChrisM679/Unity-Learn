using UnityEngine;

public class HeathSpawner : MonoBehaviour
{
    [SerializeField] private GameObject healthPickupPrefab; // Prefab for the health pickup
    [SerializeField] private Transform[] spawnPoints; // Array of spawn points
    [SerializeField] private Health playerHealth; // Reference to the player's health component
    [SerializeField] private float spawnCooldown = 10f; // Cooldown between spawns

    private float lastSpawnTime;

    void Update()
    {
        // Check if the player's health is below half and the cooldown has passed
        if (playerHealth.CurrentHealth < playerHealth.CurrentHealthPercent * 0.5f && Time.time > lastSpawnTime + spawnCooldown)
        {
            SpawnHealthPickup();
            lastSpawnTime = Time.time;
        }
    }

    private void SpawnHealthPickup()
    {
        // Choose a random spawn point
        if (spawnPoints.Length == 0) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(healthPickupPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
