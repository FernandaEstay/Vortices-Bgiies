using Gamelogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizationManager : GLMonoBehaviour {

    public static VisualizationManager Instance { set; get; }

    public PlaneVisualizationManager planeVisualization;
    public SphereVisualizationManager sphereVisualization;

    private void Awake()
    {
        Instance = this;
    }

    public void LoadVisualization()
    {
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        string currentVisualization = GLPlayerPrefs.GetString(Scope, "CurrentVisualization");

        if (currentVisualization.Equals("Sphere"))
        {
            sphereVisualization.gameObject.SetActive(true);
        }

        if (currentVisualization.Equals("Plane"))
        {
            planeVisualization.gameObject.SetActive(true);
        }
    }
}
