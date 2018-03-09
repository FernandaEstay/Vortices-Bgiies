using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Emotiv;
using Memoria;
using System;
using Gamelogic;

public class EmotivCtrl : MonoBehaviour {

    public string dataLogPath;
    public InputField userSaveDataPath, userLoadDataPath, userOfflineID;
    string profileNameForSavingUser, profileNameForLoadingUser;
    public Text statusOfflineText;
    int state;
    uint userOfflineStoredID;
    public CsvCreator csvCreator;
    public GameObject modal;
	public InputField userName;
	public InputField password;
	public InputField profileName;
    public Text statusText, trainingStatusText;
    string[] trainingStatusTextLines = new string[5];
    int trainingStatusTextLinesIndex = 0;
    public bool use_giro_as_camera = false;

	public static EmoEngine engine;
	public static int engineUserID = -1;
	public static int userCloudID = 0;
	static int version	= -1;

    private int currentPersonId;
    private string Scope = "Vortices2Config";

    /*
	 * Create instance of EmoEngine and set up his handlers for 
	 * user events, connection events and mental command training events.
	 * Init the connection
	*/
    public void Awake()
    {

    }

	public void StartEmotivInsight () 
	{
        engine = EmoEngine.Instance;
        Scope = ProfileManager.Instance.currentEvaluationScope;
        engine.MentalCommandTrainingStarted += new EmoEngine.MentalCommandTrainingStartedEventEventHandler (TrainingStarted);
		engine.MentalCommandTrainingSucceeded += new EmoEngine.MentalCommandTrainingSucceededEventHandler (TrainingSucceeded);
		engine.MentalCommandTrainingCompleted += new EmoEngine.MentalCommandTrainingCompletedEventHandler (TrainingCompleted);
		engine.MentalCommandTrainingRejected += new EmoEngine.MentalCommandTrainingRejectedEventHandler (TrainingRejected);
		engine.MentalCommandTrainingReset += new EmoEngine.MentalCommandTrainingResetEventHandler (TrainingReset);
        
        //If using VORTICES
        engine.EmoStateUpdated += new EmoEngine.EmoStateUpdatedEventHandler(OnEmoStateUpdatedVORTICES);

        /*
         * Event handlers of the example to load and save user data with EMOTIV account, see coment wall at the bottom of the code
         */
        engine.UserAdded += new EmoEngine.UserAddedEventHandler(UserAddedEvent);
        engine.UserRemoved += new EmoEngine.UserRemovedEventHandler(UserRemovedEvent);
        engine.EmoEngineConnected += new EmoEngine.EmoEngineConnectedEventHandler(EmotivConnected);
        engine.EmoEngineDisconnected += new EmoEngine.EmoEngineDisconnectedEventHandler(EmotivDisconnected);


        engine.Connect ();

        /*
         * Initialices variables to store user data locally
         */
        currentPersonId = GLPlayerPrefs.GetInt("Config", "UserID");


        /*
         * Initializes the CvsCreator to store data in a log
         */
        dataLogPath = GLPlayerPrefs.GetString(Scope, "EmotivInsightDataPath");
        
        Debug.Log("emotiv path: " + GLPlayerPrefs.GetString(Scope, "EmotivInsightDataPath"));
        if (dataLogPath.Equals(""))
        {
            csvCreator = new CsvCreator("EMOTIVDataLog\\data.csv");
        }
        else
        {
            csvCreator = new CsvCreator(dataLogPath);
        }
        
	}

	/*
	 * Init the user, password and profile name if you want it
     * Used as part of the example to load and save user data with an EMOTIV account, see comment wall at the bottom of the code
     */





        /*
         * Call the ProcessEvents() method in Update once per frame
        */
    public void UpdateEmotivInsight () {
		engine.ProcessEvents ();
       
	}

	/*
	 * Close the connection on application exit
	*/
	void OnApplicationQuit() {
		Debug.Log("Application ending after " + Time.time + " seconds");
		engine.Disconnect();
	}

    #region Training Commands
    /*
	 * Several methods for handling the EmoEngine events.
	 * They are self explanatory.
	*/

    public void SetTraining(EdkDll.IEE_MentalCommandAction_t training)
    {
        engine.MentalCommandSetTrainingAction((uint)engineUserID, training);
        //engine.MentalCommandSetTrainingControl((uint)engineUserID, EdkDll.IEE_MentalCommandTrainingControl_t.MC_START);
    }

    public void StartTraining()
    {
        engine.MentalCommandSetTrainingControl((uint)engineUserID, EdkDll.IEE_MentalCommandTrainingControl_t.MC_START);
    }

    public void RejectTraining()
    {
        EdkDll.IEE_MentalCommandSetTrainingControl((uint)engineUserID, EdkDll.IEE_MentalCommandTrainingControl_t.MC_REJECT);
    }

