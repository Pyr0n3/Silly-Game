using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject redCubePrefab;
    public GameObject greenCubePrefab;
    public GameObject blueCubePrefab;

    // Probabilities for each cube type
    private float redProbability = 0.5f;  // 1/2 chance
    private float greenProbability = 0.25f;  // 1/4 chance
    private float blueProbability = 0.125f;  // 1/8 chance

    public void SpawnRandomCube()
    {
        float randomValue = Random.value;

        if (randomValue < blueProbability)
        {
            Instantiate(blueCubePrefab, transform.position, Quaternion.identity);
            Debug.Log("Blue Cube Spawned!");
        }
        else if (randomValue < greenProbability + blueProbability)
        {
            Instantiate(greenCubePrefab, transform.position, Quaternion.identity);
            Debug.Log("Green Cube Spawned!");
        }
        else
        {
            Instantiate(redCubePrefab, transform.position, Quaternion.identity);
            Debug.Log("Red Cube Spawned!");
        }
    }
}
