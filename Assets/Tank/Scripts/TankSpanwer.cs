using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // For managing the win screen

public class TankSpanwer : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject tankPrefab; // Prefab of the tank to spawn
    [SerializeField] private Transform[] spawnPoints; // Array of spawn points
    [SerializeField] private float spawnInterval = 5f; // Time between spawn waves
    [SerializeField] private int initialSpawnCount = 3; // Initial number of tanks to spawn
    [SerializeField] private int spawnIncrement = 1; // Number of tanks to add per wave
    [SerializeField] private Transform player; // Reference to the player
    [SerializeField] private float minSpawnDistance = 20f; // Minimum distance from the player to spawn tanks

    [Header("Game Settings")]
    [SerializeField] private int pointsPerTank = 100; // Points awarded per tank destroyed
    [SerializeField] private int winScore = 50000; // Score required to win
    [SerializeField] private GameObject winScreen; // Win screen UI

    private int currentSpawnCount; // Tracks the number of tanks to spawn in the current wave
    private int playerScore; // Tracks the player's score

    void Start()
    {
        currentSpawnCount = initialSpawnCount;
        playerScore = 0;

        if (winScreen != null)
        {
            winScreen.SetActive(false); // Ensure the win screen is hidden at the start
        }

        StartCoroutine(SpawnTanks());
    }

    private IEnumerator SpawnTanks()
    {
        while (true)
        {
            for (int i = 0; i < currentSpawnCount; i++)
            {
                SpawnTank();
            }

            currentSpawnCount += spawnIncrement; // Increase the number of tanks for the next wave
            yield return new WaitForSeconds(spawnInterval); // Wait before spawning the next wave
        }
    }

    private void SpawnTank()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned to the TankSpawner.");
            return;
        }

        Transform spawnPoint;
        int attempts = 0;

        // Ensure the spawn point is far enough from the player
        do
        {
            spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            attempts++;
        } while (Vector3.Distance(spawnPoint.position, player.position) < minSpawnDistance && attempts < 10);

        // Instantiate the tank at the selected spawn point
        GameObject tank = Instantiate(tankPrefab, spawnPoint.position, spawnPoint.rotation);

        // Attach the TankDestroyed callback to the tank's destruction
        Tank tankScript = tank.GetComponent<Tank>();
        if (tankScript != null)
        {
            tankScript.OnTankDestroyed += HandleTankDestroyed;
        }
    }

    private void HandleTankDestroyed()
    {
        playerScore += pointsPerTank;

        // Check if the player has reached the win score
        if (playerScore >= winScore)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        Time.timeScale = 0f; // Pause the game
        if (winScreen != null)
        {
            winScreen.SetActive(true); // Show the win screen
        }
    }
}
