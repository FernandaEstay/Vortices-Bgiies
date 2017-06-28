using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TrainingMenu : MonoBehaviour {

    public GameObject lightNeutral;
    public GameObject lightMarcar;
    public GameObject lightAtras;
    public GameObject lightElegir;
    public GameObject lightPlus;
    public GameObject lightMinus;
    public GameObject[] rotatingFigures;
    public GameObject rotatingFigureCamera;
    Vector3 rotatingFigureOriginalPosition;
    int activeRotatingFigure;
    bool loopFigureInitiated = false;
    public Dropdown rotatingFiguresDropdown, pickTrainingDropdown;
    

    
    private Emotiv.EdkDll.IEE_MentalCommandAction_t[] mentalCommands = new Emotiv.EdkDll.IEE_MentalCommandAction_t[6];
    private bool addedPush = false, addedPull = false, addedLift = false, addedDrop = false, addedLeft = false;
    //
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /*
     * It will add 
     */
    void OnEnable()
    {
        AddTrainingControlEmotivConfig();
        Debug.Log("emotiv menu enabled");
        mentalCommands[0] = Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_NEUTRAL;
        mentalCommands[1] = Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_PUSH;
        mentalCommands[2] = Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_PULL;
        mentalCommands[3] = Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_LIFT;
        mentalCommands[4] = Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_DROP;
        mentalCommands[5] = Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_LEFT;
        //ActivateEmotivCommands();
        rotatingFigureOriginalPosition = rotatingFigures[0].transform.position;
    }

    void OnDisable()
    {
        RemoveVorticesControlEmotivConfig();
        Debug.Log("emotiv menu disabled");
    }


    private void AddTrainingEmotivLights(GameObject light, Emotiv.EdkDll.IEE_MentalCommandAction_t command)
    {
        ActionManager.Instance.updateActionsVorticesEmotivConfig.Add(() =>
               ActionManager.Instance.ActionPairing(
                   ActionManager.Instance.ActionConditionEmotiv(command),
                   light.GetComponent<ChangeColor>().ChangeGreen,
                   light.GetComponent<ChangeColor>().ChangeRed));
    }

    private void AddTrainingEmotivObjectMovement(Action movement, Emotiv.EdkDll.IEE_MentalCommandAction_t command)
    {
        ActionManager.Instance.updateActionsVorticesEmotivConfig.Add(() =>
               ActionManager.Instance.ActionPairing(
                   ActionManager.Instance.ActionConditionEmotiv(command),
                   movement));
    }

    private void AddTrainingControlEmotivConfig()
    {
        AddTrainingEmotivLights(lightNeutral, Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_NEUTRAL);
        AddTrainingEmotivLights(lightMarcar, Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_PUSH);
        AddTrainingEmotivLights(lightAtras, Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_PULL);
        AddTrainingEmotivLights(lightElegir, Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_LIFT);
        AddTrainingEmotivLights(lightPlus, Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_DROP);
        AddTrainingEmotivLights(lightMinus, Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_LEFT);

        AddTrainingEmotivObjectMovement(FigurePush, Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_PUSH);
        AddTrainingEmotivObjectMovement(FigurePull, Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_PULL);
        AddTrainingEmotivObjectMovement(FigureUp, Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_LIFT);
        AddTrainingEmotivObjectMovement(FigureDown, Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_DROP);
        AddTrainingEmotivObjectMovement(FigureLeft, Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_LEFT);

    }

    public void SetActiveTrainingCommand()
    {
        switch (pickTrainingDropdown.value)
        {
            case 0:
                AddPushTraining();
                break;
            case 1:
                AddPullTraining();
                break;
            case 2:
                AddLiftTraining();
                break;
            case 3:
                AddDropTraining();
                break;
            case 4:
                AddLeftTraining();
                break;
            case 5:
                AddNeutralTraining();
                break;
            case 6:
                SetNoTraining();
                break;

        }
    }

    void AddNeutralTraining()
    {
        EmotivCtrl.Instance.SetTraining(Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_NEUTRAL);
        EmotivCtrl.Instance.csvCreator.AddLines("Training set for Neutral", "");
        EmotivCtrl.Instance.AddTrainingStatusUpdate("Training set for Neutral");
    }

    void SetNoTraining()
    {
        EmotivCtrl.Instance.NoneTrainingControlCommand();
        EmotivCtrl.Instance.csvCreator.AddLines("Training set for None", "");
        EmotivCtrl.Instance.AddTrainingStatusUpdate("Training set for None");
    }

    void AddPushTraining()
    {
        if (!addedPush)
        {
            addedPush = true;
            EmotivCtrl.Instance.AddActiveCommand(Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_PUSH);
        }            
        EmotivCtrl.Instance.SetTraining(Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_PUSH);
        EmotivCtrl.Instance.csvCreator.AddLines("Training set for Push", "");
        EmotivCtrl.Instance.AddTrainingStatusUpdate("Training set for Push");
    }

    void AddPullTraining()
    {       if (!addedPull)
        {
            addedPull = true;
            EmotivCtrl.Instance.AddActiveCommand(Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_PULL);
        }
        EmotivCtrl.Instance.SetTraining(Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_PULL);
        EmotivCtrl.Instance.csvCreator.AddLines("Training set for Pull", "");
        EmotivCtrl.Instance.AddTrainingStatusUpdate("Training set for Pull");
    }

    void AddLiftTraining()
    {
        if (!addedLift)
        {
            addedLift = true;
            EmotivCtrl.Instance.AddActiveCommand(Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_LIFT);
        }
        EmotivCtrl.Instance.SetTraining(Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_LIFT);
        EmotivCtrl.Instance.csvCreator.AddLines("Training set for Lift", "");
        EmotivCtrl.Instance.AddTrainingStatusUpdate("Training set for Lift");

    }

    void AddDropTraining()
    {
        if (!addedDrop)
        {
            addedDrop = true;
            EmotivCtrl.Instance.AddActiveCommand(Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_DROP);
        }
        EmotivCtrl.Instance.SetTraining(Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_DROP);
        EmotivCtrl.Instance.csvCreator.AddLines("Training set for Drop", "");
        EmotivCtrl.Instance.AddTrainingStatusUpdate("Training set for Drop");
    }

    void AddLeftTraining()
    {
        if (!addedLeft)
        {
            addedLeft = true;
            EmotivCtrl.Instance.AddActiveCommand(Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_LEFT);
        }
        EmotivCtrl.Instance.SetTraining(Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_LEFT);
        EmotivCtrl.Instance.csvCreator.AddLines("Training set for Left", "");
        EmotivCtrl.Instance.AddTrainingStatusUpdate("Training set for Left");
    }

    private IEnumerator ResetTrainingCoroutine()
    {
        //resets all the training
        for (int i = 0;  i< 6; i++)
        {
            EmotivCtrl.Instance.ResetTraining(mentalCommands[i]);
            yield return null;
        }
    }

    public void ResetTraining()
    {
        StartCoroutine("ResetTrainingCoroutine");
    }

    void RemoveVorticesControlEmotivConfig()
    {
        ActionManager.Instance.updateActionsVorticesEmotivConfig.Clear();
    }

    public void ChangeRotatingFigure()
    {
        rotatingFigures[activeRotatingFigure].SetActive(false);
        activeRotatingFigure = rotatingFiguresDropdown.value;
        rotatingFigures[activeRotatingFigure].SetActive(true);
    }

    public void FigurePush()
    {
        if (loopFigureInitiated)
            return;
        loopFigureInitiated = true;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().velocity = Vector3.zero;
        rotatingFigures[activeRotatingFigure].GetComponentInParent<Animation>().enabled = false;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().AddForce(rotatingFigureCamera.GetComponent<Camera>().transform.forward * 10, ForceMode.Impulse);
        StartCoroutine("SetTimerResetFigure", 2.3f);
        
    }

    public void FigurePull()
    {
        if (loopFigureInitiated)
            return;
        loopFigureInitiated = true;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().velocity = Vector3.zero;
        rotatingFigures[activeRotatingFigure].GetComponentInParent<Animation>().enabled = false;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().AddForce(rotatingFigureCamera.GetComponent<Camera>().transform.forward * -10, ForceMode.Impulse);
        StartCoroutine("SetTimerResetFigure", 2.3f);
        
    }

    public void FigureLeft()
    {
        if (loopFigureInitiated)
            return;
        loopFigureInitiated = true;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().velocity = Vector3.zero;
        rotatingFigures[activeRotatingFigure].GetComponentInParent<Animation>().enabled = false;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().AddForce(rotatingFigureCamera.GetComponent<Camera>().transform.right * -10, ForceMode.Impulse);
        StartCoroutine("SetTimerResetFigure", 2.3f);
        
    }

    public void FigureRight()
    {
        if (loopFigureInitiated)
            return;
        loopFigureInitiated = true;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().velocity = Vector3.zero;
        rotatingFigures[activeRotatingFigure].GetComponentInParent<Animation>().enabled = false;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().AddForce(rotatingFigureCamera.GetComponent<Camera>().transform.right * 10, ForceMode.Impulse);
        StartCoroutine("SetTimerResetFigure", 2.3f);
        
    }

    public void FigureUp()
    {
        if (loopFigureInitiated)
            return;
        loopFigureInitiated = true;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().velocity = Vector3.zero;
        rotatingFigures[activeRotatingFigure].GetComponentInParent<Animation>().enabled = false;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().AddForce(rotatingFigureCamera.GetComponent<Camera>().transform.up * 10, ForceMode.Impulse);
        StartCoroutine("SetTimerResetFigure", 2.3f);
        
    }

    public void FigureDown()
    {
        if (loopFigureInitiated)
            return;
        loopFigureInitiated = true;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().velocity = Vector3.zero;
        rotatingFigures[activeRotatingFigure].GetComponentInParent<Animation>().enabled = false;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().AddForce(rotatingFigureCamera.GetComponent<Camera>().transform.up * -10, ForceMode.Impulse);
        StartCoroutine("SetTimerResetFigure", 2.3f);
        
    }

    private IEnumerator SetTimerResetFigure(float time)
    {
        float startTime = Time.unscaledTime;
        while ((Time.unscaledTime - startTime)<time)
        {
            yield return null;
        }
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().velocity = Vector3.zero;
        rotatingFigures[activeRotatingFigure].transform.position = rotatingFigureOriginalPosition;
        rotatingFigures[activeRotatingFigure].GetComponentInParent<Animation>().enabled = true;
        loopFigureInitiated = false;
        
    }

    
}
