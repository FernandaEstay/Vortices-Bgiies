using System.Linq;
using Gamelogic;
using UnityCallbacks;
using UnityEngine;
using Microsoft.Kinect.VisualGestureBuilder;
using System;
using System.Collections.Generic;
using Microsoft.Kinect;
using Windows.Kinect;

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
            public string nombre;
            public float resultado;

            public gesturesContinuous(string name, float result)
            {
                nombre = name;
                resultado = result;
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
        gesturesContinuous gesRel = new gesturesContinuous();

        KinectSensor kinectSensor;
        private Body[] bodies;
        public GameObject BodySrcManager;
        public BodySourceManager bodyManager;
        private ulong _trackingId = 0;

        bool initialize = false;

        // Gesture Detection Events
        public delegate void GestureAction(EventArgs e);
        public event GestureAction OnGesture;

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
            foreach (Gesture gesture in database.AvailableGestures)
            {
                this.vgbFrameSource.AddGesture(gesture);
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
                                        gesRel = new gesturesContinuous(gesture.Name, resultRel);
                                    }

                                    if (gesture.Name == "HandUpProgress")
                                    {
                                        if (result.Progress < 0.3f && !HandUpActive)
                                            HandUpActive = true;
                                    }
                                    if (gesture.Name == "HandDownProgress")
                                    {
                                        if (result.Progress < 0.3f && !HandDownActive)
                                            HandDownActive = true;
                                    }
                                    if (gesture.Name == "HandRightProgress")
                                    {
                                        if (result.Progress < 0.3f && !HandRightActive)
                                            HandRightActive = true;
                                    }
                                    if (gesture.Name == "HandLeftProgress")
                                    {
                                        if (result.Progress < 0.2f && !HandLeftActive)
                                        {
                                            HandLeftActive = true;
                                        }
                                    }

                                }

                            }
                        }
                    }

                    if (gesRel.resultado != 0)
                    {
                        if (gesRel.nombre == "HandUpProgress")
                        {
                            if (gesRel.resultado > 0.6f && HandUpActive)
                            {
                                HandUpActive = false;
                                dioManager.panelBgiies.SelectBt1();
                                ActiveZoomOut = false;
                                return;
                            }
                            if(gesRel.resultado < 4f)
                            {
                                ActiveZoomOut = true;
                            }
                        }
                                                                               
                        if (gesRel.nombre == "HandDownProgress")
                        {
                            if (gesRel.resultado > 0.7f && HandDownActive)
                            {
                                HandDownActive = false;
                                dioManager.panelBgiies.SelectBt2();
                                ActiveZoomOut = false;
                                return;
                            }
                            if(gesRel.resultado < 5f)
                            {
                                ActiveZoomOut = true;
                            }
                        }
                                        
                        if(gesRel.nombre == "HandRightProgress")
                        {
                            if (gesRel.resultado > 0.6f && HandRightActive)
                            {
                                HandRightActive = false;
                                dioManager.panelBgiies.SelectBt3();
                                ActiveZoomOut = false;
                                return;
                            }
                            if(gesRel.resultado < 4)
                            {
                                ActiveZoomOut = true;
                            }
                        }
                                        
                        if (gesRel.nombre == "HandLeftProgress")
                        {
                            if (gesRel.resultado > 0.5f && HandLeftActive)
                            {
                                HandLeftActive = false;
                                dioManager.panelBgiies.SelectBt4();
                                ActiveZoomOut = false;
                                return;
                            }
                            if(gesRel.resultado < 3f)
                            {
                                ActiveZoomOut = true;
                            }
                        }
                    }
                }                    
            }
        }

    }
}

/*
VisualGestureBuilderDatabase _gestureDatabase;
VisualGestureBuilderFrameSource _gestureFrameSource;
VisualGestureBuilderFrameReader _gestureFrameReader;
Gesture handUp;
Gesture handUpProgress;
private KinectSensor kinectSensor;
private Body[] bodies;
public GameObject BodySrcManager;
public BodySourceManager bodyManager;
private ulong _trackingId = 0;
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
    kinectSensor = KinectSensor.GetDefault();
    _gestureDatabase = VisualGestureBuilderDatabase.Create(Application.streamingAssetsPath + "/kinectBDGestures.gbd");
    _gestureFrameSource = VisualGestureBuilderFrameSource.Create(kinectSensor, 0);
    Gesture[] gestureArray = _gestureDatabase.AvailableGestures.ToArray();
    foreach(var gesture in gestureArray)
    {
        Debug.Log("gesture name");
        _gestureFrameSource.AddGesture(gesture);
        if (gesture.Name == "HandUp")
            handUp = gesture;
        if (gesture.Name == "HandUpProgress")
            handUpProgress = gesture;
    }
    _gestureFrameReader = _gestureFrameSource.OpenReader();
    _gestureFrameReader.IsPaused = true;
}
public void SetTrackingId(ulong id)
{
    _gestureFrameReader.IsPaused = false;
    _gestureFrameSource.TrackingId = id;
    _gestureFrameReader.FrameArrived += _gestureFrameReader_FrameArrived;
}
private void FixedUpdate()
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
    foreach (var body in bodies)
    {
        Debug.Log("llega a update");
        if (body != null && body.IsTracked)
        {
            _trackingId = body.TrackingId;
            SetTrackingId(body.TrackingId);
            break;
        }
    }
}
void _gestureFrameReader_FrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs e)
{
    Debug.Log("llega aca frame reader");
    VisualGestureBuilderFrameReference frameReference = e.FrameReference;
    using (VisualGestureBuilderFrame frame = frameReference.AcquireFrame())
    {
        if (frame != null && frame.DiscreteGestureResults != null)
        {
            DiscreteGestureResult result = null;
            if (frame.DiscreteGestureResults.Count > 0)
                result = frame.DiscreteGestureResults[handUp];
            if (result == null)
                return;
            if (result.Detected == true)
            {
                Debug.Log("detecta gesto discreto");
                var progressResult = frame.ContinuousGestureResults[handUpProgress];
                var prog = progressResult.Progress;
                Debug.Log("detecta gesto continuo con pregreso : " + prog);
            }
        }
    }
}*/