    public void EraseTraining()
    {
        EdkDll.IEE_MentalCommandSetTrainingControl((uint)engineUserID, EdkDll.IEE_MentalCommandTrainingControl_t.MC_ERASE);
    }

    public void NoneTrainingControlCommand()
    {
        EdkDll.IEE_MentalCommandSetTrainingControl((uint)engineUserID, EdkDll.IEE_MentalCommandTrainingControl_t.MC_NONE);
    }


    public void TrainingStarted(object sender, EmoEngineEventArgs e)
    {
        Debug.Log("Trainig started");        
    }

    public void TrainingCompleted(object sender, EmoEngineEventArgs e)
    {
        Debug.Log("Training completed!!");
        csvCreator.AddLines("Training completed", "");
        //engine.MentalCommandSetTrainingControl((uint)engineUserID, EdkDll.IEE_MentalCommandTrainingControl_t.MC_ACCEPT);
    }

    public void TrainingRejected(object sender, EmoEngineEventArgs e)
    {
        Debug.Log("Trainig rejected");
        csvCreator.AddLines("Training rejected", "");
    }

    public void TrainingSucceeded(object sender, EmoEngineEventArgs e)
    {
        Debug.Log("Training Succeeded!!");
        engine.MentalCommandSetTrainingControl((uint)engineUserID, EdkDll.IEE_MentalCommandTrainingControl_t.MC_ACCEPT);
        csvCreator.AddLines("Training succeeded", "");
    }

    public void TrainingReset(object sender, EmoEngineEventArgs e)
    {
        Debug.Log("Command reseted");
        csvCreator.AddLines("Training reseted", "");
    }

    #endregion

    public void Close(){
        csvCreator.AddLines("Application closed", "");
        Application.Quit ();
	}

    /*
 * This method handle the EmoEngine update event, 
 * if the EmoState has the PUSH action, it does "something"
 * The same example could be changed to trigger different actions for all 15 mental commands.
*/
    void OnEmoStateUpdated(object sender, EmoStateUpdatedEventArgs e)
    {
        EmoState es = e.emoState;
        if (e.userId != 0)
            return;
        Debug.Log("Corrent action: " + es.MentalCommandGetCurrentAction().ToString());
        if (es.MentalCommandGetCurrentAction() == EdkDll.IEE_MentalCommandAction_t.MC_PUSH)
        {
            /*
             *An action or something is triggered 
             */
            Debug.Log("Push command detected");
        }

    }

    void OnEmoStateUpdatedVORTICES(object sender, EmoStateUpdatedEventArgs e)
    {
        EEGManager.Instance.MentalCommandCurrentAction = e.emoState.MentalCommandGetCurrentAction();
        ActionManager.Instance.EmoStateUpdate();
        EEGManager.Instance.MentalCommandCurrentActionPower = e.emoState.MentalCommandGetCurrentActionPower();
        EEGManager.Instance.FacialExpressionIsRightEyeWinking = e.emoState.FacialExpressionIsRightWink();
        EEGManager.Instance.FacialExpressionIsLeftEyeWinking = e.emoState.FacialExpressionIsLeftWink();
        EEGManager.Instance.FacialExpressionIsUserBlinking = e.emoState.FacialExpressionIsBlink();
        EEGManager.Instance.FacialExpressionUpperFaceActionPower = e.emoState.FacialExpressionGetUpperFaceActionPower();
        EEGManager.Instance.FacialExpressionSmileExtent = e.emoState.FacialExpressionGetSmileExtent();
        EEGManager.Instance.FacialExpressionLowerFaceActionPower = e.emoState.FacialExpressionGetLowerFaceActionPower();
        EEGManager.Instance.FacialExpressionLowerFaceAction = e.emoState.FacialExpressionGetLowerFaceAction();
        EEGManager.Instance.FacialExpressionUpperFaceAction = e.emoState.FacialExpressionGetUpperFaceAction();
        //All actions below are for the Log
        csvCreator.AddLines("Clench Extent: "+e.emoState.FacialExpressionGetClenchExtent().ToString(), "Facial Expression");
        csvCreator.AddLines(" Eyebrow Extent: " + e.emoState.FacialExpressionGetEyebrowExtent().ToString(), "Facial Expression");
        csvCreator.AddLines(" Lower Face Action: " + e.emoState.FacialExpressionGetLowerFaceAction().ToString(), "Facial Expression");
        csvCreator.AddLines(" Lower Face Action Power:  " + e.emoState.FacialExpressionGetLowerFaceActionPower().ToString(), "Facial Expression");
        csvCreator.AddLines(" Upper Face Action: " + e.emoState.FacialExpressionGetUpperFaceAction().ToString(), "Facial Expression");
        csvCreator.AddLines(" Upper Face Action Power: " + e.emoState.FacialExpressionGetUpperFaceActionPower().ToString(), "Facial Expression");
        csvCreator.AddLines(" Smile Extent: " + e.emoState.FacialExpressionGetSmileExtent().ToString(), "Facial Expression");
        csvCreator.AddLines(" Time since start: " + e.emoState.GetTimeFromStart().ToString(), "Time");
        csvCreator.AddLines(" Current Action: " + e.emoState.MentalCommandGetCurrentAction().ToString(), "Mental Command");
        csvCreator.AddLines(" Current Action Power: " + e.emoState.MentalCommandGetCurrentActionPower().ToString(), "Mental Command");
        csvCreator.AddLines(" Is blinking? " + e.emoState.FacialExpressionIsBlink().ToString(), "Facial Expression");
        csvCreator.AddLines(" Are eyes open? " + e.emoState.FacialExpressionIsEyesOpen().ToString(), "Facial Expression");
        csvCreator.AddLines(" Is left winking? " + e.emoState.FacialExpressionIsLeftWink().ToString(), "Facial Expression");
        csvCreator.AddLines(" Is right winking? " + e.emoState.FacialExpressionIsRightWink().ToString(), "Facial Expression");
        csvCreator.AddLines(" Is looking down? " + e.emoState.FacialExpressionIsLookingDown().ToString(), "Facial Expression");
        csvCreator.AddLines(" Is looking left? " + e.emoState.FacialExpressionIsLookingLeft().ToString(), "Facial Expression");
        csvCreator.AddLines(" Is looking right? " + e.emoState.FacialExpressionIsLookingRight().ToString(), "Facial Expression");
        csvCreator.AddLines(" Is looking up? " + e.emoState.FacialExpressionIsLookingUp().ToString(), "Facial Expression");        
    }

