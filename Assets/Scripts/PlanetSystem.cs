using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class PlanetSystem : MonoBehaviour {

    public GameObject Camera;
    
    public GameObject SelectedPlanet;

    public GameObject planetPrefab;

    public UIController uiController;

    public long globalScale; // global scaling factor, globalScale (irl size) = 1 unit within simulation (displayed size) 
    public float timeScale; // calculations that one planet per update repeats, higher results in both faster and less accurate simulation
    public double timePassed; // amount of time passed in the simulation
    public double totalMass; // mass of the entire system
    //public float centerOfMassPositionX, centerOfMassPositionY; // position of the center of mass

    public float maximumVelocity; // velocity limit for automatic time scale control
    public bool  autoTimeScale; // activate  automatic time scale control for optimised accuracy and speed

    public float accuracy; // size of the calculated steps in seconds

    void Start() {

        Time.timeScale = 100f; // make simulation go brrrrrr

    }

    void Update() {

        if(timeScale > 0) { // if time is not paused

            timePassed = timePassed + timeScale  * accuracy; // update time within simulation

            for(int t = 0; t < timeScale; t++) {

                for(int i = 0; i < gameObject.transform.childCount; i++) { // calculate new positions for each planet
                    gameObject.transform.GetChild(i).GetComponent<Planet>().CalculateNextPosition(1 / accuracy);
                }

                for(int i = 0; i < gameObject.transform.childCount; i++) { // update positions of each planet
                    gameObject.transform.GetChild(i).GetComponent<Planet>().EnforceNextPosition();
                }

            }

            for(int i = 0; i < gameObject.transform.childCount; i++) { // update positions of each planet on screen
                    gameObject.transform.GetChild(i).GetComponent<Planet>().UpdatePosition();
            }

        }

    }

    public void SetTimeStepScale(float scale) {
        timeScale = scale;
    }

    public void SetAutoTimeScale(bool state) {
        autoTimeScale = state;
    }

    public void SetMaximumVelocity(float limit) {
        maximumVelocity = limit;
    }

    public float GetHighestAngleChange() { // returns the highest angle change per update cycle in the system
        float buffer = 0;
        for(int i=0; i < gameObject.transform.childCount; i++) {
            if(Math.Abs(gameObject.transform.GetChild(i).GetComponent<Planet>().GetAngleChange()) > buffer) {
                buffer = Math.Abs(gameObject.transform.GetChild(i).GetComponent<Planet>().GetAngleChange());
            }
        }
        return buffer;
    }

    public void SetSelectedPlanet(GameObject Selected) {
        if(SelectedPlanet != null) {
            SelectedPlanet.GetComponent<Planet>().SetSelected(false); // deselect old selected planet
        }
        SelectedPlanet = Selected; // set new planet as selected
        uiController.SetSelected(Selected); // update selected planet in UIController
    }

    public void DeselectPlanet() {
        SelectedPlanet = null;
        uiController.SetSelected(null);
    }

    public void NewPlanet() {

        GameObject buffer; 

        buffer = Instantiate(planetPrefab, gameObject.transform); // instantiate new planet        

        if(SelectedPlanet == null) {

            // set all variables to 0
            buffer.GetComponent<Planet>().mass = 0f;
            buffer.GetComponent<Planet>().position = new Vector2d(0, 0);
            buffer.GetComponent<Planet>().velocity = new Vector2d(0, 0);
            buffer.GetComponent<Planet>().acceleration = new Vector2d(0, 0);
            buffer.GetComponent<Planet>().force = new Vector2d(0, 0);

        } else {

            // copy planet data from selected planet
            buffer.GetComponent<Planet>().mass = SelectedPlanet.GetComponent<Planet>().mass;
            buffer.GetComponent<Planet>().position = Vector2d.zero;
            buffer.GetComponent<Planet>().velocity = SelectedPlanet.GetComponent<Planet>().velocity;
            buffer.GetComponent<Planet>().acceleration = Vector2d.zero;
            buffer.GetComponent<Planet>().force = Vector2d.zero;
            buffer.GetComponent<Planet>().diameter = SelectedPlanet.GetComponent<Planet>().diameter;

            SelectedPlanet.GetComponent<Planet>().ToggleSelectedState();

        }

    }

    public void DeletePlanet(GameObject ded) { // remove selected planet from both the simulation and existance

        if(SelectedPlanet == ded) { // deselect if it was selected
            ded.GetComponent<Planet>().ToggleSelectedState();
        }

        Destroy(ded); // destroy planet to remove

    }

    public Vector2d GetCenterOfMass() { // returns the position of the center of mass

        double totalMass = GetTotalMass();
        Vector2d center = Vector2d.zero;
        Planet buffer;

        for(int i = 0; i < gameObject.transform.childCount; i++) {
            buffer = gameObject.transform.GetChild(i).GetComponent<Planet>();
            center.x = center.x + buffer.position.x * (buffer.mass / totalMass); 
            center.y = center.y + buffer.position.y * (buffer.mass / totalMass); 
        }

        return center;

    }

    public double GetTotalMass() { // returns the amount of the total mass present in the simulated system

        double mass = 0;

        for(int i = 0; i < gameObject.transform.childCount; i++) {
            mass = mass + gameObject.transform.GetChild(i).GetComponent<Planet>().mass;
        }

        return mass;

    }

}