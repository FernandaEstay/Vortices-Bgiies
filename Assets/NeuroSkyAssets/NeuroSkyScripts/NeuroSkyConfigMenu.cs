using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gamelogic;

public class NeuroSkyConfigMenu : MonoBehaviour {

    public Slider blinkStrength, meditationLevel, attentionLevel;
    public Text blinkStrengthNumber, meditationLevelNumber, attentionLevelNumber;
    public Dropdown blinkActionsDropdown, meditationActionsDropdown, attentionActionsDropdown;
    int blinkAssignedActionIndex, meditationAssignedActionIndex, attentionAssignedActionIndex;
    string Scope;

    private void Awake()
    {

    }

    void OnEnable()
    {
        ActionManager.Instance.ReloadProfileDropdown(attentionActionsDropdown);
        ActionManager.Instance.ReloadProfileDropdown(blinkActionsDropdown);
        ActionManager.Instance.ReloadProfileDropdown(meditationActionsDropdown);
        LoadPlayerPreferences();
        CleanNeuroSkyActions();        

        if (EEGManager.Instance.blinkStrengthTrigger != 0)
        {
            SetTriggerValues(ref EEGManager.Instance.blinkStrengthTrigger, blinkStrength, blinkStrengthNumber);
        }
        else
        {
            blinkStrength.value = 50f;
            UpdateTriggerValues(ref EEGManager.Instance.blinkStrengthTrigger, blinkStrength, blinkStrengthNumber);
        }

        if (EEGManager.Instance.meditationLevelTrigger != 0)
        {
            SetTriggerValues(ref EEGManager.Instance.meditationLevelTrigger, meditationLevel, meditationLevelNumber);
        }
        else
        {
            meditationLevel.value = 50f;
            UpdateTriggerValues(ref EEGManager.Instance.meditationLevelTrigger, meditationLevel, meditationLevelNumber);
        }

        if (EEGManager.Instance.attentionLevelTrigger != 0)
        {
            SetTriggerValues(ref EEGManager.Instance.attentionLevelTrigger, attentionLevel, attentionLevelNumber);
        }
        else
        {
            attentionLevel.value = 50f;
            UpdateTriggerValues(ref EEGManager.Instance.attentionLevelTrigger, attentionLevel, attentionLevelNumber);
        }

        blinkActionsDropdown.value = blinkAssignedActionIndex;
        meditationActionsDropdown.value = meditationAssignedActionIndex;
        attentionActionsDropdown.value = attentionAssignedActionIndex;
        attentionActionsDropdown.RefreshShownValue();
        blinkActionsDropdown.RefreshShownValue();
        meditationActionsDropdown.RefreshShownValue();
        
    }

