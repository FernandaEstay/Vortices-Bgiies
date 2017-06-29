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

        if (ActionManager.Instance.bgiiesMode)
        {

            pushTicks = GLPlayerPrefs.GetInt(Scope, "EmotivPushCommandTicksBgiies");
            pullTicks = GLPlayerPrefs.GetInt(Scope, "EmotivPullCommandTicksBgiies");
            liftTicks = GLPlayerPrefs.GetInt(Scope, "EmotivLiftCommandTicksBgiies");
            dropTicks = GLPlayerPrefs.GetInt(Scope, "EmotivDropCommandTicksBgiies");
            leftTicks = GLPlayerPrefs.GetInt(Scope, "EmotivLeftCommandTicksBgiies");

            pushMistakes = GLPlayerPrefs.GetInt(Scope, "EmotivPushCommandMistakesBgiies");
            pullMistakes = GLPlayerPrefs.GetInt(Scope, "EmotivPullCommandMistakesBgiies");
            liftMistakes = GLPlayerPrefs.GetInt(Scope, "EmotivLiftCommandMistakesBgiies");
            dropMistakes = GLPlayerPrefs.GetInt(Scope, "EmotivDropCommandMistakesBgiies");
            leftMistakes = GLPlayerPrefs.GetInt(Scope, "EmotivLeftCommandMistakesBgiies");

            pushTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivPushCommandTriggerLevelBgiies");
            pullTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivPullCommandTriggerLevelBgiies");
            liftTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivLiftCommandTriggerLevelBgiies");
            dropTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivDropCommandTriggerLevelBgiies");
            leftTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivLeftCommandTriggerLevelBgiies");

            leftWinkTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivLeftWinkExpressionTriggerLevelBgiies");
            rightWinkTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivRightWinkExpressionTriggerLevelBgiies");
            anyWinkTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivAnyWinkExpressionTriggerLevelBgiies");
            smileTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivSmileExpressionTriggerLevelBgiies");


            pushAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "EmotivPushCommandActionIndexBgiies");
            pullAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "EmotivPullCommandActionIndexBgiies");
            liftAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "EmotivLiftCommandActionIndexBgiies");
            dropAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "EmotivDropCommandActionIndexBgiies");
            leftAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "EmotivLeftCommandActionIndexBgiies");

            leftWinkAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "EmotivLeftWinkExpressionActionIndexBgiies");
            rightWinkAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "EmotivRightWinkExpressionActionIndexBgiies");
            anyWinkAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "EmotivAnyWinkExpressionActionIndexBgiies");
            smileAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "EmotivSmileExpressionActionIndexBgiies");

            ActionManager.Instance.endTime = GLPlayerPrefs.GetFloat(Scope, "EmotivTickSensibilityBgiies");
        }
        else
        {

            pushTicks = GLPlayerPrefs.GetInt(Scope, "EmotivPushCommandTicksVortices");
            pullTicks = GLPlayerPrefs.GetInt(Scope, "EmotivPullCommandTicksVortices");
            liftTicks = GLPlayerPrefs.GetInt(Scope, "EmotivLiftCommandTicksVortices");
            dropTicks = GLPlayerPrefs.GetInt(Scope, "EmotivDropCommandTicksVortices");
            leftTicks = GLPlayerPrefs.GetInt(Scope, "EmotivLeftCommandTicksVortices");

            pushMistakes = GLPlayerPrefs.GetInt(Scope, "EmotivPushCommandMistakesVortices");
            pullMistakes = GLPlayerPrefs.GetInt(Scope, "EmotivPullCommandMistakesVortices");
            liftMistakes = GLPlayerPrefs.GetInt(Scope, "EmotivLiftCommandMistakesVortices");
            dropMistakes = GLPlayerPrefs.GetInt(Scope, "EmotivDropCommandMistakesVortices");
            leftMistakes = GLPlayerPrefs.GetInt(Scope, "EmotivLeftCommandMistakesVortices");

            pushTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivPushCommandTriggerLevelVortices");
            pullTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivPullCommandTriggerLevelVortices");
            liftTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivLiftCommandTriggerLevelVortices");
            dropTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivDropCommandTriggerLevelVortices");
            leftTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivLeftCommandTriggerLevelVortices");

            leftWinkTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivLeftWinkExpressionTriggerLevelVortices");
            rightWinkTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivRightWinkExpressionTriggerLevelVortices");
            anyWinkTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivAnyWinkExpressionTriggerLevelVortices");
            smileTriggerLevel = GLPlayerPrefs.GetFloat(Scope, "EmotivSmileExpressionTriggerLevelVortices");


            pushAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "EmotivPushCommandActionIndexVortices");
            pullAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "EmotivPullCommandActionIndexVortices");
            liftAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "EmotivLiftCommandActionIndexVortices");
            dropAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "EmotivDropCommandActionIndexVortices");
            leftAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "EmotivLeftCommandActionIndexVortices");

            leftWinkAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "EmotivLeftWinkExpressionActionIndexVortices");
            rightWinkAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "EmotivRightWinkExpressionActionIndexVortices");
            anyWinkAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "EmotivAnyWinkExpressionActionIndexVortices");
            smileAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "EmotivSmileExpressionActionIndexVortices");

            ActionManager.Instance.endTime = GLPlayerPrefs.GetFloat(Scope, "EmotivTickSensibilityVortices");
        }

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
        ActionManager.Instance.ReloadProfileDropdown(facialExpresionActionsDropdown);
        ActionManager.Instance.ReloadProfileDropdown(mentalCommandActionsDropdow);
    }

    private void OnDisable()
    {
        UpdateActionsEmotivInsight();
        if (ActionManager.Instance.bgiiesMode)
        {
            GLPlayerPrefs.SetInt(Scope, "EmotivPushCommandTicksBgiies", pushTicks);
            GLPlayerPrefs.SetInt(Scope, "EmotivPullCommandTicksBgiies", pullTicks);
            GLPlayerPrefs.SetInt(Scope, "EmotivLiftCommandTicksBgiies", liftTicks);
            GLPlayerPrefs.SetInt(Scope, "EmotivDropCommandTicksBgiies", dropTicks);
            GLPlayerPrefs.SetInt(Scope, "EmotivLeftCommandTicksBgiies", leftTicks);

            GLPlayerPrefs.SetInt(Scope, "EmotivPushCommandMistakesBgiies", pushMistakes);
            GLPlayerPrefs.SetInt(Scope, "EmotivPullCommandMistakesBgiies", pullMistakes);
            GLPlayerPrefs.SetInt(Scope, "EmotivLiftCommandMistakesBgiies", liftMistakes);
            GLPlayerPrefs.SetInt(Scope, "EmotivDropCommandMistakesBgiies", dropMistakes);
            GLPlayerPrefs.SetInt(Scope, "EmotivLeftCommandMistakesBgiies", leftMistakes);

            GLPlayerPrefs.SetFloat(Scope, "EmotivPushCommandTriggerLevelBgiies", pushTriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "EmotivPullCommandTriggerLevelBgiies", pullTriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "EmotivLiftCommandTriggerLevelBgiies", liftTriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "EmotivDropCommandTriggerLevelBgiies", dropTriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "EmotivLeftCommandTriggerLevelBgiies", leftTriggerLevel);

            GLPlayerPrefs.SetFloat(Scope, "EmotivLeftWinkExpressionTriggerLevelBgiies", leftWinkTriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "EmotivRightWinkExpressionTriggerLevelBgiies", rightWinkTriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "EmotivAnyWinkExpressionTriggerLevelBgiies", anyWinkTriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "EmotivSmileExpressionTriggerLevelBgiies", smileTriggerLevel);


            GLPlayerPrefs.SetInt(Scope, "EmotivPushCommandActionIndexBgiies", pushAssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "EmotivPullCommandActionIndexBgiies", pullAssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "EmotivLiftCommandActionIndexBgiies", liftAssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "EmotivDropCommandActionIndexBgiies", dropAssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "EmotivLeftCommandActionIndexBgiies", leftAssignedActionIndex);

            GLPlayerPrefs.SetInt(Scope, "EmotivLeftWinkExpressionActionIndexBgiies", leftWinkAssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "EmotivRightWinkExpressionActionIndexBgiies", rightWinkAssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "EmotivAnyWinkExpressionActionIndexBgiies", anyWinkAssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "EmotivSmileExpressionActionIndexBgiies", smileAssignedActionIndex);

            GLPlayerPrefs.SetFloat(Scope, "EmotivTickSensibilityBgiies", ActionManager.Instance.endTime);
        }
        else
        {
            GLPlayerPrefs.SetInt(Scope, "EmotivPushCommandTicksVortices", pushTicks);
            GLPlayerPrefs.SetInt(Scope, "EmotivPullCommandTicksVortices", pullTicks);
            GLPlayerPrefs.SetInt(Scope, "EmotivLiftCommandTicksVortices", liftTicks);
            GLPlayerPrefs.SetInt(Scope, "EmotivDropCommandTicksVortices", dropTicks);
            GLPlayerPrefs.SetInt(Scope, "EmotivLeftCommandTicksVortices", leftTicks);

            GLPlayerPrefs.SetInt(Scope, "EmotivPushCommandMistakesVortices", pushMistakes);
            GLPlayerPrefs.SetInt(Scope, "EmotivPullCommandMistakesVortices", pullMistakes);
            GLPlayerPrefs.SetInt(Scope, "EmotivLiftCommandMistakesVortices", liftMistakes);
            GLPlayerPrefs.SetInt(Scope, "EmotivDropCommandMistakesVortices", dropMistakes);
            GLPlayerPrefs.SetInt(Scope, "EmotivLeftCommandMistakesVortices", leftMistakes);

            GLPlayerPrefs.SetFloat(Scope, "EmotivPushCommandTriggerLevelVortices", pushTriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "EmotivPullCommandTriggerLevelVortices", pullTriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "EmotivLiftCommandTriggerLevelVortices", liftTriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "EmotivDropCommandTriggerLevelVortices", dropTriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "EmotivLeftCommandTriggerLevelVortices", leftTriggerLevel);

            GLPlayerPrefs.SetFloat(Scope, "EmotivLeftWinkExpressionTriggerLevelVortices", leftWinkTriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "EmotivRightWinkExpressionTriggerLevelVortices", rightWinkTriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "EmotivAnyWinkExpressionTriggerLevelVortices", anyWinkTriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "EmotivSmileExpressionTriggerLevelVortices", smileTriggerLevel);


            GLPlayerPrefs.SetInt(Scope, "EmotivPushCommandActionIndexVortices", pushAssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "EmotivPullCommandActionIndexVortices", pullAssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "EmotivLiftCommandActionIndexVortices", liftAssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "EmotivDropCommandActionIndexVortices", dropAssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "EmotivLeftCommandActionIndexVortices", leftAssignedActionIndex);

            GLPlayerPrefs.SetInt(Scope, "EmotivLeftWinkExpressionActionIndexVortices", leftWinkAssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "EmotivRightWinkExpressionActionIndexVortices", rightWinkAssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "EmotivAnyWinkExpressionActionIndexVortices", anyWinkAssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "EmotivSmileExpressionActionIndexVortices", smileAssignedActionIndex);

            GLPlayerPrefs.SetFloat(Scope, "EmotivTickSensibilityVortices", ActionManager.Instance.endTime);
        }
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
        if (pushAssignedActionIndex != 0)
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[0] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionEmotiv(Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_PUSH, pushTicks, pushMistakes, pushTriggerLevel),
                ActionManager.Instance.currentActionList[pushAssignedActionIndex]
                );
        }
        else
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[0] = null;
        }

        if (pullAssignedActionIndex != 0)
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[1] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionEmotiv(Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_PULL, pullTicks, pullMistakes, pullTriggerLevel),
                ActionManager.Instance.currentActionList[pullAssignedActionIndex]
                );
        }
        else
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[1] = null;
        }

        if (liftAssignedActionIndex != 0)
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[2] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionEmotiv(Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_LIFT, liftTicks, liftMistakes, liftTriggerLevel),
                ActionManager.Instance.currentActionList[liftAssignedActionIndex]
                );
        }
        else
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[2] = null;
        }

        if (dropAssignedActionIndex != 0)
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[3] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionEmotiv(Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_DROP, dropTicks, dropMistakes, dropTriggerLevel),
                ActionManager.Instance.currentActionList[dropAssignedActionIndex]
                );
        }
        else
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[3] = null;
        }

        if (leftAssignedActionIndex != 0)
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[4] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionEmotiv(Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_LEFT, leftTicks, leftMistakes, leftTriggerLevel),
                ActionManager.Instance.currentActionList[leftAssignedActionIndex]
                );
        }
        else
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[4] = null;
        }

        //last 4 slots are for facial expressions
        //
        //

        if (anyWinkAssignedActionIndex != 0)
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[5] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionEmotiv(Emotiv.EdkDll.IEE_FacialExpressionAlgo_t.FE_BLINK, true, anyWinkTriggerLevel),
                ActionManager.Instance.currentActionList[anyWinkAssignedActionIndex]);
        }
        else
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[5] = null;
        }

        if (leftWinkAssignedActionIndex != 0)
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[6] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionEmotiv(Emotiv.EdkDll.IEE_FacialExpressionAlgo_t.FE_WINK_LEFT, true, leftWinkTriggerLevel),
                ActionManager.Instance.currentActionList[leftWinkAssignedActionIndex]);
        }
        else
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[6] = null;
        }

        if (rightWinkAssignedActionIndex != 0)
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[7] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionEmotiv(Emotiv.EdkDll.IEE_FacialExpressionAlgo_t.FE_WINK_RIGHT, true, rightWinkTriggerLevel),
                ActionManager.Instance.currentActionList[rightWinkAssignedActionIndex]);
        }
        else
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[7] = null;
        }

        if (smileAssignedActionIndex != 0)
        {
            ActionManager.Instance.updateActionsVorticesEmotiv[8] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionEmotiv(Emotiv.EdkDll.IEE_FacialExpressionAlgo_t.FE_SMILE, false, smileTriggerLevel),
                ActionManager.Instance.currentActionList[smileAssignedActionIndex]);
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
