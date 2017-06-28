using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gamelogic;

public class KinectCommandConfigMenu : MonoBehaviour {
    
    public Dropdown basicGesturesActionsDropdown, basicGesturesDropdown, dbGesturesActionsDropdown, dbGesturesDropdown;
    public Slider dbGestureTriggerLevel, dbGestureUntriggerLevel;
    float gesture1TriggerLevel, gesture2TriggerLevel, gesture3TriggerLevel, gesture4TriggerLevel, gesture5TriggerLevel, gesture6TriggerLevel, gesture7TriggerLevel, gesture1UntriggerLevel, gesture2UntriggerLevel, gesture3UntriggerLevel, gesture4UntriggerLevel, gesture5UntriggerLevel, gesture6UntriggerLevel, gesture7UntriggerLevel;
    int openHandRightAssignedActionIndex, openHandLeftAssignedActionIndex, closeHandRightAssignedActionIndex, closeHandLeftAssignedActionIndex, lassoHandRightAssignedActionIndex, lassoHandLeftAssignedActionIndex, gesture1AssignedActionIndex, gesture2AssignedActionIndex, gesture3AssignedActionIndex, gesture4AssignedActionIndex, gesture5AssignedActionIndex, gesture6AssignedActionIndex, gesture7AssignedActionIndex;
    public Text dbGestureTriggerNumber, dbGestureUntriggerNumber;
    public Text openHandRightAssignedActionText, openHandLeftAssignedActionText, closeHandRightAssignedActionText, closeHandLeftAssignedActionText, lassoHandRightAssignedActionText, lassoHandLeftAssignedActionText, gesture1AssignedActionText, gesture2AssignedActionText, gesture3AssignedActionText, gesture4AssignedActionText, gesture5AssignedActionText, gesture6AssignedActionText, gesture7AssignedActionText;
    string Scope;

