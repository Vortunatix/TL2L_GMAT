using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Planet : MonoBehaviour {
    
    private GameObject PlanetSystem, Sprite, Shadow, NameTag, Camera;
    private float infoTextSize = 0.125f; // scale of name tag

    public Vector2 initialPosition; 
    public Vector2 initialVelocity;
    public float diameter;
    public float mass;
    public float graviationalAcceleration;

    private float timeScale; // number of operations per update, higher means faster yet less accurate simulation
    private float timeMultiplier; // multiplier for calculating sub one-second steps of the simulation when timescale is below 1
    private float globalScale; // global scaling factor for calculating the coordinates determining where to draw the sprites
    private float cameraScale; // current orthographic scale of the camera
    public Vector2 position;
    public Vector2 velocity;
    public Vector2 acceleration;
    public double forceX, forceY;

    public Vector2[] positionHistory; // stores positions the planet has visited in the past
    public float historyCompletness; // defines on a scale from 0 to 1 how many of the calculated points are stored, with 0 meaning none and 1 meaning all points will be stored

    void Start() {

        //initialize all variables
        PlanetSystem = gameObject.transform.parent.gameObject;
        Sprite = gameObject.transform.GetChild(1).gameObject;
        NameTag = gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject;
        position = initialPosition;
        velocity = initialVelocity;
        globalScale = PlanetSystem.GetComponent<PlanetSystem>().globalScale;
        Camera = PlanetSystem.GetComponent<PlanetSystem>().Camera;
        Shadow = gameObject.transform.GetChild(0).gameObject;

        // Setup
        NameTag.GetComponent<TMPro.TextMeshProUGUI>().text = gameObject.name; // set name tag
        Sprite.transform.localScale = new Vector3(diameter/globalScale, diameter/globalScale, 1); // set planet sprite scale 
        UpdatePosition();
    }

    void Update() {

        timeScale = PlanetSystem.GetComponent<PlanetSystem>().timeScale; // update timeScale

        cameraScale = Camera.GetComponent<Camera>().orthographicSize;
        NameTag.transform.localPosition = new Vector3(0, -20-diameter/globalScale*30*(cameraScale/5), 0); // move nametag outside the planet to make it readable
        NameTag.transform.localScale = new Vector3(cameraScale * infoTextSize, cameraScale * infoTextSize, cameraScale * infoTextSize); // update nametag size
        Shadow.transform.localScale = new Vector3(cameraScale / 5, cameraScale / 5, 1);

        if(timeScale < 1 && timeScale > 0) { // when doing sub-second steps
            timeMultiplier = timeScale; // set time multiplier to decrease the size of the calculated steps
        } else {
            timeMultiplier = 1; // set time multiplier to do nothing
        }

        for(int t = 0; t < timeScale; t++) { // repeat calculation until timeScale is hit

            forceX = forceY = 0; // reset forces

            for(int i = 0; i < PlanetSystem.GetComponent<PlanetSystem>().list.Length; i++) { // calculate for every planet in list

                Planet buffer = PlanetSystem.GetComponent<PlanetSystem>().list[i].GetComponent<Planet>(); // create buffer variable

                Vector3 deltaPosition = new Vector3(buffer.position.x - position.x, buffer.position.y - position.y); // calculate distance of the bodies

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
        gameObject.transform.localPosition = new Vector3(position.x/globalScale, position.y/globalScale, 0);
    }

}
