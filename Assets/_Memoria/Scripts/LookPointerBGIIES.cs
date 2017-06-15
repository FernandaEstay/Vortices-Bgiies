using System;
using System.Collections;
using Gamelogic;
using Memoria.Core;
using UnityCallbacks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections.Generic;

namespace Memoria
{
    public class LookPointerBGIIES : LookPointer, IAwake, IUpdate
    {
        public bool zoomActive = false;
        public List<DIOController> listOfDio = null;

        public void Awake()
        {

            actualPitchGrabObject = null;
            posibleActualPitchGrabObject = null;

            zoomingOut = false;
            zoomingIn = false;


        }
        public void Update()
        {
            if ((Input.GetMouseButtonDown(1) || dioManager.kinectGestures.kinectGestureZoomOut()) && actualPitchGrabObject != null && !dioManager.movingPlane)
            {
                if (!zoomingOut && !zoomingIn)
                {
                    if (zoomActive)
                    {
                        StartCoroutine(ZoomingOut(null));
                        //dioManager.panelBgiies.noInteractableButtons();
                        zoomActive = false;
                    }
                }

            }

        }

        public override void LookPointerStay(PitchGrabObject pitchGrabObject)
        {
            posibleActualPitchGrabObject = pitchGrabObject;

            if ((Input.GetMouseButtonDown(0) || dioManager.kinectGestures.kinectGestureZoomIn()) && actualPitchGrabObject == null && !dioManager.movingPlane)
            {
                if (!zoomingIn && !zoomingOut)
                {
                    if (!zoomActive)
                    {
                        StartCoroutine(ZoomingIn(pitchGrabObject, null));
                        dioManager.panelBgiies.interactableButtons(posibleActualPitchGrabObject);
                        zoomActive = true;
                    }
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

                dioManager.csvCreator.AddLines("Zooming In", pitchGrabObject.idName);

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
                dioManager.csvCreator.AddLines("Zooming Out", actualPitchGrabObject.idName);
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

                    if (positionTargetReached && scaleTargetReaced)
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
                PitchGrabObject obj =  Instantiate(actualPitchGrabObject);
                obj.transform.localPosition = _actualPitchObjectOriginalPosition;
                obj.transform.localScale = _actualPitchObjectOriginalScale;
                
                obj.transform.localScale =
                    Vector3.MoveTowards(actualPitchGrabObject.transform.localScale,
                        _actualPitchObjectOriginalScale, _scaleSteps);

                if (obj.transform.localScale.EqualOrMinorCompareVector(_actualPitchObjectOriginalScale, 0.001f) )
                {
                    obj.transform.localScale = _actualPitchObjectOriginalScale;
                }

                OcultarImagenes();
                createMarcador(dioManager.panelBgiies.aceptBt1, dioManager.panelBgiies.bt1);
                dioManager.panelBgiies.PositiveCatButton(dioManager.panelBgiies.bt1);
            }
            else
            {
                deleteMarcador(dioManager.panelBgiies.aceptBt1, dioManager.panelBgiies.bt1);
                dioManager.panelBgiies.NegativeCatButton(dioManager.panelBgiies.bt1);
            }

            //dioManager.panelBgiies.interactableButtons(actualPitchGrabObject);

            var action = actualPitchGrabObject.isSelectedCat1 ? "Select " : "Deselect ";
            action = action + dioManager.panelBgiies.bt1.tag.ToString();
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
                createMarcador(dioManager.panelBgiies.aceptBt2, dioManager.panelBgiies.bt2);
                dioManager.panelBgiies.PositiveCatButton(dioManager.panelBgiies.bt2);
            }
            else
            {
                deleteMarcador(dioManager.panelBgiies.aceptBt2, dioManager.panelBgiies.bt2);
                dioManager.panelBgiies.NegativeCatButton(dioManager.panelBgiies.bt2);
            }

            //dioManager.panelBgiies.interactableButtons(actualPitchGrabObject);

            var action = actualPitchGrabObject.isSelectedCat2 ? "Select " : "Deselect ";
            action = action + dioManager.panelBgiies.bt2.tag.ToString();
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
                createMarcador(dioManager.panelBgiies.aceptBt3, dioManager.panelBgiies.bt3);
                dioManager.panelBgiies.PositiveCatButton(dioManager.panelBgiies.bt3);
            }
            else
            {
                deleteMarcador(dioManager.panelBgiies.aceptBt3, dioManager.panelBgiies.bt3);
                dioManager.panelBgiies.NegativeCatButton(dioManager.panelBgiies.bt3);
            }

