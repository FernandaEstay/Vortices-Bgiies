using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gamelogic;

public class ImmersionCanvasController : MonoBehaviour {

    public Slider   visualSlider, auditiveSlider, objectsNumberSlider, qualitySettingsSlider;
    public Text     visualText, auditiveText;
    public Dropdown sceneSelector, reflectionResolutionDropdown, renderingPathDropdown, ambientColorDropdown, skyBoxDropdown;

    string scope;
    string currentVisualization;
    string currentObject;
    string[] m_sceneNames = new string[]
    {
        "Sci-Fi Scene",
        "Farm Scene",
        "No Scene"
    };

    string[] reflectionNames = new string[]
    {
        "128",
        "256",
        "512",
        "1024"
    };

    string[] renderingPathNames = new string[]
    {
        "Vertex",
        "Forward",
        "Deferred"
    };

    string[] ambientNames = new string[]
    {
        "None",
        "Flat",
        "Pallet"
    };

    string[] skyBoxNames = new string[]
    {
        "None",
        "Skybox",
    };

    string[] visualImmersion = new string[0];

    private void OnEnable()
    {
        scope = ProfileManager.Instance.currentEvaluationScope;
        SetImmersionConfigMenuValues();

        visualImmersion = GLPlayerPrefs.GetStringArray(scope, "visualImmersion");
        if (visualImmersion.Length == 0) {
            visualImmersion = new string[]
            {
                "0",            //objects (0... 6)
                "flat",         //ambient color (none, flat, pallet)
                "skybox",       //skybox (none, skybox)
                "128",     //reflection resolution (128, 256, 512, 1024)
                "deferred",     //rendering path (vertex, forward, deferred)
                "0",            //quality settings (0... 5)
            };
            GLPlayerPrefs.SetStringArray(scope, "visualImmersion", visualImmersion);
        };
    }

    public void SetImmersionConfigMenuValues()
    {

        SetImmersionValues(GLPlayerPrefs.GetInt(scope, "Visual Immersion Level"), visualSlider, visualText);
        SetImmersionValues(GLPlayerPrefs.GetInt(scope, "Auditive Immersion Level"), auditiveSlider, auditiveText);

        AddArrayToDropdown(sceneSelector, m_sceneNames);
        AddArrayToDropdown(reflectionResolutionDropdown, reflectionNames);
        AddArrayToDropdown(renderingPathDropdown, renderingPathNames);
        AddArrayToDropdown(ambientColorDropdown, ambientNames);
        AddArrayToDropdown(skyBoxDropdown, skyBoxNames);

        UpdateDropDownValues();
        sceneSelector.RefreshShownValue();
    }

    public void UpdateSceneOnSelection()
    {
        int sceneIndex = sceneSelector.value;
        GLPlayerPrefs.SetInt(scope, "Scene", sceneIndex);
    }
    
    public void UpdateVisualValues()
    {
        int vLevel = (int)visualSlider.value;
        string[] visualImmersion = new string[0];

        switch (vLevel)
        {
            case 0:
                visualImmersion = new string[] { "0", "none", "none", "128", "vertex", "0"};
                break;
            case 1:
                visualImmersion = new string[] { "1", "skybox", "none", "128", "vertex", "1" };
                break;
            case 2:
                visualImmersion = new string[] { "2", "skybox", "none", "256", "vertex", "2" };
                break;
            case 3:
                visualImmersion = new string[] { "3", "skybox", "none", "256", "forward", "3" };
                break;
            case 4:
                visualImmersion = new string[] { "4", "skybox", "flat", "512", "deferred", "4" };
                break;
            case 5:
                visualImmersion = new string[] { "5", "skybox", "flat", "512", "deferred", "5" };
                break;
            case 6:
                visualImmersion = new string[] { "6", "skybox", "pallet", "1024", "deferred", "6" };
                break;
        }

        GLPlayerPrefs.SetStringArray(scope, "visualImmersion", visualImmersion);
        GLPlayerPrefs.SetInt(scope , "Visual Immersion Level", vLevel);
        visualText.text = immersionToString(vLevel);
    }

    public void UpdateAuditiveValues()
    {
        int aLevel = (int)auditiveSlider.value;
        GLPlayerPrefs.SetInt(scope, "Auditive Immersion Level", aLevel);
        auditiveText.text = immersionToString(aLevel);
    }

    string immersionToString(int intLevel)
    {
        string stringLevel;

        switch (intLevel)
        {
            case 0:
                stringLevel = "Min";
                return stringLevel;
            case 1:
                stringLevel = "Very Low";
                return stringLevel;
            case 2:
                stringLevel = "Low";
                return stringLevel;
            case 3:
                stringLevel = "Medium";
                return stringLevel;
            case 4:
                stringLevel = "High";
                return stringLevel;
            case 5:
                stringLevel = "Very High";
                return stringLevel;
            case 6:
                stringLevel = "Max";
                return stringLevel;
            default:
                return "Error";
        }
    }

    #region update values in UI methods

    void UpdateDropDownValues()
    {
        sceneSelector.value = GLPlayerPrefs.GetInt(scope, "Scene");
    }

    void AddArrayToDropdown(Dropdown availableInputDropdown, string[] names)
    {
        availableInputDropdown.ClearOptions();
        foreach (string s in names)
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
        text.text = immersionToString(immersionlevel);
    }

    void SetImmersionValues(int immersionlevel, Slider slider, Text text)
    {
        slider.value = immersionlevel;
        text.text = immersionToString(immersionlevel);
    }

    void SetImmersionValues(float immersionlevel, Slider slider, Text text)
    {
        slider.value = immersionlevel;
        int aux = (int)immersionlevel;
        text.text = aux.ToString();
    }
    #endregion
}
