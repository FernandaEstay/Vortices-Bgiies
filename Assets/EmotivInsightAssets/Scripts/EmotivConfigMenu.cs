using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gamelogic;

public class EmotivConfigMenu : MonoBehaviour {

    //Ticking scripts
    [HideInInspector]
    public int pushTicks = 5, pullTicks = 5, liftTicks = 5, dropTicks = 5, leftTicks = 5, pushMistakes = 5, pullMistakes = 5, liftMistakes = 5, dropMistakes = 5, leftMistakes = 5;
    public Slider tickSensibilityValue, mentalCommandSensibilityValue, mentalCommandMistakeValue;
    public Text tickSensibilityText, mentalCommandMistakeText, mentalCommandSensibilityText;
    //EEG variables
    public Dropdown mentalCommandActionsDropdow, mentalCommandDropdown, facialExpresionActionsDropdown, facialExpresionDropdown;
    public Slider mentalCommandTriggerLevel, facialExpressionTriggerLevel;
    float pushTriggerLevel = 0.5f, pullTriggerLevel = 0.5f, liftTriggerLevel = 0.5f, dropTriggerLevel = 0.5f, leftTriggerLevel = 0.5f, leftWinkTriggerLevel = 0.5f, rightWinkTriggerLevel = 0.5f, anyWinkTriggerLevel = 0.5f, smileTriggerLevel = 0.5f;
    int pushAssignedActionIndex = 5, pullAssignedActionIndex = 5, liftAssignedActionIndex = 5, dropAssignedActionIndex = 5, leftAssignedActionIndex = 5, leftWinkAssignedActionIndex = 5, rightWinkAssignedActionIndex = 5, anyWinkAssignedActionIndex = 5, smileAssignedActionIndex = 5;
    public Text mentalCommandTriggerNumber, facialExpressionTriggerNumber;
    string Scope;
    string currentVisualization;
    string currentObject;
    string[] mentalCommandName = new string[]
    {
        "Push",
        "Pull",
        "Lift",
        "Drop",
        "Left"
    };

    Emotiv.EdkDll.IEE_MentalCommandAction_t[] mentalCommandCode = new Emotiv.EdkDll.IEE_MentalCommandAction_t[]
    {
        Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_PUSH,
        Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_PULL,
        Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_LIFT,
        Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_DROP,
        Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_LEFT
    };

    string[] facialExpresionName = new string[]
    {
        "LeftWink",
        "RightWink",
        "AnyWink",
        "Smile"
    };

    Emotiv.EdkDll.IEE_FacialExpressionAlgo_t[] facialExpresionCode = new Emotiv.EdkDll.IEE_FacialExpressionAlgo_t[]
    {
        Emotiv.EdkDll.IEE_FacialExpressionAlgo_t.FE_WINK_LEFT,
        Emotiv.EdkDll.IEE_FacialExpressionAlgo_t.FE_WINK_RIGHT,
        Emotiv.EdkDll.IEE_FacialExpressionAlgo_t.FE_BLINK,
        Emotiv.EdkDll.IEE_FacialExpressionAlgo_t.FE_SMILE
    };

    bool[] facilExpresionIsUpperFace = new bool[]
    {
        true,
        true,
        true,
        false
    };

    /*
    EEGManager.Instance.MentalCommandCurrentActionPower;
    EEGManager.Instance.FacialExpressionIsRightEyeWinking;
    EEGManager.Instance.FacialExpressionIsLeftEyeWinking;
    EEGManager.Instance.FacialExpressionIsUserBlinking;
    EEGManager.Instance.FacialExpressionUpperFaceActionPower;
    EEGManager.Instance.FacialExpressionSmileExtent;
    EEGManager.Instance.FacialExpressionLowerFaceActionPower;
    EEGManager.Instance.FacialExpressionLowerFaceAction;
    EEGManager.Instance.FacialExpressionUpperFaceAction;
    */


    void OnEnable()
    {
        currentVisualization = GLPlayerPrefs.GetString(Scope, "CurrentVisualization");
        currentObject = GLPlayerPrefs.GetString(Scope, "CurrentInformationObject");
        Scope = ProfileManager.Instance.currentEvaluationScope;

        float aux = GLPlayerPrefs.GetFloat(Scope, "EmotivTickSensibility") *10;
        SetTriggerValues((int)aux, tickSensibilityValue, tickSensibilityText);

        ActionManager.Instance.ReloadProfileDropdown(facialExpresionActionsDropdown);
        ActionManager.Instance.ReloadProfileDropdown(mentalCommandActionsDropdow);

        SetMentalCommandConfigMenuValues();
        SetFacialExpressionConfigMenuValues();
        
    }

    #region UI triggers

    public void SetTickConfigMenuValues()
    {        
        ActionManager.Instance.endTime = (tickSensibilityValue.value / 10);
        int aux = (int)tickSensibilityValue.value;
        tickSensibilityText.text = aux.ToString();
        GLPlayerPrefs.SetFloat(Scope, "EmotivTickSensibility", ActionManager.Instance.endTime);
    }

