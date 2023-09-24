using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class UIController : MonoBehaviour {

    public PlanetSystem planetSystem;
    public GameObject mainCamera; // main mainCamera
    public GameObject selected; // currently selected planet

    public TMP_Text textTimePassed;

    // speed control references
    public TMP_InputField inputFieldSpeed;
    public Slider sliderSpeed;

    public TMP_InputField inputFieldAccuracy;

    // infoscreen references
    public TMP_InputField inputFieldPositionX, inputFieldPositionY;
    public TMP_InputField inputFieldVelocityX, inputFieldVelocityY;
    public TMP_Text textMagnitudeVelocity;
    public TMP_InputField inputFieldAccelerationX, inputFieldAccelerationY;
    public TMP_Text textMagnitudeAcceleration;
    public TMP_InputField inputFieldForceX, inputFieldForceY;
    public TMP_Text textMagnitudeForce;



    void Start() {

        inputFieldAccuracy.text = "1";
        planetSystem.accuracy = 1;
        PauseSimulation();

    }

    void Update() {

        textTimePassed.text = GetTimePassedReadable(planetSystem.timePassed);

        if(selected != null) { // update values in infoscreen
            
            Planet buffer = selected.GetComponent<Planet>();

            if(planetSystem.timeScale != 0) {
                UpdatePlanetDataInputFields(buffer);
            }

            // update text fields
            textMagnitudeVelocity.text = ((float)buffer.velocity.magnitude).ToString() + " m/s";
            textMagnitudeAcceleration.text = ((float)buffer.acceleration.magnitude).ToString() + " m/s²";
            textMagnitudeForce.text = ((float)buffer.force.magnitude).ToString() + " N";

        }

    }

    public void UpdatePlanetDataInputFields(Planet planet) {
        inputFieldPositionX.text = ((float)planet.position.x).ToString(); inputFieldPositionY.text = ((float)planet.position.y).ToString();
        inputFieldVelocityX.text = ((float)planet.velocity.x).ToString(); inputFieldVelocityY.text = ((float)planet.velocity.y).ToString();
        inputFieldAccelerationX.text = ((float)planet.acceleration.x).ToString(); inputFieldAccelerationY.text = ((float)planet.acceleration.y).ToString();
        inputFieldForceX.text = ((float)planet.force.x).ToString(); inputFieldForceY.text = ((float)planet.force.y).ToString();
    }

    public void SetSelected(GameObject Planet) { // set Planet as the new selected planet
        
        selected = Planet;

        mainCamera.GetComponent<CameraBehaviour>().TrackObject(Planet);
        
        if(Planet == null) { // clear infoscreen from values
            
            inputFieldPositionX.text = ""; inputFieldPositionY.text = "";
            inputFieldVelocityX.text = ""; inputFieldVelocityY.text = "";
            inputFieldAccelerationX.text = ""; inputFieldAccelerationY.text = "";
            inputFieldForceX.text = ""; inputFieldForceY.text = "";

            textMagnitudeVelocity.text = "";
            textMagnitudeAcceleration.text = "";
            textMagnitudeForce.text = "";
        
        } else {
            UpdatePlanetDataInputFields(selected.GetComponent<Planet>());
        }
    }

    public string GetTimePassedReadable(double time) { // returns a string containing the time passed measured in years, days and hours
        double buffer = time;
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

    public void PauseSimulation() { // sets timeScale to 0, pausing the simu
        
        planetSystem.timeScale = 0;
        sliderSpeed.value = 0;
        inputFieldSpeed.text = "0";
    
    }

    public void SliderSpeedUpdate() { // called when sliderspeed updates
        
        planetSystem.timeScale = sliderSpeed.value;
        inputFieldSpeed.text = sliderSpeed.value.ToString();
    
    }
    public void InputFieldSpeedUpdate() { // called when inputfieldspeed updates
    
        planetSystem.timeScale = float.Parse(inputFieldSpeed.text);
        sliderSpeed.value = float.Parse(inputFieldSpeed.text);
    
    }
    public void InputFieldAccuracyUpdate() { // called when inputfieldaccuracy updates

        if(inputFieldAccuracy.text != "") { // if number is valid update value
            planetSystem.accuracy = float.Parse(inputFieldAccuracy.text);;
        }
    
    }
    public void InputFieldPositionXUpdate() {
        if(planetSystem.timeScale == 0 && selected != null) {
            selected.GetComponent<Planet>().position.x = double.Parse(inputFieldPositionX.text);
        }
    }
    public void InputFieldPositionYUpdate() {
        if(planetSystem.timeScale == 0 && selected != null) {
            selected.GetComponent<Planet>().position.y = double.Parse(inputFieldPositionY.text);
        }
    }
    public void InputFieldVelocityXUpdate() {
        if(planetSystem.timeScale == 0 && selected != null) {
            selected.GetComponent<Planet>().velocity.x = double.Parse(inputFieldVelocityX.text);
        }
    }
    public void InputFieldVelocityYUpdate() {
        if(planetSystem.timeScale == 0 && selected != null) {
            selected.GetComponent<Planet>().velocity.y = double.Parse(inputFieldVelocityY.text);
        }
    }
    public void InputFieldAccelerationXUpdate() {
        if(planetSystem.timeScale == 0 && selected != null) {
            selected.GetComponent<Planet>().acceleration.x = double.Parse(inputFieldAccelerationX.text);
        }
    }
    public void InputFieldAccelerationYUpdate() {
        if(planetSystem.timeScale == 0 && selected != null) {
            selected.GetComponent<Planet>().acceleration.y = double.Parse(inputFieldAccelerationY.text);
        }
    }
    public void InputFieldForceXUpdate() {
        if(planetSystem.timeScale == 0 && selected != null) {
            selected.GetComponent<Planet>().force.x = double.Parse(inputFieldForceX.text);
        }
    }
    public void InputFieldForceYUpdate() {
        if(planetSystem.timeScale == 0 && selected != null) {
            selected.GetComponent<Planet>().force.y = double.Parse(inputFieldForceY.text);
        }
    }

}
