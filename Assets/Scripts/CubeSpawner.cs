using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class CubeSpawner : MonoBehaviour
{
    //luck boost booleans
    public bool secretOn;
    public bool hasCompletedGame;

    //Cube prefabs
    public GameObject redCubePrefab;
    public GameObject greenCubePrefab;
    public GameObject blueCubePrefab;
    public GameObject goldCubePrefab;
    public GameObject purpleCubePrefab;
    public GameObject godCubePrefab;
    public GameObject cyanCubePrefab; // Added cyan cube prefab
    public GameObject blackCubePrefab; // Black hole "cube"

    public GameObject Trophy;

    public TextMeshPro pityText;
    public AudioSource godCubeBuildupAudio;
    public AudioSource cyanCubeBuildupAudio; // Added cyan buildup audio reference

    private float blackProbability = 0.0000001f;
    private float godProbability = 0.000001f;
    private float cyanProbability = 0.00001f; // 1 in 100,000 chance for cyan cube
    private float purpleProbability = 0.0001f;
    private float goldProbability = 0.001f;
    private float blueProbability = 0.125f;
    private float greenProbability = 0.25f;
    private float redProbability = 0.5f; 

    private List<GameObject> spawnedCubes = new List<GameObject>();
    private int maxCubes = 3000;

    private int blackPity = 1000000;
    private int godPity = 250000;
    private int purplePity = 6000;
    private int cyanPity = 20000; // Cyan cube pity value

    private void Update()
    {
        if (GameObject.Find("Trophy").activeSelf)
            hasCompletedGame = true;
        else hasCompletedGame = false;
    }
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
        UpdatePityText();


    }

    public void SpawnSingleCubeInstance()
    {
        float randomValue = Random.value;
        Debug.Log(randomValue);

        blackPity--;
        if (blackPity < 0) blackPity = 1000000;
        godPity--;
        if (godPity < 0) godPity = 250000;
        purplePity--;
        if (purplePity < 0) purplePity = 4000;
        cyanPity--;
        if (cyanPity < 0) cyanPity = 45000;

        if (randomValue < blackProbability)
        {
           PlayBlackCubeSpawn();
        }
        else if (randomValue < godProbability)
        {
            StartCoroutine(PlayGodCubeSpawn());
        }
        else if (randomValue < cyanProbability)
        {
            StartCoroutine(PlayCyanCubeSpawn()); 
        }
        else if (randomValue <purpleProbability)
        {
            PlayPurpleCubeSpawn();
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

        }



        if (secretOn == true || hasCompletedGame == true)
        {
            blackProbability = blackPity == 0 ? 1f : 0.0000005f;
            godProbability = godPity == 0 ? 1f : 0.000005f;
            cyanProbability = cyanPity == 0 ? 1f : 0.00005f;
            purpleProbability = purplePity == 0 ? 1f : 0.0005f;
            UpdatePityText();
        }
        else
        {
            blackProbability = blackPity == 0 ? 1f : 0.0000001f;
            godProbability = godPity == 0 ? 1f : 0.000001f;
            cyanProbability = cyanPity == 0 ? 1f : 0.00001f;
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
    if (secretOn || hasCompletedGame)
    {
        blackProbability = 0.0000005f;
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
        blackProbability = 0.0000001f;
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

    cumulativeProbability += blackProbability;
    if (randomValue < cumulativeProbability) return blackCubePrefab;

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
    private void PlayBlackCubeSpawn()
    {
        Vector3 spawnPos = new Vector3(243f, 216f, 171f);

        GameObject blackCube = Instantiate(blackCubePrefab, spawnPos, Quaternion.identity);
        spawnedCubes.Add(blackCube);
    }

    private void PlayPurpleCubeSpawn()
    {
        Vector3 spawnPos = new Vector3(
    Random.Range(176f, 371f),
    157.5f,
    Random.Range(63f, 298f)
        );

        GameObject purpleCube = Instantiate(purpleCubePrefab, spawnPos, Quaternion.identity);
        spawnedCubes.Add(purpleCube);
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


        AudioSource godCubeAudio = godCube.GetComponent<AudioSource>();
        if (godCubeAudio != null)
        {
            godCubeAudio.time = 2f;
            godCubeAudio.Play(); // Continue audio after the buildup
        }
    }
/*
    private IEnumerator PlayBlackCubeSpawn()
    {
        Vector3 spawnPos = new Vector3(243f, 216f, 171f);

        GameObject blackCube = Instantiate(blackCubePrefab, spawnPos, Quaternion.identity);
        spawnedCubes.Add(blackCube);
        
    }
*/
    private void UpdatePityText()
    {
        if (pityText != null)
        {
            pityText.text = $"God Pity: {godPity}\nCyan Pity: {cyanPity}\nPurple Pity: {purplePity}\n Black Pity: {blackPity}";
        }
    }
}
