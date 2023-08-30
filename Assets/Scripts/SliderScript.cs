using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderScript : MonoBehaviour {

    public GameObject InputField;

    public void UpdateSlider() { // called when text field value is changed
        gameObject.GetComponent<Slider>().value = float.Parse(InputField.GetComponent<TMP_InputField>().text);
    }

    public void UpdateTextField() { // called when slider is changed
        InputField.GetComponent<TMP_InputField>().text = gameObject.GetComponent<Slider>().value.ToString();
    }
}
