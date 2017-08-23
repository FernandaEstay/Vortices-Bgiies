using SimpleFileBrowser;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManager : MonoBehaviour {

    string outputPath;
    [HideInInspector]
    public List<string[]> mappedActions;
	// Use this for initialization
	void Start () {
        mappedActions = new List<string[]>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ExploreFolderPath()
    {
        FileBrowser.AddQuickLink(null, "Users", "C:\\Users");
        FileBrowser.ShowLoadDialog(null, null, true, null, "Load", "Select");
        StartCoroutine(GetOutputFolder());
    }
    IEnumerator GetOutputFolder()
    {
        yield return FileBrowser.WaitForLoadDialog(true, null, "Load File", "Load");
        if (FileBrowser.Result != null)
            Debug.Log(FileBrowser.Result+"\\testfolder\\data.csv");

    }
    /*
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
