using Gamelogic;
using Memoria;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneImageController : MonoBehaviour {
    public InformationObjectController objectController;
    string objectName = "PlaneImage";    

    private void OnEnable()
    {
        //this is because this script will be instanced and the public variables will not be asigned, only the logic that assigns actions to the ActionManager by reading the PlayerPrefs
        if(objectController != null)
        {
            SelectThisObject();
            objectController.scrollDown.LaunchScrollDown("Plane Image description", "Long text about the image");
        }        
    }

    public void SelectThisObject()
    {
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        GLPlayerPrefs.SetString(Scope, "CurrentInformationObject", objectName);
        objectController.UpdateCurrentSelectedObjectText();
    }

    

}
