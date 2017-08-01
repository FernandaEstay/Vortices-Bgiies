using System.Linq;
using Gamelogic;
using UnityCallbacks;
using UnityEngine;
using Microsoft.Kinect.VisualGestureBuilder;
using System;
using System.Collections.Generic;
using Microsoft.Kinect;
using Windows.Kinect;
using OpenGlove_API_C_Sharp_HL;
using OpenGlove_API_C_Sharp_HL.ServiceReference1;
using System.Collections;

namespace Memoria
{

    public class KinectGestureManager : GLMonoBehaviour
    {
        public struct EventArgs
        {
            public string name;
            public float confidence;

            public EventArgs(string _name, float _confidence)
            {
                name = _name;
                confidence = _confidence;
            }
        }

        public struct gesturesContinuous
        {
            public string name;
            public float result;

            public gesturesContinuous(string name, float result)
            {
                this.name = name;
                this.result = result;
            }
        }

        private VisualGestureBuilderFrameSource vgbFrameSource = null;
        private VisualGestureBuilderFrameReader vgbFrameReader = null;
        VisualGestureBuilderDatabase database;

        DIOManager dioManager;

        bool HandUpActive = true;
        bool HandDownActive = true;
        bool HandRightActive = true;
        bool HandLeftActive = true;

        public bool ActiveZoomOut;
        float resultRel = 0;
        public static gesturesContinuous currentContinuousGesture = new gesturesContinuous();
        public gesturesContinuous maxContinuosGesture = new gesturesContinuous();

        KinectSensor kinectSensor;
        private Body[] bodies;
        public GameObject BodySrcManager;
        public BodySourceManager bodyManager;
        private ulong _trackingId = 0;

        bool initialize = false;

        //public values
        public static string[] gestureNames = new string[7];
        public float[] gestureUntrigger = new float[7];
        public static bool[] isGestureActive = {true, true, true, true, true, true, true };
        // Gesture Detection Events
        public delegate void GestureAction(EventArgs e);
        public event GestureAction OnGesture;

        int[] regionsSelectionSelect = {
                (int) PalmarRegion.FingerIndexDistal,
                (int) PalmarRegion.FingerMiddleDistal,
                (int) PalmarRegion.FingerThumbDistal,
                (int) PalmarRegion.FingerSmallDistal,
                (int) PalmarRegion.FingerRingDistal
        };

        int intensityMax = 255;
        public int intensityZero = 0;

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

            kinectSensor = KinectSensor.GetDefault();

            vgbFrameSource = VisualGestureBuilderFrameSource.Create(kinectSensor, 0);
            vgbFrameSource.TrackingIdLost += Source_TrackingIdLost;

            vgbFrameReader = vgbFrameSource.OpenReader();
            if (vgbFrameReader != null)
            {
                vgbFrameReader.IsPaused = true;
                vgbFrameReader.FrameArrived += this.GestureFrameArrived;
            }

            database = VisualGestureBuilderDatabase.Create(Application.streamingAssetsPath + "/KinectDB.gbd");
            int i = 0;
            foreach (Gesture gesture in database.AvailableGestures)
            {
                this.vgbFrameSource.AddGesture(gesture);
                gestureNames[i] = gesture.Name;
                i++;
            }

            initialize = true;

        }

        // Public setter for Body ID to track
        public void SetBody(ulong id)
        {
            if (id > 0)
            {
                vgbFrameSource.TrackingId = id;
                vgbFrameReader.IsPaused = false;
            }
            else
            {
                vgbFrameSource.TrackingId = 0;
                vgbFrameReader.IsPaused = true;
            }
        }


        public void FixedUpdate()
        {
            if (!initialize)
                return;
            if (!vgbFrameSource.IsTrackingIdValid)
            {
                FindValidBody();
            }
        }

        void FindValidBody()
        {

            if (bodyManager != null)
            {
                Body[] bodies = bodyManager.GetData();
                if (bodies != null)
                {
                    foreach (Body body in bodies)
                    {
                        if (body.IsTracked)
                        {
                            if(!dioManager.panelBgiies.primerMovimiento)
                                SetBody(body.TrackingId);
                                break;
                        }
                    }
                }
            }
        }

