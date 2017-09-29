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
    }
}
