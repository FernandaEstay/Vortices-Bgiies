using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopMenu : MonoBehaviour {

    public Button emotivButton, eyetribeButton, neuroskyButton;

    private void OnEnable()
    {
        if (!EEGManager.Instance.useEmotivInsight)
        {
            emotivButton.interactable = false;
        }
        else
        {
            emotivButton.interactable = true;
        }

        if (!EEGManager.Instance.useNeuroSky)
        {
            neuroskyButton.interactable = false;
        }
        else
        {
            neuroskyButton.interactable = true;
        }

        if (!EyetrackerManager.Instance._useEyetribe)
        {
            eyetribeButton.interactable = false;
        }
        else
        {
            eyetribeButton.interactable = true;
        }
    }

}