    void LoadPlayerPreferences()
    {
        Scope = ProfileManager.Instance.currentEvaluationScope;

        if (ActionManager.Instance.bgiiesMode)
        {
            blinkAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "NeuroSkyBlinkActionIndexBgiies");
            meditationAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "NeuroSkyMeditationActionIndexBgiies");
            attentionAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "NeuroSkyAttentionActionIndexBgiies");

            EEGManager.Instance.blinkStrengthTrigger = GLPlayerPrefs.GetInt(Scope, "NeuroSkyBlinkStrengthTriggerBgiies");
            EEGManager.Instance.meditationLevelTrigger = GLPlayerPrefs.GetInt(Scope, "NeuroSkyMeditationLevelTriggerBgiies");
            EEGManager.Instance.attentionLevelTrigger = GLPlayerPrefs.GetInt(Scope, "NeuroSkyAttentionLevelTriggerBgiies");
        }
        else
        {
            blinkAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "NeuroSkyBlinkActionIndexVortices");
            meditationAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "NeuroSkyMeditationActionIndexVortices");
            attentionAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "NeuroSkyAttentionActionIndexVortices");

            EEGManager.Instance.blinkStrengthTrigger = GLPlayerPrefs.GetInt(Scope, "NeuroSkyBlinkStrengthTriggerVortices");
            EEGManager.Instance.meditationLevelTrigger = GLPlayerPrefs.GetInt(Scope, "NeuroSkyMeditationLevelTriggerVortices");
            EEGManager.Instance.attentionLevelTrigger = GLPlayerPrefs.GetInt(Scope, "NeuroSkyAttentionLevelTriggerVortices");
        }
    }

    void SavePlayerPreferences()
    {
        Scope = ProfileManager.Instance.currentEvaluationScope;
        if (ActionManager.Instance.bgiiesMode)
        {
            GLPlayerPrefs.SetInt(Scope, "NeuroSkyBlinkActionIndexBgiies", blinkAssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "NeuroSkyMeditationActionIndexBgiies", meditationAssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "NeuroSkyAttentionActionIndexBgiies", attentionAssignedActionIndex);

            GLPlayerPrefs.SetInt(Scope, "NeuroSkyBlinkStrengthTriggerBgiies", EEGManager.Instance.blinkStrengthTrigger);
            GLPlayerPrefs.SetInt(Scope, "NeuroSkyMeditationLevelTriggerBgiies", EEGManager.Instance.meditationLevelTrigger);
            GLPlayerPrefs.SetInt(Scope, "NeuroSkyAttentionLevelTriggerBgiies", EEGManager.Instance.attentionLevelTrigger);
        }
        else
        {
            GLPlayerPrefs.SetInt(Scope, "NeuroSkyBlinkActionIndexVortices", blinkAssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "NeuroSkyMeditationActionIndexVortices", meditationAssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "NeuroSkyAttentionActionIndexVortices", attentionAssignedActionIndex);

            GLPlayerPrefs.SetInt(Scope, "NeuroSkyBlinkStrengthTriggerVortices", EEGManager.Instance.blinkStrengthTrigger);
            GLPlayerPrefs.SetInt(Scope, "NeuroSkyMeditationLevelTriggerVortices", EEGManager.Instance.meditationLevelTrigger);
            GLPlayerPrefs.SetInt(Scope, "NeuroSkyAttentionLevelTriggerVortices", EEGManager.Instance.attentionLevelTrigger);
        }
    }

    private void OnDisable()
    {        
        UpdateActionsNeuroSky();
        SavePlayerPreferences();
    }

    void CleanNeuroSkyActions()
    {
        for (int i = 0; i < ActionManager.Instance.updateActionsNeuroSky.Length; i++)
        {
            ActionManager.Instance.updateActionsNeuroSky[i] = null;
        }
    }

    public void UpdateActionsNeuroSky()
    {
        if (blinkAssignedActionIndex != 0)
        {
            ActionManager.Instance.updateActionsNeuroSky[0] = () => ActionManager.Instance.ActionPairing(
                    ActionManager.Instance.ActionConditionIntValueGreaterThan(ref EEGManager.Instance.blinkStrength, ref EEGManager.Instance.blinkStrengthTrigger),
                    ActionManager.Instance.currentActionList[blinkAssignedActionIndex]
                );
        }
        else
        {
            ActionManager.Instance.updateActionsNeuroSky[0] = null;
        }

        if (meditationAssignedActionIndex != 0)
        {
            ActionManager.Instance.updateActionsNeuroSky[1] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionIntValueGreaterThan(ref EEGManager.Instance.meditationLevel, ref EEGManager.Instance.meditationLevelTrigger),
                ActionManager.Instance.currentActionList[meditationAssignedActionIndex]
            );
        }
        else
        {
            ActionManager.Instance.updateActionsNeuroSky[1] = null;
        }

        if (attentionAssignedActionIndex != 0)
        {
            ActionManager.Instance.updateActionsNeuroSky[2] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionIntValueGreaterThan(ref EEGManager.Instance.attentionLevel, ref EEGManager.Instance.attentionLevelTrigger),
                ActionManager.Instance.currentActionList[attentionAssignedActionIndex]
            );
        }
        else
        {
            ActionManager.Instance.updateActionsNeuroSky[2] = null;
        }
    }

    public void RemoveNeuroSkyAction()
    {
        for (int i = 0; i<ActionManager.Instance.updateActionsNeuroSky.Length;i++ )
        {
            ActionManager.Instance.updateActionsNeuroSky[i] = null;
        }
    }

   

    public void UpdateAttentionValueTriggers()
    {        
        UpdateTriggerValues(ref EEGManager.Instance.attentionLevelTrigger, attentionLevel, attentionLevelNumber);
    }

    public void UpdateBlinkValueTriggers()
    {
        UpdateTriggerValues(ref EEGManager.Instance.blinkStrengthTrigger, blinkStrength, blinkStrengthNumber);
    }

    public void UpdateMeditationValueTriggers()
    {
        UpdateTriggerValues(ref EEGManager.Instance.meditationLevelTrigger, meditationLevel, meditationLevelNumber);
    }

    public void UpdateBlinkActionIndex()
    {
        blinkAssignedActionIndex = blinkActionsDropdown.value;
    }

    public void UpdateMeditationActionIndex()
    {
        meditationAssignedActionIndex = meditationActionsDropdown.value;
    }

    public void UpdateAttentionActionIndex()
    {
        attentionAssignedActionIndex = attentionActionsDropdown.value;
    }


    void UpdateTriggerValues(ref int trigger, Slider slider, Text text)
    {
        trigger = (int)slider.value;
        text.text = trigger.ToString();
    }

    void SetTriggerValues(ref int trigger, Slider slider, Text text)
    {
        slider.value = (float)trigger;
        text.text = trigger.ToString();
    }


}