    public void SetMentalCommandConfigMenuValues()
    {
        int command = mentalCommandDropdown.value;
        //This logic should have an intermediary function, it's a bit confusing to explain
        int visIndex = GLPlayerPrefs.GetInt(Scope, "Emotiv" + mentalCommandName[command] + currentVisualization + "VisualizationIndex");
        int objIndex = GLPlayerPrefs.GetInt(Scope, "Emotiv" + mentalCommandName[command] + currentObject + "ObjectIndex");
        if (visIndex != 0)
        {
            mentalCommandActionsDropdow.value = visIndex;
        } else if (objIndex != 0)
        {
            int aux = objIndex + ActionManager.Instance.currentVisualizationActions.Length - 1;
            mentalCommandActionsDropdow.value = aux;
        }
        else
        {
            mentalCommandActionsDropdow.value = 0;
        } 

        SetTriggerValues(GLPlayerPrefs.GetInt(Scope, "Emotiv" + mentalCommandName[command] + "CommandTicks"), mentalCommandSensibilityValue, mentalCommandSensibilityText);
        SetTriggerValues(GLPlayerPrefs.GetInt(Scope, "Emotiv" + mentalCommandName[command] + "CommandMistakes"), mentalCommandMistakeValue, mentalCommandMistakeText);
        SetTriggerValues(GLPlayerPrefs.GetFloat(Scope, "Emotiv" + mentalCommandName[command] + "CommandTriggerLevel") * 10, mentalCommandTriggerLevel, mentalCommandTriggerNumber);

        mentalCommandActionsDropdow.RefreshShownValue();
    }

    public void SetFacialExpressionConfigMenuValues()
    {
        int facial = facialExpresionDropdown.value;
        int visIndex = GLPlayerPrefs.GetInt(Scope, "Emotiv" + facialExpresionName[facial] + currentVisualization + "VisualizationIndex");
        int objIndex = GLPlayerPrefs.GetInt(Scope, "Emotiv" + facialExpresionName[facial] + currentObject + "ObjectIndex");
        if (visIndex != 0)
        {
            facialExpresionActionsDropdown.value = visIndex;
        }else if (objIndex != 0)
        {
            int aux = objIndex + ActionManager.Instance.currentVisualizationActions.Length - 1;
            facialExpresionActionsDropdown.value = aux;
        }
        else
        {
            facialExpresionActionsDropdown.value = 0;
        }

        SetTriggerValues(GLPlayerPrefs.GetFloat(Scope, "Emotiv" + facialExpresionName[facial] + "TriggerLevel" ) * 10, facialExpressionTriggerLevel, facialExpressionTriggerNumber);

        facialExpresionActionsDropdown.RefreshShownValue();
    }

    public void UpdateMentalCommandActionDropdownValues()
    {
        int command = mentalCommandDropdown.value;
        int action = mentalCommandActionsDropdow.value;
        string currentVisualization = GLPlayerPrefs.GetString(Scope, "CurrentVisualization");
        string currentObject = GLPlayerPrefs.GetString(Scope, "CurrentInformationObject");
        int aux; 
        if(action == 0)
        {
            GLPlayerPrefs.SetInt(Scope, "Emotiv" + mentalCommandName[command] + currentVisualization + "VisualizationIndex", 0);
            GLPlayerPrefs.SetInt(Scope, "Emotiv" + mentalCommandName[command] + currentObject + "ObjectIndex", 0);
        }else if (action < ActionManager.Instance.currentVisualizationActions.Length)
        {
            GLPlayerPrefs.SetInt(Scope, "Emotiv" + mentalCommandName[command] + currentVisualization + "VisualizationIndex", action);
            GLPlayerPrefs.SetInt(Scope, "Emotiv" + mentalCommandName[command] + currentObject + "ObjectIndex", 0);
        }
        else
        {
            GLPlayerPrefs.SetInt(Scope, "Emotiv" + mentalCommandName[command] + currentVisualization + "VisualizationIndex", 0);
            aux = action - ActionManager.Instance.currentVisualizationActions.Length + 1;
            GLPlayerPrefs.SetInt(Scope, "Emotiv" + mentalCommandName[command] + currentObject + "ObjectIndex", aux);
        }
        //
        //
        // First five slots are for the mental commands
        int ticks = GLPlayerPrefs.GetInt(Scope, "Emotiv" + mentalCommandName[command] + "CommandTicks");
        int mistakes = GLPlayerPrefs.GetInt(Scope, "Emotiv" + mentalCommandName[command] + "CommandMistakes");
        float triggerLevel = GLPlayerPrefs.GetFloat(Scope, "Emotiv" + mentalCommandName[command] + "CommandTriggerLevel");
        ActionManager.Instance.updateActionsEmotivInsight[command] = () => ActionManager.Instance.ActionPairing(
            ActionManager.Instance.ActionConditionEmotiv(mentalCommandCode[command], ticks, mistakes, triggerLevel),
            ActionManager.Instance.currentActionList[action]);

        //
        //
        //  Here, store the name to the list of asigned actions
        //
        //
    }

