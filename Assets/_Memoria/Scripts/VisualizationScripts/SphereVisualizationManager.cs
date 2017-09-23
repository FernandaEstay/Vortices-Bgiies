using Memoria;
using Memoria.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic;

public class SphereVisualizationManager : MonoBehaviour {

    //Sphere Configuration    
    public SphereController spherePrefab;
    public List<SphereController> sphereControllers;

    //DELETE THIS both configurations are now tied together, this can't be.
    //Plane Configuration
    //public PlaneController planePrefab;
    
    //public List<PlaneController> planeControllers;

    // variables
    public int actualVisualization;
    public List<Tuple<float, float>> radiusAlphaVisualizationList;
    public bool movingSphere;

    // Use this for initialization
    void Start () {
        movingSphere = false;
        string currentObject = GLPlayerPrefs.GetString(ProfileManager.Instance.currentEvaluationScope, "CurrentInformationObject");

        //plane image behaviour
        if (currentObject.Equals("PlaneImage"))
        {
            InformationObjectManager.Instance.planeImages.Initialize(this);
            var visualizationTextureIndex = 0;
            var visualizationIndex = 0;
            actualVisualization = 0;
            radiusAlphaVisualizationList = new List<Tuple<float, float>> { Tuple.New(0.0f, 0.0f) };
            AutoTuneSpheresForImages(InformationObjectManager.Instance.planeImages.loadImageController.images);
            foreach (var sphereController in sphereControllers)
            {
                sphereController.InitializeDioControllers(visualizationIndex, transform.position, visualizationTextureIndex, true);
                radiusAlphaVisualizationList.Add(Tuple.New(sphereController.sphereRadius, sphereController.alpha));

                visualizationTextureIndex += sphereController.elementsToDisplay;
                visualizationIndex += 1;
            }

            InformationObjectManager.Instance.planeImages.LoadObjects();
        }
        
        
    }
	
    

    public void AutoTuneSpheresForImages(int objectAmount)
    {
        //This 39 represents an ideal maximum amount of images in a single sphere considering the size of the images, but
        //  it can can should change depending on the object size. 
        //Use this as reference and play with the "39" value, object amount, alpha and radius. And of course, the IdealSphereConfiguration. 
        int sphereToShow = objectAmount / 39;
        int extraImages = objectAmount % 39;

        if (extraImages != 0)
            sphereToShow++;
        else
            extraImages = 39;

        if (sphereControllers != null)
        {
            foreach (var sphereController in sphereControllers)
            {
                DestroyImmediate(sphereController.gameObject);
            }
        }

        sphereControllers = new List<SphereController>();

        for (int i = 0; i < sphereToShow; i++)
        {
            var newAlpha = 0.7f - 0.3f * i;
            var newRadius = 0.45f + 0.15f * i;

            if (newAlpha < 0.0f)
                newAlpha = 0.0f;

            var newSphere = IdealSphereConfiguration(
                i == sphereToShow - 1 ? extraImages : 39,
                newRadius,
                newAlpha);

            sphereControllers.Add(newSphere);
        }
    }

    SphereController IdealSphereConfiguration(int elements, float radius, float alpha)
    {
        //Just like the AutoTuneSphereForImages, each row here considers the 39 objects inside a sphere (there are 3 rows, 13 * 3 = 39).
        //If a different object is added, change the 13 to better fit the new objects, and change the rowHightDistance, RadiusDifference and scaleFactor. Figure out
        //  by yourself what these values change, as 3D changes are difficult to explain with words.
        var rows = elements / 13;

        if (elements % 13 != 0)
            rows++;

        var sphereController = Instantiate(spherePrefab, Vector3.zero, Quaternion.identity);
        sphereController.transform.parent = transform;
        sphereController.transform.ResetLocal();

        sphereController.elementsToDisplay = elements;
        sphereController.visualizationRow = rows;
        sphereController.rowHightDistance = 0.2f;
        sphereController.rowRadiusDifference = 0.05f;
        sphereController.scaleFactor = new Vector3(0.2f, 0.2f, 0.001f);
        sphereController.sphereRadius = radius;
        sphereController.alpha = alpha;
        sphereController.autoAngleDistance = true;
        sphereController.debugGizmo = false;

        return sphereController;
    }
}
