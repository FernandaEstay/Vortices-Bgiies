using Gamelogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionMapingController : MonoBehaviour {
    public Dropdown currentInterfaceDropdown;
    public ScrolldownContent scrollDown;
    public PopUpController popUp;
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
        scrollDown.LaunchScrollDown("clean", "");
        ActionManager.Instance.LoadMappingActionsNames();
        UpdateCurrentObject();
    }

    public void UpdateCurrentObject()
    {
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        if (!GLPlayerPrefs.GetBool(Scope, "use" + MOTIONSManager.Instance.interfacesWithInputNames[currentInterfaceDropdown.value]))
        {
            popUp.LaunchPopUpMessage("Interface not active", "Caution: The selected interface is not selected as being active for the evaluation");
        }
        interfacesConfigurationArray[lastInterfaceUsed].SetActive(false);
        interfacesConfigurationArray[currentInterfaceDropdown.value].SetActive(true);
        lastInterfaceUsed = currentInterfaceDropdown.value;
    }
}