            //dioManager.panelBgiies.interactableButtons(actualPitchGrabObject);

            var action = actualPitchGrabObject.isSelectedCat3 ? "Select " : "Deselect ";
            action = action + dioManager.panelBgiies.bt3.tag.ToString();
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
                createMarcador(dioManager.panelBgiies.aceptBt4, dioManager.panelBgiies.bt4);
                dioManager.panelBgiies.PositiveCatButton(dioManager.panelBgiies.bt4);

            }
            else
            {
                deleteMarcador(dioManager.panelBgiies.aceptBt4, dioManager.panelBgiies.bt4);
                dioManager.panelBgiies.NegativeCatButton(dioManager.panelBgiies.bt4);
            }

            //dioManager.panelBgiies.interactableButtons(actualPitchGrabObject);

            var action = actualPitchGrabObject.isSelectedCat4 ? "Select " : "Deselect ";
            action = action + dioManager.panelBgiies.bt4.tag.ToString();
            dioManager.csvCreator.AddLines(action, actualPitchGrabObject.idName);

            if (unPitchedAccept)
                actualPitchGrabObject = null;
        }

        public void createMarcador(Color color, Button boton)
        {
            GameObject obj = Instantiate(dioManager.childPrefab) as GameObject;
            obj.transform.parent = actualPitchGrabObject.transform;

            int i = actualPitchGrabObject.transform.childCount;

            actualPitchGrabObject.transform.GetChild(i - 1).transform.position = new Vector3(actualPitchGrabObject.transform.position.x - 0.142f + ((i - 1) * 0.095f), actualPitchGrabObject.transform.position.y - 0.12f, actualPitchGrabObject.transform.position.z - 0.001f);
            obj.transform.GetComponent<Renderer>().material.color = color;

            string[] textBt = boton.GetComponentInChildren<Text>().text.ToString().Split(':');
            int contador = Int32.Parse(textBt[1].Trim()) + 1;
            boton.GetComponentInChildren<Text>().text = textBt[0] + ": " + contador.ToString();
        }

        public void deleteMarcador(Color color, Button boton)
        {
            int child = actualPitchGrabObject.transform.childCount;
            int index = 0;
            for (int i = 0; i < child; i++)
            {
                if (actualPitchGrabObject.transform.GetChild(i).transform.GetComponent<Renderer>().material.color == color)
                {
                    Destroy(actualPitchGrabObject.transform.GetChild(i).gameObject);
                }
                else
                {
                    actualPitchGrabObject.transform.GetChild(i).transform.position = new Vector3(actualPitchGrabObject.transform.position.x - 0.142f + (index * 0.095f), actualPitchGrabObject.transform.position.y - 0.12f, actualPitchGrabObject.transform.position.z - 0.001f);
                    index += 1;
                }
            }

            string[] textBt = boton.GetComponentInChildren<Text>().text.ToString().Split(':');
            int contador = Int32.Parse(textBt[1].Trim()) - 1;
            boton.GetComponentInChildren<Text>().text = textBt[0] + ": " + contador.ToString();
        }

        public void OcultarImagenes()
        {
            listOfDio = dioManager.sphereControllers.Count > dioManager.planeControllers.Count ? dioManager.sphereControllers.SelectMany(sc => sc.dioControllerList).ToList() : dioManager.planeControllers.SelectMany(sc => sc.dioControllerList).ToList();
            foreach (var photo in listOfDio)
            {
                photo.pitchGrabObject.gameObject.SetActive(false);
            }
        }
    }
}
