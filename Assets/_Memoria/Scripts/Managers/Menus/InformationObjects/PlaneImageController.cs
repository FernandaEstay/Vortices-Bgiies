﻿using Gamelogic;
using Memoria;
using SimpleFileBrowser;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneImageController : MonoBehaviour {
    public InformationObjectController objectController;
    string objectName = "PlaneImage";
    public InputField prefixInput, sufixInput;
    public Text folderPathText, groupPathText;
    string folderPath, groupPath;
    //Variables to be stored on the PlayerPrefs:
    /*
     * Image folder path "FolderPath"
     * Images prefix "Prefix"
     * Images sufix "Sufix"
     * Group files path "GroupFilePath"
     */
    string[] planeImageActionsNames = new string[]
    {
        "Select/Deselect image",
            "Zoom in image",
            "Zoom out image"
    };

    private void OnEnable()
    {
        if(objectController != null)
        {
            string Scope = ProfileManager.Instance.currentEvaluationScope;
            SelectThisObject();
            objectController.scrollDown.LaunchScrollDown("Plane Image description", "A plane image uses a primitive unity object called QUAD, which looks like the plane but its edges are only one unit long and the surface is oriented in the XY plane of the local coordinate space.");
            LoadPreferences();
        }        
    }

    public void LoadPreferences()
    {
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        folderPath = GLPlayerPrefs.GetString(Scope, objectName + "FolderPath");
        groupPath = GLPlayerPrefs.GetString(Scope, objectName + "GroupFilePath");
        folderPathText.text = folderPath;
        groupPathText.text = groupPath;
        prefixInput.text = GLPlayerPrefs.GetString(Scope, objectName + "Prefix");
        sufixInput.text = GLPlayerPrefs.GetString(Scope, objectName + "Sufix");
    }

    public void SelectThisObject()
    {
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        GLPlayerPrefs.SetString(Scope, "CurrentInformationObject", objectName);
        objectController.UpdateCurrentSelectedObjectText();
        ActionManager.Instance.currentObjectActionsNames = new string[planeImageActionsNames.Length];
        planeImageActionsNames.CopyTo(ActionManager.Instance.currentObjectActionsNames, 0);
    }

    public void ApplyChanges()
    {
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        GLPlayerPrefs.SetString(Scope, objectName + "Prefix", prefixInput.text);
        GLPlayerPrefs.SetString(Scope, objectName + "Sufix", sufixInput.text);
        GLPlayerPrefs.SetString(Scope, objectName + "FolderPath", folderPath);
        GLPlayerPrefs.SetString(Scope, objectName + "GroupFilePath", groupPath);
    }

    public void GetImagesFolderPath()
    {
        FileBrowser.AddQuickLink(null, "Users", "C:\\Users");
        FileBrowser.ShowLoadDialog(null, null, true, null, "Load", "Select");
        StartCoroutine(GetImagesFolder());
    }

    IEnumerator GetImagesFolder()
    {
        yield return FileBrowser.WaitForLoadDialog(true, null, "Load File", "Load");
        if (FileBrowser.Result != null)
        {
            folderPath = FileBrowser.Result;
            folderPathText.text = FileBrowser.Result;
        }            
    }

    public void GetGroupFolderPath()
    {
        FileBrowser.AddQuickLink(null, "Users", "C:\\Users");
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Text Files", ".csv"));
        FileBrowser.SetDefaultFilter(".csv");
        FileBrowser.ShowLoadDialog(null, null, false, null, "Load", "Select");
        StartCoroutine(GetGroupPath());
    }

    IEnumerator GetGroupPath()
    {
        yield return FileBrowser.WaitForLoadDialog(false, null, "Load File", "Load");
        if (FileBrowser.Result != null)
        {
            groupPath = FileBrowser.Result;
            groupPathText.text = FileBrowser.Result;
        }
    }

    public void ShowAvailableActions()
    {
        objectController.popUpScrollDown.LaunchScrollDown("Plane images actions", "[·]Select/Deselect image: The image that is being highlighted (by the Mouse, for example) " +
            "changes to a greenish color. Making this action again returns the color to normal if the same image is highlighted. " +
            "\n[·]Zoom in image: Increases the image's size" +
            "\n[·]Zoom out image: Returns the previously Zoomed in image's size to normal");
    }

}
