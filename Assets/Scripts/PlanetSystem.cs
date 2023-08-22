using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSystem : MonoBehaviour {

    //public GameObject CenterOfMassSprite;

    public Planet[] list; // list of all planet that interact with each other
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

        timePassed = timePassed + timeScale;

        /*centerOfMassPositionX = centerOfMassPositionY = 0;

        for(int i = 0; i < list.Length; i++) { // calculate the center of mass

            double scalingFactor = list[i].GetComponent<Planet>().mass / totalMass;
            centerOfMassPositionX = (float)(list[i].GetComponent<Planet>().GetPositionX() * scalingFactor);
            centerOfMassPositionY = (float)(list[i].GetComponent<Planet>().GetPositionY() * scalingFactor);
        
        }

        CenterOfMassSprite.transform.localPosition = new Vector3(centerOfMassPositionX/100000000, centerOfMassPositionY/100000000, 0);
        */
    }
}
