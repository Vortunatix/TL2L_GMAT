using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AutomaticTimeScale : MonoBehaviour {

    public GameObject PlanetSystem;
    public GameObject SimulationSpeedSlider;

    public GameObject InputField;

    public bool enabled;
    void Start() {
        
    }

    void Update() {
        if(enabled) {
            if(SimulationSpeedSlider.GetComponent<SimulationSpeedController>().GetSpeed() == 0) {
                SimulationSpeedSlider.GetComponent<SimulationSpeedController>().SetSpeed(1);
            }
            SimulationSpeedSlider.GetComponent<SimulationSpeedController>().SetSpeed(SimulationSpeedSlider.GetComponent<SimulationSpeedController>().GetSpeed() * (float.Parse(InputField.GetComponent<TMP_InputField>().text) / PlanetSystem.GetComponent<PlanetSystem>().GetHighestAngleChange()));
        }
    }

    public void ToggleEnable() {
        enabled = !enabled;
    }
}
