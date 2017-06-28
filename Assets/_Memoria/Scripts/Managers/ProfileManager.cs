using System.Collections;
using System.Collections.Generic;
using Gamelogic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour {

    public static ProfileManager Instance { set; get; }
    [HideInInspector]
    public string currentProfileScope;
    
    [HideInInspector]
    public int lastProfileUsed;
    [HideInInspector]
    public string[] profiles;

    string profileManagerScope = "ProfileManagerData";
    // Use this for initialization
    public void Awake () {
        Instance = this;
        DontDestroyOnLoad(this);        
        profiles = GLPlayerPrefs.GetStringArray(profileManagerScope,"ProfileNamesList");
        lastProfileUsed = GLPlayerPrefs.GetInt(profileManagerScope, "LastProfileUsed");
        currentProfileScope = profiles[lastProfileUsed];
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool UpdateCurrentProfile(int lastProfileUsedNumber)
    {
        lastProfileUsed = lastProfileUsedNumber;
        GLPlayerPrefs.SetInt(profileManagerScope, "LastProfileUsed", lastProfileUsed);
        currentProfileScope = profiles[lastProfileUsed];
        return true;
    }

    public bool AddNewProfile(string newProfile)
    {
        if (CheckRepeatedProfileName(newProfile))
        {
            string[] aux = new string[profiles.Length];
            profiles.CopyTo(aux, 0);
            int newLength = profiles.Length+1;
            profiles = new string[newLength];
            aux.CopyTo(profiles, 0);
            profiles[newLength - 1] = newProfile;
            GLPlayerPrefs.SetStringArray(profileManagerScope, "ProfileNamesList", profiles);
            return true;
        }
        else
        {
            return false;
        }
    }

    bool CheckRepeatedProfileName(string newProfile)
    {
        foreach(string s in profiles)
        {
            if (newProfile.Equals(s))
                return false;
        }
        return true;
    }

}
