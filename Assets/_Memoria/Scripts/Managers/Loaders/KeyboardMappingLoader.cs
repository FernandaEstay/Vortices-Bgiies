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

    int[] actionIndex;

    private void OnEnable()
    {
        LoadActions();
    }

    public void LoadActions()
    {
        //this is bad idea, it gets a reference instead of a clone
        //Action[] actionArray = new Action[keyName.Length];
        ActionManager.Instance.updateActionsNeuroSky = new Action[keyName.Length];
        actionIndex = new int[keyName.Length];
        //Unfortunately, this can't be done with a "for" cicle as delegates only work with references, and all indexes would use a reference to the "i" value, which would
        //      be then the same for every array, and out of index every time.
        //On the bright side this only has to be done once, as the actions and indexes they reference are given and modified by the ActionManager.
        //Also once you write the code for the first one, you can easily copy and replicate it quickly changing the numbers with a text editor like Sublime text
        for (int i = 0; i < keyName.Length; i++)
        {
            actionIndex[i] = ActionManager.Instance.GetMappedActionIndex(interfaceName, keyName[i]);
        }

        ActionManager.Instance.updateActionsNeuroSky[0] = () => ActionManager.Instance.ActionPairing(
            ActionManager.Instance.ActionConditionButtons(KeyCode.Q),
            ActionManager.Instance.currentActionList[actionIndex[0]]);

        ActionManager.Instance.updateActionsNeuroSky[1] = () => ActionManager.Instance.ActionPairing(
            ActionManager.Instance.ActionConditionButtons(KeyCode.W),
            ActionManager.Instance.currentActionList[actionIndex[1]]);

        ActionManager.Instance.updateActionsNeuroSky[2] = () => ActionManager.Instance.ActionPairing(
            ActionManager.Instance.ActionConditionButtons(KeyCode.E),
            ActionManager.Instance.currentActionList[actionIndex[2]]);

        ActionManager.Instance.updateActionsNeuroSky[3] = () => ActionManager.Instance.ActionPairing(
            ActionManager.Instance.ActionConditionButtons(KeyCode.A),
            ActionManager.Instance.currentActionList[actionIndex[3]]);

        ActionManager.Instance.updateActionsNeuroSky[4] = () => ActionManager.Instance.ActionPairing(
            ActionManager.Instance.ActionConditionButtons(KeyCode.S),
            ActionManager.Instance.currentActionList[actionIndex[4]]);

        ActionManager.Instance.updateActionsNeuroSky[5] = () => ActionManager.Instance.ActionPairing(
            ActionManager.Instance.ActionConditionButtons(KeyCode.D),
            ActionManager.Instance.currentActionList[actionIndex[5]]);


        ActionManager.Instance.updateActionArrayList.Add(ActionManager.Instance.updateActionsNeuroSky);
        //DELETE THIS try deleting the game object and see what happens
    }
}
