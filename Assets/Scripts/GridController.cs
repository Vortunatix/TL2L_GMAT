using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridController : MonoBehaviour {

    public GameObject MainCamera, PlanetSystem;

    public float leftEdge, rightEdge, topEdge, bottomEdge, cameraWidth, cameraHeight;

    public GameObject[] gridLines; // array where all lines are stored

    public GameObject gridLine; // prefab reference

    public float lineWidthModifier; // modifier for the line width of the grid lines
    public float textPositionModifier;
    public float textBorderDistanceY; // distance of the text from the bottom screen border
    public float textBorderDistanceX; // distance of the text from the left screen border

    private Camera Cam;
    private long globalScale;

    void Start() {

        gridLines = new GameObject[64]; // set array size to 64 , there will probably never be more than 60 children but 64 is a nice number
        Cam = MainCamera.GetComponent<Camera>();
        globalScale = PlanetSystem.GetComponent<PlanetSystem>().globalScale;

    }

    void Update () {

        leftEdge = MainCamera.transform.localPosition.x - (Cam.orthographicSize * 2);
        rightEdge = MainCamera.transform.localPosition.x + (Cam.orthographicSize * 2);
        topEdge = MainCamera.transform.localPosition.y + Cam.orthographicSize;
        bottomEdge = MainCamera.transform.localPosition.y - Cam.orthographicSize;
        cameraWidth = rightEdge - leftEdge;
        cameraHeight = topEdge - bottomEdge;

        // draw vertical Lines
        int pos = (int)Math.Floor(leftEdge);
        int i = 0;
        for(; pos <= Math.Ceiling(rightEdge); pos++) {
            if(pos % Math.Pow(10, DigitAmount(Cam.orthographicSize)) == 0) { // if current position is multiple of 10 draw new line
                if(gridLines[i] == null) { // instanciate new line if needed
                    gridLines[i] = Instantiate(gridLine, gameObject.transform);
                }
                gridLines[i].SetActive(true);
                LineRenderer current = gridLines[i].GetComponent<LineRenderer>();
                current.SetPosition(0, new Vector3(pos, topEdge + Cam.orthographicSize / 2, 10));
                current.SetPosition(1, new Vector3(pos, topEdge, 10));
                current.SetPosition(2, new Vector3(pos, bottomEdge, 10));
                current.SetPosition(3, new Vector3(pos, bottomEdge - Cam.orthographicSize / 2, 10));
                current.SetWidth((float)Math.Pow(10, DigitAmount(Cam.orthographicSize)) * lineWidthModifier, (float)Math.Pow(10, DigitAmount(Cam.orthographicSize)) * lineWidthModifier);
                gridLines[i].transform.localPosition = new Vector3((pos - MainCamera.transform.localPosition.x) / Cam.orthographicSize * textPositionModifier, (bottomEdge - MainCamera.transform.localPosition.y) / Cam.orthographicSize * textPositionModifier + textBorderDistanceY, 0);
                gridLines[i].transform.GetChild(0).gameObject.transform.GetChild(0).transform.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = (pos * (float)globalScale).ToString();
                i++;
            }
        }

        // draw horizontal lines
        pos = (int)Math.Floor(bottomEdge);
        for(; pos <= Math.Ceiling(topEdge); pos++) {
            if(pos % Math.Pow(10, DigitAmount(Cam.orthographicSize)) == 0) { // if current position is multiple of 10 draw new line
                if(gridLines[i] == null) { // instanciate new line if needed
                    gridLines[i] = Instantiate(gridLine, gameObject.transform);
                }
                gridLines[i].SetActive(true);
                LineRenderer current = gridLines[i].GetComponent<LineRenderer>();
                current.SetPosition(0, new Vector3(rightEdge + Cam.orthographicSize / 2, pos, 10));
                current.SetPosition(1, new Vector3(rightEdge, pos, 10));
                current.SetPosition(2, new Vector3(leftEdge, pos, 10));
                current.SetPosition(3, new Vector3(leftEdge - Cam.orthographicSize / 2, pos, 10));
                current.SetWidth((float)Math.Pow(10, DigitAmount(Cam.orthographicSize)) * lineWidthModifier, (float)Math.Pow(10, DigitAmount(Cam.orthographicSize)) * lineWidthModifier);
                gridLines[i].transform.localPosition = new Vector3((leftEdge - MainCamera.transform.localPosition.x) / Cam.orthographicSize * textPositionModifier + textBorderDistanceX, (pos - MainCamera.transform.localPosition.y) / Cam.orthographicSize * textPositionModifier, 0);
                gridLines[i].transform.GetChild(0).gameObject.transform.GetChild(0).transform.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = (pos * (float)globalScale).ToString();
                i++;
            }
        }

        for(; i < gridLines.Length; i++) { // disable all unused lines
            if(gridLines[i] != null) {
                gridLines[i].SetActive(false);
            }
        }
        
    }

    public int DigitAmount(double a) { // dont mind this monstrosity, just a simple and fast way to "calculate" the digits of a number 
        double n = Math.Abs(a);
        if(n < 10e+1) return 1;
        if(n < 10e+2) return 2;
        if(n < 10e+3) return 3;
        if(n < 10e+4) return 4;
        if(n < 10e+5) return 5;
        if(n < 10e+6) return 6;
        if(n < 10e+7) return 7;
        if(n < 10e+8) return 8;
        if(n < 10e+9) return 9;
        if(n < 10e+10) return 10;
        if(n < 10e+11) return 11;
        if(n < 10e+12) return 12;
        if(n < 10e+13) return 13;
        if(n < 10e+14) return 14;
        if(n < 10e+15) return 15;
        if(n < 10e+16) return 16;
        if(n < 10e+17) return 17;
        if(n < 10e+18) return 18;
        if(n < 10e+19) return 19;
        if(n < 10e+20) return 20;
        if(n < 10e+21) return 21;
        if(n < 10e+22) return 22;
        if(n < 10e+23) return 23;
        if(n < 10e+24) return 24;
        if(n < 10e+25) return 25;
        if(n < 10e+26) return 26;
        if(n < 10e+27) return 27;
        if(n < 10e+28) return 28;
        if(n < 10e+29) return 29;
        if(n < 10e+30) return 30;
        if(n < 10e+31) return 31;
        if(n < 10e+32) return 32;
        if(n < 10e+33) return 33;
        if(n < 10e+34) return 34;
        if(n < 10e+35) return 35;
        if(n < 10e+36) return 36;
        if(n < 10e+37) return 37;
        return 38;
    }

}
