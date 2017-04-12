using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;



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

            /*
            DisableButton(bt1, bt1ClicAction);
            DisableButton(bt2, bt2ClicAction);
            DisableButton(bt3, bt3ClicAction);
            DisableButton(bt4, bt4ClicAction);
            */
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
    }
}

