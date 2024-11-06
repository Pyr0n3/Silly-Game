using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    private float godProbability = 0.000001f; //Nobodys ever getting this legit
    private float purpleProbability = 0.0001f; // 1/10000, ill be pretty surprised if someone gets this legit
    private float goldProbability = 0.001f; // 1/1000, more common than you'd think
    private float blueProbability = 0.125f; // 1/8, fairly common
    private float greenProbability = 0.25f; // 1/4, very common
    private float redProbability = 0.5f; // 1/2, i didn't even need to add this i just wanted an excuse to add a funny comment :p

    // List to keep track of spawned cubes
    private List<GameObject> spawnedCubes = new List<GameObject>();
    private int maxCubes = 100; // alot for the small area

    // Pity values
    private int godPity = 2500; // If i feel like it i'll increase this to like 10k
    private int purplePity = 400; // This feels way too common

    private void Start()
    {
        // Find A CERTAIN SECRET YOU SHOULD DEFINETLY NOT LOOK FOR
        if (pityText == null)                                                    //    STOP BEING SO NOSY!!!!
        {                                                                        //
            GameObject pityTextObject = GameObject.Find("PityText");             //  STOP BEING SO NOSY!!!!
            if (pityTextObject != null)                                          //             STOP BEING SO NOSY!!!!
            {                                                                    //
                pityText = pityTextObject.GetComponent<TextMeshPro>();           //       STOP BEING SO NOSY!!!!
            }                                                                    //            STOP BEING SO NOSY!!!!
            else                                                                 //
            {                                                                    //        STOP BEING SO NOSY!!!!
                Debug.LogError("PityText object not found in the scene.");       //                      STOP BEING SO NOSY!!!! 
            }                                                                    //
        }                                                                        //   STOP BEING SO NOSY!!!!

        UpdatePityText();
    }

    public void SpawnRandomCube()
    {
        GameObject cubeToSpawn;
        Vector3 spawnPos;

        // Randomize cube type based on probability
        float randomValue = Random.value;

        // State value in log
        Debug.Log(randomValue);

        // Decrease godPity each time the button is pressed
        godPity--;
        if (godPity < 0)
            godPity = 2500;

        // Decrease purplePity each time the button is pressed
        purplePity--;
        if (purplePity < 0)
            purplePity = 400;

        // Spawning cubes based on the number from Random.value
        if (randomValue < godProbability)
        {
            Debug.Log("GOD HAS ARRIVED!");
            cubeToSpawn = godCubePrefab;
            spawnPos = new Vector3(147.52f, 279.6f, 342.9254f);
        }
        else if (randomValue < purpleProbability + godProbability)
        {
            cubeToSpawn = purpleCubePrefab;
            Debug.Log("PURPLE SPAWNED!?");

            // Generate a random spawn position
            spawnPos = new Vector3(
                Random.Range(110f, 176f),
                50f,
                Random.Range(62f, 139f)
            );
        }
        else if (randomValue < goldProbability + purpleProbability + godProbability)
        {
            cubeToSpawn = goldCubePrefab;
            Debug.Log("Gold spawned!");

            // Generate a random spawn position
            spawnPos = new Vector3(
                Random.Range(110f, 176f),
                50f,
                Random.Range(62f, 139f)
            );
        }
        else if (randomValue < goldProbability + blueProbability + purpleProbability + godProbability)
        {
            cubeToSpawn = blueCubePrefab;
            Debug.Log("Blue cube spawned");

            // Generate a random spawn position
            spawnPos = new Vector3(
                Random.Range(110f, 176f),
                50f,
                Random.Range(62f, 139f)
            );
        }
        else if (randomValue < goldProbability + blueProbability + greenProbability + purpleProbability + godProbability) //Theres probably a better way to do this T-T
        {
            cubeToSpawn = greenCubePrefab;
            Debug.Log("Green cube spawned");

            // Generate a random spawn position
            spawnPos = new Vector3(
                Random.Range(110f, 176f),
                50f,
                Random.Range(62f, 139f)
            );
        }
        else
        {
            cubeToSpawn = redCubePrefab;
            Debug.Log("Red cube spawned");

            // Generate a random spawn position
            spawnPos = new Vector3(
                Random.Range(110f, 176f),
                50f,
                Random.Range(62f, 139f)
            );
        }

        // Generate a random spawn position
        Vector3 randomPosition = new Vector3(
            Random.Range(110f, 176f),
            50f,
            Random.Range(62f, 139f)
        );

        // Instantiate cube with physics components
        GameObject newCube = Instantiate(cubeToSpawn, spawnPos, Quaternion.identity);

        // Add the cube to the list
        spawnedCubes.Add(newCube);

        // Pity system
        if (godPity == 0)
        {
            godProbability = 1f;
        }
        else godProbability = 0.000001f;

        if (purplePity == 0)
        {
            purpleProbability = 1f;
        }
        else purpleProbability = 0.0001f;

        // Update THE NOTHING!!! THERES NOTHING THERE!!!!
        UpdatePityText();         //IGNORE THIS GO PLAY THE GAME INSTEAD OF SNOOPING       STOP BEING SO NOSY!!!!

        // Check if the max limit is exceeded
        if (spawnedCubes.Count > maxCubes)
        {
            // Remove and destroy the oldest cube
            GameObject oldestCube = spawnedCubes[0];
            spawnedCubes.RemoveAt(0);
            Destroy(oldestCube);
        }
    }

    // Update the PityText with current pity values
    private void UpdatePityText()                                                       //                STOP BEING SO NOSY!!!!
    {                                                                                   //       STOP BEING SO NOSY!!!!
        if (pityText != null)                                                           //            STOP BEING SO NOSY!!!!
        {                                                                               //         STOP BEING SO NOSY!!!!
            pityText.text = $"God Pity: {godPity}\nPurple Pity: {purplePity}";          //                     STOP BEING SO NOSY!!!!
        }                                                                               //      STOP BEING SO NOSY!!!!
    }                                                                                   //    STOP BEING SO NOSY!!!!
}