    void OnEnable()
    {
        Scope = ProfileManager.Instance.currentProfileScope;


        gesture1TriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture1TriggerLevel");
        gesture2TriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture2TriggerLevel");
        gesture3TriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture3TriggerLevel");
        gesture4TriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture4TriggerLevel");
        gesture5TriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture5TriggerLevel");
        gesture6TriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture6TriggerLevel");
        gesture7TriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture7TriggerLevel");
        gesture1UntriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture1UntriggerLevel");
        gesture2UntriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture2UntriggerLevel");
        gesture3UntriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture3UntriggerLevel");
        gesture4UntriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture4UntriggerLevel");
        gesture5UntriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture5UntriggerLevel");
        gesture6UntriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture6UntriggerLevel");
        gesture7UntriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture7UntriggerLevel");

        openHandRightAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "openHandRightAssignedActionIndex");
        openHandLeftAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "openHandLeftAssignedActionIndex");
        closeHandRightAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "closeHandRightAssignedActionIndex");
        closeHandLeftAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "closeHandLeftAssignedActionIndex");
        lassoHandRightAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "lassoHandRightAssignedActionIndex");
        lassoHandLeftAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "lassoHandLeftAssignedActionIndex");
        gesture1AssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "gesture1AssignedActionIndex");
        gesture2AssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "gesture2AssignedActionIndex");
        gesture3AssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "gesture3AssignedActionIndex");
        gesture4AssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "gesture4AssignedActionIndex");
        gesture5AssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "gesture5AssignedActionIndex");
        gesture6AssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "gesture6AssignedActionIndex");
        gesture7AssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "gesture7AssignedActionIndex");

        SetActionNameByIndex(gesture1AssignedActionText, gesture1AssignedActionIndex);
        SetActionNameByIndex(gesture2AssignedActionText, gesture2AssignedActionIndex);
        SetActionNameByIndex(gesture3AssignedActionText, gesture3AssignedActionIndex);
        SetActionNameByIndex(gesture4AssignedActionText, gesture4AssignedActionIndex);
        SetActionNameByIndex(gesture5AssignedActionText, gesture5AssignedActionIndex);
        SetActionNameByIndex(gesture6AssignedActionText, gesture6AssignedActionIndex);
        SetActionNameByIndex(gesture7AssignedActionText, gesture7AssignedActionIndex);

        SetActionNameByIndex(openHandRightAssignedActionText, openHandRightAssignedActionIndex);
        SetActionNameByIndex(openHandLeftAssignedActionText, openHandLeftAssignedActionIndex);
        SetActionNameByIndex(closeHandRightAssignedActionText, closeHandRightAssignedActionIndex);
        SetActionNameByIndex(closeHandLeftAssignedActionText, closeHandLeftAssignedActionIndex);
        SetActionNameByIndex(lassoHandRightAssignedActionText, lassoHandRightAssignedActionIndex);
        SetActionNameByIndex(lassoHandLeftAssignedActionText, lassoHandLeftAssignedActionIndex);

        CleanKinectActions();
        SetBasicGesturesConfigMenuValues();
        SetDbGesturesConfigMenuValues();
    }

    private void OnDisable()
    {
        UpdateActionsEmotivInsight();

        GLPlayerPrefs.SetFloat(Scope, "gesture1TriggerLevel", gesture1TriggerLevel);
        GLPlayerPrefs.SetFloat(Scope, "gesture2TriggerLevel", gesture2TriggerLevel);
        GLPlayerPrefs.SetFloat(Scope, "gesture3TriggerLevel", gesture3TriggerLevel);
        GLPlayerPrefs.SetFloat(Scope, "gesture4TriggerLevel", gesture4TriggerLevel);
        GLPlayerPrefs.SetFloat(Scope, "gesture5TriggerLevel", gesture5TriggerLevel);
        GLPlayerPrefs.SetFloat(Scope, "gesture6TriggerLevel", gesture6TriggerLevel);
        GLPlayerPrefs.SetFloat(Scope, "gesture7TriggerLevel", gesture7TriggerLevel);

        GLPlayerPrefs.SetFloat(Scope, "gesture1UntriggerLevel", gesture1UntriggerLevel);
        GLPlayerPrefs.SetFloat(Scope, "gesture2UntriggerLevel", gesture2UntriggerLevel);
        GLPlayerPrefs.SetFloat(Scope, "gesture3UntriggerLevel", gesture3UntriggerLevel);
        GLPlayerPrefs.SetFloat(Scope, "gesture4UntriggerLevel", gesture4UntriggerLevel);
        GLPlayerPrefs.SetFloat(Scope, "gesture5UntriggerLevel", gesture5UntriggerLevel);
        GLPlayerPrefs.SetFloat(Scope, "gesture6UntriggerLevel", gesture6UntriggerLevel);
        GLPlayerPrefs.SetFloat(Scope, "gesture7UntriggerLevel", gesture7UntriggerLevel);

        GLPlayerPrefs.SetInt(Scope, "openHandRightAssignedActionIndex", openHandRightAssignedActionIndex);
        GLPlayerPrefs.SetInt(Scope, "openHandLeftAssignedActionIndex", openHandLeftAssignedActionIndex);
        GLPlayerPrefs.SetInt(Scope, "closeHandRightAssignedActionIndex", closeHandRightAssignedActionIndex);
        GLPlayerPrefs.SetInt(Scope, "closeHandLeftAssignedActionIndex", closeHandLeftAssignedActionIndex);
        GLPlayerPrefs.SetInt(Scope, "lassoHandRightAssignedActionIndex", lassoHandRightAssignedActionIndex);
        GLPlayerPrefs.SetInt(Scope, "lassoHandLeftAssignedActionIndex", lassoHandLeftAssignedActionIndex);

        GLPlayerPrefs.SetInt(Scope, "gesture1AssignedActionIndex", gesture1AssignedActionIndex);
        GLPlayerPrefs.SetInt(Scope, "gesture2AssignedActionIndex", gesture2AssignedActionIndex);
        GLPlayerPrefs.SetInt(Scope, "gesture3AssignedActionIndex", gesture3AssignedActionIndex);
        GLPlayerPrefs.SetInt(Scope, "gesture4AssignedActionIndex", gesture4AssignedActionIndex);
        GLPlayerPrefs.SetInt(Scope, "gesture5AssignedActionIndex", gesture5AssignedActionIndex);
        GLPlayerPrefs.SetInt(Scope, "gesture6AssignedActionIndex", gesture6AssignedActionIndex);
        GLPlayerPrefs.SetInt(Scope, "gesture7AssignedActionIndex", gesture7AssignedActionIndex);

    }

    #region Manage actions added

    void CleanKinectActions()
    {
        for (int i = 0; i < ActionManager.Instance.updateActionsKinectGestures.Length; i++)
        {
            ActionManager.Instance.updateActionsKinectGestures[i] = null;
        }
    }

    public void UpdateActionsEmotivInsight()
    {

        // First five slots are for the mental commands
        //
        //
        if (openHandRightAssignedActionIndex != 8)
        {
            ActionManager.Instance.updateActionsKinectGestures[0] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionKinect(Windows.Kinect.HandState.Open, true),
                ActionManager.Instance.bgiiesActionList[openHandRightAssignedActionIndex]
                );
        }
        else
        {
            ActionManager.Instance.updateActionsKinectGestures[0] = null;
        }

        if (openHandLeftAssignedActionIndex != 8)
        {
            ActionManager.Instance.updateActionsKinectGestures[1] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionKinect(Windows.Kinect.HandState.Open, false),
                ActionManager.Instance.bgiiesActionList[openHandLeftAssignedActionIndex]
                );
        }
        else
        {
            ActionManager.Instance.updateActionsKinectGestures[1] = null;
        }

        if (closeHandRightAssignedActionIndex != 8)
        {
            ActionManager.Instance.updateActionsKinectGestures[2] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionKinect(Windows.Kinect.HandState.Closed, true),
                ActionManager.Instance.bgiiesActionList[closeHandRightAssignedActionIndex]
                );
        }
        else
        {
            ActionManager.Instance.updateActionsKinectGestures[2] = null;
        }

        if (closeHandLeftAssignedActionIndex != 8)
        {
            ActionManager.Instance.updateActionsKinectGestures[3] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionKinect(Windows.Kinect.HandState.Closed, false),
                ActionManager.Instance.bgiiesActionList[closeHandLeftAssignedActionIndex]
                );
        }
        else
        {
            ActionManager.Instance.updateActionsKinectGestures[3] = null;
        }

        if (lassoHandRightAssignedActionIndex != 8)
        {
            ActionManager.Instance.updateActionsKinectGestures[4] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionKinect(Windows.Kinect.HandState.Lasso, true),
                ActionManager.Instance.bgiiesActionList[lassoHandRightAssignedActionIndex]
                );
        }
        else
        {
            ActionManager.Instance.updateActionsKinectGestures[4] = null;
        }

        if (lassoHandLeftAssignedActionIndex != 8)
        {
            ActionManager.Instance.updateActionsKinectGestures[5] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionKinect(Windows.Kinect.HandState.Lasso, false),
                ActionManager.Instance.bgiiesActionList[lassoHandLeftAssignedActionIndex]
                );
        }
        else
        {
            ActionManager.Instance.updateActionsKinectGestures[5] = null;
        }

        Debug.Log("Action asignation completed");
    }

    #endregion

    #region UI triggers

    public void SetDbGesturesConfigMenuValues()
    {
        switch (dbGesturesDropdown.value)
        {
            case 0:
                dbGesturesActionsDropdown.value = gesture1AssignedActionIndex;
                SetTriggerValues(gesture1TriggerLevel * 10, dbGestureTriggerLevel, dbGestureTriggerNumber);
                SetTriggerValues(gesture1UntriggerLevel * 10, dbGestureUntriggerLevel, dbGestureUntriggerNumber);
                break;
            case 1:
                dbGesturesActionsDropdown.value = gesture2AssignedActionIndex;
                SetTriggerValues(gesture2TriggerLevel * 10, dbGestureTriggerLevel, dbGestureTriggerNumber);
                SetTriggerValues(gesture2UntriggerLevel * 10, dbGestureUntriggerLevel, dbGestureUntriggerNumber);
                break;
            case 2:
                dbGesturesActionsDropdown.value = gesture3AssignedActionIndex;
                SetTriggerValues(gesture3TriggerLevel * 10, dbGestureTriggerLevel, dbGestureTriggerNumber);
                SetTriggerValues(gesture3UntriggerLevel * 10, dbGestureUntriggerLevel, dbGestureUntriggerNumber);
                break;
            case 3:
                dbGesturesActionsDropdown.value = gesture4AssignedActionIndex;
                SetTriggerValues(gesture4TriggerLevel * 10, dbGestureTriggerLevel, dbGestureTriggerNumber);
                SetTriggerValues(gesture4UntriggerLevel * 10, dbGestureUntriggerLevel, dbGestureUntriggerNumber);
                break;
            case 4:
                dbGesturesActionsDropdown.value = gesture5AssignedActionIndex;
                SetTriggerValues(gesture5TriggerLevel * 10, dbGestureTriggerLevel, dbGestureTriggerNumber);
                SetTriggerValues(gesture5UntriggerLevel * 10, dbGestureUntriggerLevel, dbGestureUntriggerNumber);
                break;
            case 5:
                dbGesturesActionsDropdown.value = gesture6AssignedActionIndex;
                SetTriggerValues(gesture6TriggerLevel * 10, dbGestureTriggerLevel, dbGestureTriggerNumber);
                SetTriggerValues(gesture6UntriggerLevel * 10, dbGestureUntriggerLevel, dbGestureUntriggerNumber);
                break;
            case 6:
                dbGesturesActionsDropdown.value = gesture7AssignedActionIndex;
                SetTriggerValues(gesture7TriggerLevel * 10, dbGestureTriggerLevel, dbGestureTriggerNumber);
                SetTriggerValues(gesture7UntriggerLevel * 10, dbGestureUntriggerLevel, dbGestureUntriggerNumber);
                break;
        }
        dbGesturesActionsDropdown.RefreshShownValue();
    }

    public void SetBasicGesturesConfigMenuValues()
    {
        switch (basicGesturesDropdown.value)
        {
            case 0:
                basicGesturesActionsDropdown.value = openHandRightAssignedActionIndex;
                SetActionNameByIndex(openHandRightAssignedActionText, openHandRightAssignedActionIndex);
                break;
            case 1:
                basicGesturesActionsDropdown.value = openHandLeftAssignedActionIndex;
                SetActionNameByIndex(openHandLeftAssignedActionText, openHandLeftAssignedActionIndex);
                break;
            case 2:
                basicGesturesActionsDropdown.value = closeHandRightAssignedActionIndex;
                SetActionNameByIndex(closeHandRightAssignedActionText, closeHandRightAssignedActionIndex);
                break;
            case 3:
                basicGesturesActionsDropdown.value = closeHandLeftAssignedActionIndex;
                SetActionNameByIndex(closeHandLeftAssignedActionText, closeHandLeftAssignedActionIndex);
                break;
            case 4:
                basicGesturesActionsDropdown.value = lassoHandRightAssignedActionIndex;
                SetActionNameByIndex(lassoHandRightAssignedActionText, lassoHandRightAssignedActionIndex);
                break;
            case 5:
                basicGesturesActionsDropdown.value = lassoHandLeftAssignedActionIndex;
                SetActionNameByIndex(lassoHandLeftAssignedActionText, lassoHandLeftAssignedActionIndex);
                break;
        }
        basicGesturesActionsDropdown.RefreshShownValue();
    }

    public void UpdateDbGesturesActionDropdownValues()
    {
        switch (dbGesturesDropdown.value)
        {
            case 0:
                gesture1AssignedActionIndex = dbGesturesActionsDropdown.value;
                SetActionNameByIndex(gesture1AssignedActionText, gesture1AssignedActionIndex);
                break;
            case 1:
                gesture2AssignedActionIndex = dbGesturesActionsDropdown.value;
                SetActionNameByIndex(gesture2AssignedActionText, gesture2AssignedActionIndex);
                break;
            case 2:
                gesture3AssignedActionIndex = dbGesturesActionsDropdown.value;
                SetActionNameByIndex(gesture3AssignedActionText, gesture3AssignedActionIndex);
                break;
            case 3:
                gesture4AssignedActionIndex = dbGesturesActionsDropdown.value;
                SetActionNameByIndex(gesture4AssignedActionText, gesture4AssignedActionIndex);
                break;
            case 4:
                gesture5AssignedActionIndex = dbGesturesActionsDropdown.value;
                SetActionNameByIndex(gesture5AssignedActionText, gesture5AssignedActionIndex);
                break;
            case 5:
                gesture6AssignedActionIndex = dbGesturesActionsDropdown.value;
                SetActionNameByIndex(gesture6AssignedActionText, gesture6AssignedActionIndex);
                break;
            case 6:
                gesture7AssignedActionIndex = dbGesturesActionsDropdown.value;
                SetActionNameByIndex(gesture7AssignedActionText, gesture7AssignedActionIndex);
                break;
        }
    }

    public void UpdateDbGestureTriggerValues()
    {
        switch (dbGesturesDropdown.value)
        {
            case 0:
                UpdateTriggerValues(ref gesture1TriggerLevel, dbGestureTriggerLevel, dbGestureTriggerNumber);
                break;
            case 1:
                UpdateTriggerValues(ref gesture2TriggerLevel, dbGestureTriggerLevel, dbGestureTriggerNumber);
                break;
            case 2:
                UpdateTriggerValues(ref gesture3TriggerLevel, dbGestureTriggerLevel, dbGestureTriggerNumber);
                break;
            case 3:
                UpdateTriggerValues(ref gesture4TriggerLevel, dbGestureTriggerLevel, dbGestureTriggerNumber);
                break;
            case 4:
                UpdateTriggerValues(ref gesture5TriggerLevel, dbGestureTriggerLevel, dbGestureTriggerNumber);
                break;
            case 5:
                UpdateTriggerValues(ref gesture6TriggerLevel, dbGestureTriggerLevel, dbGestureTriggerNumber);
                break;
            case 6:
                UpdateTriggerValues(ref gesture7TriggerLevel, dbGestureTriggerLevel, dbGestureTriggerNumber);
                break;
        }
    }

    public void UpdateDbGestureUntriggerValues()
    {
        switch (dbGesturesDropdown.value)
        {
            case 0:
                UpdateTriggerValues(ref gesture1UntriggerLevel, dbGestureUntriggerLevel, dbGestureUntriggerNumber);
                break;
            case 1:
                UpdateTriggerValues(ref gesture2UntriggerLevel, dbGestureUntriggerLevel, dbGestureUntriggerNumber);
                break;
            case 2:
                UpdateTriggerValues(ref gesture3UntriggerLevel, dbGestureUntriggerLevel, dbGestureUntriggerNumber);
                break;
            case 3:
                UpdateTriggerValues(ref gesture4UntriggerLevel, dbGestureUntriggerLevel, dbGestureUntriggerNumber);
                break;
            case 4:
                UpdateTriggerValues(ref gesture5UntriggerLevel, dbGestureUntriggerLevel, dbGestureUntriggerNumber);
                break;
            case 5:
                UpdateTriggerValues(ref gesture6UntriggerLevel, dbGestureUntriggerLevel, dbGestureUntriggerNumber);
                break;
            case 6:
                UpdateTriggerValues(ref gesture7UntriggerLevel, dbGestureUntriggerLevel, dbGestureUntriggerNumber);
                break;
        }
    }


    public void UpdateBasicGesturesActionDropdownValues()
    {
        switch (basicGesturesDropdown.value)
        {
            case 0:
                openHandRightAssignedActionIndex = basicGesturesActionsDropdown.value;
                SetActionNameByIndex(openHandRightAssignedActionText, openHandRightAssignedActionIndex);
                break;
            case 1:
                openHandLeftAssignedActionIndex = basicGesturesActionsDropdown.value;
                SetActionNameByIndex(openHandLeftAssignedActionText, openHandLeftAssignedActionIndex);
                break;
            case 2:
                closeHandRightAssignedActionIndex = basicGesturesActionsDropdown.value;
                SetActionNameByIndex(closeHandRightAssignedActionText, closeHandRightAssignedActionIndex);
                break;
            case 3:
                closeHandLeftAssignedActionIndex = basicGesturesActionsDropdown.value;
                SetActionNameByIndex(closeHandLeftAssignedActionText, closeHandLeftAssignedActionIndex);
                break;
            case 4:
                lassoHandRightAssignedActionIndex = basicGesturesActionsDropdown.value;
                SetActionNameByIndex(lassoHandRightAssignedActionText, lassoHandRightAssignedActionIndex);
                break;
            case 5:
                lassoHandLeftAssignedActionIndex = basicGesturesActionsDropdown.value;
                SetActionNameByIndex(lassoHandLeftAssignedActionText, lassoHandLeftAssignedActionIndex);
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
        trigger = (slider.value / 10);
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

    void SetActionNameByIndex(Text text, int index)
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
