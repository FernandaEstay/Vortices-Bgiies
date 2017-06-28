using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private Toggle m_MenuToggle;
    private float m_VolumeRef = 1f;
    private bool m_Paused;
    


    void Awake()
    {
        m_MenuToggle = transform.GetComponent <Toggle> ();
	}


    private void MenuOn ()
    {
        Debug.Log("menu on");
        //This lines will stop the game time, making any on-going animation, effect or time-dependant script stop. 
        //m_TimeScaleRef = Time.timeScale;
        //Time.timeScale = 0f;

        m_VolumeRef = AudioListener.volume;
        AudioListener.volume = 0f;

        m_Paused = true;
    }


    public void MenuOff ()
    {
        //Time.timeScale = m_TimeScaleRef;
        AudioListener.volume = m_VolumeRef;
        m_Paused = false;
    }


    public void OnMenuStatusChange ()
    {
        if (m_MenuToggle.isOn && !m_Paused)
        {
            MenuOn();
        }
        else if (!m_MenuToggle.isOn && m_Paused)
        {
            MenuOff();
        }
    }


	void Update()
	{
        //if (!EEGManager.Instance.useEmotivInsight && !EEGManager.Instance.useNeuroSky && !EyetrackerManager.Instance._useEyetribe)
        //    return;
		if(Input.GetKeyUp(KeyCode.Escape))
		{
		    m_MenuToggle.isOn = !m_MenuToggle.isOn;
            Cursor.visible = m_MenuToggle.isOn;//force the cursor visible if anythign had hidden it
            if (EventSystemChecker.Instance.originalEventSystemExist)
            {
                if (m_MenuToggle.isOn)
                {
                    EventSystemChecker.Instance.ActivateSecondaryEventSystem();
                }
                else
                {
                    EventSystemChecker.Instance.ActivateOrigianlEventSystem();
                }
            }
		}
	}


}
