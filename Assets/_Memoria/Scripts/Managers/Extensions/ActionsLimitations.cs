using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsLimitations : MonoBehaviour {

    public List<Action> limitedActionsArrayList = new List<Action>();

    public List<bool>[] currentActionListLimitations;
    public List<bool>[] currentVisualizationActionsLimitations;
    public List<bool>[] currentObjectActionsLimitations;


    public bool AddLimitationToAction(bool limitation, int actionIndex)
    {
        //¿Why is there a new arraylist? because it's the only way to use an "auxiliar" variable that will not be deleted or changed, otherwise as
        //  the operator works, the asociation would be with the Action variable and not the anonym. function itself.
        // This should not use many resources as it only holds references.
        limitedActionsArrayList.Add( ActionManager.Instance.currentActionList[actionIndex]);
        /*ActionManager.Instance.currentActionList[actionIndex] = () => ActionManager.Instance.ActionPairing(
            limitation,
            limitedActionsArrayList[limitedActionsArrayList.Count - 1]);
        Debug.Log("limitation added");*/
        return true;
    }

    public bool ReloadActionLimitations()
    {
        //deletes the previous action list and names by forming them again from the visualization and object arrays
        currentActionListLimitations = new List<bool>[currentObjectActionsLimitations.Length + currentVisualizationActionsLimitations.Length - 1];
        currentVisualizationActionsLimitations.CopyTo(currentActionListLimitations, 0);
        int actionListLen = currentVisualizationActionsLimitations.Length;
        List<bool>[] aux = new List<bool>[currentObjectActionsLimitations.Length - 1];
        for (int i = 1; i < currentObjectActionsLimitations.Length; i++)
        {
            aux[i - 1] = currentObjectActionsLimitations[i];
        }
        aux.CopyTo(currentActionListLimitations, actionListLen);
        return true;
    }

    public void ReloadVisualizationActionsLimitations()
    {
        currentVisualizationActionsLimitations = new List<bool>[ActionManager.Instance.currentVisualizationActions.Length];
        for(int i = 0; i < currentVisualizationActionsLimitations.Length; i++)
        {
            currentVisualizationActionsLimitations[i] = new List<bool>();
            currentVisualizationActionsLimitations[i].Add(true);
        }
    }

    public void ReloadObjectActionsLimitations()
    {
        currentObjectActionsLimitations = new List<bool>[ActionManager.Instance.currentObjectActions.Length];
        for (int i = 0; i < currentObjectActionsLimitations.Length; i++)
        {
            currentObjectActionsLimitations[i] = new List<bool>();
            currentObjectActionsLimitations[i].Add(true);
        }
    }

    public void AddActionLimitation(int actionIndex, Action<bool> limitation)
    {
        //currentActionListLimitations[actionIndex].Add(limitation);
    }


}