    public void UpdateFacialExpressionActionDropdownValues()
    {
        int expresion = facialExpresionDropdown.value;
        int action = facialExpresionActionsDropdown.value;
        string currentVisualization = GLPlayerPrefs.GetString(Scope, "CurrentVisualization");
        string currentObject = GLPlayerPrefs.GetString(Scope, "CurrentInformationObject");
        int aux;

        if (action == 0)
        {
            GLPlayerPrefs.SetInt(Scope, "Emotiv" + facialExpresionName[expresion] + currentVisualization + "VisualizationIndex", 0);
            GLPlayerPrefs.SetInt(Scope, "Emotiv" + facialExpresionName[expresion] + currentObject + "ObjectIndex", 0);
        }
        else if (action < ActionManager.Instance.currentVisualizationActions.Length)
        {
            GLPlayerPrefs.SetInt(Scope, "Emotiv" + facialExpresionName[expresion] + currentVisualization + "VisualizationIndex", action);
            GLPlayerPrefs.SetInt(Scope, "Emotiv" + facialExpresionName[expresion] + currentObject + "ObjectIndex", 0);
        }
        else
        {
            GLPlayerPrefs.SetInt(Scope, "Emotiv" + facialExpresionName[expresion] + currentVisualization + "VisualizationIndex", 0);
            aux = action - ActionManager.Instance.currentVisualizationActions.Length + 1;
            GLPlayerPrefs.SetInt(Scope, "Emotiv" + facialExpresionName[expresion] + currentObject + "ObjectIndex", aux);
        }

        float triggerLevel = GLPlayerPrefs.GetFloat(Scope, "Emotiv" + facialExpresionName[expresion] + "TriggerLevel");
        aux = expresion + 5;
        ActionManager.Instance.updateActionsEmotivInsight[aux] = () => ActionManager.Instance.ActionPairing(
            ActionManager.Instance.ActionConditionEmotiv(facialExpresionCode[expresion], facilExpresionIsUpperFace[expresion] ,triggerLevel),
            ActionManager.Instance.currentActionList[action]);
        //
        //
        //  Here, store the name to the list of asigned actions
        //
        //

    }

    public void UpdateMentalCommandSensibilityValues()
    {
        int command = mentalCommandDropdown.value;
        int aux = (int)mentalCommandSensibilityValue.value;
        GLPlayerPrefs.SetInt(Scope, "Emotiv" + mentalCommandName[command] + "CommandTicks", aux);
        mentalCommandSensibilityText.text = aux.ToString();
    }

    public void UpdateMentalCommandMistakesValues()
    {
        int command = mentalCommandDropdown.value;
        int aux = (int)mentalCommandMistakeValue.value;
        GLPlayerPrefs.SetInt(Scope, "Emotiv" + mentalCommandName[command] + "CommandMistakes", aux);
        mentalCommandMistakeText.text = aux.ToString();

    }

    public void UpdateMentalCommandTriggerValues()
    {
        int command = mentalCommandDropdown.value;
        float trigger = (mentalCommandTriggerLevel.value / 10);
        int aux = (int)mentalCommandTriggerLevel.value;
        GLPlayerPrefs.SetFloat(Scope, "Emotiv" + mentalCommandName[command] + "CommandTriggerLevel", trigger);
        mentalCommandTriggerNumber.text = aux.ToString();

    }

    public void UpdateUpperFacialExpressionTriggerValues()
    {
        int expresion = facialExpresionDropdown.value;
        float trigger = (facialExpressionTriggerLevel.value / 10);
        int aux = (int)facialExpressionTriggerLevel.value;
        GLPlayerPrefs.SetFloat(Scope, "Emotiv" + facialExpresionName[expresion] + "TriggerLevel", trigger);
        facialExpressionTriggerNumber.text = aux.ToString();
    }

    

#endregion

    #region update values in UI methods

    void UpdateTriggerValues(ref int trigger, Slider slider, Text text)
    {
        trigger = (int)slider.value;
        text.text = trigger.ToString();
    }

    void UpdateTriggerValues(int trigger, Slider slider, Text text)
    {
        trigger = (int)slider.value;
        text.text = trigger.ToString();
    }

    void UpdateTriggerValues(ref float trigger, Slider slider, Text text)
    {
        trigger = (slider.value/10);
        int aux = (int)slider.value;
        text.text = aux.ToString();
    }

    void SetTriggerValues(ref int trigger, Slider slider, Text text)
    {
        slider.value = trigger;
        text.text = trigger.ToString();
    }

    void SetTriggerValues(int trigger, Slider slider, Text text)
    {
        slider.value = trigger;
        text.text = trigger.ToString();
    }

    void SetTriggerValues(float trigger, Slider slider, Text text)
    {
        Debug.Log("trigger: " + trigger.ToString());
        slider.value = trigger;
        int aux = (int)trigger;
        text.text = aux.ToString();
    }

    void SetActionNameByIndex( Text text, int index)
    {
        if (ActionManager.Instance.bgiiesMode)
        {
            text.text = ActionManager.Instance.bgiiesActionListNames[index];
        }
        else
        {
            text.text = ActionManager.Instance.vorticesActionListNames[index];
        }
    }

#endregion
}
