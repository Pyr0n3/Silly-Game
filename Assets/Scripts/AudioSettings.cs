using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public AudioSource spawnAudio;
    public AudioSource thump1Audio;
    public AudioSource thump2Audio;
    public AudioSource godBuildupAudio;  // AudioSource for the god cube buildup sound
    public AudioSource godMainAudio;     // AudioSource for the god cube main sound
    public AudioSource backgroundMusic;
    public AudioSource cyanBuildupAudio;
    public AudioSource cyanMainAudio;

    public Toggle spawnToggle;
    public Toggle thump1Toggle;
    public Toggle thump2Toggle;
    public Toggle godToggle;             // Single toggle for both god cube sounds (buildup + main)
    public Toggle backgroundToggle;
    public Toggle cyanToggle;

    private void Start()
    {
        // Set all toggles to unchecked when the options menu is opened
        ResetToggles();

        // Set toggle listeners
        spawnToggle.onValueChanged.AddListener(delegate { ToggleAudio(spawnAudio, spawnToggle); });
        thump1Toggle.onValueChanged.AddListener(delegate { ToggleAudio(thump1Audio, thump1Toggle); });
        thump2Toggle.onValueChanged.AddListener(delegate { ToggleAudio(thump2Audio, thump2Toggle); });
        godToggle.onValueChanged.AddListener(delegate { ToggleGodAudio(godToggle); });
        backgroundToggle.onValueChanged.AddListener(delegate { ToggleAudio(backgroundMusic, backgroundToggle); });
        cyanToggle.onValueChanged.AddListener(delegate {  ToggleCyanAudio(cyanToggle); });

        // Initialize audio states based on toggles (without playing god audio)
        ToggleAudio(spawnAudio, spawnToggle);
        ToggleAudio(thump1Audio, thump1Toggle);
        ToggleAudio(thump2Audio, thump2Toggle);
        ToggleGodAudio(godToggle);  // Initialize god cube sounds together
        ToggleAudio(backgroundMusic, backgroundToggle);
        ToggleCyanAudio(cyanToggle);
    }

    private void ResetToggles()
    {
        // Uncheck all toggles when the options menu is opened
        spawnToggle.isOn = false;
        thump1Toggle.isOn = false;
        thump2Toggle.isOn = false;
        godToggle.isOn = false;
        backgroundToggle.isOn = false;
    }

    private void ToggleAudio(AudioSource audioSource, Toggle toggle)
    {
        if (audioSource != null)
        {
            if (toggle.isOn)  // If toggle is checked, mute the audio
            {
                audioSource.mute = true;
            }
            else  // If toggle is unchecked, unmute the audio
            {
                audioSource.mute = false;
            }
        }
    }

    private void ToggleCyanAudio(Toggle toggle)
    {
        if (toggle.isOn)
        {
            cyanBuildupAudio.mute = true;
            cyanMainAudio.mute = true;
        }
        else
        {
            cyanBuildupAudio.mute = false;
            cyanMainAudio.mute = false;
        }
    }
    private void ToggleGodAudio(Toggle toggle)
    {
        if (godBuildupAudio != null && godMainAudio != null)
        {
            if (toggle.isOn)  // If toggle is checked, mute both god cube sounds
            {
                godBuildupAudio.mute = true;
                godMainAudio.mute = true;
            }
            else  // If toggle is unchecked, unmute both sounds, but don't play them automatically
            {
                godBuildupAudio.mute = false;
                godMainAudio.mute = false;

                // We don't want to start playing the audio immediately when toggling off
                // The audio will only play when it's triggered by your other script

                // You can add a flag to check whether the value that triggers the audio has been reached,
                // and only allow the buildup and main audio to play in that case.
            }
        }
    }
}
