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

        float tiempo;
        float tiempoOpen;
        float tiempoClose;
        DIOManager dioManager;

        bool initialize = false;

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
                                break;
                            case HandState.Open:
                                Debug.Log("hand open" + (tiempo += Time.deltaTime));
                                if (!zoomIn)
                                {
                                    zoomIn = true;
                                    zoomOut = false;
                                }
                                break;
                            case HandState.Closed:
                                Debug.Log("hand close" + +(tiempo += Time.deltaTime));
                                if (!zoomOut)
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
