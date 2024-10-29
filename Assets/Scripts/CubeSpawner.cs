using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject redCubePrefab;
    public GameObject greenCubePrefab;
    public GameObject blueCubePrefab;
    public GameObject goldCubePrefab;

    // Probability for each cube type
    private float goldProbability = 0.001f; // 1/1000 probability for gold
    private float blueProbability = 0.125f;   // 1/8 probability for blue
    private float greenProbability = 0.25f;   // 1/4 probability for green
    private float redProbability = 0.5f;      // 1/2 probability for red

    // List to keep track of spawned cubes
    private List<GameObject> spawnedCubes = new List<GameObject>();
    private int maxCubes = 10;

    public void SpawnRandomCube()
    {
        GameObject cubeToSpawn;

        // Randomize cube type based on probability
        float randomValue = Random.value;
        if (randomValue < goldProbability)
            cubeToSpawn = goldCubePrefab;
        else if (randomValue < goldProbability + blueProbability)
            cubeToSpawn = blueCubePrefab;
        else if (randomValue < goldProbability + blueProbability + greenProbability)
            cubeToSpawn = greenCubePrefab;
        else
            cubeToSpawn = redCubePrefab;

        // Generate a random spawn position
        Vector3 randomPosition = new Vector3(
            Random.Range(110f, 176f), // Replace with your min/max spawn positions
            50f,                     // Fixed Y position
            Random.Range(62f, 139f)  // Replace with your min/max spawn positions
        );

        // Instantiate cube with physics components
        GameObject newCube = Instantiate(cubeToSpawn, randomPosition, Quaternion.identity);
        newCube.AddComponent<Rigidbody>(); // Ensure Rigidbody is added if not already on prefab

        // Add the cube to the list
        spawnedCubes.Add(newCube);

        // Check if the max limit is exceeded
        if (spawnedCubes.Count > maxCubes)
        {
            // Remove and destroy the oldest cube
            GameObject oldestCube = spawnedCubes[0];
            spawnedCubes.RemoveAt(0);
            Destroy(oldestCube);
        }
    }
}
