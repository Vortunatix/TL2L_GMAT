using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class PlanetSystem : MonoBehaviour {

    //public GameObject CenterOfMassSprite;
    public GameObject Camera;
    public GameObject SliderTimeScale, InputFieldAccuracy;
    public GameObject SelectedPlanet;

    public GameObject[] list; // list of all planet that interact with each other
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

        timeScale = SliderTimeScale.GetComponent<Slider>().value;

        timePassed = timePassed + timeScale  * accuracy; // update time within simulation


        for(int t = 0; t < timeScale; t++) {

            for(int i = 0; i < list.Length; i++) { // calculate new positions for each planet
                list[i].GetComponent<Planet>().CalculateNextPosition(accuracy);
            }

            for(int i = 0; i < list.Length; i++) { // update positions of each planet
                list[i].GetComponent<Planet>().EnforceNextPosition();
            }

        }

        for(int i = 0; i < list.Length; i++) { // update positions of each planet on screen
                list[i].GetComponent<Planet>().UpdatePosition();
            }

    }

    public void SetTimeStepScale(float scale) {
        timeScale = scale;
    }

    public string GetTimePassedReadable() { // returns a string containing the time passed measured in years, days and hours
        double buffer = timePassed;
        int yearsPassed = 0, daysPassed = 0, hoursPassed = 0/*, minutesPassed = 0, secondsPassed = 0*/;
        while(buffer > 31557600) { // count years
            buffer = buffer - 31557600;
            yearsPassed++;
        }
        while(buffer > 86400) { // count days
            buffer = buffer - 86400;
            daysPassed++;
        }
        while(buffer > 3600) { // count hours
            buffer = buffer - 3600;
            hoursPassed++;
        }
        /*while(buffer > 60) { // count minutes
            buffer = buffer - 60;
            minutesPassed++;
        }
        while(buffer > 1) { // count seconds
            buffer--;
            secondsPassed++;
        }*/
        return hoursPassed.ToString() + " h, " + daysPassed.ToString() + " d, " + yearsPassed.ToString() + " y";
    }

    public void SetAutoTimeScale(bool state) {
        autoTimeScale = state;
    }

    public void SetMaximumVelocity(float limit) {
        maximumVelocity = limit;
    }

    public float GetHighestAngleChange() { // returns the highest angle change per update cycle in the system
        float buffer = 0;
        for(int i=0; i < list.Length; i++) {
            if(Math.Abs(list[i].GetComponent<Planet>().GetAngleChange()) > buffer) {
                buffer = Math.Abs(list[i].GetComponent<Planet>().GetAngleChange());
            }
        }
        return buffer;
    }

    public void SetSelectedPlanet(GameObject Selected) {
        if(SelectedPlanet != null) {
            SelectedPlanet.GetComponent<Planet>().SetSelected(false);
        }
        SelectedPlanet = Selected;
    }

    public void DeselectPlanet() {
        SelectedPlanet = null;
    }

    public void UpdateAccuracy() {
        accuracy = float.Parse(InputFieldAccuracy.GetComponent<TMP_InputField>().text);
    }

}