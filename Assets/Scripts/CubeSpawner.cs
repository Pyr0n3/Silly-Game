using System.Collections;
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

    public TextMeshPro pityText;

    public AudioSource godCubeBuildupAudio; // New audio source for the buildup

    private float godProbability = 0.000001f;
    private float purpleProbability = 0.0001f;
    private float goldProbability = 0.001f;
    private float blueProbability = 0.125f;
    private float greenProbability = 0.25f;
    private float redProbability = 0.5f;

    private List<GameObject> spawnedCubes = new List<GameObject>();
    private int maxCubes = 1000;

    private int godPity = 200; //You're welcome - P
    private int purplePity = 40; //You're welcome - P

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

        if (godCubeBuildupAudio == null)
        {
            godCubeBuildupAudio = GameObject.Find("GodCubeBuildupAudio").GetComponent<AudioSource>();
            if (godCubeBuildupAudio == null)
            {
                Debug.LogError("GodCubeBuildupAudio object not found in the scene.");
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
        float randomValue = Random.value;
        Debug.Log("Random Value: " + randomValue);

        godPity--;
        if (godPity < 0) godPity = 250000;
        purplePity--;
        if (purplePity < 0) purplePity = 4000;

        if (randomValue < godProbability)
        {
            Debug.Log("GOD HAS ARRIVED!");
            StartCoroutine(PlayGodCubeSpawn());
        }
        else
        {
            Vector3 spawnPos = new Vector3(
                Random.Range(176f, 371f),
                57.5f,
                Random.Range(63f, 298f)
            );

            GameObject cubeToSpawn = ChooseCubeType(randomValue);
            GameObject newCube = Instantiate(cubeToSpawn, spawnPos, Quaternion.identity);
            spawnedCubes.Add(newCube);
        }

        godProbability = godPity == 0 ? 1f : 0.000001f;
        purpleProbability = purplePity == 0 ? 1f : 0.0001f;

        UpdatePityText();

        if (spawnedCubes.Count > maxCubes)
        {
            GameObject oldestCube = spawnedCubes[0];
            spawnedCubes.RemoveAt(0);
            Destroy(oldestCube);
        }
    }

    private GameObject ChooseCubeType(float randomValue)
    {
        if (randomValue < purpleProbability + godProbability)
        {
            Debug.Log("PURPLE SPAWNED!?");
            return purpleCubePrefab;
        }
        else if (randomValue < goldProbability + purpleProbability + godProbability)
        {
            Debug.Log("Gold spawned!");
            return goldCubePrefab;
        }
        else if (randomValue < blueProbability + goldProbability + purpleProbability + godProbability)
        {
            Debug.Log("Blue cube spawned");
            return blueCubePrefab;
        }
        else if (randomValue < greenProbability + blueProbability + goldProbability + purpleProbability + godProbability)
        {
            Debug.Log("Green cube spawned");
            return greenCubePrefab;
        }
        else
        {
            Debug.Log("Red cube spawned");
            return redCubePrefab;
        }
    }

    private IEnumerator PlayGodCubeSpawn()
    {
        Vector3 spawnPos = new Vector3(147.52f, 279.6f, 342.9254f);

        if (godCubeBuildupAudio != null)
        {
            godCubeBuildupAudio.Play();
        }

        yield return new WaitForSeconds(2f);

        GameObject godCube = Instantiate(godCubePrefab, spawnPos, Quaternion.identity);
        spawnedCubes.Add(godCube);

        AudioSource godCubeAudio = godCube.GetComponent<AudioSource>();
        if (godCubeAudio != null)
        {
            godCubeAudio.time = 2f; // Resume from the 2-second mark
            godCubeAudio.Play();
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
