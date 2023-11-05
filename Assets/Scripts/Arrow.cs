using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Arrow : MonoBehaviour {
    
    public LineRenderer line;
    public Transform tip;

    public Vector3 start, end;
    public Color color;

    public Camera Cam;

    private float currentRotation;
    private float distanceToTip;
    private float scale;
    private float oldScale;
    public float size;
    private Vector3 distance;
    private float zOffset;

    void Start() {

        zOffset = gameObject.transform.localPosition.z;
        currentRotation = 0f;
        distanceToTip = 0.5785f;
        SetColor(color);
        start = new Vector3(0, 0, zOffset);
        end = new Vector3(0, 0, zOffset);
        Cam = GameObject.Find("Main Camera").GetComponent<Camera>();

    }

    void Update() {

        //adjust scale
        scale = Cam.orthographicSize;
        if(oldScale != scale) {
            SetSize(Cam.orthographicSize * size); 
        }
        oldScale = scale;

    }

    public void SetStartPosition(Vector3 position) {
        Debug.Log("Start" + position);
        start = position;
        start.z = zOffset;
        line.SetPosition(0, position);
        distance = end - start;
        Debug.Log(distance);
        UpdateTip();
        UpdateRotation();
    }
    public void SetEndPosition(Vector3 position) {
        Debug.Log("End" + position);
        end = position;
        end.z = zOffset;
        distance = end - start;
        Debug.Log(distance);
        UpdateTip();
        UpdateRotation();
    }

    private void UpdateTip() { // calculate position of the tip
 
        Vector3 tipOffset = distance * ((distanceToTip * scale) / distance.magnitude);

        Debug.Log("update"+end+tipOffset);
        tip.localPosition = end - tipOffset;
        line.SetPosition(1, end - tipOffset);

    }

    public void SetSize(float newScale) {

        scale = newScale;
        
        UpdateTip();

        tip.localScale = new Vector3(scale, scale, 1);
        line.SetWidth(scale / 7, scale / 7);

    }

    private void UpdateRotation() { // calculate roation for tip sprite

        if(distance != Vector3.zero) {
        
            float target = (float)(Math.Atan(distance.y / distance.x) * 57.29577951) - 90f;
            if(distance.x < 0) {
                target = target - 180;
            }
            tip.Rotate(0, 0, target - currentRotation, Space.Self);
            currentRotation = target;
        
        }

    }

    public void SetColor(Color newColor) {
        color = newColor;
        line.endColor = color;
        line.startColor = color;
        tip.gameObject.GetComponent<SpriteRenderer>().color = color;
    }

}
