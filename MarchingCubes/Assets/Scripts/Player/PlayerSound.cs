using UnityEngine;
using System.Collections;

/// <summary>
/// Class for handling player sound effects (gun shot sounds)
/// </summary>
public class PlayerSound : MonoBehaviour {

    // Defining variables
    // Square bracket tags change how Unity displays attributes in the inspector
    [SerializeField]
    private AudioSource audioSource;

    /* ----------------
     * CUSTOM FUNCTIONS
     * ---------------- */

    // Plays an audio clip through the player's audioSource GameObject
    public void PlayPlayerSound (AudioClip clip) {
        if (audioSource.clip == null) {
            Debug.Log("Changed clip to " + clip.name);
            audioSource.clip = clip;
        } else {
            if (audioSource.clip.name != clip.name) {
                Debug.Log("Changed clip to " + clip.name);
                audioSource.clip = clip;
            }
        }

        audioSource.Play();
    }
}
