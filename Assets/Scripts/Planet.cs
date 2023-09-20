using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using System;

public class Planet : MonoBehaviour {
    
    private GameObject PlanetSystem, Sprite, Shadow, Outline, NameTag, SelectorButton, Path, Camera;
    private float infoTextSize = 0.125f; // scale of name tag

    public float diameter;
    public float mass;
    public float graviationalAcceleration;

    private float time; // multiplier for size of calcjlated steps
    private long globalScale; // global scaling factor for calculating the coordinates determining where to draw the sprites
    private float cameraScale; // current orthographic scale of the camera
    public Vector2 initialPosition;
    public Vector2d position;
    public Vector2 initialVelocity;
    public Vector2d velocity;
    public Vector2d acceleration;
    public Vector2d force;

    public Vector2d lastVelocityVector; // stores the last velocity vector

    public bool selected;

    void Start() {

        //initialize all variables
        PlanetSystem = gameObject.transform.parent.gameObject;
        Sprite = gameObject.transform.GetChild(2).gameObject;
        NameTag = gameObject.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject;
        SelectorButton = gameObject.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject;
        globalScale = PlanetSystem.GetComponent<PlanetSystem>().globalScale;
        Camera = PlanetSystem.GetComponent<PlanetSystem>().Camera;
        Shadow = gameObject.transform.GetChild(1).gameObject;
        Outline = gameObject.transform.GetChild(0).gameObject;
        Path = gameObject.transform.GetChild(4).gameObject;
        lastVelocityVector = velocity;
        position.Set(initialPosition.x, initialPosition.y);
        velocity.Set(initialVelocity.x, initialVelocity.y);

        // Setup
        NameTag.GetComponent<TMPro.TextMeshProUGUI>().text = gameObject.name; // set name tag
        Sprite.transform.localScale = new Vector3(diameter / globalScale, diameter / globalScale, 1); // set planet sprite scale 
        SetSelected(false);
        UpdatePosition();
    }

    void Update() {

        lastVelocityVector = velocity; // update the last velocity vector

        cameraScale = Camera.GetComponent<Camera>().orthographicSize;
        NameTag.transform.localScale = new Vector3(cameraScale * infoTextSize, cameraScale * infoTextSize, cameraScale * infoTextSize); // update nametag size
        Shadow.transform.localScale = new Vector3(cameraScale / 5, cameraScale / 5, 1); // update shadow size
        Outline.transform.localScale = new Vector3(diameter / globalScale + cameraScale * 0.025f, diameter / globalScale + cameraScale * 0.025f, 1); // set outline sprite scale
        
        if(cameraScale / 5 > diameter / globalScale) { // rescale selector button
            SelectorButton.transform.localScale = new Vector3(cameraScale / 5, cameraScale / 5, 1);
        } else {
            SelectorButton.transform.localScale = new Vector3(diameter / globalScale, diameter / globalScale, 1);
        }

        if(cameraScale / 5 > diameter / globalScale) { // move nametag outside the planet to make it readable
            NameTag.transform.localPosition = new Vector3(0, (-diameter * 1.1f) / globalScale - 6f * cameraScale, 0);
        } else {
            NameTag.transform.localPosition = new Vector3(0, (-diameter * 1.1f) / globalScale - 420, 0); 
        }

    }

    public void CalculateNextPosition(float timeMultiplier) { // calculates the velocity, doesnt actually calculate the new position

        lastVelocityVector = velocity; // update the last velocity vector
    
        time = timeMultiplier; // copy value from timeMultiplier for use in EnforceNextPosition()

        force = Vector2d.zero; // reset forces

            for(int i = 0; i < PlanetSystem.GetComponent<PlanetSystem>().list.Length; i++) { // calculate for every planet in list

                Planet buffer = PlanetSystem.GetComponent<PlanetSystem>().list[i].GetComponent<Planet>(); // create buffer variable

                Vector2d deltaPosition = new Vector2d(buffer.position.x - position.x, buffer.position.y - position.y); // calculate distance of the bodies

                if(deltaPosition.magnitude == 0) {goto skipFurtherCalculations;} // skip if the planets are in the same place (or self)

                double totalForce = (buffer.mass * mass) / Math.Pow(deltaPosition.magnitude, 2) * 6.6743 * Math.Pow(10, -11); // calculate force resulting from currently computed relation

                force.Set(force.x + totalForce * (deltaPosition.x / deltaPosition.magnitude), force.y + totalForce * (deltaPosition.y / deltaPosition.magnitude)); // calculate force vector

                skipFurtherCalculations:;

            }

            acceleration.x = (float)(force.x / mass); // update acceleration affecting the planet along the x axis
            acceleration.y = (float)(force.y / mass); // update acceleration affecting the planet along the y axis

            velocity = velocity + acceleration * time; // update the velocity of the planet according to formula v = a * t

    }

    public void EnforceNextPosition() { // calculates the new position

        position = position + velocity * time; // update the position of the planet according to formula s = v * t

    }


    public void UpdatePosition() { // move the gameObject to the correct position 

        gameObject.transform.localPosition = new Vector3((float)(position.x/globalScale), (float)(position.y/globalScale), 0f);
    
    }

    public float GetAngleChange() { // returns the the change of the angle of the velocity vector since the last time it was recalculated
        return (float)Vector2d.Angle(lastVelocityVector, velocity);
    }

    public void SetSelected(bool state) {
        selected = state;
        Outline.SetActive(state);
        if(state == true) {
            PlanetSystem.GetComponent<PlanetSystem>().SetSelectedPlanet(gameObject);
        } else {
            PlanetSystem.GetComponent<PlanetSystem>().DeselectPlanet();
        }
    }

    public void ToggleSelectedState() {
        if(selected) {
            selected = false;
            Outline.SetActive(false);
            PlanetSystem.GetComponent<PlanetSystem>().DeselectPlanet();
        } else {
            selected = true;
            Outline.SetActive(true);
            PlanetSystem.GetComponent<PlanetSystem>().SetSelectedPlanet(gameObject);
        }
    }

}
