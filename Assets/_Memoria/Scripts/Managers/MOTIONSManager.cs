using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOTIONSManager : MonoBehaviour {
    public static MOTIONSManager Instance { set; get; }
    public GameObject eegManagerPrefab;
    GameObject eegManager;
    [HideInInspector]
    public List<GameObject> activatedGameObjects;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }
    // Use this for initialization
    void Start () {
        eegManager = Instantiate(eegManagerPrefab);
        eegManager.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
