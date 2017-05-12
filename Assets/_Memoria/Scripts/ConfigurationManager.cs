using System;
using UnityEngine;
using Gamelogic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConfigurationManager : MonoBehaviour
{
    public string sceneName;
    public GameObject mainMenuPanel;
    public GameObject hardwarePanel;
    public GameObject openGlovePanel;
    public GameObject loadImagePanel;
    public GameObject dataOutputPanel;
    public Button backButton;

    [Header("Main Menu")]
    public Dropdown modeDropDown;
    public Button LoadImages;
    public Button Output;
    public Dropdown testDropdown;
    public Dropdown visualization;

    [Header("Hardware")]
    public Toggle useLeapMotionToggle;
    public Toggle usePitchGrabToggle;
    public Toggle useJoystickToggle;
    public Toggle mouseInput;
    public Toggle kinectInput;

    [Header("Data Output")]
    public InputField dataOutputText;
    public InputField userIdText;


    [Header("Unity Open Glove")]
    public Toggle useUnityOpenGlove;
    

    [Header("Load Images")]
    public InputField imagesText;
    public InputField folderImageAssetText;
    public InputField folderSmallText;
    public InputField fileNameText;
    public InputField groupPathText;



    private const string Scope = "Config";

    public void Awake()
    {
        useLeapMotionToggle.isOn = GLPlayerPrefs.GetBool(Scope, "UseLeapMotion");
        if (useLeapMotionToggle.isOn) {
            usePitchGrabToggle.enabled = true;
            usePitchGrabToggle.enabled = true;
            usePitchGrabToggle.isOn = GLPlayerPrefs.GetBool(Scope, "UsePitchGrab");
        }

        useJoystickToggle.isOn = GLPlayerPrefs.GetBool(Scope, "UseJoystic");

        useUnityOpenGlove.isOn = GLPlayerPrefs.GetBool(Scope, "useUnityOpenGlove");

        kinectInput.isOn = GLPlayerPrefs.GetBool(Scope, "KinectInput");
        mouseInput.isOn = GLPlayerPrefs.GetBool(Scope, "MouseInput");

        if (GLPlayerPrefs.GetBool(Scope, "BGIIESMode"))
            modeDropDown.value = 1;
        else
            modeDropDown.value = 0;

        if (GLPlayerPrefs.GetBool(Scope, "visualizationPlane"))
            visualization.value = 1;
        else
            visualization.value = 0;


            dataOutputText.text = GLPlayerPrefs.GetString(Scope, "DataOutput");


        userIdText.text = GLPlayerPrefs.GetInt(Scope, "UserID").ToString();

        imagesText.text = GLPlayerPrefs.GetString(Scope, "Images");
        folderImageAssetText.text = GLPlayerPrefs.GetString(Scope, "FolderImageAssetText");
        folderSmallText.text = GLPlayerPrefs.GetString(Scope, "FolderSmallText");
        fileNameText.text = GLPlayerPrefs.GetString(Scope, "FileName");
        groupPathText.text = GLPlayerPrefs.GetString(Scope, "GroupPath");

        testDropdown.value = GLPlayerPrefs.GetInt(Scope, "Test");
    }

    public void StartSimulation()
    {
        Debug.Log(useUnityOpenGlove.isOn);
        GLPlayerPrefs.SetBool(Scope, "UseLeapMotion", useLeapMotionToggle.isOn);
        GLPlayerPrefs.SetBool(Scope, "UsePitchGrab", usePitchGrabToggle.isOn);
        GLPlayerPrefs.SetBool(Scope, "UseHapticGlove", useUnityOpenGlove.isOn);
        GLPlayerPrefs.SetBool(Scope, "UseJoystic", useJoystickToggle.isOn);

        GLPlayerPrefs.SetBool(Scope, "MouseInput", mouseInput.isOn);
        GLPlayerPrefs.SetBool(Scope, "KinectInput", kinectInput.isOn);

        if(modeDropDown.value == 0)
            GLPlayerPrefs.SetBool(Scope, "BGIIESMode", false);
        else
            GLPlayerPrefs.SetBool(Scope, "BGIIESMode", true);

        if(visualization.value == 0)
            GLPlayerPrefs.SetBool(Scope, "visualizationPlane", false);
        else
            GLPlayerPrefs.SetBool(Scope, "visualizationPlane", true);

        GLPlayerPrefs.SetString(Scope, "DataOutput", dataOutputText.text);


        GLPlayerPrefs.SetInt(Scope, "UserID", Convert.ToInt32(userIdText.text));

        GLPlayerPrefs.SetString(Scope, "Images", imagesText.text);
        GLPlayerPrefs.SetString(Scope, "FolderImageAssetText", folderImageAssetText.text);
        GLPlayerPrefs.SetString(Scope, "FolderSmallText", folderSmallText.text);
        GLPlayerPrefs.SetString(Scope, "FileName", fileNameText.text);
        GLPlayerPrefs.SetString(Scope, "GroupPath", groupPathText.text);

        GLPlayerPrefs.SetInt(Scope, "Test", testDropdown.value);

        SceneManager.LoadScene(sceneName);


    }
    public void Update()
    {
        if (!mainMenuPanel.activeSelf)
        {
            backButton.gameObject.SetActive(true);
        }
        else
        {
            backButton.gameObject.SetActive(false);
        }
        if (modeDropDown.value == 0)
        {
            mouseInput.isOn = false;
            kinectInput.isOn = false;

            mouseInput.gameObject.SetActive(false);
            kinectInput.gameObject.SetActive(false);

            useLeapMotionToggle.gameObject.SetActive(true);
            useJoystickToggle.gameObject.SetActive(true);

            if (useLeapMotionToggle.isOn)
            {
                if (!usePitchGrabToggle.gameObject.activeSelf)
                {
                    usePitchGrabToggle.gameObject.SetActive(true);
                }
                useJoystickToggle.isOn = false;
                useJoystickToggle.interactable = false;
                useUnityOpenGlove.interactable = true;
            }
            else
            {
                usePitchGrabToggle.gameObject.SetActive(false);
                useJoystickToggle.interactable = true;
            }
            if (useJoystickToggle.isOn)
            {
                useLeapMotionToggle.isOn = false;
                useLeapMotionToggle.interactable = false;

                useUnityOpenGlove.isOn = false;
                useUnityOpenGlove.interactable = false;
            }
            else 
                useLeapMotionToggle.interactable = true;

        }
        else
        {
            useLeapMotionToggle.isOn = false;
            useJoystickToggle.isOn = false;

            useLeapMotionToggle.gameObject.SetActive(false);
            useJoystickToggle.gameObject.SetActive(false);
            usePitchGrabToggle.gameObject.SetActive(false);

            kinectInput.gameObject.SetActive(true);
            mouseInput.gameObject.SetActive(true);

            if (kinectInput.isOn)
            {
                mouseInput.isOn = false;
                mouseInput.interactable = false;
                useUnityOpenGlove.interactable = true;
            }
            else
            {
                mouseInput.interactable = true;
                useUnityOpenGlove.isOn = false;
                useUnityOpenGlove.interactable = false;
            }

            if (mouseInput.isOn)
            {
                kinectInput.isOn = false;
                kinectInput.interactable = false;
            }
            else
                kinectInput.interactable = true;
        }
    }

    public void LoadImage()
    {
        mainMenuPanel.SetActive(false);

        loadImagePanel.SetActive(true);
        backButton.gameObject.SetActive(true);
    }

    public void DataOutput()
    {
        mainMenuPanel.SetActive(false);

        dataOutputPanel.SetActive(true);
        backButton.gameObject.SetActive(true);
    }

    public void Back()
    {
        mainMenuPanel.SetActive(true);

        loadImagePanel.SetActive(false);
        dataOutputPanel.SetActive(false);
        backButton.gameObject.SetActive(false);
    }
}