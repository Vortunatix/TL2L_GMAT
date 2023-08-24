using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Planet : MonoBehaviour {
    
    private GameObject PlanetSystem, Sprite, NameTag;

    public Vector2 initialPosition; 
    public Vector2 initialVelocity;
    public float diameter;
    public float mass;
    public float graviationalAcceleration;

    private float timeScale; // number of operations per update, higher means faster yet less accurate simulation
    private float timeMultiplier; // multiplier for calculating sub one-second steps of the simulation when timescale is below 1
    private float globalScale; // global scaling factor, globalScale (irl size) = 1 unit within simulation (displayed size) 

    public Vector2 position;
    public Vector2 velocity;
    public Vector2 acceleration;
    public double forceX, forceY;

    void Start() {

        //initialize all variables
        PlanetSystem = gameObject.transform.parent.gameObject;
        Sprite = gameObject.transform.GetChild(1).gameObject;
        NameTag = gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject;
        position = initialPosition;
        velocity = initialVelocity;
        globalScale = PlanetSystem.GetComponent<PlanetSystem>().globalScale;

        // Setup
        NameTag.transform.localPosition = new Vector3(0, -20-diameter/globalScale*30, 0); // move nametag outside the planet to make it readable
        NameTag.GetComponent<TMPro.TextMeshProUGUI>().text = gameObject.name;

        UpdatePosition();
    }

    void Update() {

        timeScale = PlanetSystem.GetComponent<PlanetSystem>().timeScale; // update timeScale
        globalScale = PlanetSystem.GetComponent<PlanetSystem>().globalScale; // update global scale
        Sprite.transform.localScale = new Vector3(diameter/globalScale, diameter/globalScale, diameter/globalScale); // update planet sprite scale 

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


    /*private GameObject PlanetSystem;
    private GameObject Sprite;
    private GameObject NameTag;

    public Vector2 initialPosition; // in meters
    public Vector2 initialVelocity; // in m/s
    public float diameter; // in meters
    public float mass; // in kilograms
    public double graviationalAcceleration; // in m/s²

    private int timeScale; //number of calculations per update, higher leads to faster simulation at the cost of accuracy

    public Vector2 position; // absolute position of the planet in space
    public Vector2 velocity; // absolute velocity of the planet in space
    public Vector2 force; // gravitational force affecting the planet
    public Vector2 acceleration; // current acceleration of the planet

    void Start() {

        // initialize all variables
        PlanetSystem = gameObject.transform.parent.gameObject;
        NameTag = gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject;
        NameTag.GetComponent<TMPro.TextMeshProUGUI>().text = gameObject.name;
        Sprite = gameObject.transform.GetChild(1).gameObject;
        SetPosition(initialPosition);
        UpdatePosition();
        velocity = initialVelocity;

        Sprite.transform.localScale = new Vector3(diameter/100000000, diameter/100000000, diameter/100000000); // set planet sprite scale to size
        NameTag.transform.localPosition = new Vector3(0, -20-diameter/100000000*30, 0); // move nametag outside the planet to make it readable

    }


    void Update() {

        timeScale = PlanetSystem.GetComponent<PlanetSystem>().timeScale; // update the current timescale

        for(int t = 0; t < timeScale; t++) { // repeat calculations to speed up the simulation

            force = new Vector2(0, 0); // reset the force affecting the planet

            for(int i = 0; i < PlanetSystem.GetComponent<PlanetSystem>().list.Length; i++) { // for every listed planet
                Planet buffer = PlanetSystem.GetComponent<PlanetSystem>().list[i]; 

                Vector2 deltaposition = new Vector2(buffer.GetPosition().x - GetPosition().x, buffer.GetPosition().y - GetPosition().x);

                /*double deltaposition.x = buffer.Getposition.x() - Getposition.x(); // calculate position difference along X axis
                double deltaposition.y = buffer.Getposition.y() - Getposition.y(); // calculate position difference along Y axis
                double deltaPostionSqr = Math.Pow(deltaposition.x, 2) + Math.Pow(deltaposition.y, 2); // calculate total position difference, leave it squared for convenience
                
                if(deltaposition.magnitude == 0) {goto skipFurtherCalculations;} // skip further calculations if the bodies are at the same position

                double totalForce = (buffer.mass * mass) / Math.Pow(deltaposition.magnitude, 2) * 6.6743 * Math.Pow(10, -11); // calculate force affecting the planet in this moment

                double scalingFactor = totalForce / deltaposition.magnitude; // calculate the scaling factor to dissasemble totalForce vector into its components

                force.x = (float)(force.x + totalForce * scalingFactor); // force affecting the planet along the X axis
                force.y = (float)(force.y + totalForce * scalingFactor); // force affecting the planet along the Y axis

                skipFurtherCalculations:;
            }

            acceleration = force / mass; // calculate acceleration due to force

            velocity = velocity + acceleration; // calculate velocity from current velocity and acceleration along x axis


            Move(velocity.x, velocity.y); // update the position of the planet 
            UpdatePosition(); // enforce update

        }

        
    }

    /*public void Setposition.x(float newPosition) {
        position.x = newPosition;
    }
    public void Setposition.y(float newPosition) {
        position.y = newPosition;   
    }
    /*public void SetPosition(float newPosition.x, float newPosition.y) {
        position.x = newposition.x;
        position.y = newposition.y;
    }
    public void SetPosition(Vector2 newPosition) {
        position = newPosition;
    }

    public void Move(float amountX, float amountY) {
        position.x = position.x + amountX;
        position.y = position.y + amountY;
    }
    public void Move(Vector2 amount) {
        position = position + amount;
    }

    public void UpdatePosition() {
        gameObject.transform.localPosition = new Vector3(position.x/100000000, position.y/100000000, 0);
    }

    public Vector2 GetPosition() {
        return position;
    }*/
}
