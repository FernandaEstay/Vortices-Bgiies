using Emotiv;
using Memoria;
using Gamelogic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityCallbacks;
using UnityEngine;

public class EEGManager : MonoBehaviour, IAwake, IFixedUpdate {

    [HideInInspector]
    public static EEGManager Instance { set; get; }
    private bool initialized = false;
    private string Scope = "Vortices2Config";
    #region Variable declaration
    /*
     * These are the input variables 
     */


    /*
     * 
     * NeuroSky variables
     * 
     */
    public bool useNeuroSky;
    [HideInInspector]
    public int blinkStrength;
    [HideInInspector]
    public int blinkStrengthTrigger;
    [HideInInspector]
    public int meditationLevel;
    [HideInInspector]
    public int meditationLevelTrigger;
    [HideInInspector]
    public int attentionLevel;
    [HideInInspector]
    public int attentionLevelTrigger;

    /*
     * 
     * Emotiv Insight variables
     * 
     */
    public bool useEmotivInsight;
    [HideInInspector]
    public EdkDll.IEE_MentalCommandAction_t MentalCommandCurrentAction;
    [HideInInspector]
    public float MentalCommandCurrentActionPower;
    [HideInInspector]
    public bool FacialExpressionIsRightEyeWinking;
    [HideInInspector]
    public bool FacialExpressionIsLeftEyeWinking;
    [HideInInspector]
    public bool FacialExpressionIsUserBlinking;
    [HideInInspector]
    public float FacialExpressionUpperFaceActionPower;
    [HideInInspector]
    public float FacialExpressionSmileExtent;
    [HideInInspector]
    public float FacialExpressionLowerFaceActionPower;
    [HideInInspector]
    public EdkDll.IEE_FacialExpressionAlgo_t FacialExpressionLowerFaceAction;
    [HideInInspector]
    public EdkDll.IEE_FacialExpressionAlgo_t FacialExpressionUpperFaceAction;

    #endregion

    #region Initialization
    public void InitializeManager()
    {
        Scope = ProfileManager.Instance.currentProfileScope;
        useEmotivInsight = GLPlayerPrefs.GetBool(Scope, "UseEmotivInsight");
        Debug.Log("Use emotiv:"+GLPlayerPrefs.GetBool(Scope, "UseEmotivInsight").ToString());
        if (useEmotivInsight){
            EmotivCtrl.Instance.StartEmotivInsight();            
        }else{
            NegateEmotivInsight();
        }

        useNeuroSky = GLPlayerPrefs.GetBool(Scope, "UseNeuroSkyMindwave");
        Debug.Log("Use neurosky:" + GLPlayerPrefs.GetBool(Scope, "UseNeuroSkyMindwave").ToString());

        if (useNeuroSky)
        {
            NeuroSkyData.Instance.StartNeuroSkyData();
        }else{
            NegateNeuroSky();
        }

        initialized = true;
    }

    // Use this for initialization
    public void Awake () {
        Instance = this;
    }


    #endregion

    #region Update functions
    // Update is called once per frame
    void Update () {
        Debug.Log("update triggered");
        if (!initialized)
            return;

        if (useEmotivInsight)
        {
            EmotivCtrl.Instance.UpdateEmotivInsight();

        }

	}

    public void FixedUpdate()
    {
        if (!initialized || !useNeuroSky)
            return;

        NeuroSkyData.Instance.ResetBlink();
        blinkStrength = NeuroSkyData.Instance.getBlink();
        attentionLevel = NeuroSkyData.Instance.getAttention();
        meditationLevel = NeuroSkyData.Instance.getMeditation();
    }

    #endregion

    #region Neuro Sky functions
    /*
     * Neuro Sky functions
     * 
     */

    



    void NegateNeuroSky()
    {
        useNeuroSky = false;
    }

    #endregion

    #region Emotiv Insight Functions
    void NegateEmotivInsight()
    {
        useEmotivInsight = false;
    }



    #endregion
}
