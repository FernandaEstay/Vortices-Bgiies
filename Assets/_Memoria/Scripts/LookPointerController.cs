using System;
using System.Collections;
using Gamelogic;
using Memoria.Core;
using UnityCallbacks;
using UnityEngine;

namespace Memoria
{
    public abstract class LookPointerController : GLMonoBehaviour
    {
        protected DIOManager dioManager;

        public float _initialAlpha;
        [HideInInspector]
        public bool zoomingIn;
        [HideInInspector]
        public bool zoomingOut;

        [HideInInspector]
        public PitchGrabObject actualPitchGrabObject;
        [HideInInspector]
        public PitchGrabObject posibleActualPitchGrabObject;

        public Vector3 _actualPitchObjectOriginalPosition;
        public Quaternion _actualPitchObjectOriginalRotation;
        public Vector3 _actualPitchObjectOriginalScale;

        public void Initialize(DIOManager fatherDioManager)
        {
            dioManager = fatherDioManager;
        }

        public void LookPointerEnter(PitchGrabObject pitchGrabObject)
        {
            _initialAlpha = pitchGrabObject.dioController.visualizationController.alpha;
            var objectColor = pitchGrabObject.objectMeshRender.material.color;
            objectColor.a = 1.0f;
            pitchGrabObject.objectMeshRender.material.color = objectColor;

            posibleActualPitchGrabObject = pitchGrabObject;
        }



    }
}
