using SimpleFileBrowser;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    /*
    public void ExaminarFolderPath()
    {
        FileBrowser.AddQuickLink(null, "Users", "C:\\Users");
        FileBrowser.ShowLoadDialog(null, null, true, null, "Load", "Select");
        StartCoroutine(ObtenerGroupFolder());
    }
    IEnumerator ObtenerGroupFolder()
    {
        yield return FileBrowser.WaitForLoadDialog(true, null, "Load File", "Load");
        if (FileBrowser.Result != null)
            folderImageAssetText.text = FileBrowser.Result + "\\";

    }

    public void ExaminarGroupPath()
    {
        FileBrowser.AddQuickLink(null, "Users", "C:\\Users");
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Text Files", ".csv"));
        FileBrowser.SetDefaultFilter(".csv");
        FileBrowser.ShowLoadDialog(null, null, false, null, "Load", "Select");
        StartCoroutine(ShowResultCoroutine());
    }

    IEnumerator ShowResultCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(false, null, "Load File", "Load");
        if (FileBrowser.Result != null)
            groupPathText.text = FileBrowser.Result;
    }
    */
}
