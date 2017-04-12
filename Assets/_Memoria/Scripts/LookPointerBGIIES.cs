using System;
using System.Collections;
using Gamelogic;
using Memoria.Core;
using UnityCallbacks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Memoria
{
    public class LookPointerBGIIES : LookPointerController, IAwake, IUpdate
    {
        /*
        public Button floraFauna;
        public EventTrigger floraFaunaEventTrigger;

        public Button superficies;
        public EventTrigger superficieEventTrigger;

        public Button mitigacion;
        public EventTrigger mitigacionEventTrigger;

        public Button estructuras;
        public EventTrigger estructurasEventTrigger;
        */

        public void Awake()
        {

            actualPitchGrabObject = null;
            posibleActualPitchGrabObject = null;

            zoomingOut = false;
            zoomingIn = false;
            /*
            ColorBlock cb = dioManager.panelBgiies.bt1.colors;
            cb.normalColor = Color.gray;
            dioManager.panelBgiies.bt1.colors = cb;
            dioManager.panelBgiies.bt2.colors = cb;
            dioManager.panelBgiies.bt3.colors = cb;
            dioManager.panelBgiies.bt4.colors = cb;
            */
        }
        public void Update()
        {
            if (Input.GetMouseButtonDown(0) && actualPitchGrabObject != null && !dioManager.movingPlane)
            {
                if (!zoomingOut && !zoomingIn)
                {
                    StartCoroutine(ZoomingOut(null));
                }
            }

        }

        public override void LookPointerStay(PitchGrabObject pitchGrabObject)
        {
            posibleActualPitchGrabObject = pitchGrabObject;

            if (Input.GetMouseButtonDown(0) && actualPitchGrabObject == null && !dioManager.movingPlane)
            {
                if (!zoomingIn && !zoomingOut)
                {
                    StartCoroutine(ZoomingIn(pitchGrabObject, null));
                }
            }
        }

        public override void SetZoomInInitialStatus(PitchGrabObject pitchGrabObject)
        {
            actualPitchGrabObject = pitchGrabObject;
            actualPitchGrabObject.dioController.inVisualizationPosition = false;
            _actualPitchObjectOriginalPosition = pitchGrabObject.transform.position;
            _actualPitchObjectOriginalRotation = pitchGrabObject.transform.rotation;
            _actualPitchObjectOriginalScale = pitchGrabObject.transform.localScale;
        }
        public override void DirectZoomInCall(Action finalAction)
        {
            if (!zoomingIn && !zoomingOut && actualPitchGrabObject == null && !dioManager.movingSphere)
            {
                StartCoroutine(ZoomingIn(posibleActualPitchGrabObject, finalAction));
            }
        }

        public override void DirectZoomInCall(PitchGrabObject pitchGrabObject, Action finalAction)
        {
            if (!zoomingIn && !zoomingOut && actualPitchGrabObject == null && !dioManager.movingSphere)
            {
                StartCoroutine(ZoomingIn(pitchGrabObject, finalAction));
            }
        }

        public override IEnumerator ZoomingIn(PitchGrabObject pitchGrabObject, Action finalAction)
        {
            zoomingIn = true;
            SetZoomInInitialStatus(pitchGrabObject);

            dioManager.csvCreator.AddLines("ZoomingIn", pitchGrabObject.idName);

            var counter = 0;
            while (true)
            {
                pitchGrabObject.transform.position = Vector3.MoveTowards(pitchGrabObject.transform.position, Vector3.zero, 0.01f);

                if (counter >= dioManager.closeRange)
                {
                    break;
                }

                counter++;
                yield return new WaitForFixedUpdate();
            }

            if (finalAction != null)
                finalAction();

            zoomingIn = false;
        }

        public override void DirectZoomOutCall(Action finalAction)
        {
            if (!zoomingOut && !zoomingIn && actualPitchGrabObject != null && !dioManager.movingSphere)
            {
                StartCoroutine(ZoomingOut(finalAction));
            }
        }

        public override IEnumerator ZoomingOut(Action finalAction)
        {
            dioManager.csvCreator.AddLines("ZoomingOut", actualPitchGrabObject.idName);
            zoomingOut = true;

            var positionTargetReached = false;
            var scaleTargetReaced = false;

            while (true)
            {
                //Position
                actualPitchGrabObject.transform.position =
                    Vector3.MoveTowards(actualPitchGrabObject.transform.position,
                        _actualPitchObjectOriginalPosition, _positionSteps);

                if (actualPitchGrabObject.transform.position.EqualOrMayorCompareVector(_actualPitchObjectOriginalPosition, -0.0001f) && !positionTargetReached)
                {
                    positionTargetReached = true;
                    actualPitchGrabObject.transform.position = _actualPitchObjectOriginalPosition;
                }

                //Scale
                actualPitchGrabObject.transform.localScale =
                    Vector3.MoveTowards(actualPitchGrabObject.transform.localScale,
                        _actualPitchObjectOriginalScale, _scaleSteps);

                if (actualPitchGrabObject.transform.localScale.EqualOrMinorCompareVector(_actualPitchObjectOriginalScale, 0.001f) && !scaleTargetReaced)
                {
                    scaleTargetReaced = true;
                    actualPitchGrabObject.transform.localScale = _actualPitchObjectOriginalScale;
                }

                if (positionTargetReached  && scaleTargetReaced)
                    break;

                yield return new WaitForFixedUpdate();
            }

            actualPitchGrabObject.OnUnDetect();
            actualPitchGrabObject.dioController.inVisualizationPosition = true;
            actualPitchGrabObject = null;
            zoomingOut = false;
          if (finalAction != null)
                finalAction();
        }
    }
}
