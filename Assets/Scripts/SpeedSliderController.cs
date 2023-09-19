using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimulationSpeedController : MonoBehaviour {

    public GameObject InputSlider;
    public GameObject InputField;
    public GameObject PlanetSystem;

    public void SetMaxSpeed(float maximum) {
        InputSlider.GetComponent<Slider>().maxValue = maximum;
    }

    public void SetSpeed(float speed) {
        InputSlider.GetComponent<Slider>().value = speed;
        InputField.GetComponent<TMP_InputField>().text = InputSlider.GetComponent<Slider>().value.ToString();
        PlanetSystem.GetComponent<PlanetSystem>().timeScale = InputSlider.GetComponent<Slider>().value;
    }

    public float GetSpeed() {
        return PlanetSystem.GetComponent<PlanetSystem>().timeScale;
    }

    public void InputFieldUpdated() { // called when input field changes
        SetSpeed(float.Parse(InputField.GetComponent<TMP_InputField>().text));
    }

    public void SliderUpdated() { // called when slider changes
        SetSpeed(InputSlider.GetComponent<Slider>().value);
    }
}
