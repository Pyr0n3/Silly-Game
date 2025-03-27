using UnityEngine;
using UnityEngine.SceneManagement;

public class Level0Transition2 : MonoBehaviour
{
    public static bool hasCompletedGame = false;
    public static bool hasCompletedSecret = false;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("Silly Game");
            hasCompletedGame = true;
            hasCompletedSecret = true;
        }
    }
}
