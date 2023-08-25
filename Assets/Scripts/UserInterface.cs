using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterface : MonoBehaviour {

    public GameObject PlanetSystem;

    public float globalScale;

    void Start() {
        
    }

    void Update() {
        
        globalScale = globalScale + (-Input.mouseScrollDelta.y) * globalScale / 2;
        if(globalScale != globalScale) { // set globalScale to maximum if value is NaN
            globalScale = 3.042823466e+38f;
        }

        if(globalScale < 1.175494351e-38f) { // set global scale to minimum if value is 0
            globalScale = 	1.175494351e-38f;
        }

    }
}
