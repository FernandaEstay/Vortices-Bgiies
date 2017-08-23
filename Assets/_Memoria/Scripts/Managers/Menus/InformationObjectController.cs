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
    //whenever you add a new object, just add one slot in the array of the inspector
    public GameObject[] objectPlanesArray;
    int lastObjectUsed = 0;

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
}
