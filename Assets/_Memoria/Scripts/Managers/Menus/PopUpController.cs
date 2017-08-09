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
    public GameObject acceptButton;
    bool useInputField = false;

    /// <summary>
    /// Creates a pop-up with window name, message and Accept button to close the window
    /// </summary>
    /// <param name="windowName"></param>
    /// <param name="messageText"></param>
	public void LaunchPopUpMessage(string windowName, string messageText){
        useInputField = false;
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
    /// Creates a pop-up with window name, message, accept and a cancel button. The function assigned will be triggered when the Accept button is pressed, no parameters.
    /// </summary>
    /// <param name="windowName"></param>
    /// <param name="messageText"></param>
    /// <param name="function"></param>
    public void LaunchPopUpConfirmationMessage(string windowName, string messageText, Action function)
    {
        useInputField = false;
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
        useInputField = false;
        this.messageText.text = messageText;
        this.windowName.text = windowName;
        gameObject.SetActive(true);
        popUpTopBar.SetActive(true);
        inputTextLabel.gameObject.SetActive(false);
        inputField.gameObject.SetActive(false);
        cancelButton.SetActive(true);
        SetAcceptPopUpFunction(function, true);
        this.messageText.gameObject.SetActive(true);
        acceptButton.GetComponentInChildren<Text>().text = acceptButtonText;
    }

    /// <summary>
    /// Create a pop-up with window name, message, accept and a cancel button. The function assigned will be triggered when the Accept button is pressed, with the inputField as parameter.
    /// </summary>
    /// <param name="windowName"></param>
    /// <param name="messageText"></param>
    /// <param name="function"></param>
    public void LaunchPopUpInputChangeMessage(string windowName, string labelText, Action<string> function, string inputPlaceholder, bool overwriteCurrentFunction)
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
        acceptButton.GetComponentInChildren<Text>().text = "Accept";

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
        if (confirmFunctionString == null)
        {
            confirmFunctionString = function;
            Debug.Log(function.ToString());
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

    public void ClosePopUp()
    {
        gameObject.SetActive(false);
        popUpTopBar.SetActive(false);
    }

}
