using UnityEngine;
using System.Collections;

public class PlayerSound : MonoBehaviour {

    [SerializeField]
    private AudioSource audioSource;

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
