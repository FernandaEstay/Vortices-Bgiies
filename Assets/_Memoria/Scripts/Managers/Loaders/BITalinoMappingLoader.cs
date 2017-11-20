using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BITalinoMappingLoader : MonoBehaviour {

    string interfaceName = "BITalino";

    string[]  triggers = new string[]
      {
            "ECG",
            "EDM",
            "EMG",
            "EDA"
      };

    private void OnEnable()
    {
        LoadActions();
    }

    private void LoadActions() {

    }

}
