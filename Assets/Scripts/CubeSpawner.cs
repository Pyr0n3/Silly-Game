using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class CubeSpawner : MonoBehaviour
{
    public bool secretOn;

    public GameObject redCubePrefab;
    public GameObject greenCubePrefab;
    public GameObject blueCubePrefab;
    public GameObject goldCubePrefab;
    public GameObject purpleCubePrefab;
    public GameObject godCubePrefab;
    public GameObject cyanCubePrefab; // Added cyan cube prefab
    public GameObject Trophy;

    public TextMeshPro pityText;
    public AudioSource godCubeBuildupAudio;
    public AudioSource cyanCubeBuildupAudio; // Added cyan buildup audio reference

    private float godProbability = 0.000001f;
    private float cyanProbability = 0.00001f; // 1 in 100,000 chance for cyan cube
    private float purpleProbability = 0.0001f;
    private float goldProbability = 0.001f;
    private float blueProbability = 0.125f;
    private float greenProbability = 0.25f;
    private float redProbability = 0.5f; 

    private List<GameObject> spawnedCubes = new List<GameObject>();
    private int maxCubes = 3000;

    private int godPity = 250000;
    private int purplePity = 4000;
    private int cyanPity = 10000; // Cyan cube pity value

    private ScreenShake screenShake;

    public void Start()
    {
        // Initialize text, audio, and screen shake as before
        if (godCubeBuildupAudio == null)
        {
            godCubeBuildupAudio = GameObject.Find("GodCubeBuildupAudio").GetComponent<AudioSource>();
            if (godCubeBuildupAudio == null)
            {
                Debug.LogError("GodCubeBuildupAudio object not found in the scene.");
            }
        }

        if (cyanCubeBuildupAudio == null)
        {
            cyanCubeBuildupAudio = GameObject.Find("CyanCubeBuildupAudio").GetComponent<AudioSource>();
            if (cyanCubeBuildupAudio == null)
            {
                Debug.LogError("CyanCubeBuildupAudio object not found in the scene.");
            }
        }

        screenShake = Camera.main.GetComponent<ScreenShake>();
        UpdatePityText();


    }

    public void SpawnSingleCubeInstance()
    {
        float randomValue = Random.value;
        Debug.Log(randomValue);
        
        godPity--;
        if (godPity < 0) godPity = 250000;
        purplePity--;
        if (purplePity < 0) purplePity = 4000;

        cyanPity--; // Decrease cyan pity

        if (cyanPity == 0) // If cyan pity reaches 0, force a cyan cube spawn
        {
            StartCoroutine(PlayCyanCubeSpawn());
            cyanPity = 45000; // Reset cyan pity after spawning
            return;
        }

        if (randomValue < godProbability)
        {
            StartCoroutine(PlayGodCubeSpawn());
        }
        else if (randomValue < cyanProbability)
        {
            StartCoroutine(PlayCyanCubeSpawn()); // Start cyan cube spawn sequence
        }
        else
        {
            Vector3 spawnPos = new Vector3(
                Random.Range(176f, 371f),
                157.5f,
                Random.Range(63f, 298f)
            );

            GameObject cubeToSpawn = ChooseCubeType(randomValue);
            GameObject newCube = Instantiate(cubeToSpawn, spawnPos, Quaternion.identity);
            spawnedCubes.Add(newCube);

            // Apply screen shake based on cube type
            if (cubeToSpawn == purpleCubePrefab && screenShake != null)
            {
                screenShake.TriggerShake(1f, 1f); // Shake for purple cube
            }

        }



        if (secretOn == true)
        {
            godProbability = godPity == 0 ? 1f : 0.000005f;
            purpleProbability = purplePity == 0 ? 1f : 0.0005f;
            UpdatePityText();
        }
        else
        {
            godProbability = godPity == 0 ? 1f : 0.000001f;
            purpleProbability = purplePity == 0 ? 1f : 0.0001f;
            UpdatePityText();
        }

        if (spawnedCubes.Count > maxCubes)
        {
            GameObject oldestCube = spawnedCubes[0];
            spawnedCubes.RemoveAt(0);
            Destroy(oldestCube);
        }
    }
     
    public void SpawnMultipleCubes(int numberOfCubes)
    {
        // Spawn a random number of cubes from 1 to 10
        for (int i = 0; i < numberOfCubes; i++)
        {
            SpawnSingleCubeInstance();
        }
    }

    private GameObject ChooseCubeType(float randomValue)
{
    // Adjust probabilities based on the secret buff
    if (secretOn)
    {
        godProbability = 0.000005f;
        cyanProbability = 0.00005f;
        purpleProbability = 0.0005f;
        goldProbability = 0.001f;
        blueProbability = 0.125f;
        greenProbability = 1f;
        redProbability = 0f; // Red cube becomes unobtainable
    }
    else
    {
        godProbability = 0.000001f;
        cyanProbability = 0.00001f;
        purpleProbability = 0.0001f;
        goldProbability = 0.001f;
        blueProbability = 0.125f;
        greenProbability = 0.25f;
        redProbability = 1f;
    }

    // Normalize probabilities for comparison
    float cumulativeProbability = 0f;

    cumulativeProbability += godProbability;
    if (randomValue < cumulativeProbability) return godCubePrefab;

    cumulativeProbability += cyanProbability;
    if (randomValue < cumulativeProbability) return cyanCubePrefab;

    cumulativeProbability += purpleProbability;
    if (randomValue < cumulativeProbability) return purpleCubePrefab;

    cumulativeProbability += goldProbability;
    if (randomValue < cumulativeProbability) return goldCubePrefab;

    cumulativeProbability += blueProbability;
    if (randomValue < cumulativeProbability) return blueCubePrefab;

    cumulativeProbability += greenProbability;
    if (randomValue < cumulativeProbability) return greenCubePrefab;

    cumulativeProbability += redProbability;
    if (randomValue < cumulativeProbability) return redCubePrefab;

    // If no match, return null (this shouldn't happen if probabilities sum to 1)
    Debug.LogWarning("Random value did not match any probability range!");
    return null;
}


    private IEnumerator PlayCyanCubeSpawn()
    {
        Vector3 spawnPos = new Vector3(
                       Random.Range(176f, 371f),
                       57.5f,
                       Random.Range(63f, 298f)
                   );

        if (cyanCubeBuildupAudio != null)
        {
            cyanCubeBuildupAudio.Play(); // Play buildup audio
        }

        yield return new WaitForSeconds(3f); // Wait for 3 seconds for buildup

        GameObject cyanCube = Instantiate(cyanCubePrefab, spawnPos, Quaternion.identity);
        spawnedCubes.Add(cyanCube);

        // Trigger screen shake for cyan cube (moderate shake)
        if (screenShake != null)
        {
            screenShake.TriggerShake(2f, 2.5f); // Moderate shake for cyan cube
        }

        AudioSource cyanCubeAudio = cyanCube.GetComponent<AudioSource>();
        if (cyanCubeAudio != null)
        {
            cyanCubeAudio.time = 3f;
            cyanCubeAudio.Play();
        }
    }

    private IEnumerator PlayGodCubeSpawn()
    {
        Vector3 spawnPos = new Vector3(147.52f, 279.6f, 342.9254f);

        if (godCubeBuildupAudio != null)
        {
            godCubeBuildupAudio.Play();
        }

        yield return new WaitForSeconds(2f); // Wait for 2 seconds for buildup

        GameObject godCube = Instantiate(godCubePrefab, spawnPos, Quaternion.identity);
        spawnedCubes.Add(godCube);

        // Trigger screen shake for god cube (strong shake)
        if (screenShake != null)
        {
            screenShake.TriggerShake(1f, 5f); // Strong shake for god cube
        }

        AudioSource godCubeAudio = godCube.GetComponent<AudioSource>();
        if (godCubeAudio != null)
        {
            godCubeAudio.time = 2f;
            godCubeAudio.Play(); // Continue audio after the buildup
        }
    }

    private void UpdatePityText()
    {
        if (pityText != null)
        {
            pityText.text = $"God Pity: {godPity}\nCyan Pity: {cyanPity}\nPurple Pity: {purplePity}";
        }
    }
}
