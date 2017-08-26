using SimpleFileBrowser;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManager : MonoBehaviour {

    public static ConfigManager Instance { set; get; }
    [HideInInspector]
    public List<string[]> mappedActions;

    private void Awake()
    {
        Instance = this;
    }
    // Use this for initialization
    void Start () {
        mappedActions = new List<string[]>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
