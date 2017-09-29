﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gamelogic;

public class KeyboardMappingController : MonoBehaviour {

    public ActionMapingController actionMapController;
    public Dropdown keyboardActionsDropdow, keyboardDropdown;
    string interfaceName = "Keyboard";
    string Scope;
    string currentVisualization;
    string currentObject;
    string[] keyName = new string[]
    {
        "Q",
        "W",
        "E",
        "A",
        "S",
        "D"
    };

    private void OnEnable()
    {
        currentVisualization = GLPlayerPrefs.GetString(Scope, "CurrentVisualization");
        currentObject = GLPlayerPrefs.GetString(Scope, "CurrentInformationObject");
        Scope = ProfileManager.Instance.currentEvaluationScope;
        AddArrayToDropdown(keyboardDropdown, keyName);
        ActionManager.Instance.ReloadMappingActionsDropdown(keyboardActionsDropdow);
        UpdateMappedActions(keyName);
        SetKeyboardConfigMenuValues();
    }

    #region update values in UI methods

    public void SetKeyboardConfigMenuValues()
    {
        int level = keyboardDropdown.value;
        keyboardActionsDropdow.value = ActionManager.Instance.GetMappedActionIndex(interfaceName, keyName[level]);

        keyboardActionsDropdow.RefreshShownValue();
    }

    public void UpdateKeyboardActionDropdownValues()
    {
        int level = keyboardDropdown.value;
        int action = keyboardActionsDropdow.value;
        ActionManager.Instance.SetMappedActionIndex(interfaceName, keyName[level], action);
        UpdateMappedActions(keyName);
    }

    void AddArrayToDropdown(Dropdown availableInputDropdown, string[] actionsNames)
    {
        availableInputDropdown.ClearOptions();
        foreach (string s in actionsNames)
        {
            availableInputDropdown.options.Add(new Dropdown.OptionData() { text = s });
        }
        availableInputDropdown.RefreshShownValue();
    }

    void UpdateMappedActions(string[] inputNames)
    {
        string aux = "";
        foreach (string s in ActionManager.Instance.GetMappedActionsListNames(interfaceName, inputNames))
        {
            aux = aux + s + "\n";
        }
        actionMapController.scrollDown.LaunchScrollDown("Actions Paired", aux);
    }
    #endregion
}