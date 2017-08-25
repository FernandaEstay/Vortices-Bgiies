using Gamelogic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationObjectController : MonoBehaviour {
    public Dropdown currentObjectDropdown;
    public Text currentSelectedObjectText;
    public ScrolldownContent scrollDown;
    public Text scrollDownCurrentDescription;
    //whenever you add a new object, just add one slot in the array of the inspector
    public GameObject[] objectPlanesArray;
    int lastObjectUsed = 0;

    string scrollDownDescriptionTextImagePlane = "A plane image uses a primitive unity object called QUAD, which looks like the plane but its edges are only one unit long and the surface is oriented in the XY plane of the local coordinate space.";
    string currentSelectObjectTextPlaneImage = "Image Plane";

    private void OnEnable()
    {
        UpdateCurrentSelectedObjectText();
    }

    public void UpdateCurrentSelectedObjectText()
    {
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        currentSelectedObjectText.text = GLPlayerPrefs.GetString(Scope, "CurrentInformationObject");
    }

    public void UpdateCurrentObject()
    {
        objectPlanesArray[lastObjectUsed].SetActive(false);
        objectPlanesArray[currentObjectDropdown.value].SetActive(true);
    }

    public void changeDropDownCurrentObject()
    {
        if(currentObjectDropdown.value == 0)
        {
            currentSelectedObjectText.text = currentSelectObjectTextPlaneImage;
            scrollDownCurrentDescription.text = scrollDownDescriptionTextImagePlane;
        }
    }    
}
