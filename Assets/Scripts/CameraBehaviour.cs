using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

    private Vector3 oldMousePosition;

    public float globalScale;
    public float mouseScrollDelta;
    public float scrollIntensity;

    void Start() {
        
    }

    void Update() {

        //dragging when left-clicking with the mouse
        if(Input.GetKey(KeyCode.Mouse0)) {
            if(Input.GetKey(KeyCode.LeftControl)) { // decrease how much the camera moves when dragging and holding left control key
                gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + (oldMousePosition.x - Input.mousePosition.x) / 100,  gameObject.transform.localPosition.y + (oldMousePosition.y - Input.mousePosition.y) / 100, -100);
            } else {
                gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + (oldMousePosition.x - Input.mousePosition.x) / 51,  gameObject.transform.localPosition.y + (oldMousePosition.y - Input.mousePosition.y) / 51, -100);
            }
        }
        oldMousePosition = Input.mousePosition;

        // zooming using the mouse wheel
        mouseScrollDelta = mouseScrollDelta + Input.mouseScrollDelta.y;
        if(mouseScrollDelta != 0f) {
            mouseScrollDelta = mouseScrollDelta / scrollIntensity; // decrease mouseScrollDelta slightly
            if(mouseScrollDelta < 0.00000000001f && mouseScrollDelta > -0.00000000001f) { // set mouseScrollDelta to 0 if the value is too close to 0
                mouseScrollDelta = 0;
            }
            float scalingFactor = globalScale / (globalScale + (-mouseScrollDelta) * globalScale / 2); // calculate scaling factor to adjust camera postion
            globalScale = globalScale + (-mouseScrollDelta) * globalScale / 2; // increase / decrease size of scalingFactor
            if(globalScale != globalScale || globalScale >= 3.042823466e+38f) { // set globalScale to maximum if value is NaN
                globalScale = 3.042823466e+38f;
                goto dontAdjustPosition; // skip position adjust when the new scale is invalid
            }
            if(globalScale < 100) { // set global scale to minimum if value is below 100
                globalScale = 	100;
                goto dontAdjustPosition; // skip position adjust when the new scale is invalid
            }
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x * scalingFactor, gameObject.transform.localPosition.y * scalingFactor, -100); // adjust camera position to zoom into the middle of the visible area rather towards absolute 0
            dontAdjustPosition:;
        }
        
    }

}
