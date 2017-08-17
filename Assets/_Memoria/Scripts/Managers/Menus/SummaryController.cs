using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gamelogic;

public class SummaryController : MonoBehaviour {

    public Dropdown profilesDropdown, evaluationsDropdown;
    public InputField newProfileInputField, newEvaluationInputField;
    public PopUpController popUpWindowView;
    public ScrolldownContent fullListScrollView;
    public Text userIDText, informationObjectText, visualizationText, immersionText;
    bool initialized = false;

    private void OnEnable()
    {
        if (!initialized)
            return;
        ReloadSummaryData();
    }

    // Use this for initialization
    void Start()
    {
        ReloadProfileDropdown();
        initialized = true;
    }

    public void UpdateCurrentProfile()
    {
        if (ProfileManager.Instance.UpdateCurrentProfile(profilesDropdown.value))
        {
            ReloadProfileDropdown();
        }
    }

    public void UpdateCurrentEvaluation()
    {
        if (ProfileManager.Instance.UpdateCurrentEvaluation(evaluationsDropdown.value))
        {
            ReloadEvaluationDropdown();
        }
    }

    public void AddNewProfile()
    {
        string newProfile = newProfileInputField.text;
        if (ProfileManager.Instance.AddNewProfile(newProfile))
        {
            popUpWindowView.LaunchPopUpMessage("Success", "The new profile was successfully added!");
            ReloadProfileDropdown();
        }
        else
        {
            popUpWindowView.LaunchPopUpMessage("Creation failed", "The new profile name was already in use, please try again with a different name");
        }
    }

    public void AddNewEvaluation()
    {
        string newEvaluation = newEvaluationInputField.text;
        if (ProfileManager.Instance.AddNewEvaluation(newEvaluation))
        {
            popUpWindowView.LaunchPopUpMessage("Success", "The new evaluation was successfully added!");
            ReloadEvaluationDropdown();
        }
        else
        {
            popUpWindowView.LaunchPopUpMessage("Creation failed", "The new evaluation name was already in use, please try again with a different name");
        }
    }

    void ReloadProfileDropdown()
    {
        profilesDropdown.ClearOptions();
        foreach (string s in ProfileManager.Instance.profiles)
        {
            profilesDropdown.options.Add(new Dropdown.OptionData() { text = s });
        }
        profilesDropdown.value = ProfileManager.Instance.currentProfile;
        profilesDropdown.RefreshShownValue();
        ReloadEvaluationDropdown();
    }

    void ReloadEvaluationDropdown()
    {
        evaluationsDropdown.ClearOptions();
        foreach (string s in ProfileManager.Instance.evaluations)
        {
            evaluationsDropdown.options.Add(new Dropdown.OptionData() { text = s });
        }
        evaluationsDropdown.value = ProfileManager.Instance.currentEvaluation;
        evaluationsDropdown.RefreshShownValue();
        ReloadSummaryData();
    }

    void ReloadSummaryData()
    {
        ReloadUserIDText();
        informationObjectText.text = GLPlayerPrefs.GetString(ProfileManager.Instance.currentEvaluationScope, "CurrentInformationObject");
        visualizationText.text = GLPlayerPrefs.GetString(ProfileManager.Instance.currentEvaluationScope, "CurrentVisualization");
        immersionText.text = GLPlayerPrefs.GetString(ProfileManager.Instance.currentEvaluationScope, "CurrentImmersion");
    }

    void ReloadUserIDText()
    {
        int aux1, aux2;
        aux1 = GLPlayerPrefs.GetInt(ProfileManager.Instance.currentEvaluationScope, "CurrentUserID");
        aux2 = GLPlayerPrefs.GetInt(ProfileManager.Instance.currentEvaluationScope, "LastUserIDUsed");
        
        if (aux1 == aux2)
        {
            aux1++;
            GLPlayerPrefs.SetInt(ProfileManager.Instance.currentEvaluationScope, "CurrentUserID", aux1);
        }
        userIDText.text = aux1.ToString();
    }

    public void ChangeUserIDButton()
    {
        int aux = GLPlayerPrefs.GetInt(ProfileManager.Instance.currentEvaluationScope, "CurrentUserID");
        popUpWindowView.LaunchPopUpInputChangeMessage("Change user ID", "User ID: ", ChangeUserID, aux.ToString(), true);
    }

    public void ChangeUserID(string userID)
    {
        popUpWindowView.ClosePopUp();
        int aux;
        if(int.TryParse(userID,out aux))
        {
            GLPlayerPrefs.SetInt(ProfileManager.Instance.currentEvaluationScope, "CurrentUserID", aux);
            aux--;
            GLPlayerPrefs.SetInt(ProfileManager.Instance.currentEvaluationScope, "LastUserIDUsed", aux);
            ReloadUserIDText();
            popUpWindowView.LaunchPopUpMessage("Successful", "New User ID was changed successfully");
        }
        else
        {
            popUpWindowView.LaunchPopUpMessage("Change failed", "There was an error trying to converse the input to a number, please try again");
        }

    }

}
