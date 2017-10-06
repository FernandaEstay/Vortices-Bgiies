using Gamelogic;
using Memoria.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereVisualizationLoader : MonoBehaviour {

    //LeapMotion Configuration
    public LeapHeadMountedRig leapMotionRig;

    public void LoadInstances()
    {
        InterfaceManager.Instance.leapMotionManager.leapMotionRig = leapMotionRig;
        //DestroyObject(this.gameObject);
        //Here are the interactions asigned, given the chosen visualization. They're in this script to
        //  relieve a bit of the work already done in each individual visualization manager, which is already large.
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        if (GLPlayerPrefs.GetBool(Scope, "useMouse"))
        {
            InteractionManager.Instance.updateList.Add(() =>
               InteractionManager.Instance.raycastingSpherePlane.CreateRay(
               InterfaceManager.Instance.mouseManager.raycast, VisualizationManager.Instance.sphereVisualization.actualVisualization)
                );
        }

        if (GLPlayerPrefs.GetBool(Scope, "useOcculusRift"))
        {
            InteractionManager.Instance.updateList.Add(() =>
               InteractionManager.Instance.raycastingSpherePlane.CreateRay(
               Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), VisualizationManager.Instance.sphereVisualization.actualVisualization)
                );

        }
    }
}
