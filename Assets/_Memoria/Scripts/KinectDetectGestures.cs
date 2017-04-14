using System;
using Gamelogic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Windows.Kinect;
using System.Collections;
using UnityCallbacks;

namespace Memoria
{
    public class KinectDetectGestures :  GLMonoBehaviour, IFixedUpdate
    {

        private KinectSensor kinectSensor;
        private Body[] bodies;
        public GameObject BodySrcManager;
        public BodySourceManager bodyManager;

        public bool zoomIn;
        public bool zoomInActive = false;
        public bool zoomOut;

        DIOManager dioManager;


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
        }

        // Update is called once per frame
        public void FixedUpdate()
        {
            if (bodyManager == null)
            {
                return;
            }
            bodies = bodyManager.GetData();
            if (bodies == null)
            {
                return;
            }
            Debug.Log("detectando cuerpo");
            foreach (var body in bodies)
            {
                if (body == null)
                {
                    continue;
                }
                if (body.IsTracked)
                {
                    switch (body.HandRightState)
                    {
                        case HandState.Lasso:
                            dioManager.MovePlaneInside(1, dioManager.initialPlaneAction, dioManager.finalPlaneAction);
                            break;
                        case HandState.Open:
                            zoomIn = true;
                            break;
                        case HandState.Closed:
                            zoomOut = true;
                            break;
                        default:
                            zoomIn = false;
                            zoomOut = false;
                            break;
                    }
                    switch (body.HandLeftState)
                    {
                        case HandState.Lasso:
                            dioManager.MovePlaneOutside(1, dioManager.initialPlaneAction, dioManager.finalPlaneAction);
                            break;
                        default:
                            break;
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
