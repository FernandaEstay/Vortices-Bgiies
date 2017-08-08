using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic;

public class InterfaceManager : MonoBehaviour {
    string Scope;
    public GameObject EEGManager, EyeTrackerManager, KinectManager; 

    //Every check should be as large as the amount of devices available. It does not matter how large it gets, this will only trigger on evaluation start so the loading time is fine
    public void CheckEEGInterfaces()
    {        
        Scope = ProfileManager.Instance.currentProfileScope;
        if (GLPlayerPrefs.GetBool(Scope, "UseEmotivInsight") || 
            GLPlayerPrefs.GetBool(Scope, "UseNeuroSkyMindwave"))
        {

        }
    }


}
