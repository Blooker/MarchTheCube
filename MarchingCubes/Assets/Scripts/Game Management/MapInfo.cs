using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapInfo : MonoBehaviour {
    public string seed;

    void Start () {
        Object.DontDestroyOnLoad(this.gameObject);
    }

    void Update () {
        if (SceneManager.GetActiveScene().name == "MainGame") {
            MapGenerator mapGenerator = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();

            mapGenerator.SetSeed(seed);
            mapGenerator.GenerateMap();
            Destroy(this.gameObject);
        }
    }
}
