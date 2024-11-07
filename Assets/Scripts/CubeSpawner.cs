using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CubeSpawner : MonoBehaviour
{
    public GameObject redCubePrefab;
    public GameObject greenCubePrefab;
    public GameObject blueCubePrefab;
    public GameObject goldCubePrefab;
    public GameObject purpleCubePrefab;
    public GameObject godCubePrefab;

    // Reference to the PityText object with TextMeshPro
    public TextMeshPro pityText;

    // Probability for each cube type
    private float godProbability = 0.000001f;
    private float purpleProbability = 0.0001f;
    private float goldProbability = 0.001f;
    private float blueProbability = 0.125f;
    private float greenProbability = 0.25f;
    private float redProbability = 0.5f;

    // List to keep track of spawned cubes
    private List<GameObject> spawnedCubes = new List<GameObject>();
    private int maxCubes = 1000;

    // Pity values
    private int godPity = 250000;
    private int purplePity = 4000;

    private void Start()
    {
        if (pityText == null)
        {
            GameObject pityTextObject = GameObject.Find("PityText");
            if (pityTextObject != null)
            {
                pityText = pityTextObject.GetComponent<TextMeshPro>();
            }
            else
            {
                Debug.LogError("PityText object not found in the scene.");
            }
        }

        UpdatePityText();
    }

    public void SpawnSingleCube()
    {
        SpawnSingleCubeInstance();
    }

    public void SpawnMultipleCubes(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnSingleCubeInstance();
        }
    }

    private void SpawnSingleCubeInstance()
    {
        GameObject cubeToSpawn;
        Vector3 spawnPos;

        // Randomize cube type based on probability
        float randomValue = Random.value;
        Debug.Log("Random Value: " + randomValue);

        // Decrease godPity and purplePity
        godPity--;
        if (godPity < 0) godPity = 250000;
        purplePity--;
        if (purplePity < 0) purplePity = 4000;

        // Determine cube type based on probabilities
        if (randomValue < godProbability)
        {
            Debug.Log("GOD HAS ARRIVED!");
            cubeToSpawn = godCubePrefab;
            spawnPos = new Vector3(147.52f, 279.6f, 342.9254f); // Fixed spawn location for God cube
        }
        else
        {
            spawnPos = new Vector3(
                Random.Range(176f, 371f),
                57.5f,
                Random.Range(63f, 298f)
            );

            if (randomValue < purpleProbability + godProbability)
            {
                cubeToSpawn = purpleCubePrefab;
                Debug.Log("PURPLE SPAWNED!?");
            }
            else if (randomValue < goldProbability + purpleProbability + godProbability)
            {
                cubeToSpawn = goldCubePrefab;
                Debug.Log("Gold spawned!");
            }
            else if (randomValue < blueProbability + goldProbability + purpleProbability + godProbability)
            {
                cubeToSpawn = blueCubePrefab;
                Debug.Log("Blue cube spawned");
            }
            else if (randomValue < greenProbability + blueProbability + goldProbability + purpleProbability + godProbability)
            {
                cubeToSpawn = greenCubePrefab;
                Debug.Log("Green cube spawned");
            }
            else
            {
                cubeToSpawn = redCubePrefab;
                Debug.Log("Red cube spawned");
            }
        }

        // Instantiate cube with physics components
        GameObject newCube = Instantiate(cubeToSpawn, spawnPos, Quaternion.identity);

        // Add the cube to the list
        spawnedCubes.Add(newCube);

        // Pity system
        godProbability = godPity == 0 ? 1f : 0.000001f;
        purpleProbability = purplePity == 0 ? 1f : 0.0001f;

        // Update pity text
        UpdatePityText();

        // Check if the max limit is exceeded
        if (spawnedCubes.Count > maxCubes)
        {
            GameObject oldestCube = spawnedCubes[0];
            spawnedCubes.RemoveAt(0);
            Destroy(oldestCube);
        }
    }

    private void UpdatePityText()
    {
        if (pityText != null)
        {
            pityText.text = $"God Pity: {godPity}\nPurple Pity: {purplePity}";
        }
    }
}
