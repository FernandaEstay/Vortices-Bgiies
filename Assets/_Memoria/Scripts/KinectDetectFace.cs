using System.Linq;
using Gamelogic;
using UnityCallbacks;
using UnityEngine;
using Windows.Kinect;
using Microsoft.Kinect.Face;

namespace Memoria
{
    public class KinectDetectFace : GLMonoBehaviour
    {
        private KinectSensor kinectSensor;
        private int bodyCount;
        private Body[] bodies;
        private FaceFrameSource[] faceFrameSources;
        private FaceFrameReader[] faceFrameReaders;
        private BodySourceManager _BodyManager;
        private int updateFrame;

        private const double FaceRotationIncrementInDegrees = 1.0f;

        private float multX = -1.42f;
        private float multY = 2.6f;

        public GameObject BodySrcManager;
        private BodySourceManager bodyManager;

        private Vector3 posRay;
        private Vector3 posWorld;

        public DIOManager dioManager;

        public void Initialize(DIOManager dioManager)
        {
            this.dioManager = dioManager;
            updateFrame = 0;

            // one sensor is currently supported
            kinectSensor = KinectSensor.GetDefault();

            // set the maximum number of bodies that would be tracked by Kinect
            bodyCount = kinectSensor.BodyFrameSource.BodyCount;

            // allocate storage to store body objects
            bodies = new Body[bodyCount];

            if (BodySrcManager == null)
            {
                Debug.Log("Falta asignar Game Object as BodySrcManager");
            }
            else
            {
                bodyManager = BodySrcManager.GetComponent<BodySourceManager>();
            }

            // specify the required face frame results
            FaceFrameFeatures faceFrameFeatures =
                FaceFrameFeatures.BoundingBoxInColorSpace
                    | FaceFrameFeatures.PointsInColorSpace
                    | FaceFrameFeatures.BoundingBoxInInfraredSpace
                    | FaceFrameFeatures.PointsInInfraredSpace
                    | FaceFrameFeatures.RotationOrientation
                    | FaceFrameFeatures.FaceEngagement
                    | FaceFrameFeatures.Glasses
                    | FaceFrameFeatures.Happy
                    | FaceFrameFeatures.LeftEyeClosed
                    | FaceFrameFeatures.RightEyeClosed
                    | FaceFrameFeatures.LookingAway
                    | FaceFrameFeatures.MouthMoved
                    | FaceFrameFeatures.MouthOpen;

            // create a face frame source + reader to track each face in the FOV
            faceFrameSources = new FaceFrameSource[bodyCount];
            faceFrameReaders = new FaceFrameReader[bodyCount];
            for (int i = 0; i < bodyCount; i++)
            {
                // create the face frame source with the required face frame features and an initial tracking Id of 0
                faceFrameSources[i] = FaceFrameSource.Create(kinectSensor, 0, faceFrameFeatures);

                // open the corresponding reader
                faceFrameReaders[i] = faceFrameSources[i].OpenReader();
            }
        }
    }
}
