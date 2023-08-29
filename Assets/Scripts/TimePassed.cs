using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePassed : MonoBehaviour {

    public GameObject PlanetSystem;

    void Update() {
        gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = PlanetSystem.GetComponent<PlanetSystem>().GetTimePassedReadable();
    }
}
