using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic;
using UnityEngine.SceneManagement;

public class MOTIONSManager : MonoBehaviour {
    public static MOTIONSManager Instance { set; get; }
    [HideInInspector]
    public List<GameObject> activatedGameObjects;

    [HideInInspector]
    public string[] interfacesWithInputNames;

    [HideInInspector]
    public string[] interfacesNames;


    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        GLPlayerPrefs.SetBool("ProfileManagerScope3", "SimulationStarted", false);
        interfacesWithInputNames = new string[]
        {
            "Emotiv",
            "Kinect",
            "NeuroSky"
        };

        interfacesNames = new string[]
        {
            "Emotiv",
            "EyeTribe",
            "NeuroSky",
            "Kinect",
            "OcculusRift",
            "OpenGlove",
            "LeapMotion",
            "Gamepad",
            "Mouse"
        };
    }


    public void StartEvaluation()
    {
        GLPlayerPrefs.SetInt(ProfileManager.Instance.currentEvaluationScope, "LastUserIDUsed", GLPlayerPrefs.GetInt(ProfileManager.Instance.currentEvaluationScope, "CurrentUserID"));
        SceneManager.LoadScene("EmotivTraining");
        //SceneManager.LoadScene("FullScene");
    }

    /*
     *                                      IMPORTANT:
     * To maintain data integrity, please always note here the names of the Visualizations, 
     * Objects and Interfaces you're adding, so they DON'T repeat themselves when using keys 
     * to store data in the player preferences (GLPlayerPrefs).
     * 
     ***Visualizations:
     *  -Plane
     *  -Sphere
     * 
     ***Objects:
     *  -PlaneImage
     * 
     ***Interfaces:
     *  -Emotiv
     *  -EyeTribe
     *  -NeuroSky
     *  -Kinect
     *  -OcculusRift
     *  -OpenGlove
     *  -LeapMotion
     *  -Gamepad
     *  -Mouse
     * 
     */

    /// <summary>
    /// Plane, Sphere
    /// </summary>
    /// <param name=""></param>
    /// <param name=""></param>
    public void VisualizationNames()
    {
        //this function does nothing, but the description shows the names of the current string key of the visualizations available
    }

    /// <summary>
    /// PlaneImage
    /// </summary>
    /// <param name=""></param>
    /// <param name=""></param>
    public void ObjectsNames()
    {
        //this function does nothing, but the description shows the names of the current string key of the objects available
    }

    /// <summary>
    /// Emotiv, EyeTribe, NeuroSky, Kinect, OcculusRift, OpenGlove, LeapMotion, Gamepad. If asking if an interfaces is being used, they keys are "useEmotiv", "useKinect" and so on.
    /// </summary>
    /// <param name=""></param>
    /// <param name=""></param>
    public void InterfacesNames()
    {
        //this function does nothing, but the description shows the names of the current string key of the interfaces available
    }

    /// <summary>
    /// ProfileManager, ActionManager, DIOManager, ConfigManager
    /// </summary>
    /// <param name=""></param>
    /// <param name=""></param>
    public void ManagersNames()
    {
        
    }

    /// <summary>
    /// CurrentInformationObject, CurrentVisualization, CurrentImmersion, CurrentUserID, OutputFolderPath 
    /// </summary>
    /// <param name=""></param>
    /// <param name=""></param>
    public void ConfigurationNames()
    {
        //this function does nothing, but the description shows the names of the current string key of the most common configurations available
    }
}
