using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic;

public class MOTIONSManager : MonoBehaviour {
    public static MOTIONSManager Instance { set; get; }
    [HideInInspector]
    public List<GameObject> activatedGameObjects;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        GLPlayerPrefs.SetBool("ProfileManagerScope3", "SimulationStarted", false);
    }
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartEvaluation()
    {
        GLPlayerPrefs.SetInt(ProfileManager.Instance.currentEvaluationScope, "LastUserIDUsed", GLPlayerPrefs.GetInt(ProfileManager.Instance.currentEvaluationScope, "CurrentUserID"));
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
     * 
     */
}
