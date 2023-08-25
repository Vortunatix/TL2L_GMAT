using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterface : MonoBehaviour {

    public GameObject PlanetSystem;
    public GameObject Camera;

    public float globalScale;

    private Vector3 oldMousePosition;
    void Start() {
        
    }

    void Update() {
        
        // zooming using the mouse wheel
        globalScale = globalScale + (-Input.mouseScrollDelta.y) * globalScale / 2;
        if(globalScale != globalScale) { // set globalScale to maximum if value is NaN
            globalScale = 3.042823466e+38f;
        }
        if(globalScale < 1.175494351e-38f) { // set global scale to minimum if value is 0
            globalScale = 	1.175494351e-38f;
        }

        if(Input.GetKey(KeyCode.Mouse0)) {
            if(Input.GetKey(KeyCode.LeftControl)) {
                Camera.transform.localPosition = new Vector3( Camera.transform.localPosition.x + (oldMousePosition.x - Input.mousePosition.x) / 100,  Camera.transform.localPosition.y + (oldMousePosition.y - Input.mousePosition.y) / 100, -100);
            } else {
                Camera.transform.localPosition = new Vector3( Camera.transform.localPosition.x + (oldMousePosition.x - Input.mousePosition.x) / 51,  Camera.transform.localPosition.y + (oldMousePosition.y - Input.mousePosition.y) / 51, -100);
            }
        }
        oldMousePosition = Input.mousePosition;

    }
}
