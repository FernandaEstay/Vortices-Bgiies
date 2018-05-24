using Memoria;
using Gamelogic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityCallbacks;
using UnityEngine;

public class PhysiologicalManager : MonoBehaviour { //*NOTE: NO HEREDA DE LOS MISMOS QUE EL EEGManager

    [HideInInspector]
    public static PhysiologicalManager Instance { set; get; }
    private bool initialized = false;
    private string scope = "Vortices2Config";

    #region Variable declaration
    /*
     * These are the input variables 
     */

    /*
     * 
     * BITalino variables
     * 
     */
    public BITalinoCtrl bitalinoController;
    public GameObject canvas;
    public GameObject bitalinoExtras;
    [HideInInspector]
    public bool useBITalino;    
    [HideInInspector]
    public float ecg;
    [HideInInspector]
    public float ecgTrigger;
    [HideInInspector]
    public float emg;
    [HideInInspector]
    public float emgTrigger;
    [HideInInspector]
    public float acc;
    [HideInInspector]
    public float accTrigger;
    [HideInInspector]
    public float eda;
    [HideInInspector]
    public float edaTrigger;

    #endregion

    #region Initialization

    public void CheckInterfaces() 
    {
        scope = ProfileManager.Instance.currentEvaluationScope;
        useBITalino = GLPlayerPrefs.GetBool(scope, "useBITalino");
        Debug.Log("Use BITalino:" + GLPlayerPrefs.GetBool(scope, "useBITalino").ToString());

        if (useBITalino)
        {
            canvas.SetActive(true);
            bitalinoExtras.SetActive(true);
            bitalinoController.gameObject.SetActive(true);
            bitalinoController.InitializeBITalino();
        }
        else
        {
            NegateBITalino();
            canvas.SetActive(false);
            bitalinoExtras.SetActive(false);
            bitalinoController.gameObject.SetActive(false);
        }
    }

    // Use this for initialization
    public void Awake()
    {
        Instance = this;
    }

    #endregion

    #region Update functions
    // Update is called once per frame
    void Update()
    {
        if (!useBITalino)
            return;
        else
        {
            bitalinoController.UpdateBITalino();
            ecg = bitalinoController.GetEcg();
            emg = bitalinoController.GetEmg();
            acc = bitalinoController.GetAcc();
            eda = bitalinoController.GetEda();
        }
    }

    #endregion


    #region BITalino functions
    /*
     * BITalino functions
     * 
     */

    void NegateBITalino()
    {
        useBITalino = false;
    }

    #endregion
}
