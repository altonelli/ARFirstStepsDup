using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BezierPathManager : MonoBehaviour
{
    public float TimeToRun;
    private GameObject obj;
    private PointCluster startPoint;
    private PointCluster endPoint;
    private Vector3 prevPosition;
    // private Vector3 wayPoint;
    private delegate Vector3 GetPointAtTimeFunction(float time);
    private GetPointAtTimeFunction getPointAtTime;
    private bool isRunning;
    private float startTime;
    private float currTime;
    private ButtonInteractionController playButtonController;

    // Start is called before the first frame update
    void Start()
    {
        isRunning = false;
        playButtonController = FindObjectOfType<ButtonInteractionController>();
        createPathFunction();
    }

    public void setPoints(PointCluster start, PointCluster end)
    {
        startPoint = start;
        endPoint = end;
    }

    public void retrieveParentObj(String name)
    {
        obj = GameObject.Find(name);
        Debug.Log("Printing from Path Manager");
        Debug.Log(obj);
        prevPosition = startPoint.wayPoint;
    }

    void createPathFunction()
    {
        // F(t) = (1−t)^3P1 + 3(1−t)^2tP2 +3(1−t)t^2P3 + t^3P4
        getPointAtTime = delegate(float time)
        {
            Debug.Log(time);
            var oneMinus = 1.0f - time;
            return Mathf.Pow(oneMinus, 3.0f) * startPoint.wayPoint + 
                    Mathf.Pow(oneMinus, 2.0f) * time * startPoint.controlPoint + 
                    oneMinus * Mathf.Pow(time, 2.0f) * endPoint.controlPoint + 
                    Mathf.Pow(time, 2.0f) * time * endPoint.wayPoint;

        };
    }

    public void startRunning()
    {
        startTime = Time.time;
        Debug.Log(startTime);
        isRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        currTime = Time.time;
        // Debug.Log(isRunning);
        if (isRunning && currTime - startTime > 1.0f)
        {
            Debug.Log("Getting Time");
            currTime = Time.time;
            var timeAsPercent = (currTime - startTime - 1.0f) / TimeToRun;
            Debug.Log("Getting position");
            var currLocation = getPointAtTime(timeAsPercent);
            Debug.Log("Setting Position");

            var step = currLocation - prevPosition;
            obj.transform.position += step;

            prevPosition = currLocation;
            if (timeAsPercent >= 1.0f)
            {
                isRunning = false;
            }
        }
    }
}

public class PointCluster
{
    public Vector3 wayPoint;
    public Vector3 controlPoint;

    public PointCluster(Vector3 wayPoint, Vector3 controlPoint)
    {
        this.wayPoint = wayPoint;
        this.controlPoint = controlPoint;
    }
}
