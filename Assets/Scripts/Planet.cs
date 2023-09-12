using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Planet : MonoBehaviour {
    
    private GameObject PlanetSystem, Sprite, Shadow, Outline, NameTag, SelectorButton, Path, Camera;
    private float infoTextSize = 0.125f; // scale of name tag

    public float diameter;
    public float mass;
    public float graviationalAcceleration;

    private float timeScale; // number of operations per update, higher means faster yet less accurate simulation
    private float timeMultiplier; // multiplier for calculating sub one-second steps of the simulation when timescale is below 1
    private float globalScale; // global scaling factor for calculating the coordinates determining where to draw the sprites
    private float cameraScale; // current orthographic scale of the camera
    public Vector2 initialPosition;
    public Vector2d position;
    public Vector2 initialVelocity;
    public Vector2d velocity;
    public Vector2d acceleration;
    public double forceX, forceY;

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
        Path.GetComponent<PathController>().Reset();
    }

    void Update() {

        timeScale = PlanetSystem.GetComponent<PlanetSystem>().timeScale; // update timeScale

        lastVelocityVector = velocity; // update the last velocity vector

        cameraScale = Camera.GetComponent<Camera>().orthographicSize;
        NameTag.transform.localPosition = new Vector3(0, -20-diameter/globalScale*30*(cameraScale/5), 0); // move nametag outside the planet to make it readable
        NameTag.transform.localScale = new Vector3(cameraScale * infoTextSize, cameraScale * infoTextSize, cameraScale * infoTextSize); // update nametag size
        Shadow.transform.localScale = new Vector3(cameraScale / 5, cameraScale / 5, 1); // update shadow size
        Outline.transform.localScale = new Vector3(diameter / globalScale + cameraScale * 0.025f, diameter / globalScale + cameraScale * 0.025f, 1); // set outline sprite scale
        if(cameraScale / 5 > diameter / globalScale) {
            SelectorButton.transform.localScale = new Vector3(cameraScale / 5, cameraScale / 5, 1);
        } else {
            SelectorButton.transform.localScale = new Vector3(diameter / globalScale, diameter / globalScale, 1);
        }

        if(timeScale < 1 && timeScale > 0) { // when doing sub-second steps
            timeMultiplier = timeScale; // set time multiplier to decrease the size of the calculated steps
        } else {
            timeMultiplier = 1; // set time multiplier to do nothing
        }

        for(int t = 0; t < timeScale; t++) { // repeat calculation until timeScale is hit

            forceX = forceY = 0; // reset forces

            for(int i = 0; i < PlanetSystem.GetComponent<PlanetSystem>().list.Length; i++) { // calculate for every planet in list

                Planet buffer = PlanetSystem.GetComponent<PlanetSystem>().list[i].GetComponent<Planet>(); // create buffer variable

                Vector2d deltaPosition = new Vector2d(buffer.position.x - position.x, buffer.position.y - position.y); // calculate distance of the bodies

                if(deltaPosition.magnitude == 0) {goto skipFurtherCalculations;} // skip if the planets are in the same place (or self)

                double totalForce = (buffer.mass * mass) / Math.Pow(deltaPosition.magnitude, 2) * 6.6743 * Math.Pow(10, -11); // calculate force resulting from currently computed relation

                forceX = forceX + totalForce * (deltaPosition.x / deltaPosition.magnitude); // calculate x component of the force vector
                forceY = forceY + totalForce * (deltaPosition.y / deltaPosition.magnitude); // calculate y component of the force vector

                skipFurtherCalculations:;

            }

            acceleration.x = (float)(forceX / mass); // update acceleration affecting the planet along the x axis
            acceleration.y = (float)(forceY / mass); // update acceleration affecting the planet along the y axis

            velocity = velocity + acceleration * timeMultiplier; // update the velocity of the planet according to formula v = a * t

            position = position + velocity * timeMultiplier; // update the position of the planet according to formula s = v * t

            UpdatePosition(); // enforce update
            
        }

    }

    public void UpdatePosition() {
        gameObject.transform.localPosition = new Vector3((float)(position.x/globalScale), (float)(position.y/globalScale), 0f);
    }

    public float GetAngleChange() { // returns the the change of the angle of the velocity vector since the last time update() was executed
        return (float)Vector2d.Angle(lastVelocityVector, velocity);
    }

    public void SetSelected(bool state) {
        selected = state;
        Outline.SetActive(state);
        if(state == true) {
            PlanetSystem.GetComponent<PlanetSystem>().SetSelectedPlanet(gameObject);
            Camera.GetComponent<CameraBehaviour>().TrackObject(gameObject);
        } else {
            PlanetSystem.GetComponent<PlanetSystem>().DeselectPlanet();
            Camera.GetComponent<CameraBehaviour>().TrackObject(null);
        }
    }

    public void ToggleSelectedState() {
        if(selected) {
            selected = false;
            Outline.SetActive(false);
            PlanetSystem.GetComponent<PlanetSystem>().DeselectPlanet();
            Camera.GetComponent<CameraBehaviour>().TrackObject(null);
        } else {
            selected = true;
            Outline.SetActive(true);
            PlanetSystem.GetComponent<PlanetSystem>().SetSelectedPlanet(gameObject);
            Camera.GetComponent<CameraBehaviour>().TrackObject(gameObject);
        }
    }

}
