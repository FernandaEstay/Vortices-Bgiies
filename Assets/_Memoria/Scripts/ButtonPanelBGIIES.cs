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
    public enum Categorias {
        Categoria1,
        Categoria2,
        Categoria3,
        Categoria4
    };
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

        public Vector3 posInicialMouse;
        public bool primerMovimiento;

        public bool mostrarCategoria = false;
        public string nombreCategoria;

        //public Categorias categorias;

        public override void Initialize(DIOManager dioManager)
        {
            base.dioManager = dioManager;
            EnableMoveCameraInside();
            EnableMoveCameraOutside();

            bt1.name = "floraYfauna";
            bt2.name = "superficies";
            bt3.name = "mitigaciones";
            bt4.name = "estructuras";

            NegativeAllButtons();

            mostrarCategoria = false;
            primerMovimiento = false;

            if (dioManager.mouseInput)
                posInicialMouse = Input.mousePosition;
        }

        public void Update()
        {
        }

        public void InitExperiment()
        {
            StartCoroutine(CalculaTiempo());
        }

        IEnumerator CalculaTiempo()
        {
            min = 0;
            seg = 0;
            while (min != 1)
            {
                if(seg == 60)
                {
                    min++;
                    seg = 0;
                }
                if (min.ToString().Length == 1)
                    text = "Tiempo: 0" + min.ToString();
                else
                    text = "Tiempo: " + min.ToString();

                if (seg.ToString().Length == 1)
                    text = text + ":0" + seg.ToString();
                else
                    text = text + ":" + seg.ToString();
                txtTime.text = text;
                seg++;
                yield return new WaitForSeconds(1f);
            }
            EndExperiment();
        }
        public override void Inside()
        {
            if (!mostrarCategoria)
                dioManager.MovePlaneInside(1, dioManager.initialPlaneAction, dioManager.finalPlaneAction);
            else
                dioManager.lookPointerInstanceBgiies.InsideCategoria(dioManager.lookPointerInstanceBgiies.actualListaCat);
        }

        public override void Outside()
        {
            if(!mostrarCategoria)
                dioManager.MovePlaneOutside(1, dioManager.initialPlaneAction, dioManager.finalPlaneAction);
            else
                dioManager.lookPointerInstanceBgiies.OutsideCategoria(dioManager.lookPointerInstanceBgiies.actualListaCat);
        }


        public void SelectBt1()
        {
            if (!mostrarCategoria)
            {
                if (dioManager.lookPointerInstanceBgiies.zoomActive)
                    dioManager.lookPointerInstanceBgiies.SelectCat1();
                else
                    IngresarACategoria(dioManager.lookPointerInstanceBgiies.listaCat1, bt1, new Button[] { bt2, bt3, bt4 }, 0);
            }
            else
            {
                if (dioManager.lookPointerInstanceBgiies.zoomActive)
                    DeseleccionarFromCategoria(dioManager.lookPointerInstanceBgiies.listaCat1, (int)Categorias.Categoria1, bt1, aceptBt1);
                else
                    SalirDeCategoria(dioManager.lookPointerInstanceBgiies.listaCat1, new Button[] { bt2, bt3, bt4 });
            }
        }
        public void SelectBt2()
        {
            if (!mostrarCategoria)
            {
                if (dioManager.lookPointerInstanceBgiies.zoomActive)
                    dioManager.lookPointerInstanceBgiies.SelectCat2();
                else
                    IngresarACategoria(dioManager.lookPointerInstanceBgiies.listaCat2, bt2, new Button[] { bt1, bt3, bt4 }, 0);
            }
            else
            {
                if (dioManager.lookPointerInstanceBgiies.zoomActive)
                    DeseleccionarFromCategoria(dioManager.lookPointerInstanceBgiies.listaCat2, (int)Categorias.Categoria2, bt2, aceptBt2);
                else
                    SalirDeCategoria(dioManager.lookPointerInstanceBgiies.listaCat2, new Button[] { bt1, bt3, bt4 });
            }
        }
        public void SelectBt3()
        {
            if (!mostrarCategoria)
            {
                if (dioManager.lookPointerInstanceBgiies.zoomActive)
                    dioManager.lookPointerInstanceBgiies.SelectCat3();
                else
                    IngresarACategoria(dioManager.lookPointerInstanceBgiies.listaCat3, bt3, new Button[] { bt1, bt2, bt4 }, 0);
            }
            else
            {
                if (dioManager.lookPointerInstanceBgiies.zoomActive)
                    DeseleccionarFromCategoria(dioManager.lookPointerInstanceBgiies.listaCat3, (int)Categorias.Categoria3, bt3, aceptBt3);
                else
                    SalirDeCategoria(dioManager.lookPointerInstanceBgiies.listaCat3, new Button[] { bt1, bt2, bt4 });
            }
        }
        public void SelectBt4()
        {
            if (!mostrarCategoria)
            {
                if (dioManager.lookPointerInstanceBgiies.zoomActive)
                    dioManager.lookPointerInstanceBgiies.SelectCat4();
                else
                    IngresarACategoria(dioManager.lookPointerInstanceBgiies.listaCat4, bt4, new Button[] { bt1, bt2, bt3 }, 0);
            }
            else
            {
                if (dioManager.lookPointerInstanceBgiies.zoomActive)
                    DeseleccionarFromCategoria(dioManager.lookPointerInstanceBgiies.listaCat4, (int)Categorias.Categoria4, bt4, aceptBt4);
                else
                    SalirDeCategoria(dioManager.lookPointerInstanceBgiies.listaCat4, new Button[] { bt1, bt2, bt3 });
            }
        }

        public void IngresarACategoria(List<PitchGrabObject> lista, Button botonCategoria, Button[] botonesDesactive, int indexPhotos)
        {
            NegativeAllButtons();
            mostrarCategoria = true;
            nombreCategoria = botonCategoria.tag;
            dioManager.lookPointerInstanceBgiies.MostrarCategoria(lista, indexPhotos);
            PositiveCatButton(botonCategoria);
            ActiveDesactiveButtons(botonesDesactive, false);
        }

        public void SalirDeCategoria(List<PitchGrabObject> categoriaActual, Button[] botonesActive)
        {
            dioManager.lookPointerInstanceBgiies.MostrarImagenes(categoriaActual);
            mostrarCategoria = false;
            ActiveDesactiveButtons(botonesActive, true);
            EnableMoveCameraInside();
            EnableMoveCameraOutside();
        }

        public void DeseleccionarFromCategoria(List<PitchGrabObject> listaActual, int nombreCategoria, Button botonCategoria, Color colorBotonEncendido)
        {
            dioManager.lookPointerInstanceBgiies.DeseleccionarFromCategoria(listaActual, nombreCategoria, botonCategoria, colorBotonEncendido);
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

        public void EndExperiment()
        {
            if (min == 10)
            {
                SceneManager.LoadScene("ConfigCanvas");
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

        public void NegativeAllButtons()
        {
            NegativeCatButton(bt1);
            NegativeCatButton(bt2);
            NegativeCatButton(bt3);
            NegativeCatButton(bt4);
        }

        public void ActiveDesactiveButtons(Button[] buttons, bool isActive)
        {
            foreach (var button in buttons)
                button.gameObject.SetActive(isActive);
        }

    }
}

