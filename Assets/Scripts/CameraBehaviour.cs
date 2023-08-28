using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

    public Vector3 oldCameraPosition; // position of camera as of last update
    public Vector3 cameraVelocity; // speed of the camera movement
    public float dragSensitivity; // multiplier controlling the sensitivity when dragging, higher value means lower sensitivity
    public float dragSensitivity2; // multiplier controlling the sensitivity when both dragging and holding down left control, higher value means lower sensitivity
    public float postDragMovement; // multiplier for how much the camera moves when not dragging anymore, higher means less movement

    
    public float orthographicScale; // scale of the displayed simulation, higher value results in higher scale 
    private Vector3 oldMousePosition; // position of mouse as of last update
    public float mouseScrollDelta; // how much the mousewheel moved since last update
    public float scrollIntensity; // how sensitive the mousewheel reacts, higher value results in higher sensitivity

    void Start() {
        
    }

    void Update() {

        //dragging when left-clicking with the mouse
        if(Input.GetKey(KeyCode.Mouse0)) {
            oldCameraPosition = gameObject.transform.localPosition; // remember old camera position
            if(Input.GetKey(KeyCode.LeftControl)) { // decrease how much the camera moves when dragging and holding left control key
                MoveCamera((oldMousePosition.x - Input.mousePosition.x) * (orthographicScale / dragSensitivity2), (oldMousePosition.y - Input.mousePosition.y) * (orthographicScale / dragSensitivity2));
            } else {
                MoveCamera((oldMousePosition.x - Input.mousePosition.x) * (orthographicScale / dragSensitivity), (oldMousePosition.y - Input.mousePosition.y) * (orthographicScale / dragSensitivity));
            }
            cameraVelocity = new Vector3(gameObject.transform.localPosition.x - oldCameraPosition.x, gameObject.transform.localPosition.y - oldCameraPosition.y, 0); // calculate new value for camera velocity
        } else if(cameraVelocity.magnitude != 0f) { // if camera velocity is not 0 when letting go
            MoveCamera(cameraVelocity.x, cameraVelocity.y); // move camera according to vamera velocity
            cameraVelocity = cameraVelocity / postDragMovement; // decrease camera velocity
            if(cameraVelocity.magnitude < 0.0001f) { // set camera velocity to 0 if value is too small to show any effect
                cameraVelocity = new Vector3(0, 0, 0);
            }
        }
        oldMousePosition = Input.mousePosition;

        // zooming using the mouse wheel
        mouseScrollDelta = mouseScrollDelta + Input.mouseScrollDelta.y; // increase mouseScrollDelta when scrolling
        if(mouseScrollDelta != 0f) { 
            mouseScrollDelta = mouseScrollDelta / scrollIntensity; // decrease mouseScrollDelta slightly
            if(mouseScrollDelta < 0.00000000001f && mouseScrollDelta > -0.00000000001f) { // set mouseScrollDelta to 0 if the value is too close to 0
                mouseScrollDelta = 0;
            }
            orthographicScale = orthographicScale + (-mouseScrollDelta) * orthographicScale / 2; // increase / decrease size of scalingFactor
            if(orthographicScale != orthographicScale || orthographicScale >= 3.042823466e+38f) { // set globalScale to maximum if value is NaN
                orthographicScale = 3.042823466e+38f;
            }
            if(orthographicScale < 1) { // set global scale to minimum if value is below 100
                orthographicScale = 1;
            }
            gameObject.GetComponent<Camera>().orthographicSize = orthographicScale; // update orthographic scale
        }
        
    }

    public void MoveCamera(float deltaX, float deltaY) { // moves the camera by a given amount
        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + deltaX, gameObject.transform.localPosition.y + deltaY, -100);
    }

}
