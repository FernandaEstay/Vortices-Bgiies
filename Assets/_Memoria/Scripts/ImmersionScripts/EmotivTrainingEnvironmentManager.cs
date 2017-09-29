using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Memoria;
using Gamelogic;

public class EmotivTrainingEnvironmentManager : MonoBehaviour {

    public GameObject sphereVisualizationSocket, planeVisualizationSocket;

	// Use this for initialization
	void Start () {
        //Per initialization order, the only scripts to start by themselves in the scene are the ones related to the Immersion itself, nothing else. All the scripts
        //      related to the Interfaces, Visualization and InformationObjects must be triggered by either scripts that come from the ConfigurationMenu or by game objects
        //      activated by the Controller of the Immersion scene.

        //Any special consideration for any given visualization (like changing an object's size, color, creating more or less of some instanced object or whatever) must also
        //      be done in here.
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        string currentVisualization = GLPlayerPrefs.GetString(Scope, "CurrentVisualization");

        if (currentVisualization.Equals("Sphere"))
        {
            sphereVisualizationSocket.SetActive(true);
        }

        if (currentVisualization.Equals("Plane"))
        {
            planeVisualizationSocket.SetActive(true);
        }
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
