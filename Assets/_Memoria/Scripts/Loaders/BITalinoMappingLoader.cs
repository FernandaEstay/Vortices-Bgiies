using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic;

public class BITalinoMappingLoader : MonoBehaviour
{

    string interfaceName = "BITalino";

    string[] physicalResponseNames = new string[]
      {
        "ECG",
        "EMG",
        "ACC",
        "EDA"
      };

    string[] triggerNames = new string[]
      {
        "LowestRange",
        "HighestRange",
        "Treshold"
      };

    int[] actionIndex;
    float[,] triggerLevelIndex;

    private void OnEnable()
    {
        LoadActions();
    }

    private void LoadActions()
    {

        string Scope = ProfileManager.Instance.currentEvaluationScope;
        actionIndex = new int[physicalResponseNames.Length];
        triggerLevelIndex = new float[physicalResponseNames.Length, triggerNames.Length];

        for (int i = 0; i < physicalResponseNames.Length; i++)
        {
            actionIndex[i] = ActionManager.Instance.GetMappedActionIndex(interfaceName, physicalResponseNames[i]);
            triggerLevelIndex[i, 0] = GLPlayerPrefs.GetFloat(Scope, interfaceName + physicalResponseNames[i] + "LowestRange");
            triggerLevelIndex[i, 1] = GLPlayerPrefs.GetFloat(Scope, interfaceName + physicalResponseNames[i] + "HighestRange");
            triggerLevelIndex[i, 2] = GLPlayerPrefs.GetFloat(Scope, interfaceName + physicalResponseNames[i] + "Treshold");
        }

        AddAction(0, 0);
        AddAction(0, 1);
        AddAction(1, 0);
        AddAction(1, 1);
        AddAction(2, 0);
        AddAction(2, 1);
        AddAction(3, 0);
        AddAction(3, 1);
    }

    void AddAction(int index, int type)
    {
        //if the index is 0 it means the action is null, so no need to add it to the update.
        if (actionIndex[index] == 0)
            return;

        //They are different because they all hold a reference to a different constantly changing value in the BITalino Manager
        if (index == 0)
        {
            //ECG 
            if (type == 0)
            {
                //Lowest Range
                ActionManager.Instance.updateActionArrayList.Add(() => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionFloatValueGreaterThan(ref PhysiologicalManager.Instance.ecg, triggerLevelIndex[index, 0]), //condicion bool
                actionIndex[index])
                 );
                //Highest Range
                ActionManager.Instance.updateActionArrayList.Add(() => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionFloatValueLowerThan(ref PhysiologicalManager.Instance.ecg, triggerLevelIndex[index, 1]), //condicion bool
                actionIndex[index])
                 );
            }
            else if (type == 1)
            {
                //Treshold
                ActionManager.Instance.updateActionArrayList.Add(() => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionFloatValueGreaterThan(ref PhysiologicalManager.Instance.ecg, triggerLevelIndex[index, 2]), //condicion bool
                actionIndex[index])
                 );
            }
        }

        if (index == 1)
        {
            //EMG 
            if (type == 0)
            {
                //Lowest Range
                ActionManager.Instance.updateActionArrayList.Add(() => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionFloatValueGreaterThan(ref PhysiologicalManager.Instance.emg, triggerLevelIndex[index, 0]), //condicion bool
                actionIndex[index])
                 );
                //Highest Range
                ActionManager.Instance.updateActionArrayList.Add(() => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionFloatValueLowerThan(ref PhysiologicalManager.Instance.emg, triggerLevelIndex[index, 1]), //condicion bool
                actionIndex[index])
                 );
            }
            else if (type == 1)
            {
                //Treshold
                ActionManager.Instance.updateActionArrayList.Add(() => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionFloatValueGreaterThan(ref PhysiologicalManager.Instance.emg, triggerLevelIndex[index, 2]), //condicion bool
                actionIndex[index])
                 );
            }
        }
        if (index == 2)
        {
            //EMG 
            if (type == 0)
            {
                //Lowest Range
                ActionManager.Instance.updateActionArrayList.Add(() => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionFloatValueGreaterThan(ref PhysiologicalManager.Instance.acc, triggerLevelIndex[index, 0]), //condicion bool
                actionIndex[index])
                 );
                //Highest Range
                ActionManager.Instance.updateActionArrayList.Add(() => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionFloatValueLowerThan(ref PhysiologicalManager.Instance.acc, triggerLevelIndex[index, 1]), //condicion bool
                actionIndex[index])
                 );
            }
            else if (type == 1)
            {
                //Treshold
                ActionManager.Instance.updateActionArrayList.Add(() => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionFloatValueGreaterThan(ref PhysiologicalManager.Instance.acc, triggerLevelIndex[index, 2]), //condicion bool
                actionIndex[index])
                 );
            }
        }

        if (index == 3)
        {
            //EMG 
            if (type == 0)
            {
                //Lowest Range
                ActionManager.Instance.updateActionArrayList.Add(() => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionFloatValueGreaterThan(ref PhysiologicalManager.Instance.eda, triggerLevelIndex[index, 0]), //condicion bool
                actionIndex[index])
                 );
                //Highest Range
                ActionManager.Instance.updateActionArrayList.Add(() => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionFloatValueLowerThan(ref PhysiologicalManager.Instance.eda, triggerLevelIndex[index, 1]), //condicion bool
                actionIndex[index])
                 );
            }
            else if (type == 1)
            {
                //Treshold
                ActionManager.Instance.updateActionArrayList.Add(() => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionFloatValueGreaterThan(ref PhysiologicalManager.Instance.eda, triggerLevelIndex[index, 2]), //condicion bool
                actionIndex[index])
                 );
            }
        }

        //For debug purposes
        PrintAddedAction(physicalResponseNames[index], ActionManager.Instance.currentActionListNames[actionIndex[index]] + " index: " + actionIndex[index]);

    }

    void PrintAddedAction(string inputName, string pairedActionName)
    {
        Debug.Log("Paired: " + inputName + " to " + pairedActionName);
    }
}