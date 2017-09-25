using Gamelogic;
using Leap.Unity;
using Memoria;
using Memoria.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneImageManager : MonoBehaviour {

    //This shows the black screen with the loading percentage, and deactivates the canvas at the end.
    public LoadingScene loadingScene;
    public Text visualizationCounter;

    //Controller that loads the images into the objects
    public LoadImagesController loadImageController;

    //Game object with the required scripts to show the loading canvas and to load the images
    public GameObject planeImagesUtilities;

    //
    public DIOController informationPrefab;
    //DELETE THIS i don't really believe this should even exist, there are only plane images, what's the point?
    public DIOController informationPlanePrefab;

    //initialization for Sphere visualization
    public void Initialize(SphereVisualizationManager sphereVisualizationManager)
    {
        planeImagesUtilities.SetActive(true);

        string Scope = ProfileManager.Instance.currentEvaluationScope;

        loadImageController.Initialize(sphereVisualizationManager);
        loadImageController.images = Convert.ToInt32(GLPlayerPrefs.GetString(Scope, "PlaneImageAmount"));
        loadImageController.LoadImageBehaviour.pathImageAssets = GLPlayerPrefs.GetString(Scope, "PlaneImageFolderPath");

        //loadImageController.LoadImageBehaviour.pathSmall = GLPlayerPrefs.GetString(Scope, "FolderSmallText");
        loadImageController.LoadImageBehaviour.pathSmall = "";
        loadImageController.LoadImageBehaviour.filename = GLPlayerPrefs.GetString(Scope, "PlaneImagePrefix");
    }

    //initialization for Plane visualization
    public void Initialize(PlaneVisualizationManager planeVisualizationManager)
    {
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        loadImageController.Initialize(planeVisualizationManager);
        loadImageController.images = Convert.ToInt32(GLPlayerPrefs.GetString(Scope, "PlaneImageAmount"));
        loadImageController.LoadImageBehaviour.pathImageAssets = GLPlayerPrefs.GetString(Scope, "PlaneImageFolderPath");

        //loadImageController.LoadImageBehaviour.pathSmall = GLPlayerPrefs.GetString(Scope, "FolderSmallText");
        loadImageController.LoadImageBehaviour.pathSmall = "";
        loadImageController.LoadImageBehaviour.filename = GLPlayerPrefs.GetString(Scope, "PlaneImagePrefix");
    }

    public void LoadObjects()
    {
        loadingScene.Initialize();
        StartCoroutine(loadImageController.LoadFolderImages());
    }
}
