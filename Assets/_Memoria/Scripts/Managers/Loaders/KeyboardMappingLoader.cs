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
        actionIndex = new int[keyName.Length];
        //Unfortunately, this can't be done with a "for" cicle as delegates only work with references, and all indexes would use a reference to the "i" value, which would
        //      be then the same for every array, and out of index every time.
        //On the bright side this only has to be done once, as the actions and indexes they reference are given and modified by the ActionManager.
        //Also once you write the code for the first one, you can easily copy and replicate it quickly changing the numbers with a text editor like Sublime text
        for (int i = 0; i < keyName.Length; i++)
        {
            actionIndex[i] = ActionManager.Instance.GetMappedActionIndex(interfaceName, keyName[i]);
        }

        AddAction(0, KeyCode.Q);
        AddAction(1, KeyCode.W);
        AddAction(2, KeyCode.E);
        AddAction(3, KeyCode.A);
        AddAction(4, KeyCode.S);
        AddAction(5, KeyCode.D);

    }

    void AddAction(int index, KeyCode key)
    {
        ActionManager.Instance.updateActionArrayList.Add( () => ActionManager.Instance.ActionPairing(
            ActionManager.Instance.ActionConditionButtons(key),
            ActionManager.Instance.currentActionList[actionIndex[index]])
            );
    }
}
