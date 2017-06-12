using System;
using System.Collections;
using Gamelogic;
using Memoria.Core;
using UnityCallbacks;
using UnityEngine;

namespace Memoria
{
    public class LookPointerVortices : LookPointer, IAwake, IFixedUpdate
    {
        [SerializeField]
        private float _rotationSteps = 2.0f;

        #region Unity Callback

        public void Awake()
        {
            zoomingIn = false;
            zoomingOut = false;
            actualPitchGrabObject = null;
            posibleActualPitchGrabObject = null;
        }

        public void FixedUpdate()
        {
            if ((ZoomOutKeyboardInput() || ZoomOutJoystickInput())
                && actualPitchGrabObject != null && !dioManager.movingSphere)
            {
                if (!zoomingOut && !zoomingIn)
                {
                    StartCoroutine(ZoomingOut(null));
                }
            }

            if ((AcceptObjectKeyboardInput() || AcceptObjectJoystickInput())
                && posibleActualPitchGrabObject != null && !dioManager.movingSphere)
            {
                if (!zoomingOut && !zoomingIn)
                {
                    AcceptObject();
                }
            }
        }

        public override void LookPointerStay(PitchGrabObject pitchGrabObject)
        {
            posibleActualPitchGrabObject = pitchGrabObject;

            if ((ZoomInKeyboardInput() || ZoomInJoystickInput()) &&
                actualPitchGrabObject == null && !dioManager.movingSphere)
            {
                if (!zoomingIn && !zoomingOut)
                {
                    StartCoroutine(ZoomingIn(pitchGrabObject, null));
                }
            }
        }

        #endregion

        #region ZoomIn

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
                pitchGrabObject.transform.position =
                    Vector3.MoveTowards(pitchGrabObject.transform.position,
                        Vector3.zero, 0.01f);

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

        private bool ZoomInKeyboardInput()
        {
            //if (dioManager.useKeyboard && !dioManager.useMouse)
            //{
            //	return Input.GetKeyDown(dioManager.action1Key);
            //}

            return false;
        }

        private bool ZoomInJoystickInput()
        {
            //if (dioManager.useJoystick)
            //{
            //	var zoomIn = Input.GetAxis("Action1");

            //	return zoomIn == 1.0f;
            //}

            return false;
        }

        #endregion

        #region ZoomOut

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
            var rotationTargetReached = false;
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

                //Rotation
                actualPitchGrabObject.transform.rotation =
                    Quaternion.RotateTowards(actualPitchGrabObject.transform.rotation,
                        _actualPitchObjectOriginalRotation, _rotationSteps);

                if (actualPitchGrabObject.transform.rotation.EqualOrMayorCompareQuaternion(_actualPitchObjectOriginalRotation, -0.0001f) && !rotationTargetReached)
                {
                    rotationTargetReached = true;
                    actualPitchGrabObject.transform.rotation = _actualPitchObjectOriginalRotation;
                }

                actualPitchGrabObject.transform.parent.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                //Scale
                actualPitchGrabObject.transform.localScale =
                    Vector3.MoveTowards(actualPitchGrabObject.transform.localScale,
                        _actualPitchObjectOriginalScale, _scaleSteps);

                if (actualPitchGrabObject.transform.localScale.EqualOrMinorCompareVector(_actualPitchObjectOriginalScale, 0.001f) && !scaleTargetReaced)
                {
                    scaleTargetReaced = true;
                    actualPitchGrabObject.transform.localScale = _actualPitchObjectOriginalScale;
                }

                if (positionTargetReached && rotationTargetReached && scaleTargetReaced)
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

        private bool ZoomOutKeyboardInput()
        {
            //if (dioManager.useKeyboard && !dioManager.useMouse)
            //{
            //	return Input.GetKeyDown(dioManager.action2Key);
            //}

            return false;
        }

        private bool ZoomOutJoystickInput()
        {
            //if (dioManager.useJoystick)
            //{
            //	var zoomIn = Input.GetAxis("Action2");

            //	return zoomIn == 1.0f;
            //}

            return false;
        }

        #endregion

        #region Accept

        public void AcceptObject()
        {
            bool unPitchedAccept = false;

            if (actualPitchGrabObject == null)
            {
                if (posibleActualPitchGrabObject == null)
                    return;

                unPitchedAccept = true;
                actualPitchGrabObject = posibleActualPitchGrabObject;
            }

            var pitchMaterial = actualPitchGrabObject.GetComponent<MeshRenderer>();

            actualPitchGrabObject.isSelected = !actualPitchGrabObject.isSelected;
            pitchMaterial.material.color = actualPitchGrabObject.isSelected ? Color.green : Color.white;

            if (actualPitchGrabObject.isSelected)
            {
                dioManager.buttonPanel.NegativeAcceptButton();
            }
            else
            {
                dioManager.buttonPanel.PositiveAcceptButton();
            }

            var action = actualPitchGrabObject.isSelected ? "Select" : "Deselect";
            dioManager.csvCreator.AddLines(action, actualPitchGrabObject.idName);

            if (unPitchedAccept)
                actualPitchGrabObject = null;
        }

        private bool AcceptObjectKeyboardInput()
        {
            if (dioManager.useKeyboard && !dioManager.useMouse)
            {
                return Input.GetKeyDown(dioManager.action5Key);
            }

            return false;
        }

        private bool _acceptJoystickPushed = false;
        private bool AcceptObjectJoystickInput()
        {
            if (dioManager.useJoystick)
            {
                var zoomIn = Input.GetAxis("Submit");

                if (zoomIn == 1.0f)
                {
                    if (_acceptJoystickPushed)
                        return false;

                    _acceptJoystickPushed = true;

                    return true;
                }

                if (zoomIn == 0.0f)
                {
                    _acceptJoystickPushed = false;
                }
            }

            return false;
        }

        #endregion
    }
}