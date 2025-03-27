using UnityEngine;

public class TrophyController : MonoBehaviour
{
    public GameObject trophy; 
    public GameObject secretTrophy;

    void Start()
    {
        // Show trophy only if hasCompletedGame is true
        trophy.SetActive(Level0Transition1.hasCompletedGame);
        secretTrophy.SetActive(Level0Transition2.hasCompletedSecret);
    }
}
