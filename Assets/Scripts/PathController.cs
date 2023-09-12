using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour {

    public TrailRenderer path;
    private GameObject PlanetSystem;
    public float pathLength;

    void Start() {
        path = gameObject.GetComponent<TrailRenderer>();
        PlanetSystem = gameObject.transform.parent.gameObject.transform.parent.gameObject;
    }

    void Update() {
        path.time = pathLength / PlanetSystem.GetComponent<PlanetSystem>().timeScale;
    }

}
