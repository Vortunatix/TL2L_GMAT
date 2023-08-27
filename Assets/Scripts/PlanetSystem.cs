using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetSystem : MonoBehaviour {

    //public GameObject CenterOfMassSprite;
    public GameObject Camera;
    public GameObject TimeScaleSlider;

    public Planet[] list; // list of all planet that interact with each other
    public float globalScale; // global scaling factor, globalScale (irl size) = 1 unit within simulation (displayed size) 
    public float timeScale; // calculations that one planet per update repeats, higher results in both faster and less accurate simulation
    public double timePassed; // amount of time passed in the simulation
    public double totalMass; // mass of the entire system
    //public float centerOfMassPositionX, centerOfMassPositionY; // position of the center of mass


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
        globalScale = Camera.GetComponent<CameraBehaviour>().globalScale; // update global scaling factor

        /*centerOfMassPositionX = centerOfMassPositionY = 0;

        for(int i = 0; i < list.Length; i++) { // calculate the center of mass

            double scalingFactor = list[i].GetComponent<Planet>().mass / totalMass;
            centerOfMassPositionX = (float)(list[i].GetComponent<Planet>().GetPositionX() * scalingFactor);
            centerOfMassPositionY = (float)(list[i].GetComponent<Planet>().GetPositionY() * scalingFactor);
        
        }

        CenterOfMassSprite.transform.localPosition = new Vector3(centerOfMassPositionX/100000000, centerOfMassPositionY/100000000, 0);
        */
    }

    public void SetTimeStepScale(float scale) {
        timeScale = scale;
    }
}
