using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class for passing user-inputted map information from the GameSetup scene to the MainGame scene (Unity scene system)
/// </summary>

public class MapInfo : MonoBehaviour {

    // Defining variables
    public string seed;

    /* ------------------
     * BUILT-IN FUNCTIONS
     * ------------------ */

    // Use this for initialization
    void Start () {

        /* When a new scene is loaded in Unity, game objects from the old scene are destroyed by default
         * This line ensures that the game object this script is attached to doesn't get destroyed
         * when switching from the GameSetup scene to the MainGame scene
         */
        Object.DontDestroyOnLoad(this.gameObject);
    }

}
