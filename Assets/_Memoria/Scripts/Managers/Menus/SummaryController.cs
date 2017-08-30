using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gamelogic;
using SimpleFileBrowser;

public class SummaryController : MonoBehaviour {

    public Dropdown profilesDropdown, evaluationsDropdown;
    public InputField newProfileInputField, newEvaluationInputField;
    public PopUpController popUpWindowView;
    public ScrolldownContent fullListScrollView;
    public Text userIDText, informationObjectText, visualizationText, immersionText, outputPathText;
    string outputPath;
    bool initialized = false;

    public VideoPlayer videoInicio;

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
        //commented only for faster testing, UNCOMMENT FOR TESTS
        //videoInicio.gameObject.SetActive(true);
        //StartCoroutine(StopVideoInicio());
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
        ActionManager.Instance.LoadMappingActionsNames();
        ReloadOutputPath();
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

    IEnumerator StopVideoInicio()
    {
        yield return new WaitForSeconds(3f);
        videoInicio.gameObject.SetActive(false);
    }

    public void GetOutputFolderPath()
    {
        FileBrowser.AddQuickLink(null, "Users", "C:\\Users");
        FileBrowser.ShowLoadDialog(null, null, true, null, "Load", "Select");
        StartCoroutine(GetOutputFolder());
    }

    IEnumerator GetOutputFolder()
    {
        yield return FileBrowser.WaitForLoadDialog(true, null, "Load File", "Load");
        if (FileBrowser.Result != null)
        {
            outputPath = FileBrowser.Result;
            outputPathText.text = FileBrowser.Result;
        }
    }

    public void ApplyChangesOutputPath()
    {
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        GLPlayerPrefs.SetString(Scope, "OutputFolderPath", outputPath);
    }

    public void ReloadOutputPath()
    {
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        outputPath = GLPlayerPrefs.GetString(Scope, "OutputFolderPath");
        outputPathText.text = outputPath;
    }

    public void ShowAllPairedActions()
    {
        string allActionsList = "";
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        foreach(string s in MOTIONSManager.Instance.interfacesWithInputNames)
        {
            if (GLPlayerPrefs.GetBool(Scope, "use" + s))
            {
                foreach(string a in GLPlayerPrefs.GetStringArray(Scope,s+ "SummaryActions"))
                {
                    allActionsList = allActionsList + a + "\n";
                }
            }
        }
        fullListScrollView.LaunchScrollDown("Mapped actions list", allActionsList);
    }

    public void ShowAllInterfacesOnUse()
    {
        string allInterfacesList = "";
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        foreach (string s in MOTIONSManager.Instance.interfacesNames)
        {
            if (GLPlayerPrefs.GetBool(Scope, "use" + s))
            {
                allInterfacesList = allInterfacesList + s + "\n";

            }
        }
        fullListScrollView.LaunchScrollDown("Interfaces selected for evaluation", allInterfacesList);
    }
}