        private void Source_TrackingIdLost(object sender, TrackingIdLostEventArgs e)
        {
            // update the GestureResultView object to show the 'Not Tracked' image in the UI
            Debug.Log("Source_trackingIdLost");
        }
        public bool IsPaused
        {
            get
            {
                return this.vgbFrameReader.IsPaused;
            }

            set
            {
                if (this.vgbFrameReader.IsPaused != value)
                {
                    this.vgbFrameReader.IsPaused = value;
                }
            }
        }

        public ulong TrackingId
        {
            get
            {
                return this.vgbFrameSource.TrackingId;
            }

            set
            {
                if (this.vgbFrameSource.TrackingId != value)
                {
                    this.vgbFrameSource.TrackingId = value;
                }
            }
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.vgbFrameReader != null)
                {
                    this.vgbFrameReader.FrameArrived -= this.GestureFrameArrived;
                    this.vgbFrameReader.Dispose();
                    this.vgbFrameReader = null;
                }

                if (this.vgbFrameSource != null)
                {
                    this.vgbFrameSource.TrackingIdLost -= this.Source_TrackingIdLost;
                    this.vgbFrameSource.Dispose();
                    this.vgbFrameSource = null;
                }
            }
        }
        private void GestureFrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs e)
        {
            VisualGestureBuilderFrameReference frameReference = e.FrameReference;
            using (VisualGestureBuilderFrame frame = frameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    // get the discrete gesture results which arrived with the latest frame
                    IDictionary<Gesture, DiscreteGestureResult> discreteResults = frame.DiscreteGestureResults;
                    var continuosResults = frame.ContinuousGestureResults;
                    // we only have one gesture in this source object, but you can get multiple gestures

                    List<gesturesContinuous> gestures = new List<gesturesContinuous>();

                    resultRel = 0;
                    foreach (Gesture gesture in this.vgbFrameSource.Gestures)
                    {
                        if (continuosResults != null)
                        {
                            if (gesture.GestureType == GestureType.Continuous)
                            {
                                ContinuousGestureResult result = null;
                                continuosResults.TryGetValue(gesture, out result);
                                if (result != null)
                                {
                                    if (result.Progress > resultRel)
                                    {
                                        resultRel = result.Progress;
                                        maxContinuosGesture = new gesturesContinuous(gesture.Name, resultRel);
                                    }

                                    for(int i = 0; i < 7; i++)
                                    {
                                        if (gesture.Name.Equals(gestureNames[i]))
                                        {
                                           if(result.Progress < gestureUntrigger[i] && !isGestureActive[i])
                                            {
                                                isGestureActive[i] = true;
                                            }
                                        }                                    
                                    }
                                }

                            }
                        }
                    }

                    currentContinuousGesture = maxContinuosGesture;
                    if (currentContinuousGesture.result != 0)
                    {
                        ActionManager.Instance.KinectGestureUpdate();
                        /*
                        if (currentContinuousGesture.nombre.Equals("HandUpProgress"))
                        {
                            if (currentContinuousGesture.resultado > 0.6f && HandUpActive)
                            {
                                HandUpActive = false;
                                dioManager.panelBgiies.SelectBt1();
                                if (dioManager.useHapticGlove)
                                {
                                    dioManager.unityHapticGlove.ActiveMotorRegions(regionsSelectionSelect, intensityMax, dioManager.unityHapticGlove.gloveRight);
                                    StartCoroutine(dioManager.unityHapticGlove.DeactiveMotorRegions(0.5f, regionsSelectionSelect, intensityZero, dioManager.unityHapticGlove.gloveRight));
                                }
                                ActiveZoomOut = false;
                                return;
                            }
                            if(currentContinuousGesture.resultado < 4f)
                            {
                                ActiveZoomOut = true;
                            }
                        }
                        */
                                                                               
                        
                    }
                }                    
            }
        }
        public void RegisterGestureProgress(string action , float progress)
        { 

            dioManager.csvCreator.AddLines(action, progress.ToString());
        }

    }

}
