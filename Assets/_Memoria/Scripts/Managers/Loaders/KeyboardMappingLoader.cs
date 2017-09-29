using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMappingLoader : MonoBehaviour {

    string interfaceName = "Keyboard";

    string[] keyName = new string[]
    {
        "Q",
        "W",
        "E",
        "A",
        "S",
        "D"
    };

    public KeyCode[] keyValue = new KeyCode[]
    {
        KeyCode.Q,
        KeyCode.W,
        KeyCode.E,
        KeyCode.A,
        KeyCode.S,
        KeyCode.D

    };

    private void OnEnable()
    {
        LoadActions();
    }

    public void LoadActions()
    {
        int actionIndex;
        //this is bad idea, it gets a reference instead of a clone
        //Action[] actionArray = new Action[keyName.Length];
        ActionManager.Instance.updateActionsNeuroSky = new Action[keyName.Length];
        /*
        for (int j = 0; j < keyName.Length; j++)
        {
            actionIndex = ActionManager.Instance.GetMappedActionIndex(interfaceName, keyName[j]);
            ActionManager.Instance.updateActionsNeuroSky[j] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionButtons(ActionManager.Instance.keyValue[j]),
                ActionManager.Instance.currentActionList[actionIndex]);
        }*/
        actionIndex = ActionManager.Instance.GetMappedActionIndex(interfaceName, keyName[0]);
        ActionManager.Instance.updateActionsNeuroSky[0] = () => ActionManager.Instance.ActionPairing(
            ActionManager.Instance.ActionConditionButtons(KeyCode.Q),
            ActionManager.Instance.currentActionList[1]);

        actionIndex = ActionManager.Instance.GetMappedActionIndex(interfaceName, keyName[1]);
        ActionManager.Instance.updateActionsNeuroSky[1] = () => ActionManager.Instance.ActionPairing(
            ActionManager.Instance.ActionConditionButtons(KeyCode.W),
            ActionManager.Instance.currentActionList[actionIndex]);

        actionIndex = ActionManager.Instance.GetMappedActionIndex(interfaceName, keyName[2]);
        ActionManager.Instance.updateActionsNeuroSky[2] = () => ActionManager.Instance.ActionPairing(
            ActionManager.Instance.ActionConditionButtons(KeyCode.E),
            ActionManager.Instance.currentActionList[actionIndex]);

        actionIndex = ActionManager.Instance.GetMappedActionIndex(interfaceName, keyName[3]);
        ActionManager.Instance.updateActionsNeuroSky[3] = () => ActionManager.Instance.ActionPairing(
            ActionManager.Instance.ActionConditionButtons(KeyCode.A),
            ActionManager.Instance.currentActionList[actionIndex]);

        actionIndex = ActionManager.Instance.GetMappedActionIndex(interfaceName, keyName[4]);
        ActionManager.Instance.updateActionsNeuroSky[4] = () => ActionManager.Instance.ActionPairing(
            ActionManager.Instance.ActionConditionButtons(KeyCode.S),
            ActionManager.Instance.currentActionList[actionIndex]);

        actionIndex = ActionManager.Instance.GetMappedActionIndex(interfaceName, keyName[5]);
        ActionManager.Instance.updateActionsNeuroSky[5] = () => ActionManager.Instance.ActionPairing(
            ActionManager.Instance.ActionConditionButtons(KeyCode.D),
            ActionManager.Instance.currentActionList[actionIndex]);

        ActionManager.Instance.updateActionArrayList.Add(ActionManager.Instance.updateActionsNeuroSky);
        //DELETE THIS try deleting the game object and see what happens
    }
}
