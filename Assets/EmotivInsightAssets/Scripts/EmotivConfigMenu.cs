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
    public Text pushAssignedActionText, pullAssignedActionText, liftAssignedActionText, dropAssignedActionText, leftAssignedActionText, leftWinkAssignedActionText, rightWinkAssignedActionText, anyWinkAssignedActionText, smileAssignedActionText;
    string Scope;
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

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnEnable()
    {
        Scope = ProfileManager.Instance.currentProfileScope;

        pushTicks = GLPlayerPrefs.GetInt(Scope, "EmotivPushCommandTicks");
        pullTicks = GLPlayerPrefs.GetInt(Scope, "EmotivPullCommandTicks");
        liftTicks = GLPlayerPrefs.GetInt(Scope, "EmotivLiftCommandTicks");
        dropTicks = GLPlayerPrefs.GetInt(Scope, "EmotivDropCommandTicks");
        leftTicks = GLPlayerPrefs.GetInt(Scope, "EmotivLeftCommandTicks");

        pushMistakes = GLPlayerPrefs.GetInt(Scope, "EmotivPushCommandMistakes");
        pullMistakes = GLPlayerPrefs.GetInt(Scope, "EmotivPullCommandMistakes");
        liftMistakes = GLPlayerPrefs.GetInt(Scope, "EmotivLiftCommandMistakes");
        dropMistakes = GLPlayerPrefs.GetInt(Scope, "EmotivDropCommandMistakes");
        leftMistakes = GLPlayerPrefs.GetInt(Scope, "EmotivLeftCommandMistakes");

        pushTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivPushCommandTriggerLevel");
        pullTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivPullCommandTriggerLevel");
        liftTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivLiftCommandTriggerLevel");
        dropTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivDropCommandTriggerLevel");
        leftTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivLeftCommandTriggerLevel");

        leftWinkTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivLeftExpressionTriggerLevel");
        rightWinkTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivRightExpressionTriggerLevel");
        anyWinkTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivAnyWinkExpressionTriggerLevel");
        smileTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivSmileExpressionTriggerLevel");

        pushTicks = GLPlayerPrefs.GetInt(Scope, "EmotivPushCommandTicks");
        pullTicks = GLPlayerPrefs.GetInt(Scope, "EmotivPullCommandTicks");
        liftTicks = GLPlayerPrefs.GetInt(Scope, "EmotivLiftCommandTicks");
        dropTicks = GLPlayerPrefs.GetInt(Scope, "EmotivDropCommandTicks");
        leftTicks = GLPlayerPrefs.GetInt(Scope, "EmotivLeftCommandTicks");

        pushMistakes = GLPlayerPrefs.GetInt(Scope, "EmotivPushCommandMistakes");
        pullMistakes = GLPlayerPrefs.GetInt(Scope, "EmotivPullCommandMistakes");
        liftMistakes = GLPlayerPrefs.GetInt(Scope, "EmotivLiftCommandMistakes");
        dropMistakes = GLPlayerPrefs.GetInt(Scope, "EmotivDropCommandMistakes");
        leftMistakes = GLPlayerPrefs.GetInt(Scope, "EmotivLeftCommandMistakes");

        pushTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivPushCommandTriggerLevel");
        pullTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivPullCommandTriggerLevel");
        liftTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivLiftCommandTriggerLevel");
        dropTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivDropCommandTriggerLevel");
        leftTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivLeftCommandTriggerLevel");

        leftWinkTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivLeftWinkExpressionTriggerLevel");
        rightWinkTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivRightWinkExpressionTriggerLevel");
        anyWinkTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivAnyWinkExpressionTriggerLevel");
        smileTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivSmileExpressionTriggerLevel");


        pushAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "EmotivPushCommandActionIndex");
        pullAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "EmotivPullCommandActionIndex");
        liftAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "EmotivLiftCommandActionIndex");
        dropAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "EmotivDropCommandActionIndex");
        leftAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "EmotivLeftCommandActionIndex");

        leftWinkAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "EmotivLeftWinkExpressionActionIndex");
        rightWinkAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "EmotivRightWinkExpressionActionIndex");
        anyWinkAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "EmotivAnyWinkExpressionActionIndex");
        smileAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "EmotivSmileExpressionActionIndex");

        ActionManager.Instance.endTime = GLPlayerPrefs.GetFloat(Scope, "EmotivTickSensibility");

        SetActionNameByIndex(pushAssignedActionText, pushAssignedActionIndex);
        SetActionNameByIndex(pullAssignedActionText, pullAssignedActionIndex);
        SetActionNameByIndex(liftAssignedActionText, liftAssignedActionIndex);
        SetActionNameByIndex(dropAssignedActionText, dropAssignedActionIndex);
        SetActionNameByIndex(leftAssignedActionText, leftAssignedActionIndex);

        SetActionNameByIndex(anyWinkAssignedActionText, anyWinkAssignedActionIndex);
        SetActionNameByIndex(leftWinkAssignedActionText, leftWinkAssignedActionIndex);
        SetActionNameByIndex(rightWinkAssignedActionText, rightWinkAssignedActionIndex);
        SetActionNameByIndex(smileAssignedActionText, smileAssignedActionIndex);

        CleanEmotivActions();
        float aux = ActionManager.Instance.endTime*10;
        SetTriggerValues((int)aux, tickSensibilityValue, tickSensibilityText);
        SetTickConfigMenuValues();
        //mentalCommandDropdown.value = 0;
        SetMentalCommandConfigMenuValues();
        SetFacialExpressionConfigMenuValues();
    }

    private void OnDisable()
    {
        UpdateActionsEmotivInsight();
        GLPlayerPrefs.SetInt(Scope, "EmotivPushCommandTicks", pushTicks);
        GLPlayerPrefs.SetInt(Scope, "EmotivPullCommandTicks", pullTicks);
        GLPlayerPrefs.SetInt(Scope, "EmotivLiftCommandTicks", liftTicks);
        GLPlayerPrefs.SetInt(Scope, "EmotivDropCommandTicks", dropTicks);
        GLPlayerPrefs.SetInt(Scope, "EmotivLeftCommandTicks", leftTicks);

        GLPlayerPrefs.SetInt(Scope, "EmotivPushCommandMistakes", pushMistakes);
        GLPlayerPrefs.SetInt(Scope, "EmotivPullCommandMistakes", pullMistakes);
        GLPlayerPrefs.SetInt(Scope, "EmotivLiftCommandMistakes", liftMistakes);
        GLPlayerPrefs.SetInt(Scope, "EmotivDropCommandMistakes", dropMistakes);
        GLPlayerPrefs.SetInt(Scope, "EmotivLeftCommandMistakes", leftMistakes);

        GLPlayerPrefs.SetFloat(Scope, "EmotivPushCommandTriggerLevel", pushTriggerLevel);
        GLPlayerPrefs.SetFloat(Scope, "EmotivPullCommandTriggerLevel", pullTriggerLevel);
        GLPlayerPrefs.SetFloat(Scope, "EmotivLiftCommandTriggerLevel", liftTriggerLevel);
        GLPlayerPrefs.SetFloat(Scope, "EmotivDropCommandTriggerLevel", dropTriggerLevel);
        GLPlayerPrefs.SetFloat(Scope, "EmotivLeftCommandTriggerLevel", leftTriggerLevel);

        GLPlayerPrefs.SetFloat(Scope, "EmotivLeftWinkExpressionTriggerLevel", leftWinkTriggerLevel);
        GLPlayerPrefs.SetFloat(Scope, "EmotivRightWinkExpressionTriggerLevel", rightWinkTriggerLevel);
        GLPlayerPrefs.SetFloat(Scope, "EmotivAnyWinkExpressionTriggerLevel", anyWinkTriggerLevel);
        GLPlayerPrefs.SetFloat(Scope, "EmotivSmileExpressionTriggerLevel", smileTriggerLevel);


        GLPlayerPrefs.SetInt(Scope, "EmotivPushCommandActionIndex", pushAssignedActionIndex);
        GLPlayerPrefs.SetInt(Scope, "EmotivPullCommandActionIndex", pullAssignedActionIndex);
        GLPlayerPrefs.SetInt(Scope, "EmotivLiftCommandActionIndex", liftAssignedActionIndex);
        GLPlayerPrefs.SetInt(Scope, "EmotivDropCommandActionIndex", dropAssignedActionIndex);
        GLPlayerPrefs.SetInt(Scope, "EmotivLeftCommandActionIndex", leftAssignedActionIndex);

        GLPlayerPrefs.SetInt(Scope, "EmotivLeftWinkExpressionActionIndex", leftWinkAssignedActionIndex);
        GLPlayerPrefs.SetInt(Scope, "EmotivRightWinkExpressionActionIndex", rightWinkAssignedActionIndex);
        GLPlayerPrefs.SetInt(Scope, "EmotivAnyWinkExpressionActionIndex", anyWinkAssignedActionIndex);
        GLPlayerPrefs.SetInt(Scope, "EmotivSmileExpressionActionIndex", smileAssignedActionIndex);

        GLPlayerPrefs.SetFloat(Scope, "EmotivTickSensibility", ActionManager.Instance.endTime);
    }

    #region Manage actions added

    void CleanEmotivActions()
    {
        for(int i=0; i < ActionManager.Instance.updateActionsVorticesEmotiv.Length; i++)
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[i] = null;
        }
    }

    public void UpdateActionsEmotivInsight()
    {

        // First five slots are for the mental commands
        //
        //
        if (pushAssignedActionIndex != 5)
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[0] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionEmotiv(Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_PUSH, pushTicks, pushMistakes, pushTriggerLevel),
                ActionManager.Instance.vorticesActionList[pushAssignedActionIndex]
                );
        }
        else
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[0] = null;
        }

        if (pullAssignedActionIndex != 5)
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[1] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionEmotiv(Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_PULL, pullTicks, pullMistakes, pullTriggerLevel),
                ActionManager.Instance.vorticesActionList[pullAssignedActionIndex]
                );
        }
        else
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[1] = null;
        }

        if (liftAssignedActionIndex != 5)
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[2] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionEmotiv(Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_LIFT, liftTicks, liftMistakes, liftTriggerLevel),
                ActionManager.Instance.vorticesActionList[liftAssignedActionIndex]
                );
        }
        else
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[2] = null;
        }

        if (dropAssignedActionIndex != 5)
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[3] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionEmotiv(Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_DROP, dropTicks, dropMistakes, dropTriggerLevel),
                ActionManager.Instance.vorticesActionList[dropAssignedActionIndex]
                );
        }
        else
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[3] = null;
        }

        if (leftAssignedActionIndex != 5)
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[4] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionEmotiv(Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_LEFT, leftTicks, leftMistakes, leftTriggerLevel),
                ActionManager.Instance.vorticesActionList[leftAssignedActionIndex]
                );
        }
        else
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[4] = null;
        }

        //last 4 slots are for facial expressions
        //
        //

        if (anyWinkAssignedActionIndex != 5)
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[5] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionEmotiv(Emotiv.EdkDll.IEE_FacialExpressionAlgo_t.FE_BLINK, true, anyWinkTriggerLevel),
                ActionManager.Instance.vorticesActionList[anyWinkAssignedActionIndex]);
        }
        else
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[5] = null;
        }

        if (leftWinkAssignedActionIndex != 5)
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[6] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionEmotiv(Emotiv.EdkDll.IEE_FacialExpressionAlgo_t.FE_WINK_LEFT, true, leftWinkTriggerLevel),
                ActionManager.Instance.vorticesActionList[leftWinkAssignedActionIndex]);
        }
        else
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[6] = null;
        }

        if (rightWinkAssignedActionIndex != 5)
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[7] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionEmotiv(Emotiv.EdkDll.IEE_FacialExpressionAlgo_t.FE_WINK_RIGHT, true, rightWinkTriggerLevel),
                ActionManager.Instance.vorticesActionList[rightWinkAssignedActionIndex]);
        }
        else
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[7] = null;
        }

        if (smileAssignedActionIndex != 5)
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[8] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionEmotiv(Emotiv.EdkDll.IEE_FacialExpressionAlgo_t.FE_SMILE, false, smileTriggerLevel),
                ActionManager.Instance.vorticesActionList[smileAssignedActionIndex]);
        }
        else
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[8] = null;
        }
        Debug.Log("Action asignation completed");
    }

    #endregion

    #region UI triggers

    public void SetTickConfigMenuValues()
    {
        
        ActionManager.Instance.endTime = (tickSensibilityValue.value / 10);
        int aux = (int)tickSensibilityValue.value;
        tickSensibilityText.text = aux.ToString();        
    }

    public void SetMentalCommandConfigMenuValues()
    {
        switch (mentalCommandDropdown.value)
        {
            case 0:
                mentalCommandActionsDropdow.value = pushAssignedActionIndex;
                SetTriggerValues(pushTicks, mentalCommandSensibilityValue, mentalCommandSensibilityText);
                SetTriggerValues(pushMistakes, mentalCommandMistakeValue, mentalCommandMistakeText);
                SetTriggerValues(pushTriggerLevel*10, mentalCommandTriggerLevel, mentalCommandTriggerNumber);
                break;
            case 1:
                mentalCommandActionsDropdow.value = pullAssignedActionIndex;
                SetTriggerValues(pullTicks, mentalCommandSensibilityValue, mentalCommandSensibilityText);
                SetTriggerValues(pullMistakes, mentalCommandMistakeValue, mentalCommandMistakeText);
                SetTriggerValues(pullTriggerLevel*10, mentalCommandTriggerLevel, mentalCommandTriggerNumber);
                break;
            case 2:
                mentalCommandActionsDropdow.value = liftAssignedActionIndex;
                SetTriggerValues(liftTicks, mentalCommandSensibilityValue, mentalCommandSensibilityText);
                SetTriggerValues(liftMistakes, mentalCommandMistakeValue, mentalCommandMistakeText);
                SetTriggerValues(liftTriggerLevel*10, mentalCommandTriggerLevel, mentalCommandTriggerNumber);
                break;
            case 3:
                mentalCommandActionsDropdow.value = dropAssignedActionIndex;
                SetTriggerValues(dropTicks, mentalCommandSensibilityValue, mentalCommandSensibilityText);
                SetTriggerValues(dropMistakes, mentalCommandMistakeValue, mentalCommandMistakeText);
                SetTriggerValues(dropTriggerLevel*10, mentalCommandTriggerLevel, mentalCommandTriggerNumber);
                break;
            case 4:
                mentalCommandActionsDropdow.value = leftAssignedActionIndex;
                SetTriggerValues(leftTicks, mentalCommandSensibilityValue, mentalCommandSensibilityText);
                SetTriggerValues(leftMistakes, mentalCommandMistakeValue, mentalCommandMistakeText);
                SetTriggerValues(leftTriggerLevel*10, mentalCommandTriggerLevel, mentalCommandTriggerNumber);
                break;
        }
        mentalCommandActionsDropdow.RefreshShownValue();
    }

    public void SetFacialExpressionConfigMenuValues()
    {
        switch (facialExpresionDropdown.value)
        {
            case 0:
                facialExpresionActionsDropdown.value = anyWinkAssignedActionIndex;
                SetTriggerValues(anyWinkTriggerLevel * 10, facialExpressionTriggerLevel, facialExpressionTriggerNumber);
                break;
            case 1:
                facialExpresionActionsDropdown.value = leftWinkAssignedActionIndex;
                SetTriggerValues(leftWinkTriggerLevel * 10, facialExpressionTriggerLevel, facialExpressionTriggerNumber);
                break;
            case 2:
                facialExpresionActionsDropdown.value = rightWinkAssignedActionIndex;
                SetTriggerValues(rightWinkTriggerLevel * 10, facialExpressionTriggerLevel, facialExpressionTriggerNumber);
                break;
            case 3:
                facialExpresionActionsDropdown.value = smileAssignedActionIndex;
                SetTriggerValues(smileTriggerLevel * 10, facialExpressionTriggerLevel, facialExpressionTriggerNumber);
                break;
        }
        facialExpresionActionsDropdown.RefreshShownValue();
    }

    public void UpdateMentalCommandActionDropdownValues()
    {
        switch (mentalCommandDropdown.value)
        {
            case 0:
                pushAssignedActionIndex = mentalCommandActionsDropdow.value;
                SetActionNameByIndex(pushAssignedActionText, pushAssignedActionIndex);
                break;
            case 1:
                pullAssignedActionIndex = mentalCommandActionsDropdow.value;
                SetActionNameByIndex(pullAssignedActionText, pullAssignedActionIndex);
                break;
            case 2:
                liftAssignedActionIndex = mentalCommandActionsDropdow.value;
                SetActionNameByIndex(liftAssignedActionText, liftAssignedActionIndex);
                break;
            case 3:
                dropAssignedActionIndex = mentalCommandActionsDropdow.value;
                SetActionNameByIndex(dropAssignedActionText, dropAssignedActionIndex);
                break;
            case 4:
                leftAssignedActionIndex = mentalCommandActionsDropdow.value;
                SetActionNameByIndex(leftAssignedActionText, leftAssignedActionIndex);
                break;
        }
    }

    public void UpdateMentalCommandSensibilityValues()
    {
        switch (mentalCommandDropdown.value)
        {
            case 0:
                UpdateTriggerValues(ref pushTicks, mentalCommandSensibilityValue, mentalCommandSensibilityText);
                break;
            case 1:
                UpdateTriggerValues(ref pullTicks, mentalCommandSensibilityValue, mentalCommandSensibilityText);
                break;
            case 2:
                UpdateTriggerValues(ref liftTicks, mentalCommandSensibilityValue, mentalCommandSensibilityText);
                break;
            case 3:
                UpdateTriggerValues(ref dropTicks, mentalCommandSensibilityValue, mentalCommandSensibilityText);
                break;
            case 4:
                UpdateTriggerValues(ref leftTicks, mentalCommandSensibilityValue, mentalCommandSensibilityText);
                break;
        }
    }

    public void UpdateMentalCommandMistakesValues()
    {
        switch (mentalCommandDropdown.value)
        {
            case 0:
                UpdateTriggerValues(ref pushMistakes, mentalCommandMistakeValue, mentalCommandMistakeText);
                break;
            case 1:
                UpdateTriggerValues(ref pullMistakes, mentalCommandMistakeValue, mentalCommandMistakeText);
                break;
            case 2:
                UpdateTriggerValues(ref liftMistakes, mentalCommandMistakeValue, mentalCommandMistakeText);
                break;
            case 3:
                UpdateTriggerValues(ref dropMistakes, mentalCommandMistakeValue, mentalCommandMistakeText);
                break;
            case 4:
                UpdateTriggerValues(ref leftMistakes, mentalCommandMistakeValue, mentalCommandMistakeText);
                break;
        }
    }

    public void UpdateMentalCommandTriggerValues()
    {
        switch (mentalCommandDropdown.value)
        {
            case 0:
                UpdateTriggerValues(ref pushTriggerLevel, mentalCommandTriggerLevel, mentalCommandTriggerNumber);
                break;
            case 1:
                UpdateTriggerValues(ref pullTriggerLevel, mentalCommandTriggerLevel, mentalCommandTriggerNumber);
                break;
            case 2:
                UpdateTriggerValues(ref liftTriggerLevel, mentalCommandTriggerLevel, mentalCommandTriggerNumber);
                break;
            case 3:
                UpdateTriggerValues(ref dropTriggerLevel, mentalCommandTriggerLevel, mentalCommandTriggerNumber);
                break;
            case 4:
                UpdateTriggerValues(ref leftTriggerLevel, mentalCommandTriggerLevel, mentalCommandTriggerNumber);
                break;
        }
    }

    public void UpdateUpperFacialExpressionTriggerValues()
    {
        switch (facialExpresionDropdown.value)
        {
            case 0:
                UpdateTriggerValues(ref anyWinkTriggerLevel, facialExpressionTriggerLevel, facialExpressionTriggerNumber);
                break;
            case 1:
                UpdateTriggerValues(ref leftWinkTriggerLevel, facialExpressionTriggerLevel, facialExpressionTriggerNumber);
                break;
            case 2:
                UpdateTriggerValues(ref rightWinkTriggerLevel, facialExpressionTriggerLevel, facialExpressionTriggerNumber);
                break;
            case 3:
                UpdateTriggerValues(ref smileTriggerLevel, facialExpressionTriggerLevel, facialExpressionTriggerNumber);
                break;
        }
    }

    public void UpdateFacialExpressionActionDropdownValues()
    {
        switch (facialExpresionDropdown.value)
        {
            case 0:
                anyWinkAssignedActionIndex = facialExpresionActionsDropdown.value;
                SetActionNameByIndex(anyWinkAssignedActionText, anyWinkAssignedActionIndex);
                break;
            case 1:
                leftWinkAssignedActionIndex = facialExpresionActionsDropdown.value;
                SetActionNameByIndex(leftWinkAssignedActionText, leftWinkAssignedActionIndex);
                break;
            case 2:
                rightWinkAssignedActionIndex = facialExpresionActionsDropdown.value;
                SetActionNameByIndex(rightWinkAssignedActionText, rightWinkAssignedActionIndex);
                break;
            case 3:
                smileAssignedActionIndex = facialExpresionActionsDropdown.value;
                SetActionNameByIndex(smileAssignedActionText, smileAssignedActionIndex);
                break;
        }
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
        switch (index)
        {
            case 0:
                text.text = "Select/Deselect image";
                break;
            case 1:
                text.text = "Change to next plane";
                break;
            case 2:
                text.text = "Change to previous plane";
                break;
            case 3:
                text.text = "Zoom in image";
                break;
            case 4:
                text.text = "Zoom out image";
                break;
            case 5:
                text.text = "No action assigned";
                break;
        }
    }

#endregion
}