    void MoveTrainingStatusTextArray()
    {
        for(int i=0; i < 4; i++)
        {
            trainingStatusTextLines[i] = trainingStatusTextLines[i + 1];
        }
    }

    public void AddTrainingStatusUpdate(string statusUpdate)
    {
        if (trainingStatusTextLinesIndex < 4)
        {
            trainingStatusTextLines[trainingStatusTextLinesIndex] = statusUpdate;
            trainingStatusTextLinesIndex++;
        }
        else
        {
            MoveTrainingStatusTextArray();
            trainingStatusTextLines[trainingStatusTextLinesIndex] = statusUpdate;
        }
        trainingStatusText.text = trainingStatusTextLines[0] + "\n" +
            trainingStatusTextLines[1] + "\n" +
            trainingStatusTextLines[2] + "\n" +
            trainingStatusTextLines[3] + "\n" +
            trainingStatusTextLines[4];
    }

    public void GetActiveActions()
    {
        uint activeactions;
        EdkDll.IEE_MentalCommandGetActiveActions((uint)engineUserID, out activeactions);
        Debug.Log("Active actions: " + activeactions.ToString());
    }

    public void AddActiveCommand(EdkDll.IEE_MentalCommandAction_t command)
    {
        EdkDll.IEE_MentalCommandSetActiveActions((uint)engineUserID, (ulong)command);
        //GetActiveActions();
    }

    public void ResetTraining(EdkDll.IEE_MentalCommandAction_t command)
    {
        EdkDll.IEE_MentalCommandSetTrainingAction((uint)engineUserID, command);
        EdkDll.IEE_MentalCommandSetTrainingControl((uint)engineUserID, EdkDll.IEE_MentalCommandTrainingControl_t.MC_RESET);
    }

    

    #region Connection Cloud
    /* This functions are to connect the to the web services of EMOTIV and load/save already recorded paters. It's left here as an example copied from the original example in 
     * the Emotiv SDK. It's comented because it serves no purpose if there are going to be many users going through the system at the fastest rate possible, as creating an account, validating it,
     * logging in and then saving and loading profile would take too much time.
     * 
      
     
     * 
     * These first are for the event handlers of connection and user added or removed 
     */
    void UserAddedEvent(object sender, EmoEngineEventArgs e)
    {
        Debug.Log("User Added");
        engineUserID = (int)e.userId;
    }

    void UserRemovedEvent(object sender, EmoEngineEventArgs e)
    {
        Debug.Log("User Removed");
    }

    void EmotivConnected(object sender, EmoEngineEventArgs e)
    {
        Debug.Log("Connected!!");
    }

    void EmotivDisconnected(object sender, EmoEngineEventArgs e)
    {
        Debug.Log("Disconnected");
    }

    /*
    * These are the functions themselves that handle the connections, the message_box.text are text fields in the GUI where the user writes down his EMOTIV credentials
    */ 
     
