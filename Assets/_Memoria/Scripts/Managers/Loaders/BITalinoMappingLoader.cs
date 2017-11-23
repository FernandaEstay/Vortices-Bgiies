using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BITalinoMappingLoader : MonoBehaviour {

    string interfaceName = "BITalino";

    string[]  triggers = new string[]
      {
        "ECG",
        "EMG",
        "ACC",
        "EDA"
      };

    private void OnEnable()
    {
        LoadActions();
    }

    private void LoadActions() {

    }

}
