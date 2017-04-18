using System.Linq;
using Gamelogic;
using UnityCallbacks;
using UnityEngine;
using Windows.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;
using System;

namespace Memoria
{

    public class KinectGestureManager : GLMonoBehaviour
    {
        VisualGestureBuilderDatabase _gestureDatabase;
        VisualGestureBuilderFrameSource _gestureFrameSource;
        VisualGestureBuilderFrameReader _gestureFrameReader;
        KinectSensor _kinect;
        Gesture handUp;
        Gesture handUpProgress;

        /*
        public void SetTrackingId(ulong id)
        {
            _gestureFrameReader.IsPaused = false;
            _gestureFrameSource.TrackingId = id;
            _gestureFrameReader.FrameArrived += _gestureFrameReader_FrameArrived;
        }
        void Start()
        {

            _kinect = KinectSensor.GetDefault();

            _gestureDatabase = VisualGestureBuilderDatabase.Create(Application.streamingAssetsPath + "/HandUp.gbd");
            _gestureFrameSource = VisualGestureBuilderFrameSource.Create(_kinect, 0);

            foreach (var gesture in _gestureDatabase.AvailableGestures)
            {
                _gestureFrameSource.AddGesture(gesture);

                if (gesture.Name == "HandUp")
                {
                    handUp = gesture;
                }
                if (gesture.Name == "HandUpProgress")
                {
                    handUpProgress = gesture;
                }
            }
            _gestureFrameReader = _gestureFrameSource.OpenReader();
            _gestureFrameReader.IsPaused = true;

        }
    }

    void _gestureFrameReader_FrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs e)
    {
        VisualGestureBuilderFrameReference frameReference = e.FrameReference;
        using (VisualGestureBuilderFrame frame = frameReference.AcquireFrame())
        {
            if (frame != null && frame.DiscreteGestureResults != null)
            {
                if (AttachedObject == null)
                    return;

                DiscreteGestureResult result = null;

                if (frame.DiscreteGestureResults.Count > 0)
                    result = frame.DiscreteGestureResults[handUp];
                if (result == null)
                    return;

                if (result.Detected == true)
                {
                    var progressResult = frame.ContinuousGestureResults[_saluteProgress];
                    if (AttachedObject != null)
                    {
                        var prog = progressResult.Progress;
                        Debug.Log("progrado de gesto continuo");
                    }
                }
                else
                {
                    //no se a detectado algun gesto
                }
            }
        }*/
    }
}