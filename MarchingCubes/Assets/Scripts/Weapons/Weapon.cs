using UnityEngine;
using System.Collections;

[System.Serializable]
public class Weapon {

    public string name = "Machine Gun";
    public float damage, range, fireRate = 5f;
    public int bulletsPerShot = 1;
    public AudioClip gunSound;
    
}
