using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.UIElements;
using System;
// using UnityEngine.UI;

public class ButtonInteractionController : MonoBehaviour
{
    // Start is called before the first frame update 
    public GameObject controlPointObj;
    public String coneName;
    public String cylinderName;
    public String ParentObjectName;
    public String EmptyObjectName;
    public Button playButton;
    public GameObject rotatingObj;
    public int handlePointScale;
    private GameObject parentObj;
    private GameObject emptyObj;

    private BezierPathManager pathManager;


	void Start ()
    {
        Debug.Log("Here is your button");
        playButton = GameObject.FindGameObjectWithTag("PlayButton").GetComponent<Button>();
        pathManager = FindObjectOfType<BezierPathManager>();

        // Debug.Log(playButton);
        // Debug.Log(playButton.text);
        // Button btn = playButton;
        

		// playButton.clicked += TaskOnClick;
        
        // playButton.onClicked += TaskOnClick;
	}

	public void TaskOnClick()
    {
		Debug.Log ("You have clicked the button!");
        GameObject cone = GameObject.Find(coneName);
        GameObject cylinder = GameObject.Find(cylinderName);

        Vector3 centroid = getCentroid(cone, cylinder);
        
        this.parentObj = Instantiate(rotatingObj, centroid, Quaternion.identity);
        this.parentObj.name = ParentObjectName;
        Debug.Log("Parent obj from ButtonInteraction");
        Debug.Log(this.parentObj);
        Debug.Log(this.parentObj.name);

        this.emptyObj = Instantiate(new GameObject(), centroid, Quaternion.identity);
        this.emptyObj.name = EmptyObjectName;

        cone.transform.SetParent(parentObj.transform);
        cylinder.transform.SetParent(parentObj.transform);

        // parentObj.transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
        // parentObj.st

        Animator coneAnimator = cone.GetComponent<Animator>();
        coneAnimator.SetTrigger("StartBouncy");
        Animator cylinderAnimator = cylinder.GetComponent<Animator>();
        cylinderAnimator.SetTrigger("StartFloat");


        this.parentObj.transform.SetParent(this.emptyObj.transform);

        var endPlacement = Camera.current.transform.position;

        var normalRelUp = getNormalRelativeToUp(centroid, endPlacement);
        var centToEndUnit = (endPlacement - centroid).normalized + centroid;
        var centroidControl = handlePointScale * (centToEndUnit + normalRelUp);

        var endToCentUnit = endPlacement + (centroid - endPlacement).normalized;
        var endPlacementControl = handlePointScale * (endToCentUnit + normalRelUp);
        // endPlacementControl += (1 * Vector3.back);

        Instantiate(controlPointObj, centroidControl, Quaternion.identity);
        Instantiate(controlPointObj, endPlacementControl, Quaternion.identity);

        pathManager.setPoints(new PointCluster(centroid,centroidControl), new PointCluster(endPlacement, endPlacementControl));
        pathManager.retrieveParentObj(EmptyObjectName);
        pathManager.startRunning();
	}

    private Vector3 getNormalRelativeToUp(Vector3 start, Vector3 end)
    {
        return Vector3.Cross(end - start, Vector3.up).normalized;
    }

    private Vector3 getCentroid(GameObject obj1, GameObject obj2)
    {
        var centroid = Vector3.zero;
        centroid += obj1.transform.position;
        centroid += obj2.transform.position;
        centroid /= 2;
        return centroid;
    }

    public void StaticButtonPressed()
    {
        Debug.Log("Printing from extra function");
        Debug.Log("Object Names: " + coneName + " " + cylinderName);
    }

    public GameObject getParentObj()
    {
        return this.parentObj;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
