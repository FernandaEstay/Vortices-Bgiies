using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Memoria;
using Gamelogic;

public class ImmersionManager : MonoBehaviour {

    public Camera m_camera;

    private int maxVisualImmersionLevel = 6;
    private int maxAuditiveImmersionLevel = 6;

    void Start ()
    {
        VisualizationManager.Instance.LoadVisualization();
        int visualImmersionLevel = PlayerPrefs.GetInt("Visual Immersion");
        int auditiveImmersionlevel = PlayerPrefs.GetInt("Auditive Immersion");

        LoadGameObjects(visualImmersionLevel, maxVisualImmersionLevel, "Immersion_");
        LoadGameObjects(auditiveImmersionlevel, maxAuditiveImmersionLevel, "A_Immersion_");
        RenderingPathConfig(visualImmersionLevel);
        EnviromentLightiningConfig(visualImmersionLevel);
    }

    void LoadGameObjects(int immersionLevel, int maxLevel,  string immersionType){

        GameObject[] objects;
        
        //Game Objects are put on the scene based on the immersion level they have on their tags
        if(immersionLevel + 1 <= maxVisualImmersionLevel)
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
    void EnviromentLightiningConfig(int immersionLevel) {
        Color color = new Color(0,0,0);
        switch (immersionLevel)
        {
            case 0:
                color.r = 0; color.g = 0; color.b = 0;
                RenderSettings.ambientLight = color;                                    //No ambient Light (RGB = 0)
                RenderSettings.skybox = null;                                           //No SkyBox
                RenderSettings.defaultReflectionResolution = 128;                       //Very Low Quality Reflection
                break;
            case 1:
                color.r = 0; color.g = 0; color.b = 0;                                  
                RenderSettings.ambientLight = color;                                    //No ambient Light (RGB = 0)
                RenderSettings.defaultReflectionResolution = 128;                       //Very Low Quality Reflection
                break;
            case 2:
                color.r = color.g = color.b = 0;                               
                RenderSettings.ambientLight = color;                                    //No ambient Light (RGB = 0)
                RenderSettings.defaultReflectionResolution = 256;                       //Low Quality Reflection
                break;
            case 3:
                color.r = color.g = color.b = 0;
                RenderSettings.ambientLight = color;                                    //No ambient Light (RGB = 0)
                RenderSettings.defaultReflectionResolution = 256;                       //Low Quality Reflection
                break;
            case 4:
                color.r = color.g = color.b = 0.3308824f;       
                RenderSettings.ambientLight = color;                                    //Basic ambient Light (Flat gray)
                RenderSettings.defaultReflectionResolution = 512;                       //Medium Quality Reflection
                break;
            case 5:
                color.r = color.g = color.b = 0.3308824f;
                RenderSettings.ambientLight = color;                                    //Basic ambient Light (Flat gray)
                RenderSettings.defaultReflectionResolution = 512;                       //Medium Quality Reflection
                break;
            case 6:
                color.r = 0.5147059f; color.g = 0.3958433f; color.b = 0.1400303f;
                RenderSettings.ambientLight = color;                                    //Ambient Light based on the color pallet
                RenderSettings.defaultReflectionResolution = 1024;                      //High Quality Reflection
                break;
            default:
                Debug.Log("Error: Visual Immersion Level not allowed");
                break;
        }
 
    }

    void RenderingPathConfig(int immersionLevel)
    {
       if (immersionLevel >= 4)
        {
            m_camera.renderingPath = RenderingPath.DeferredShading;       //Highest Quality Rendering Path
        }
        else if(immersionLevel < 3)
        {
            m_camera.renderingPath = RenderingPath.VertexLit;             //Lowest Quality Rendering Path
        }
       else
        {
            m_camera.renderingPath = RenderingPath.Forward;               //Medium Quality Rendering Path
        }
    }
}
