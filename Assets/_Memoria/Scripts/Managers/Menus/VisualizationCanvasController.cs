using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualizationCanvasController : MonoBehaviour
{

    [Header("Visualization Canvas")]
    public Text currentVisualizationText;
    public Dropdown visualizationDropDown;
    public GameObject sphereVideo;
    public GameObject planeVideo;
    public Text actionsAvailables;
    public Text visualizationDescription;

    string currentVisualizationTextSphere = "Sphere";
    string currentVisualizationTextPlane = "Plane";

    string visualizationDescriptionPlane = "This visualization is designed to be displayed in a monitor and shows objects distribuited in a plane configuration. ";
    string visualizationDescriptionSphere = "This visualization is designed for immersive virtual reality environments and shows objects distribuited in a spherical configuration around the subject.";

    string actionsAvailablesSphere = "Select/Deselect image \n Change to next sphere \n Change to previous sphere";
    string actionsAvailablesPlane = "Select/Deselect image \n Change to next plane \n Change to previous plane \n Zoom in image \n Zoom out image";
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void VisualizationDropDownChange()
    {
        if (visualizationDropDown.value == 0)
        {
            currentVisualizationText.text = currentVisualizationTextSphere;
            visualizationDescription.text = visualizationDescriptionSphere;
            actionsAvailables.text = actionsAvailablesSphere;
            sphereVideo.SetActive(true);
            planeVideo.SetActive(false);
        }
        if(visualizationDropDown.value == 1)
        {
            currentVisualizationText.text = currentVisualizationTextPlane;
            visualizationDescription.text = visualizationDescriptionPlane;
            actionsAvailables.text = actionsAvailablesPlane;
            sphereVideo.SetActive(false);
            planeVideo.SetActive(true);
        }
    }
}
