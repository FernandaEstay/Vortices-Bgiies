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

    private void OnDisable()
    {
        UpdateActionsNeuroSky();
        GLPlayerPrefs.SetInt(Scope, "NeuroSkyBlinkCommandActionIndex", blinkAssignedActionIndex);
        GLPlayerPrefs.SetInt(Scope, "NeuroSkyMeditationCommandActionIndex", meditationAssignedActionIndex);
        GLPlayerPrefs.SetInt(Scope, "NeuroSkyAttentionCommandActionIndex", attentionAssignedActionIndex);

        GLPlayerPrefs.SetInt(Scope, "NeuroSkyBlinkStrengthTrigger", EEGManager.Instance.blinkStrengthTrigger);
        GLPlayerPrefs.SetInt(Scope, "NeuroSkyMeditationLevelTrigger", EEGManager.Instance.meditationLevelTrigger);
        GLPlayerPrefs.SetInt(Scope, "NeuroSkyAttentionLevelTrigger", EEGManager.Instance.attentionLevelTrigger);
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
        ActionManager.Instance.updateActionsNeuroSky[0] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionIntValueGreaterThan(ref EEGManager.Instance.blinkStrength, ref EEGManager.Instance.blinkStrengthTrigger),
                ActionManager.Instance.vorticesActionList[blinkAssignedActionIndex]
            );

        ActionManager.Instance.updateActionsNeuroSky[1] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionIntValueGreaterThan(ref EEGManager.Instance.meditationLevel, ref EEGManager.Instance.meditationLevelTrigger),
                ActionManager.Instance.vorticesActionList[meditationAssignedActionIndex]
            );

        ActionManager.Instance.updateActionsNeuroSky[2] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionIntValueGreaterThan(ref EEGManager.Instance.attentionLevel, ref EEGManager.Instance.attentionLevelTrigger),
                ActionManager.Instance.vorticesActionList[attentionAssignedActionIndex]
            );
    }

    public void RemoveNeuroSkyAction()
    {
        for (int i = 0; i<ActionManager.Instance.updateActionsNeuroSky.Length;i++ )
        {
            ActionManager.Instance.updateActionsNeuroSky[i] = null;
        }
    }

    void OnEnable()
    {
        Scope = ProfileManager.Instance.currentProfileScope;

        blinkAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "NeuroSkyBlinkActionIndex");
        meditationAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "NeuroSkyMeditationActionIndex");
        attentionAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "NeuroSkyAttentionActionIndex");

        EEGManager.Instance.blinkStrengthTrigger = GLPlayerPrefs.GetInt(Scope, "NeuroSkyBlinkStrengthTrigger");
        EEGManager.Instance.meditationLevelTrigger = GLPlayerPrefs.GetInt(Scope, "NeuroSkyMeditationLevelTrigger");
        EEGManager.Instance.attentionLevelTrigger = GLPlayerPrefs.GetInt(Scope, "NeuroSkyAttentionLevelTrigger");
        CleanNeuroSkyActions();

        if(EEGManager.Instance.blinkStrengthTrigger != 0)
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

        attentionActionsDropdown.RefreshShownValue();
        blinkActionsDropdown.RefreshShownValue();
        meditationActionsDropdown.RefreshShownValue();

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
