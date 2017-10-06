using Memoria;
using Memoria.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic;
using System.Linq;
using System;

public class PlaneVisualizationManager : GLMonoBehaviour {

    //Plane Configuration

    public PlaneController planePrefab;
    public List<PlaneController> planeControllers;
    //visual control panel with the categories
    public ButtonPanelBGIIES panelBgiies;
    public GameObject childPrefab;
    //variables
    public int actualVisualization;
    public List<Tuple<float, float>> radiusAlphaVisualizationList;
    public bool movingPlane;

    public float horizontalSpeed = 2.0f;
    public float verticalSpeed = 1.0f;
    public float radiusFactor = 0.005f;
    public float radiusSpeed = 2.0f;
    public float alphaFactor = 0.02f;
    public float alphaSpeed = 2.0f;
    public float alphaWaitTime = 0.8f;

    public bool InLastVisualization
    {
        get { return actualVisualization == (planeControllers.Count - 1); }
    }

    Action[] visualizationActions;

    public bool AreAllDioOnSphere
    {
        get
        {
            var fullDioList = planeControllers.SelectMany(s => s.dioControllerList);
            return fullDioList.All(dio => dio.inVisualizationPosition);
        }
    }

    // Use this for initialization
    void Start () {

        movingPlane = false;
        string currentObject = GLPlayerPrefs.GetString(ProfileManager.Instance.currentEvaluationScope, "CurrentInformationObject");

        //plane image behaviour
        if (currentObject.Equals("PlaneImage"))
        {
            
            var visualizationTextureIndex = 0;
            var visualizationIndex = 0;
            actualVisualization = 0;
            InformationObjectManager.Instance.planeImages.Initialize();
            radiusAlphaVisualizationList = new List<Tuple<float, float>> { Tuple.New(0.0f, 0.0f) };
            //auto-tune of plane given the amount of images
            AutoTunePlanes(InformationObjectManager.Instance.planeImages.loadImageController.images);

            foreach (var planeController in planeControllers)
            {
                planeController.InitializeDioControllers( visualizationIndex, transform.position, visualizationTextureIndex, true);
                radiusAlphaVisualizationList.Add(Tuple.New(planeController.distance, planeController.alpha));

                visualizationTextureIndex += planeController.elementsToDisplay;
                visualizationIndex += 1;
            }

            InformationObjectManager.Instance.planeImages.Initialize();
            InformationObjectManager.Instance.planeImages.LoadObjects(planeControllers.SelectMany(sc => sc.dioControllerList).ToList());

            //Sphere actions are asigned to the action manager
            visualizationActions = new Action[]
            {
                null,
                () => panelBgiies.Inside(),
                () => panelBgiies.Outside(),
                () => panelBgiies.SelectBt1(),
                () => panelBgiies.SelectBt2(),
                () => panelBgiies.SelectBt3(),
                () => panelBgiies.SelectBt4()
            };
            ActionManager.Instance.currentVisualizationActions = new Action[visualizationActions.Length];
            visualizationActions.CopyTo(ActionManager.Instance.currentVisualizationActions, 0);

            MOTIONSManager.Instance.visualizationInitialized = true;
            MOTIONSManager.Instance.CheckActionManagerInitialization();
        }


    }



    private void AutoTunePlanes(int objAmount)
    {
        int planesToShow =  objAmount / 12;
        int extraImages = objAmount % 12;

        if (extraImages != 0)
            planesToShow++;
        else
            extraImages = 12;

        if (planeControllers != null)
        {
            foreach (var planeController in planeControllers)
            {
                DestroyImmediate(planeController.gameObject);
            }
        }

        planeControllers = new List<PlaneController>();

        for (int i = 0; i < planesToShow; i++)
        {
            var newAlpha = 0.7f - 0.3f * i;
            var newRadius = 0.45f + 0.15f * i * 4;

            if (newAlpha < 0.0f)
                newAlpha = 0.0f;

            var newPlane = IdealPlaneConfiguration(
                i == planesToShow - 1 ? extraImages : 12,
                newRadius,
                newAlpha);

            planeControllers.Add(newPlane);
        }
    }

    private PlaneController IdealPlaneConfiguration(int elements, float distance, float alpha)
    {

        var rows = elements / 4;

        if (elements % 4 != 0)
            rows++;

        var planeController = Instantiate(planePrefab, new Vector3(0, 0.5f, 0), Quaternion.identity);
        planeController.transform.parent = transform;      //hace que transform (DIOmanager) sea el padre de visualizationController (esfera actual)

        planeController.transform.ResetLocal();
        planeController.elementsToDisplay = elements;

        planeController.visualizationRow = rows;

        planeController.rowHightDistance = 0.35f;
        planeController.rowDistanceDifference = 0f;

        planeController.scaleFactor = new Vector3(0.38f, 0.33f, 0.001f);     //multiplicador para cambiar el tamaño de cada elemento, asegurando que todos se ven iguales.
        planeController.distance = distance;
        planeController.alpha = alpha;
        planeController.autoAngleDistance = true;


        planeController.debugGizmo = false;

        return planeController;
    }

#region Plane Methods

