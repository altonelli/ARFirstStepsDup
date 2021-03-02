using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.Experimental.XR;
using UnityEngine;
using System;

public class PlaceObjectController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject firstObjectToPlace;
    public GameObject secondObjectToPlace;
    public String firstObjName;
    public String secondObjName;

    private ARPlaceObjectInteraction placementDelegate;
    private bool firstObjectIsPlaced;
    private bool secondObjectIsPlaced;

    void Start()
    {
        placementDelegate = FindObjectOfType<ARPlaceObjectInteraction>();
        firstObjectIsPlaced = false;
        secondObjectIsPlaced = false;
        // firstObjName = "ConyInst";
        // secondObjName = "CylinderInst";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaceObject()
    {
        if (!firstObjectIsPlaced)
        {
            placementDelegate.PlaceObject(firstObjectToPlace, firstObjName);
            firstObjectIsPlaced = true;
        }
        else if (!secondObjectIsPlaced)
        {
            placementDelegate.PlaceObject(secondObjectToPlace, secondObjName);
            secondObjectIsPlaced = true;
        }
    }
}
