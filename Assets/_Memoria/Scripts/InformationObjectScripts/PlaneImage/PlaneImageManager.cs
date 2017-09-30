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
    public Vector3 lookPointerScale = new Vector3(0.005f,0.005f,0.005f);

    //initialization for Sphere visualization
    public void Initialize()
    {
        planeImagesUtilities.SetActive(true);
        loadImageController.Initialize();
        LoadPreferences();
    }

    void LoadPreferences()
    {
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        loadImageController.images = Convert.ToInt32(GLPlayerPrefs.GetString(Scope, "PlaneImageAmount"));
        loadImageController.LoadImageBehaviour.pathImageAssets = GLPlayerPrefs.GetString(Scope, "PlaneImageFolderPath");
        //loadImageController.LoadImageBehaviour.pathSmall = GLPlayerPrefs.GetString(Scope, "FolderSmallText");
        loadImageController.LoadImageBehaviour.pathSmall = "";
        loadImageController.LoadImageBehaviour.filename = GLPlayerPrefs.GetString(Scope, "PlaneImagePrefix");        
    }

    public void LoadLookPointerActions(List<Tuple<float, float>> radiusAlphaVisualizationListParam)
    {
        radiusAlphaVisualizationList = radiusAlphaVisualizationListParam;
        var lookPointerPosition = new Vector3(0.0f, 0.0f, radiusAlphaVisualizationList[1].First);
        lookPointerInstance = Instantiate(lookPointerPrefab, InterfaceManager.Instance.leapMotionManager.leapMotionRig.centerEyeAnchor, lookPointerPosition, Quaternion.identity);
        lookPointerInstance.transform.localScale = lookPointerScale;

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

    public void LoadObjects(List<DIOController> listOfDio)
    {
        loadingScene.Initialize();
        StartCoroutine(loadImageController.LoadFolderImages(listOfDio));
    }
}
