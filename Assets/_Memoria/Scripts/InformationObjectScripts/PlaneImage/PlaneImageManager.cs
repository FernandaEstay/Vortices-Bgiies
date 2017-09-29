using Gamelogic;
using Leap.Unity;
using Memoria;
using Memoria.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneImageManager : GLMonoBehaviour {

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

    //Lookpointer has the actions
    [HideInInspector]
    public LookPointerVortices lookPointerInstance;
    public LookPointerVortices lookPointerPrefab;
    public List<Tuple<float, float>> radiusAlphaVisualizationList;

    //initialization for Sphere visualization
    public void Initialize(SphereVisualizationManager sphereVisualizationManager)
    {
        planeImagesUtilities.SetActive(true);
        loadImageController.Initialize(sphereVisualizationManager);
        radiusAlphaVisualizationList = sphereVisualizationManager.radiusAlphaVisualizationList;
        Initialize();
    }

    //initialization for Plane visualization
    public void Initialize(PlaneVisualizationManager planeVisualizationManager)
    {
        planeImagesUtilities.SetActive(true);        
        loadImageController.Initialize(planeVisualizationManager);
        radiusAlphaVisualizationList = planeVisualizationManager.radiusAlphaVisualizationList;
        Initialize();
    }

    void Initialize()
    {
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        loadImageController.images = Convert.ToInt32(GLPlayerPrefs.GetString(Scope, "PlaneImageAmount"));
        loadImageController.LoadImageBehaviour.pathImageAssets = GLPlayerPrefs.GetString(Scope, "PlaneImageFolderPath");

        //loadImageController.LoadImageBehaviour.pathSmall = GLPlayerPrefs.GetString(Scope, "FolderSmallText");
        loadImageController.LoadImageBehaviour.pathSmall = "";
        loadImageController.LoadImageBehaviour.filename = GLPlayerPrefs.GetString(Scope, "PlaneImagePrefix");

        var lookPointerPosition = new Vector3(0.0f, 0.0f, radiusAlphaVisualizationList[1].First);
        lookPointerInstance = Instantiate(lookPointerPrefab, InterfaceManager.Instance.leapMotionManager.leapMotionRig.centerEyeAnchor, lookPointerPosition, Quaternion.identity);
        //This localScale is different in the original DIOManager as it was tied to a variable (called lookPointerScale). If needed, check the legacy code.
        lookPointerInstance.transform.localScale = Vector3.one;
        Action[] objectActions = new Action[]
            {
                () => lookPointerInstance.AcceptObject(),
                () => lookPointerInstance.DirectZoomInCall(null),
                () => lookPointerInstance.DirectZoomOutCall(null)
            };
        ActionManager.Instance.ReloadObjectActions(objectActions);
        MOTIONSManager.Instance.informationObjectInitialized = true;
        MOTIONSManager.Instance.CheckActionManagerInitialization();
    }

    public void LoadObjects()
    {
        loadingScene.Initialize();
        StartCoroutine(loadImageController.LoadFolderImages());
    }
}
