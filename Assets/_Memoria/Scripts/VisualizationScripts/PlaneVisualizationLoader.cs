using Gamelogic;
using Memoria.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneVisualizationLoader : GLMonoBehaviour {

    //LeapMotion Configuration
    public LeapHeadMountedRig leapMotionRig;



    public void LoadInstances()
    {
        InterfaceManager.Instance.leapMotionManager.leapMotionRig = leapMotionRig;

        string Scope = ProfileManager.Instance.currentEvaluationScope;
        if (GLPlayerPrefs.GetBool(Scope, "useMouse"))
        {
            InteractionManager.Instance.updateList.Add(() =>
               InteractionManager.Instance.raycastingSpherePlane.CreateRayCategories(
               InterfaceManager.Instance.mouseManager.screenPointToRay, VisualizationManager.Instance.planeVisualization.actualVisualization)
                );
        }
    }
}
