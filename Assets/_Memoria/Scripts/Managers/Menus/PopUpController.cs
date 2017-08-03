using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpController : MonoBehaviour {
	public Text messageText, windowName, inputTextLabel;
    public GameObject popUpTopBar, cancelButton;
    public Action confirmFunction;
    public Action<string> confirmFunctionString;
    public InputField inputField;
    public Button acceptButton;
    bool useInputField = false;

    /// <summary>
    /// Will immediately create a pop-up with the specified window name and message and only one Accept button to close the window
    /// </summary>
    /// <param name="windowName"></param>
    /// <param name="messageText"></param>
	public void LaunchPopUpMessage(string windowName, string messageText){
        this.messageText.text = messageText;
        this.windowName.text = windowName;
        gameObject.SetActive(true);
        popUpTopBar.SetActive(true);
        inputTextLabel.gameObject.SetActive(false);
        inputField.gameObject.SetActive(false);
        cancelButton.SetActive(false);
        this.messageText.gameObject.SetActive(true);
	}

    /// <summary>
    /// Will immediately create a pop-up with the specified window name and message and an accept and a cancel button. The function assigned will be triggered when the Accept button is pressed, no parameters.
    /// </summary>
    /// <param name="windowName"></param>
    /// <param name="messageText"></param>
    /// <param name="function"></param>
    public void LaunchPopUpConfirmationMessage(string windowName, string messageText, Action function)
    {
        this.messageText.text = messageText;
        this.windowName.text = windowName;
        gameObject.SetActive(true);
        popUpTopBar.SetActive(true);
        inputTextLabel.gameObject.SetActive(false);
        inputField.gameObject.SetActive(false);
        cancelButton.SetActive(true);
        SetAcceptPopUpFunction(function, true);
        this.messageText.gameObject.SetActive(true);
    }

    /// <summary>
    /// Overload to allow change of the accept button text.
    /// </summary>
    /// <param name="windowName"></param>
    /// <param name="messageText"></param>
    /// <param name="function"></param>
    public void LaunchPopUpConfirmationMessage(string windowName, string messageText, Action function, string acceptButtonText)
    {
        this.messageText.text = messageText;
        this.windowName.text = windowName;
        gameObject.SetActive(true);
        popUpTopBar.SetActive(true);
        inputTextLabel.gameObject.SetActive(false);
        inputField.gameObject.SetActive(false);
        cancelButton.SetActive(true);
        SetAcceptPopUpFunction(function, true);
        this.messageText.gameObject.SetActive(true);
        acceptButton.GetComponent<Text>().text = acceptButtonText;
    }

    /// <summary>
    /// Will immediately create a pop-up with the specified window name and message and an accept and a cancel button. The function assigned will be triggered when the Accept button is pressed, with the inputField as parameter.
    /// </summary>
    /// <param name="windowName"></param>
    /// <param name="messageText"></param>
    /// <param name="function"></param>
    public void LaunchPopUpInputChangeMessage(string windowName, string labelText, Action<string> function, string inputPlaceholder)
    {
        useInputField = true;
        inputField.text = inputPlaceholder;
        inputTextLabel.text = labelText;
        this.windowName.text = windowName;
        gameObject.SetActive(true);
        popUpTopBar.SetActive(true);
        inputTextLabel.gameObject.SetActive(true);
        inputField.gameObject.SetActive(true);
        cancelButton.SetActive(true);
        SetAcceptPopUpFunction(function, true);
        this.messageText.gameObject.SetActive(false);
    }

    public void AcceptButtonPress()
    {
        if (useInputField)
        {
            confirmFunctionString(inputField.text);
        }
        else
        {
            confirmFunction();
        }        
    }

    private void OnDisable()
    {
        confirmFunction = null;
        confirmFunctionString = null;
        useInputField = false;
        acceptButton.GetComponent<Text>().text = "Accept";
    }

    public bool SetAcceptPopUpFunction(Action function, bool overwriteCurrentFunction)
    {
        if (confirmFunction == null)
        {
            confirmFunction = function;
            return true;
        }
        else
        {
            if (overwriteCurrentFunction)
            {
                confirmFunction = function;
                Debug.Log("Function was already asigned in the AcceptPopUpController and was overwritten");
                return false;
            }
            else
            {
                Debug.Log("Function was already asigned in the AcceptPopUpController, function asignation failed");
                return false;
            }
        }
    }

    public bool SetAcceptPopUpFunction(Action<string> function, bool overwriteCurrentFunction)
    {
        if (confirmFunction == null)
        {
            confirmFunctionString = function;
            return true;
        }
        else
        {
            if (overwriteCurrentFunction)
            {
                confirmFunctionString = function;
                Debug.Log("Function was already asigned in the AcceptPopUpController and was overwritten");
                return false;
            }
            else
            {
                Debug.Log("Function was already asigned in the AcceptPopUpController, function asignation failed");
                return false;
            }
        }
    }

}
