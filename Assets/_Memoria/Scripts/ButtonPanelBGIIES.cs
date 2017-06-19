using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using OpenGlove_API_C_Sharp_HL;
using OpenGlove_API_C_Sharp_HL.ServiceReference1;


namespace Memoria
{
    public class ButtonPanelBGIIES : ButtonPanel
    { 
        public Text txtTime;
        private float time;
        private string text;
        float min;
        float seg;

        public Button bt1;
        public EventTrigger bt1ClicAction;

        public Button bt2;
        public EventTrigger bt2ClicAction;

        public Button bt3;
        public EventTrigger bt3ClicAction;

        public Button bt4;
        public EventTrigger bt4ClicAction;

        public Color aceptBt1;
        public Color aceptBt2;
        public Color aceptBt3;
        public Color aceptBt4;

        Vector3 posInicialMouse;
        public bool primerMovimiento;

        public bool mostrarCategoria = false;

        public override void Initialize(DIOManager dioManager)
        {
            base.dioManager = dioManager;
            EnableMoveCameraInside();
            EnableMoveCameraOutside();

            bt1.name = "floraYfauna";
            bt2.name = "superficies";
            bt3.name = "mitigaciones";
            bt4.name = "estructuras";

            NegativeCatButton(bt1);
            NegativeCatButton(bt2);
            NegativeCatButton(bt3);
            NegativeCatButton(bt4);

            mostrarCategoria = false;

            if (dioManager.mouseInput)
            {
                posInicialMouse = Input.mousePosition;
            }
            if (dioManager.kinectInput)
            {
                primerMovimiento = false;
            }
        }

        public void Update()
        {
            if (dioManager.mouseInput)
            {
                if (posInicialMouse == Input.mousePosition)
                    return;
            }
            if (dioManager.kinectInput)
            {
                if (!primerMovimiento)
                    return;
            }
            
            time += Time.deltaTime;
            min = Mathf.Floor(time / 60);
            seg = (int)time % 60;
            if (min.ToString().Length == 1)
                text = "Tiempo: 0" + min.ToString();
            else
                text = "Tiempo: " + min.ToString();

            if (Mathf.RoundToInt(seg).ToString().Length == 1)
                text = text + ":0" + Mathf.RoundToInt(seg).ToString();
            else
                text = text + ":" + Mathf.RoundToInt(seg).ToString();
            txtTime.text = text;

            if(min == 10)
            {
                SceneManager.LoadScene("ConfigCanvas");
            }

            if (!mostrarCategoria & !dioManager.lookPointerInstanceBgiies.zoomActive)
            {
                NegativeCatButton(bt1);
                NegativeCatButton(bt2);
                NegativeCatButton(bt3);
                NegativeCatButton(bt4);
            }
        }
        public override void Inside()
        {
            if(!mostrarCategoria)
                dioManager.MovePlaneInside(1, dioManager.initialPlaneAction, dioManager.finalPlaneAction);
            else
            {
                dioManager.lookPointerInstanceBgiies.InsideCategoria(dioManager.lookPointerInstanceBgiies.actualListaCat);
            }
        }

        public override void Outside()
        {
            if(!mostrarCategoria)
            dioManager.MovePlaneOutside(1, dioManager.initialPlaneAction, dioManager.finalPlaneAction);
            else
            {
                dioManager.lookPointerInstanceBgiies.OutsideCategoria(dioManager.lookPointerInstanceBgiies.actualListaCat);
            }
        }

