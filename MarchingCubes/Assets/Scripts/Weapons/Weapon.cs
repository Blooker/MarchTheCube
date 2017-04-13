using UnityEngine;
using System.Collections;

/// <summary>
/// Class for defining a weapon and its various statistics (damage, fire rate, etc.)
/// </summary>

/* The square bracket tag below allows this class to be displayed in the Unity editor
 * with all of its attributes shown as sub-properties */
[System.Serializable]
public class Weapon {
    
    // Defining variables
    public string name = "Machine Gun";
    public float damage, range, fireRate = 5f;
    public int bulletsPerShot = 1;
    public AudioClip gunSound;
    
}
