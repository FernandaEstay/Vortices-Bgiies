using Memoria;
using Memoria.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic;
using System.Linq;

public class PlaneVisualizationManager : MonoBehaviour {

    //Plane Configuration

    public PlaneController planePrefab;
    public List<PlaneController> planeControllers;

    //variables
    public int actualVisualization;
    public List<Tuple<float, float>> radiusAlphaVisualizationList;
    public bool movingPlane;

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
            radiusAlphaVisualizationList = new List<Tuple<float, float>> { Tuple.New(0.0f, 0.0f) };
            
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
}
