using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject redCubePrefab;
    public GameObject greenCubePrefab;
    public GameObject blueCubePrefab;
    public GameObject goldCubePrefab;
    public GameObject purpleCubePrefab;
    public GameObject godCubePrefab;

    // Probability for each cube type
    private float godProbability = 0.000001f; // Nobody is ever getting this
    private float purpleProbability = 0.0001f; // 1/10000 probability for purple
    private float goldProbability = 0.001f; // 1/1000 probability for gold
    private float blueProbability = 0.125f;   // 1/8 probability for blue
    private float greenProbability = 0.25f;   // 1/4 probability for green

    // List to keep track of spawned cubes
    private List<GameObject> spawnedCubes = new List<GameObject>();
    private int maxCubes = 100;

    // private List<GameObject> rolls = new List<GameObject>();
    private int godPityCount = 0;
    private int purplePityCount = 0;

    //pity values
    private int godPity = 2500;
    private int purplePity = 400;
    public void SpawnRandomCube()
    {
        GameObject cubeToSpawn;

        // Randomize cube type based on probability
        float randomValue = Random.value;

        //State value in log
        Debug.Log(randomValue);

        //Increase godPityCount each time button is pressed
        godPityCount++;
        if (godPityCount > godPity)
            godPityCount = 0;

        //Increase purplePityCount each time button is pressed
        purplePityCount++;
        if (purplePityCount > purplePity)
            purplePityCount = 0;

        //Spawning cubes based on the number from the Random.value
        if (randomValue < godProbability)
        {
            Debug.Log("GOD HAS ARRIVED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            cubeToSpawn = godCubePrefab;
        }
        else if (randomValue < purpleProbability + godProbability)
        {
            cubeToSpawn = purpleCubePrefab;
            Debug.Log("PURPLE SPAWNED!?");
        }
        else if (randomValue < goldProbability + purpleProbability + godProbability)
        {
            cubeToSpawn = goldCubePrefab;
            Debug.Log("Gold spawned!");
        }
        else if (randomValue < goldProbability + blueProbability + purpleProbability + godProbability)
        {
            cubeToSpawn = blueCubePrefab;
            Debug.Log("Blue cube spawned");
        }
        else if (randomValue < goldProbability + blueProbability + greenProbability + purpleProbability + godProbability)
        {
            cubeToSpawn = greenCubePrefab;
            Debug.Log("Green cube spawned");
        }
        else
        {
            cubeToSpawn = redCubePrefab;
            Debug.Log("Red cube spawned");
        }
            

        // Generate a random spawn position
        Vector3 randomPosition = new Vector3(
            Random.Range(110f, 176f), // Replace with your min/max spawn positions
            50f,                     // Fixed Y position
            Random.Range(62f, 139f)  // Replace with your min/max spawn positions
        );

        // Instantiate cube with physics components
        GameObject newCube = Instantiate(cubeToSpawn, randomPosition, Quaternion.identity);

        // Add the cube to the list
        spawnedCubes.Add(newCube);

        // Pity system
        if (godPityCount == godPity)
        {
            godProbability = 1f;
        }
        else godProbability = 0.000001f;

        if (purplePityCount == purplePity)
        {
            purpleProbability = 1f;
        }
        else purpleProbability = 0.0001f;

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
