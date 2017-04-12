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
            if (Input.GetMouseButtonDown(1) && actualPitchGrabObject != null && !dioManager.movingPlane)
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

        public void SelectCat1()
        {
            bool unPitchedAccept = false;

            if (actualPitchGrabObject == null)
            {
                if (posibleActualPitchGrabObject == null)
                    return;

                unPitchedAccept = true;
                actualPitchGrabObject = posibleActualPitchGrabObject;
            }

            actualPitchGrabObject.isSelectedCat1 = !actualPitchGrabObject.isSelectedCat1;

            if (actualPitchGrabObject.isSelectedCat1)
            {


                GameObject obj = Instantiate(dioManager.childPrefab) as GameObject;
                obj.transform.parent = actualPitchGrabObject.transform;

                int i = actualPitchGrabObject.transform.childCount;

                actualPitchGrabObject.transform.GetChild(i - 1).transform.position = new Vector3(actualPitchGrabObject.transform.position.x - 0.142f + ((i - 1) * 0.095f), actualPitchGrabObject.transform.position.y - 0.09f, actualPitchGrabObject.transform.position.z - 0.001f);
                obj.transform.GetComponent<Renderer>().material.color = dioManager.panelBgiies.aceptBt1;

                Debug.Log("objeto " + actualPitchGrabObject.idName + " es seleccionado por categoria 1");
            }
            else
            {
                int child = actualPitchGrabObject.transform.childCount;
                int index = 0;
                for (int i = 0; i < child; i++)
                {
                    if (actualPitchGrabObject.transform.GetChild(i).transform.GetComponent<Renderer>().material.color == dioManager.panelBgiies.aceptBt1)
                    {
                        Destroy(actualPitchGrabObject.transform.GetChild(i).gameObject);
                    }
                    else
                    {
                        actualPitchGrabObject.transform.GetChild(i).transform.position = new Vector3(actualPitchGrabObject.transform.position.x - 0.142f + (index * 0.095f), actualPitchGrabObject.transform.position.y - 0.09f, actualPitchGrabObject.transform.position.z - 0.001f);
                        index += 1;
                    }
                }
                Debug.Log("objeto " + actualPitchGrabObject.idName + " es deseleccionado por categoria 1");
            }

            var action = actualPitchGrabObject.isSelectedCat1 ? "Select categoria 1" : "Deselect categoria 1";
            dioManager.csvCreator.AddLines(action, actualPitchGrabObject.idName);

            if (unPitchedAccept)
                actualPitchGrabObject = null;
        }

        public void SelectCat2()
        {
            bool unPitchedAccept = false;

            if (actualPitchGrabObject == null)
            {
                if (posibleActualPitchGrabObject == null)
                    return;

                unPitchedAccept = true;
                actualPitchGrabObject = posibleActualPitchGrabObject;
            }

            actualPitchGrabObject.isSelectedCat2 = !actualPitchGrabObject.isSelectedCat2;

            if (actualPitchGrabObject.isSelectedCat2)
            {
                GameObject obj = Instantiate(dioManager.childPrefab) as GameObject;
                obj.transform.parent = actualPitchGrabObject.transform;

                int i = actualPitchGrabObject.transform.childCount;

                actualPitchGrabObject.transform.GetChild(i - 1).transform.position = new Vector3(actualPitchGrabObject.transform.position.x - 0.142f + ((i - 1) * 0.095f), actualPitchGrabObject.transform.position.y - 0.09f, actualPitchGrabObject.transform.position.z - 0.001f);
                obj.transform.GetComponent<Renderer>().material.color = dioManager.panelBgiies.aceptBt2;

                Debug.Log("objeto " + actualPitchGrabObject.idName + " es seleccionado por categoria 2");
            }
            else
            {
                int child = actualPitchGrabObject.transform.childCount;
                int index = 0;
                for (int i = 0; i < child; i++)
                {
                    if (actualPitchGrabObject.transform.GetChild(i).transform.GetComponent<Renderer>().material.color == dioManager.panelBgiies.aceptBt2)
                    {
                        Destroy(actualPitchGrabObject.transform.GetChild(i).gameObject);
                    }
                    else
                    {
                        actualPitchGrabObject.transform.GetChild(i).transform.position = new Vector3(actualPitchGrabObject.transform.position.x - 0.142f + (index * 0.095f), actualPitchGrabObject.transform.position.y - 0.09f, actualPitchGrabObject.transform.position.z - 0.001f);
                        //actualPitchGrabObject.transform.GetChild(i).transform.GetComponent<Renderer>().enabled = false;
                        index += 1;
                    }
                }
                Debug.Log("objeto " + actualPitchGrabObject.idName + " es deseleccionado por categoria 2");
            }

            var action = actualPitchGrabObject.isSelectedCat2 ? "Select categoria 2" : "Deselect categoria 2";
            dioManager.csvCreator.AddLines(action, actualPitchGrabObject.idName);

            if (unPitchedAccept)
                actualPitchGrabObject = null;
        }
        public void SelectCat3()
        {
            bool unPitchedAccept = false;

            if (actualPitchGrabObject == null)
            {
                if (posibleActualPitchGrabObject == null)
                    return;

                unPitchedAccept = true;
                actualPitchGrabObject = posibleActualPitchGrabObject;
            }

            actualPitchGrabObject.isSelectedCat3 = !actualPitchGrabObject.isSelectedCat3;

            if (actualPitchGrabObject.isSelectedCat3)
            {
                GameObject obj = Instantiate(dioManager.childPrefab) as GameObject;
                obj.transform.parent = actualPitchGrabObject.transform;

                int i = actualPitchGrabObject.transform.childCount;

                actualPitchGrabObject.transform.GetChild(i - 1).transform.position = new Vector3(actualPitchGrabObject.transform.position.x - 0.142f + ((i - 1) * 0.095f), actualPitchGrabObject.transform.position.y - 0.09f, actualPitchGrabObject.transform.position.z - 0.001f);
                obj.transform.GetComponent<Renderer>().material.color = dioManager.panelBgiies.aceptBt3;
            }
            else
            {
                int child = actualPitchGrabObject.transform.childCount;
                int index = 0;
                for (int i = 0; i < child; i++)
                {
                    if (actualPitchGrabObject.transform.GetChild(i).transform.GetComponent<Renderer>().material.color == dioManager.panelBgiies.aceptBt3)
                    {
                        Destroy(actualPitchGrabObject.transform.GetChild(i).gameObject);
                    }
                    else
                    {
                        actualPitchGrabObject.transform.GetChild(i).transform.position = new Vector3(actualPitchGrabObject.transform.position.x - 0.142f + (index * 0.095f), actualPitchGrabObject.transform.position.y - 0.09f, actualPitchGrabObject.transform.position.z - 0.001f);
                        //actualPitchGrabObject.transform.GetChild(i).transform.GetComponent<Renderer>().enabled = false;
                        index += 1;
                    }
                }
                Debug.Log("objeto " + actualPitchGrabObject.idName + " es deseleccionado por categoria 3");
            }

            var action = actualPitchGrabObject.isSelectedCat3 ? "Select categoria 3" : "Deselect categoria 3";
            dioManager.csvCreator.AddLines(action, actualPitchGrabObject.idName);

            if (unPitchedAccept)
                actualPitchGrabObject = null;
        }
        public void SelectCat4()
        {
            bool unPitchedAccept = false;

            if (actualPitchGrabObject == null)
            {
                if (posibleActualPitchGrabObject == null)
                    return;

                unPitchedAccept = true;
                actualPitchGrabObject = posibleActualPitchGrabObject;
            }

            actualPitchGrabObject.isSelectedCat4 = !actualPitchGrabObject.isSelectedCat4;

            if (actualPitchGrabObject.isSelectedCat4)
            {
                GameObject obj = Instantiate(dioManager.childPrefab) as GameObject;
                obj.transform.parent = actualPitchGrabObject.transform;

                int i = actualPitchGrabObject.transform.childCount;

                actualPitchGrabObject.transform.GetChild(i - 1).transform.position = new Vector3(actualPitchGrabObject.transform.position.x - 0.142f + ((i - 1) * 0.095f), actualPitchGrabObject.transform.position.y - 0.09f, actualPitchGrabObject.transform.position.z - 0.001f);
                obj.transform.GetComponent<Renderer>().material.color = dioManager.panelBgiies.aceptBt4;

            }
            else
            {
                int child = actualPitchGrabObject.transform.childCount;
                int index = 0;
                for (int i = 0; i < child; i++)
                {
                    if (actualPitchGrabObject.transform.GetChild(i).transform.GetComponent<Renderer>().material.color == dioManager.panelBgiies.aceptBt4)
                    {
                        Destroy(actualPitchGrabObject.transform.GetChild(i).gameObject);
                    }
                    else
                    {
                        actualPitchGrabObject.transform.GetChild(i).transform.position = new Vector3(actualPitchGrabObject.transform.position.x - 0.142f + (index * 0.095f), actualPitchGrabObject.transform.position.y - 0.09f, actualPitchGrabObject.transform.position.z - 0.001f);
                        //actualPitchGrabObject.transform.GetChild(i).transform.GetComponent<Renderer>().enabled = false;
                        index += 1;
                    }
                }
                Debug.Log("objeto " + actualPitchGrabObject.idName + " es deseleccionado por categoria 4");
            }

            var action = actualPitchGrabObject.isSelectedCat1 ? "Select categoria 4" : "Deselect categoria 4";
            dioManager.csvCreator.AddLines(action, actualPitchGrabObject.idName);

            if (unPitchedAccept)
                actualPitchGrabObject = null;
        }
    }
}
