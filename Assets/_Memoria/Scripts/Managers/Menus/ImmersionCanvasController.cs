using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gamelogic;

public class ImmersionCanvasController : MonoBehaviour {

    public Slider   visualSlider, auditiveSlider;
    public Text     visualText, auditiveText;
    public Dropdown sceneSelector;

    string scope;
    string currentVisualization;
    string currentObject;
    string[] sceneName = new string[]
    {
        "Sci-Fi Scene",
        "Farm Scene"
    };

    private void OnEnable()
    {
        scope = ProfileManager.Instance.currentEvaluationScope;
        SetImmersionConfigMenuValues();
    }

    public void SetImmersionConfigMenuValues()
    {
        sceneSelector.value = GLPlayerPrefs.GetInt(scope, "Immersive Scene");

        SetImmersionValues(GLPlayerPrefs.GetInt(scope, "Visual Immersion Level"), visualSlider, visualText);
        SetImmersionValues(GLPlayerPrefs.GetInt(scope, "Auditive Immersion Level"), auditiveSlider, auditiveText);

        sceneSelector.RefreshShownValue();
    }

    public void UpdateSceneDropdownValues()
    {
        //int level = mentalLevelsDropdown.value;
        //int action = mentalLevelActionsDropdow.value;
        //ActionManager.Instance.SetMappedActionIndex(interfaceName, mentalLevelName[level], action);
        //UpdateMappedActions(mentalLevelName);
    }
    
    public void UpdateVisualValues()
    {
        int vLevel = (int)visualSlider.value;
        GLPlayerPrefs.SetInt(scope , "Visual Immersion Level", vLevel);
        Debug.Log("VIS IMM LVL = " + GLPlayerPrefs.GetInt(scope, "Visual Immersion Level"));
        visualText.text = vLevel.ToString();
    }

    public void UpdateAuditiveValues()
    {
        int aLevel = (int)auditiveSlider.value;
        GLPlayerPrefs.SetInt(scope, "Auditive Immersion Level", aLevel);
        Debug.Log("AUD IMM LVL = " + GLPlayerPrefs.GetInt(scope, "Auditive Immersion Level"));
        auditiveText.text = aLevel.ToString();
    }

    #region update values in UI methods

    void UpdateImmersionValues(int immersionlevel, Slider slider, Text text)
    {
        immersionlevel = (int)slider.value;
        text.text = immersionlevel.ToString();
    }

    void UpdateImmersionValues(ref float immersionlevel, Slider slider, Text text)
    {
        immersionlevel = (slider.value);
        int aux = (int)slider.value;
        text.text = aux.ToString();
    }

    void SetImmersionValues(ref int immersionlevel, Slider slider, Text text)
    {
        slider.value = immersionlevel;
        text.text = immersionlevel.ToString();
    }

    void SetImmersionValues(int immersionlevel, Slider slider, Text text)
    {
        Debug.Log("Immersion: " + immersionlevel.ToString());
        slider.value = immersionlevel;
        text.text = immersionlevel.ToString();
    }

    void SetImmersionValues(float immersionlevel, Slider slider, Text text)
    {
        slider.value = immersionlevel;
        int aux = (int)immersionlevel;
        text.text = aux.ToString();
    }
    #endregion
}
