using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlanetSystem : MonoBehaviour {

    //public GameObject CenterOfMassSprite;
    public GameObject Camera;
    public GameObject TimeScaleSlider;

    public GameObject[] list; // list of all planet that interact with each other
    public float globalScale; // global scaling factor, globalScale (irl size) = 1 unit within simulation (displayed size) 
    public float timeScale; // calculations that one planet per update repeats, higher results in both faster and less accurate simulation
    public double timePassed; // amount of time passed in the simulation
    public double totalMass; // mass of the entire system
    //public float centerOfMassPositionX, centerOfMassPositionY; // position of the center of mass

    public float maximumVelocity; // velocity limit for automatic time scale control
    public bool  autoTimeScale; // activate  automatic time scale control for optimised accuracy and speed

    void Start() {
        Time.timeScale = 100f; // make simulation go brrrrrr
        
        //CenterOfMassSprite = gameObject.transform.GetChild(gameObject.transform.childCount-1).gameObject;

        for(int i = 0; i < list.Length; i++) { // calculate total mass
            totalMass = totalMass + list[i].GetComponent<Planet>().mass;
        }
    }

    void Update() {

        timeScale = TimeScaleSlider.GetComponent<Slider>().value;

        timePassed = timePassed + timeScale; // update time within simulation

        Debug.Log(list[1].GetComponent<Planet>().GetAngleChange());
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

}