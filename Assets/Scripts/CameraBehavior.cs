using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameObjectBehavior : MonoBehaviour {

    private Vector3 oldMousePosition;

    void Start() {
        
    }

    void Update() {

        if(Input.GetKey(KeyCode.Mouse0)) {
            if(Input.GetKey(KeyCode.LeftControl)) {
                gameObject.transform.localPosition = new Vector3( gameObject.transform.localPosition.x + (oldMousePosition.x - Input.mousePosition.x) / 100,  gameObject.transform.localPosition.y + (oldMousePosition.y - Input.mousePosition.y) / 100, -100);
            } else {
                gameObject.transform.localPosition = new Vector3( gameObject.transform.localPosition.x + (oldMousePosition.x - Input.mousePosition.x) / 51,  gameObject.transform.localPosition.y + (oldMousePosition.y - Input.mousePosition.y) / 51, -100);
            }
        }
        oldMousePosition = Input.mousePosition;
        
    }
}
