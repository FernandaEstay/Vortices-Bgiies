using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic;

public class MOTIONSManager : MonoBehaviour {
    public static MOTIONSManager Instance { set; get; }
    [HideInInspector]
    public List<GameObject> activatedGameObjects;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartEvaluation()
    {
        GLPlayerPrefs.SetInt(ProfileManager.Instance.currentEvaluationScope, "LastUserIDUsed", GLPlayerPrefs.GetInt(ProfileManager.Instance.currentEvaluationScope, "CurrentUserID"));
    }
}
