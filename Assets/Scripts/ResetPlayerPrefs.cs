using UnityEngine;



public class ResetPlayerPrefs : MonoBehaviour

{

    public void ResetAllPrefs()

    {

        PlayerPrefs.DeleteAll();

    }

}
