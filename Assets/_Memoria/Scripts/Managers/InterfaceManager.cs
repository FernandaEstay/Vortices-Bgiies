﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic;

public class InterfaceManager : MonoBehaviour {
    string Scope;
    //These are not game objects but components. In the inspector you must drag-n-drop the game object that holds the scripts you want and to access
    public EEGManager eegManager;
    public EyetrackerManager eyeTrackerManager;

    //Every check should be as large as the amount of devices available. It does not matter how large it gets, this will only trigger on evaluation start so the loading time is fine
    public void CheckEEGInterfaces()
    {        
        Scope = ProfileManager.Instance.currentEvaluationScope;
        if (GLPlayerPrefs.GetBool(Scope, "UseEmotivInsight") || 
            GLPlayerPrefs.GetBool(Scope, "UseNeuroSkyMindwave"))
        {
            eegManager.gameObject.SetActive(true);
        }
    }


}