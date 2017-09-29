using Gamelogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotivMappingLoader : MonoBehaviour {

    string interfaceName = "Emotiv";

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

    private void OnEnable()
    {
        LoadActions();
    }

    public void LoadActions()
    {
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        float triggerLevel;
        int aux, ticks, mistakes, actionIndex;
        for (int j = 0; j < mentalCommandName.Length; j++)
        {
            ticks = GLPlayerPrefs.GetInt(Scope, "Emotiv" + mentalCommandName[j] + "CommandTicks");
            mistakes = GLPlayerPrefs.GetInt(Scope, "Emotiv" + mentalCommandName[j] + "CommandMistakes");
            triggerLevel = GLPlayerPrefs.GetFloat(Scope, "Emotiv" + mentalCommandName[j] + "CommandTriggerLevel");
            actionIndex = ActionManager.Instance.GetMappedActionIndex(interfaceName, mentalCommandName[j]);
            ActionManager.Instance.updateActionsEmotivInsight[j] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionEmotiv(mentalCommandCode[j], ticks, mistakes, triggerLevel),
                ActionManager.Instance.currentActionList[actionIndex]);
        }

        for (int i = 0; i < facialExpresionName.Length; i++)
        {
            triggerLevel = GLPlayerPrefs.GetFloat(Scope, "Emotiv" + facialExpresionName[i] + "TriggerLevel");
            aux = i + mentalCommandName.Length;
            actionIndex = ActionManager.Instance.GetMappedActionIndex(interfaceName, facialExpresionName[i]);
            ActionManager.Instance.updateActionsEmotivInsight[aux] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionEmotiv(facialExpresionCode[i], facilExpresionIsUpperFace[i], triggerLevel),
                ActionManager.Instance.currentActionList[actionIndex]);
        }
    }


}