    public void MovePlaneInside(float insideAxis, Action initialAction, Action finalAction)
    {
        var actualPitchGrabObject = InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.actualPitchGrabObject;
        var zoomingIn = InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.zoomingIn ;
        var zoomingOut = InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.zoomingOut;
        Debug.Log(AreAllDioOnSphere);
        if (insideAxis == 1.0f && !movingPlane && actualPitchGrabObject == null &&
            !zoomingIn && !zoomingOut)
        {
            StartCoroutine(MovePlaneInside(initialAction, finalAction));
        }
        else
        {
            if (finalAction != null)
                finalAction();
        }
    }

    public void MovePlaneOutside(float outsideAxis, Action initialAction, Action finalAction)
    {
        var actualPitchGrabObject = InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.actualPitchGrabObject;
        var zoomingIn = InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.zoomingIn;
        var zoomingOut = InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.zoomingOut;
        if (outsideAxis == 1.0f && !movingPlane && actualPitchGrabObject == null &&
            !zoomingIn && !zoomingOut)
        {
            StartCoroutine(MovePlaneOutside(initialAction, finalAction));
        }
        else
        {
            if (finalAction != null)
                finalAction();
        }
    }

    private IEnumerator MovePlaneInside(Action initialAction, Action finalAction)
    {
        if (movingPlane)
            yield break;

        movingPlane = true;

        var notInZeroPlaneControllers =
        planeControllers.Where(
            planeController =>
                planeController.notInZero
            ).ToList();

        if (notInZeroPlaneControllers.Count == 1)
        {
            movingPlane = false;

            yield break;
        }

        var radiusAlphaTargetReached = new List<Tuple<bool, bool>>();
        for (int i = 0; i < notInZeroPlaneControllers.Count; i++)
        {
            radiusAlphaTargetReached.Add(Tuple.New(false, false));
        }

        var actualRadiusFactor = radiusFactor * -1;
        //DELETE THIS tie to csv creator
        //csvCreator.AddLines("Changing Plane", (actualVisualization + 2).ToString());

        if (initialAction != null)
            initialAction();

        while (true)
        {
            for (int i = 0; i < notInZeroPlaneControllers.Count; i++)
            {
                var planeController = notInZeroPlaneControllers[i];
                var radiusTargetReached = false;
                var alphaTargerReached = false;

                //Radius
                var targetRadius = radiusAlphaVisualizationList[i].First;
                planeController.distance += actualRadiusFactor * radiusSpeed;

                if (TargetReached(actualRadiusFactor, planeController.distance, targetRadius))
                {
                    radiusTargetReached = true;
                    planeController.distance = targetRadius;
                }

                //Alpha
                var actualAlphaFactor = i == 0 ? alphaFactor * -1 : alphaFactor;
                var targetAlpha = radiusAlphaVisualizationList[i].Second;
                planeController.alpha += actualAlphaFactor * alphaSpeed;

                if (TargetReached(actualAlphaFactor, planeController.alpha, targetAlpha))
                {
                    alphaTargerReached = true;
                    planeController.alpha = targetAlpha;
                }

                planeController.ChangeVisualizationConfiguration(transform.position, planeController.distance,
                    planeController.alpha);
                radiusAlphaTargetReached[i] = Tuple.New(radiusTargetReached, alphaTargerReached);
            }

            if (radiusAlphaTargetReached.All(t => t.First && t.Second))
                break;

            yield return new WaitForFixedUpdate();
        }

        planeControllers[actualVisualization].notInZero = false;
        //planeControllers[actualVisualization].gameObject.SetActive(false);
        actualVisualization++;

        if (finalAction != null)
            finalAction();

        movingPlane = false;
    }

