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
        "Farm Scene",
        "No Scene"
    };

    private void OnEnable()
    {
        scope = ProfileManager.Instance.currentEvaluationScope;
        SetImmersionConfigMenuValues();
    }

    public void SetImmersionConfigMenuValues()
    {

        SetImmersionValues(GLPlayerPrefs.GetInt(scope, "Visual Immersion Level"), visualSlider, visualText);
        SetImmersionValues(GLPlayerPrefs.GetInt(scope, "Auditive Immersion Level"), auditiveSlider, auditiveText);

        AddArrayToDropdown(sceneSelector, sceneName);
        sceneSelector.RefreshShownValue();
    }

    public void UpdateSceneDropdownValues()
    {
        int sceneIndex = sceneSelector.value;
        GLPlayerPrefs.SetInt(scope, "Scene", sceneIndex);
    }
    
    public void UpdateVisualValues()
    {
        int vLevel = (int)visualSlider.value;
        GLPlayerPrefs.SetInt(scope , "Visual Immersion Level", vLevel);
        visualText.text = vLevel.ToString();
    }

    public void UpdateAuditiveValues()
    {
        int aLevel = (int)auditiveSlider.value;
        GLPlayerPrefs.SetInt(scope, "Auditive Immersion Level", aLevel);
        auditiveText.text = aLevel.ToString();
    }

    #region update values in UI methods

    void AddArrayToDropdown(Dropdown availableInputDropdown, string[] scenesNames)
    {
        availableInputDropdown.ClearOptions();
        foreach (string s in scenesNames)
        {
            availableInputDropdown.options.Add(new Dropdown.OptionData() { text = s });
        }
        availableInputDropdown.RefreshShownValue();
    }

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
