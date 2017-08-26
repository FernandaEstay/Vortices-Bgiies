using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionMapingController : MonoBehaviour {
    public Dropdown currentInterfaceDropdown;
    public ScrolldownContent scrollDown;
    //whenever you add a new interface, just add one slot in the array of the inspector
    public GameObject[] interfacesConfigurationArray;
    int lastInterfaceUsed = 0;
    /*
     * 0 = Emotiv
     * 1 = Kinect
     * 2 = NeuroSky
     * 
     */

    private void OnEnable()
    {
        UpdateCurrentObject();
    }

    public void UpdateCurrentObject()
    {
        interfacesConfigurationArray[lastInterfaceUsed].SetActive(false);
        interfacesConfigurationArray[currentInterfaceDropdown.value].SetActive(true);
        lastInterfaceUsed = currentInterfaceDropdown.value;
    }
}
