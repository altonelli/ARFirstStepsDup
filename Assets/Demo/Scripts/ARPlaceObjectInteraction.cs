using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.Experimental.XR;
using System;

public class ARPlaceObjectInteraction : MonoBehaviour
{
    public GameObject placementIndicator;
    
    private ARSessionOrigin arOrigin;
    private ARRaycastManager arRaycastManager;
    private Pose placementPose;
    private bool placementPoseIsValid;
    
    void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        arRaycastManager = FindObjectOfType<ARRaycastManager>();

        Debug.Log(arOrigin);
        Debug.Log(arRaycastManager);
    }

    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
        // GUI.Label(Rect(0,0,100,100), "Taco");

        // if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        // {
        //     PlaceObject();
        // }
    }

    public void PlaceObject(GameObject obj, String name)
    {
        if (placementPoseIsValid)
        {
            GameObject gameObject = Instantiate(obj, placementPose.position, placementPose.rotation);
            gameObject.name = name;
        }
    }

    private void UpdatePlacementPose()
    {
        // Debug.Log("Camera");
        // Debug.Log(Camera.current);
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        arRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);
        
        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

}
