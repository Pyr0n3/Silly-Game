using UnityEngine;

public class TrophyController : MonoBehaviour
{
    public GameObject trophy; // Assign the trophy GameObject in the Inspector

    void Start()
    {
        // Show trophy only if hasCompletedGame is true
        trophy.SetActive(Level0Transition1.hasCompletedGame);
    }
}
