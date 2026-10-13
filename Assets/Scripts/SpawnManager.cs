using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefab;
    private PlayerController playerController;
    private Vector3 spawnPos = new Vector3(25f, 0.5f, 0.5f);

    private float spawnTime; // 2 second delay before the first spawn;
    private float spawnRange; // Range for random spawn time (between spawnRange and spawnRange + 3 seconds)
    private float increaseDifficulty; // Factor to increase difficulty (reduce spawn time)
    private float frequencyTime; // Time interval to increase difficulty
    private float difficultyTime; // Next time to increase difficulty


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnTime = Time.time + 2f;
        spawnRange = 1f;
        increaseDifficulty = 0.9f; //Must be less than 1 to reduce spawn time and increase difficulty
        frequencyTime = 10f;
        difficultyTime = Time.time + frequencyTime;

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController != null && !playerController.isGameOver)
        {
            SpawnObstacle();
        }
    }

    void SpawnObstacle()
    {
        GameObject obstacle = obstaclePrefab[Random.Range(0, obstaclePrefab.Length)]; // Randomly select an obstacle prefab
        if (obstacle != null && Time.time >= spawnTime)
        {
            Instantiate(obstacle, spawnPos, obstacle.transform.rotation);
            spawnTime = Time.time + Random.Range(spawnRange, spawnRange + 3f); // Random spawn range for next spawn
        }
        if (Time.time >= difficultyTime)
        {
            spawnRange *= increaseDifficulty; // Reduce spawn range to increase difficulty
            difficultyTime = Time.time + frequencyTime; // Set next time to increase difficulty
        }
    }

    }
