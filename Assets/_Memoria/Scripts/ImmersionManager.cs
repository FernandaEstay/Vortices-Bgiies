using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Memoria;
using Gamelogic;

public class ImmersionManager : MonoBehaviour {

    public Material m_skybox;
    public GameObject structureObjects;
    public GameObject scenarioObjects;

    private Camera m_camera;
    private int maxVisualImmersionLevel = 6;
    private int maxAuditiveImmersionLevel = 6;
    private string scope;

    void Start ()
    {
        VisualizationManager.Instance.LoadVisualization();
        scope = ProfileManager.Instance.currentEvaluationScope;
        int visualImmersionLevel = GLPlayerPrefs.GetInt(scope, "Visual Immersion Level");
        int auditiveImmersionlevel = GLPlayerPrefs.GetInt(scope, "Auditive Immersion Level");
        string[] visualImmersion = GLPlayerPrefs.GetStringArray(scope, "visualImmersion");

        m_camera = Camera.main;
        LoadGameObjects(Int32.Parse(visualImmersion[0]));
        LoadGameObjectsByTag(auditiveImmersionlevel, maxAuditiveImmersionLevel, "A_Immersion_");
        RenderingPathConfig(visualImmersion[3]);        
        EnviromentLightiningConfig(visualImmersion[1], visualImmersion[2], Int32.Parse(visualImmersion[3]));
        QualitySettingsConfig(Int32.Parse(visualImmersion[5]));
    }

    void LoadGameObjects(int objects)
    {
        if(objects <= 1)
        {
            structureObjects.SetActive(false);
        }
        if(objects <= 2)
        {
            scenarioObjects.SetActive(false);
        }

    }

    void LoadGameObjectsByTag(int immersionLevel, int maxLevel,  string immersionType){

        GameObject[] objects;

        //Game Objects are put on the scene based on the immersion level they have on their tags
        if (immersionLevel + 1 <= maxLevel)
        { 
            for (int i = immersionLevel + 1; i <= maxLevel; i++){
                objects = GameObject.FindGameObjectsWithTag(immersionType + i);
                foreach (GameObject localObject in objects){
                    localObject.SetActive(false);
                }
            }
        }
    }

    //AÑADIR UN COLOR PICKER PARA LA LUZ AMBIENTAL
    void EnviromentLightiningConfig(string ambientColor, string skybox, int reflectionResolution) {
        Color color = new Color(0,0,0);
        m_camera.clearFlags = CameraClearFlags.Skybox;

        setSkybox(skybox);
        setAmbientLight(ambientColor);
        setReflectionResolution(reflectionResolution); 
    }

    void setSkybox(string skybox)
    {
        RenderSettings.skybox = null;
        if (skybox == "skybox") { RenderSettings.skybox = m_skybox; };
    }

    void setAmbientLight(string ambientLight)
    {
        Color color = new Color(0, 0, 0);
        if(ambientLight == "flat"){ color.r = color.g = color.b = 0.3308824f; };
        if(ambientLight == "pallet") { color.r = 0.5147059f; color.g = 0.3958433f; color.b = 0.1400303f; };
        RenderSettings.ambientLight = color;
    }

    void setReflectionResolution(int reflectionResolution)
    {
        RenderSettings.defaultReflectionResolution = reflectionResolution;
    }

    void RenderingPathConfig(string renderingPath)
    {
       if (renderingPath == "deferred")
        {
            m_camera.renderingPath = RenderingPath.DeferredShading;       //Highest Quality Rendering Path
        }
        else if(renderingPath == "vertex")
        {
            m_camera.renderingPath = RenderingPath.VertexLit;             //Lowest Quality Rendering Path
        }
       else
        {
            m_camera.renderingPath = RenderingPath.Forward;               //Medium Quality Rendering Path
        }
    }

    void QualitySettingsConfig(int immersionLevel)
    {
        switch (immersionLevel)
        {
            case 0:
                QualitySettings.SetQualityLevel(0, true);
                break;
            case 1:
                QualitySettings.SetQualityLevel(1, true);
                break;
            case 2:
                QualitySettings.SetQualityLevel(2, true);
                break;
            case 3:
                QualitySettings.SetQualityLevel(3, true);
                break;
            case 4:
                QualitySettings.SetQualityLevel(4, true);
                break;
            case 5:
                QualitySettings.SetQualityLevel(5, true);
                break;
        }
    }
}