        public void SelectBt1()
        {
            if (!mostrarCategoria)
            {
                if (dioManager.lookPointerInstanceBgiies.zoomActive)
                    dioManager.lookPointerInstanceBgiies.SelectCat1();
                else
                {
                    dioManager.lookPointerInstanceBgiies.MostrarCategoria(dioManager.lookPointerInstanceBgiies.listaCat1, 0);
                    mostrarCategoria = true;
                    PositiveCatButton(bt1);
                    bt2.gameObject.SetActive(false);
                    bt3.gameObject.SetActive(false);
                    bt4.gameObject.SetActive(false);
                }
            }
            else
            {
                if (dioManager.lookPointerInstanceBgiies.zoomActive)
                {
                    dioManager.lookPointerInstanceBgiies.DeseleccionarFromCategoria(dioManager.lookPointerInstanceBgiies.listaCat1, "Categoria1", bt1, aceptBt1);
                }
                else
                {
                    dioManager.lookPointerInstanceBgiies.MostrarImagenes(dioManager.lookPointerInstanceBgiies.listaCat1);
                    mostrarCategoria = !mostrarCategoria;
                    bt2.gameObject.SetActive(true);
                    bt3.gameObject.SetActive(true);
                    bt4.gameObject.SetActive(true);
                    EnableMoveCameraInside();
                    EnableMoveCameraOutside();
                }
            }
        }
        public void SelectBt2()
        {
            if (!mostrarCategoria)
            {
                if (dioManager.lookPointerInstanceBgiies.zoomActive)
                    dioManager.lookPointerInstanceBgiies.SelectCat2();
                else
                {
                    dioManager.lookPointerInstanceBgiies.MostrarCategoria(dioManager.lookPointerInstanceBgiies.listaCat2, 0);
                    mostrarCategoria = true;
                    PositiveCatButton(bt2);
                    bt1.gameObject.SetActive(false);
                    bt3.gameObject.SetActive(false);
                    bt4.gameObject.SetActive(false);
                }
            }
            else
            {
                if (dioManager.lookPointerInstanceBgiies.zoomActive)
                {
                    dioManager.lookPointerInstanceBgiies.DeseleccionarFromCategoria(dioManager.lookPointerInstanceBgiies.listaCat2, "Categoria2", bt2, aceptBt2);
                }
                else
                {
                    dioManager.lookPointerInstanceBgiies.MostrarImagenes(dioManager.lookPointerInstanceBgiies.listaCat2);
                    mostrarCategoria = !mostrarCategoria;
                    bt1.gameObject.SetActive(true);
                    bt3.gameObject.SetActive(true);
                    bt4.gameObject.SetActive(true);
                    EnableMoveCameraInside();
                    EnableMoveCameraOutside();
                }
            }
        }
        public void SelectBt3()
        {
            if (!mostrarCategoria)
            {
                if (dioManager.lookPointerInstanceBgiies.zoomActive)
                    dioManager.lookPointerInstanceBgiies.SelectCat3();
                else
                {
                    dioManager.lookPointerInstanceBgiies.MostrarCategoria(dioManager.lookPointerInstanceBgiies.listaCat3, 0);
                    mostrarCategoria = true;
                    PositiveCatButton(bt3);
                    bt1.gameObject.SetActive(false);
                    bt2.gameObject.SetActive(false);
                    bt4.gameObject.SetActive(false);
                }
            }
            else
            {
                if (dioManager.lookPointerInstanceBgiies.zoomActive)
                {
                    dioManager.lookPointerInstanceBgiies.DeseleccionarFromCategoria(dioManager.lookPointerInstanceBgiies.listaCat3, "Categoria3", bt3, aceptBt3);
                }
                else
                {
                    dioManager.lookPointerInstanceBgiies.MostrarImagenes(dioManager.lookPointerInstanceBgiies.listaCat3);
                    mostrarCategoria = !mostrarCategoria;
                    bt1.gameObject.SetActive(true);
                    bt2.gameObject.SetActive(true);
                    bt4.gameObject.SetActive(true);
                    EnableMoveCameraInside();
                    EnableMoveCameraOutside();
                }
            }
        }
        public void SelectBt4()
        {
            if (!mostrarCategoria)
            {
                if (dioManager.lookPointerInstanceBgiies.zoomActive)
                    dioManager.lookPointerInstanceBgiies.SelectCat4();
                else
                {
                    dioManager.lookPointerInstanceBgiies.MostrarCategoria(dioManager.lookPointerInstanceBgiies.listaCat4, 0);
                    mostrarCategoria = true;
                    PositiveCatButton(bt4);
                    bt1.gameObject.SetActive(false);
                    bt2.gameObject.SetActive(false);
                    bt3.gameObject.SetActive(false);
                }
            }
            else
            {

                if (dioManager.lookPointerInstanceBgiies.zoomActive)
                {
                    dioManager.lookPointerInstanceBgiies.DeseleccionarFromCategoria(dioManager.lookPointerInstanceBgiies.listaCat4, "Categoria4", bt4, aceptBt4);
                }
                else
                {
                    mostrarCategoria = !mostrarCategoria;
                    dioManager.lookPointerInstanceBgiies.MostrarImagenes(dioManager.lookPointerInstanceBgiies.listaCat4);
                    bt1.gameObject.SetActive(true);
                    bt2.gameObject.SetActive(true);
                    bt3.gameObject.SetActive(true);
                    EnableMoveCameraInside();
                    EnableMoveCameraOutside();
                }
            }
        }
        public void changeColor(GameObject obj, Color color)
        {
            Renderer rend = obj.GetComponent<MeshRenderer>();
            rend.material.color = color;
        }
        public void noInteractableButtons()
        {
            bt1.interactable = false;
            bt2.interactable = false;
            bt3.interactable = false;
            bt4.interactable = false;
        }


        public void interactableButtons(PitchGrabObject pitchGrabObject)
        {

            bt1.interactable = true;
            bt2.interactable = true;
            bt3.interactable = true;
            bt4.interactable = true;

            if (pitchGrabObject == null)
            {
                if (pitchGrabObject.isSelectedCat1)
                    PositiveCatButton(bt1);
                else
                    NegativeCatButton(bt1);
                if (pitchGrabObject.isSelectedCat2)
                    PositiveCatButton(bt2);
                else
                    NegativeCatButton(bt2);
                if (pitchGrabObject.isSelectedCat3)
                    PositiveCatButton(bt3);
                else
                    NegativeCatButton(bt3);
                if (pitchGrabObject.isSelectedCat4)
                    PositiveCatButton(bt4);
                else
                    NegativeCatButton(bt4);
            }
            else
            {
                if (pitchGrabObject.isSelectedCat1)
                    PositiveCatButton(bt1);
                else
                    NegativeCatButton(bt1);
                if (pitchGrabObject.isSelectedCat2)
                    PositiveCatButton(bt2);
                else
                    NegativeCatButton(bt2);
                if (pitchGrabObject.isSelectedCat3)
                    PositiveCatButton(bt3);
                else
                    NegativeCatButton(bt3);
                if (pitchGrabObject.isSelectedCat4)
                    PositiveCatButton(bt4);
                else
                    NegativeCatButton(bt4);
            }
        }
    
        public void PositiveCatButton(Button boton)
        {
            ColorBlock cb = boton.colors;
            cb.normalColor = cb.highlightedColor;
            boton.colors = cb;
        }

        public void NegativeCatButton(Button boton)
        {
            ColorBlock cb = boton.colors;
            if (boton.name == "floraYfauna")
                cb.normalColor = aceptBt1;
            else if (boton.name == "superficies")
                cb.normalColor = aceptBt2;
            else if (boton.name == "mitigaciones")
                cb.normalColor = aceptBt3;
            else if (boton.name == "estructuras")
                cb.normalColor = aceptBt4;
            else
                Debug.Log("error botones Panel BGIIES");
            boton.colors = cb;
            boton.enabled = false;
            boton.enabled = true;
        }

    }
}