	public bool CloudConnected()
	{
		if (EmotivCloudClient.EC_Connect () == EdkDll.EDK_OK) {
			statusText.text = "Status: Connection to server OK";
            Debug.Log("Status: Connection to server OK");
			if (EmotivCloudClient.EC_Login (userName.text, password.text)== EdkDll.EDK_OK) {
				statusText.text = "Status: Login as " + userName.text;
                Debug.Log("Status: Login as " + userName.text);
				if (EmotivCloudClient.EC_GetUserDetail (ref userCloudID) == EdkDll.EDK_OK) {
					statusText.text = "Status: CloudID: " + userCloudID;
                    Debug.Log("Status: CloudID: " + userCloudID);
                    return true;
				}
			} 
			else 
			{
				statusText.text = "Status: Cant login as " + userName.text+", check password is correct";
                Debug.Log("Status: Cant login as " + userName.text + ", check password is correct");
            }
		} 
		else 
		{
			statusText.text = "Status: Cant connect to server";
            Debug.Log("Status: Cant connect to server");
        }
		return false;
	}

	public void SaveProfile(){
		if (CloudConnected ()) {
            int profileId = -1;
			profileId = EmotivCloudClient.EC_GetProfileId (userCloudID, profileName.text, ref profileId);
			if (profileId >= 0) {
				if (EmotivCloudClient.EC_UpdateUserProfile (userCloudID, (int)engineUserID, profileId) == EdkDll.EDK_OK) {
					statusText.text = "Status: Profile updated";
                    Debug.Log("Status: Profile updated");
                } else {
					statusText.text = "Status: Error saving profile, aborting";
                    Debug.Log("Status: Error saving profile, aborting");
                }
			} else {
				if (EmotivCloudClient.EC_SaveUserProfile (
					userCloudID, engineUserID, profileName.text, 
					EmotivCloudClient.profileFileType.TRAINING) == EdkDll.EDK_OK) {
					statusText.text = "Status: Profiled saved successfully";
                    Debug.Log("Status: Profiled saved successfully");
                } else {
					statusText.text = "Status: Error saving profile, aborting";
                    Debug.Log("Status: Error saving profile, aborting");
                }
			}
		}

	}

	public void LoadProfile(){
		if (CloudConnected ()) {
            int profileId = -1;
            if (EmotivCloudClient.EC_LoadUserProfile(
                userCloudID, (int)engineUserID,                
				EmotivCloudClient.EC_GetProfileId(userCloudID, profileName.text, ref profileId), 
				(int)version) == EdkDll.EDK_OK) {
				statusText.text = "Status: Load finished";
                Debug.Log("Status: Load finished");
            } 
			else {
				statusText.text = "Status: Problem loading";
                Debug.Log("Status: Problem loading");
            }
		}
	}

    /*
    * End of connection functions
    */

    #endregion

    #region Offline Store data

    public void CheckUserStorageDataPaths()
    {
        if (userSaveDataPath.text.Equals(""))
        {
            profileNameForSavingUser = "EMOTIVDataLog\\DataUserID" + currentPersonId.ToString() + ".emu";
            userSaveDataPath.text = profileNameForSavingUser;
        }
        else
        {
            profileNameForSavingUser = userSaveDataPath.text;
        }

        if (userLoadDataPath.text.Equals(""))
        {
            profileNameForLoadingUser = "EMOTIVDataLog\\DataUserID" + currentPersonId.ToString() + ".emu";
            userLoadDataPath.text = profileNameForLoadingUser;
        }
        else
        {
            profileNameForLoadingUser = userLoadDataPath.text;
        }

        if (userOfflineID.text.Equals(""))
        {
            userOfflineStoredID = (uint)currentPersonId;
        }
        else
        {
            if (!uint.TryParse(userOfflineID.text, out userOfflineStoredID))
            {
                Debug.Log("Error reading ID number");
                statusOfflineText.text = "Error reading ID number, please check numer is an integer";
            }
        }

    }

    public void LoadProfileOffline()
    {
        CheckUserStorageDataPaths();
        if (EdkDll.IEE_LoadUserProfile(userOfflineStoredID, profileNameForLoadingUser) == EdkDll.EDK_OK)
        {
            Debug.Log("Profile successfuly loaded.");
            statusOfflineText.text = "Profile successfuly loaded.";
        }
        else
        {
            Debug.Log("Error loading profile");
            statusOfflineText.text = "Error loading profile";
        }
    }

    public void SaveProfileOffline()
    {
        CheckUserStorageDataPaths();
        if (EdkDll.IEE_SaveUserProfile(userOfflineStoredID, profileNameForSavingUser) == EdkDll.EDK_OK)
        {
            Debug.Log("Profile successfuly saved.");
            statusOfflineText.text = "Profile successfuly saved.";
        }
        else
        {
            Debug.Log("Error saving profile");
            statusOfflineText.text = "Error saving profile";
        }
    }

    #endregion

}