    private IEnumerator MovePlaneOutside(Action initialAction, Action finalAction)
    {
        if (movingPlane)
            yield break;

        movingPlane = true;

        var notInZeroPlaneControllers =
            planeControllers.Where(
                planeController =>
                    planeController.notInZero
                ).ToList();

        var inZeroPlaneControllers =
            planeControllers.Where(
                planeController =>
                    !planeController.notInZero
                ).ToList();

        if (inZeroPlaneControllers.Count == 0)
        {
            movingPlane = false;

            yield break;
        }

        var planeControllerList = new List<PlaneController> { inZeroPlaneControllers.Last() };
        planeControllerList.AddRange(notInZeroPlaneControllers);

        var radiusAlphaTargetReached = new List<Tuple<bool, bool>>();
        for (int i = 0; i < planeControllerList.Count; i++)
        {
            radiusAlphaTargetReached.Add(Tuple.New(false, false));
        }

        planeControllers[actualVisualization - 1].gameObject.SetActive(true);
        //DELETE THIS tie to csv creator
        //csvCreator.AddLines("Changing Plane", actualVisualization.ToString());

        if (initialAction != null)
            initialAction();

        var alphaWaitTimeCounter = 0.0f;
        while (true)
        {
            for (int i = 0; i < planeControllerList.Count; i++)
            {
                var planeController = planeControllerList[i];
                var radiusTargetReached = false;
                var alphaTargerReached = false;

                //Radius
                var targetRadius = radiusAlphaVisualizationList[i + 1].First;
                planeController.distance += radiusFactor * radiusSpeed;

                if (TargetReached(radiusFactor, planeController.distance, targetRadius))
                {
                    radiusTargetReached = true;
                    planeController.distance = targetRadius;
                }

                if (alphaWaitTimeCounter >= alphaWaitTime)
                {
                    //Alpha
                    var actualAlphaFactor = i == 0
                        ? alphaFactor
                        : alphaFactor * -1;
                    var targetAlpha = radiusAlphaVisualizationList[i + 1].Second;
                    planeController.alpha += actualAlphaFactor * alphaSpeed;

                    if (TargetReached(actualAlphaFactor, planeController.alpha, targetAlpha))
                    {
                        alphaTargerReached = true;
                        planeController.alpha = targetAlpha;
                    }
                }
                alphaWaitTimeCounter += Time.fixedDeltaTime;

                planeController.ChangeVisualizationConfiguration(transform.position, planeController.distance, planeController.alpha);
                radiusAlphaTargetReached[i] = Tuple.New(radiusTargetReached, alphaTargerReached);
            }

            if (radiusAlphaTargetReached.All(t => t.First && t.Second))
                break;

            yield return new WaitForFixedUpdate();
        }

        actualVisualization--;
        planeControllers[actualVisualization].notInZero = true;

        if (finalAction != null)
            finalAction();

        movingPlane = false;
    }

    public void MovePlaneLastOutside(Action initialAction, Action finalAction)
    {


        var notInZeroPlaneControllers =
            planeControllers.Where(
                planeController =>
                    planeController.notInZero
                ).ToList();

        var inZeroPlaneControllers =
            planeControllers.Where(
                planeController =>
                    !planeController.notInZero
                ).ToList();

        if (inZeroPlaneControllers.Count == 0)
        {
            movingPlane = false;
        }

        var planeControllerList = new List<PlaneController> { inZeroPlaneControllers.Last() };
        planeControllerList.AddRange(notInZeroPlaneControllers);

        var radiusAlphaTargetReached = new List<Tuple<bool, bool>>();
        for (int i = 0; i < planeControllerList.Count; i++)
        {
            radiusAlphaTargetReached.Add(Tuple.New(false, false));
        }

        planeControllers[actualVisualization - 1].gameObject.SetActive(true);
        //DELETE THIS tie to csv creator
        //csvCreator.AddLines("Changing Plane", actualVisualization.ToString());

        if (initialAction != null)
            initialAction();

        var alphaWaitTimeCounter = 0.0f;
        while (true)
        {
            for (int i = 0; i < planeControllerList.Count; i++)
            {
                var planeController = planeControllerList[i];
                var radiusTargetReached = false;
                var alphaTargerReached = false;

                //Radius
                var targetRadius = radiusAlphaVisualizationList[i + 1].First;
                planeController.distance += radiusFactor * radiusSpeed;

                if (TargetReached(radiusFactor, planeController.distance, targetRadius))
                {
                    radiusTargetReached = true;
                    planeController.distance = targetRadius;
                }

                if (alphaWaitTimeCounter >= alphaWaitTime)
                {
                    //Alpha
                    var actualAlphaFactor = i == 0
                        ? alphaFactor
                        : alphaFactor * -1;
                    var targetAlpha = radiusAlphaVisualizationList[i + 1].Second;
                    planeController.alpha += actualAlphaFactor * alphaSpeed;

                    if (TargetReached(actualAlphaFactor, planeController.alpha, targetAlpha))
                    {
                        alphaTargerReached = true;
                        planeController.alpha = targetAlpha;
                    }
                }
                alphaWaitTimeCounter += Time.fixedDeltaTime;

                planeController.ChangeVisualizationConfiguration(transform.position, planeController.distance, planeController.alpha);
                radiusAlphaTargetReached[i] = Tuple.New(radiusTargetReached, alphaTargerReached);
            }

            if (radiusAlphaTargetReached.All(t => t.First && t.Second))
                break;
        }

        actualVisualization--;
        planeControllers[actualVisualization].notInZero = true;

        if (finalAction != null)
            finalAction();

    }

    #endregion

    private bool TargetReached(float factor, float value, float target)
    {
        if (factor >= 0)
        {
            if (value >= target)
            {
                return true;
            }
        }
        else
        {
            if (value <= target)
            {
                return true;
            }
        }

        return false;
    }
}
