using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Memoria
{
    public class ButtonPanelBGIIES : ButtonPanelController
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

        public override void Initialize(DIOManager dioManager)
        {
            base.dioManager = dioManager;
            EnableMoveCameraInside();
            EnableMoveCameraOutside();

            bt1.name = "floraYfauna";
            bt2.name = "superficies";
            bt3.name = "mitigaciones";
            bt4.name = "estructuras";
            noInteractableButtons();
        }
        public override void Inside()
        {
            dioManager.MovePlaneInside(1, dioManager.initialPlaneAction, dioManager.finalPlaneAction);
        }

        public override void Outside()
        {
            dioManager.MovePlaneOutside(1, dioManager.initialPlaneAction, dioManager.finalPlaneAction);
        }

        public void SelectBt1()
        {
            dioManager.lookPointerInstanceBgiies.SelectCat1();
        }
        public void SelectBt2()
        {
            dioManager.lookPointerInstanceBgiies.SelectCat2();
        }
        public void SelectBt3()
        {
            dioManager.lookPointerInstanceBgiies.SelectCat3();
        }
        public void SelectBt4()
        {
            dioManager.lookPointerInstanceBgiies.SelectCat4();
        }
        public void Update()
        {
            time += Time.deltaTime;
            min = Mathf.Floor(time / 60);
            seg = time % 60;
            if (min.ToString().Length == 1)
                text = "Tiempo: 0" + min.ToString();
            else
                text = "Tiempo: " + min.ToString();

            if (Mathf.RoundToInt(seg).ToString().Length == 1)
                text = text + ":0" + Mathf.RoundToInt(seg).ToString();
            else
                text = text + ":" + Mathf.RoundToInt(seg).ToString();
            txtTime.text = text;
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
            /*
            Debug.Log("llega boton " + boton.name);
            ColorBlock cb = boton.colors;
            cb.normalColor = Color.gray;
            boton.colors = cb;*/
        }
    }
}

