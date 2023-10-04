using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridController : MonoBehaviour {

    public GameObject MainCamera, PlanetSystem;

    public float leftEdge, rightEdge, topEdge, bottomEdge;

    public GameObject[] gridLines; // array where all lines are stored

    public GameObject gridLine; // prefab reference

    public float lineWidthModifier; // modifier for the line width of the grid lines
    public float textPositionModifier;
    public float bottomTextOffsetX, bottomTextOffsetY; // offset of the text from the bottom screen border
    private float newBottomTextOffsetX;
    public float leftTextOffsetX, leftTextOffsetY; // offset of the text from the left screen border
    private float newLeftTextOffsetX;

    private Camera Cam;
    private long globalScale;

    public float screenSize;

    public float screenRatio;

    void Start() {

        gridLines = new GameObject[64]; // set array size to 64 , there will probably never be more than 60 children but 64 is a nice number
        Cam = MainCamera.GetComponent<Camera>();
        globalScale = PlanetSystem.GetComponent<PlanetSystem>().globalScale;

        screenRatio = (float)Cam.pixelWidth / (float)Cam.pixelHeight;
        
        // adjust offset value for current screen resolution
        newBottomTextOffsetX = bottomTextOffsetX * (screenRatio / 1.777f); 
        newLeftTextOffsetX = leftTextOffsetX * (screenRatio / 1.777f);



    }

    void Update () {

        leftEdge = Cam.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        rightEdge = Cam.ScreenToWorldPoint(new Vector3(Cam.pixelWidth, 0, 0)).x;
        topEdge = Cam.ScreenToWorldPoint(new Vector3(0, Cam.pixelHeight, 0)).y;
        bottomEdge = Cam.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;

        int stepSize = DigitAmount(Cam.orthographicSize) - 2;
        if(stepSize < 1) {stepSize = 1;}

        // draw vertical Lines
        int pos = (int)Math.Floor(leftEdge / stepSize) * stepSize;
        int i = 0;
        for(; pos <= Math.Ceiling(rightEdge); pos = pos + stepSize) {
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
                gridLines[i].transform.localPosition = Cam.WorldToScreenPoint(new Vector3(pos - Cam.orthographicSize * newBottomTextOffsetX, bottomEdge - Cam.orthographicSize * bottomTextOffsetY, 0));
                gridLines[i].transform.GetChild(0).gameObject.transform.GetChild(0).transform.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = (pos * (float)globalScale).ToString();
                i++;
            }
        }

        // draw horizontal lines
        pos = (int)Math.Floor(bottomEdge / stepSize) * stepSize;
        for(; pos <= Math.Ceiling(topEdge); pos = pos + stepSize) {
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
                gridLines[i].transform.localPosition = Cam.WorldToScreenPoint(new Vector3(leftEdge - Cam.orthographicSize * newLeftTextOffsetX, pos - Cam.orthographicSize * leftTextOffsetY, 0));
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
