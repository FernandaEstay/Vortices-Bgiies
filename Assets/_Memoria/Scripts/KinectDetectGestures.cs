using System;
using Gamelogic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Windows.Kinect;
using System.Collections;
using UnityCallbacks;
using OpenGlove_API_C_Sharp_HL;
using OpenGlove_API_C_Sharp_HL.ServiceReference1;

namespace Memoria
{
    public class KinectDetectGestures : GLMonoBehaviour, IFixedUpdate
    {

        private KinectSensor kinectSensor;
        private Body[] bodies;
        public GameObject BodySrcManager;
        public BodySourceManager bodyManager;

        public bool zoomIn;
        public bool zoomInActive = false;
        public bool zoomOut;

        bool selectedActive = false;

        float tiempo;
        float tiempoOpen;
        float tiempoClose;
        DIOManager dioManager;

        bool initialize = false;

        public HapticGloveKinect hapticGlove;

        int[] regionsSelection = {
                (int) PalmarRegion.FingerIndexDistal,
                (int) PalmarRegion.FingerMiddleDistal,
        };

        int intensityMax = 255;
        int intensityZero = 0;

        public void Initialize(DIOManager dioManager)
        {
            this.dioManager = dioManager;
            this.BodySrcManager = dioManager.bodySrcManager;

            if (BodySrcManager == null)
            {
                Debug.Log("Falta asignar Game Object as BodySrcManager");
            }
            else
            {
                bodyManager = BodySrcManager.GetComponent<BodySourceManager>();
            }

            initialize = true;
            tiempo = Time.deltaTime;
        }

        // Update is called once per frame
        public void FixedUpdate()
        {

            if (!initialize)
            {
                return;
            }
            if (bodyManager == null)
            {
                return;
            }
            bodies = bodyManager.GetData();
            if (bodies == null)
            {
                return;
            }
            foreach (var body in bodies)
            {
                if (body == null)
                {
                    continue;
                }
                if (body.IsTracked)
                {
                        if ((int)body.HandRightConfidence == 1)
                        {
                            switch (body.HandRightState)
                            {
                                case HandState.Lasso:
                                    dioManager.MovePlaneInside(1, dioManager.initialPlaneAction, dioManager.finalPlaneAction);
                                    if (dioManager.useHapticGlove)
                                    {
                                        dioManager.unityHapticGlove.ActiveMotorRegions(regionsSelection, intensityMax, dioManager.unityHapticGlove.gloveRight);
                                        StartCoroutine(dioManager.unityHapticGlove.DeactiveMotorRegions(1f, regionsSelection, intensityZero, dioManager.unityHapticGlove.gloveRight));
                                    }
                                    break;

                                case HandState.Open:
                                    if (!zoomIn)
                                    {
                                        zoomIn = true;
                                        zoomOut = false;
                                    }
                                    break;
                                case HandState.Closed:
                                    if (!zoomOut && dioManager.kinectGestureManager.ActiveZoomOut)
                                    {
                                        zoomOut = true;
                                        zoomIn = false;
                                    }
                                    break;
                            }
                        }
                        if ((int)body.HandLeftConfidence == 1)
                        {
                            switch (body.HandLeftState)
                            {
                                case HandState.Lasso:
                                    dioManager.MovePlaneOutside(1, dioManager.initialPlaneAction, dioManager.finalPlaneAction);
                                    if (dioManager.useHapticGlove)
                                    {
                                        dioManager.unityHapticGlove.ActiveMotorRegions(regionsSelection, 255, dioManager.unityHapticGlove.gloveLeft);
                                        Debug.Log("Guante izquierdo vibra");
                                        StartCoroutine(dioManager.unityHapticGlove.DeactiveMotorRegions(1f, regionsSelection, 0 , dioManager.unityHapticGlove.gloveLeft));
                                        Debug.Log("Guante izquierdo no vibra");
                                }
                                    break;
                                default:
                                    break;
                            }
                        }

                    }
                }
            }
        public bool kinectGestureZoomIn()
        {
            return zoomIn;
        }
        public bool kinectGestureZoomOut()
        {
            return zoomOut;
        }
    }
}
