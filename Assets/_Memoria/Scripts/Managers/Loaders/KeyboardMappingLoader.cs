﻿using System;
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
        "R",
        "T",
        "Y",
        "U",
        "I",
        "O",
        "P",
        "A",
        "S",
        "D",
        "F",
        "G",
        "H",
        "J",
        "K",
        "L",
        "Z",
        "X",
        "C",
        "V",
        "B",
        "N",
        "M"
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
        AddAction(3, KeyCode.R);
        AddAction(4, KeyCode.T);
        AddAction(5, KeyCode.Y);
        AddAction(6, KeyCode.U);
        AddAction(7, KeyCode.I);
        AddAction(8, KeyCode.O);
        AddAction(9, KeyCode.P);
        AddAction(10, KeyCode.A);
        AddAction(11, KeyCode.S);
        AddAction(12, KeyCode.D);
        AddAction(13, KeyCode.F);
        AddAction(14, KeyCode.G);
        AddAction(15, KeyCode.H);
        AddAction(16, KeyCode.J);
        AddAction(17, KeyCode.K);
        AddAction(18, KeyCode.L);
        AddAction(19, KeyCode.Z);
        AddAction(20, KeyCode.X);
        AddAction(21, KeyCode.C);
        AddAction(22, KeyCode.V);
        AddAction(23, KeyCode.B);
        AddAction(24, KeyCode.N);
        AddAction(25, KeyCode.M);

    }

    void AddAction(int index, KeyCode key)
    {
        //if the index is 0 it means the action is null, so no need to add it to the update.
        if (actionIndex[index] == 0)
            return;

        ActionManager.Instance.updateActionArrayList.Add( () => ActionManager.Instance.ActionPairing(
            ActionManager.Instance.ActionConditionButtons(key),
            ActionManager.Instance.currentActionList[actionIndex[index]])
            );
    }
}
