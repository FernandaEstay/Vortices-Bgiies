using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic;
using Memoria.Core;

public class InterfaceManager : MonoBehaviour {
    public static InterfaceManager Instance { set; get; }

    string Scope;
    //These are not game objects but components. In the inspector you must drag-n-drop the game object that holds the scripts you want and to access
    public EEGManager eegManager;
    public EyetrackerManager eyeTrackerManager;
    public LeapMotionManager leapMotionManager;
    public MouseManager mouseManager;

    private void Awake()
    {
        Instance = this;
    }

    //Every check should be as large as the amount of devices available. It does not matter how large it gets, this will only trigger on evaluation start so the loading time is fine
    public void CheckEEGInterfaces()
    {        
        Scope = ProfileManager.Instance.currentEvaluationScope;
        if (GLPlayerPrefs.GetBool(Scope, "useEmotivInsight") || 
            GLPlayerPrefs.GetBool(Scope, "useNeuroSkyMindwave"))
        {
            eegManager.gameObject.SetActive(true);
        }
    }

    public void OnNewScene()
    {
        Scope = ProfileManager.Instance.currentEvaluationScope;

        if (GLPlayerPrefs.GetBool(Scope, "useMouse"))
        {
            mouseManager.gameObject.SetActive(true);
        }

        if (GLPlayerPrefs.GetBool(Scope, "useLeapMotion"))
        {
            leapMotionManager.gameObject.SetActive(true);
        }
    }

    public void OnConfigScene()
    {
        mouseManager.gameObject.SetActive(false);
        leapMotionManager.gameObject.SetActive(false);
    }
}
