using UnityEngine;
using UnityEngine.SceneManagement;

public class Level0Transition1 : MonoBehaviour
{
    public static bool hasCompletedGame = false; // Static to persist across scenes

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("Silly Game");
            hasCompletedGame = true;
        }
    }
}