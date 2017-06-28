using Emotiv;
using Memoria;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityCallbacks;
using UnityEngine;
using Windows.Kinect;

public class ActionManager : MonoBehaviour, IAwake {
    [HideInInspector]
    public static ActionManager Instance { set; get; }
    private bool initialized = false;
    protected DIOManager dioManager;
    #region Variable declaration
    [HideInInspector]
    public Action[] vorticesActionList;
    [HideInInspector]
    public Action[] bgiiesActionList;

    #endregion

    #region Emotiv Variables
    //This list has all the actions that are going to be taken on the update function, so any input pairing with an action you wish
    //  to add should be added to this list using updateActions.Add( ()=> SomeClass.SomeMethod(param1) );
    [HideInInspector]
    public List<Action> updateActionsVorticesEmotivConfig = new List<Action>();
    [HideInInspector]
    public Action[] updateActionsVorticesEmotiv;
    [HideInInspector]
    public Action[] updateActionsKinectGestures;
    [HideInInspector]
    public Action[] updateActionsNeuroSky;
    [HideInInspector]
    public float endTime = 1f;
    float tickTimer = 0f;
    float startTime = 0f;
    int emoStateTicks = 0;
    int emoStateTicksMistakes = 0;

    EdkDll.IEE_MentalCommandAction_t currentCommand;
    EdkDll.IEE_MentalCommandAction_t previousCommand;
    int emoStateNeutralTicking = 0;
    #endregion
    // Use this for initialization
    public void Awake () {
        Instance = this;
    }

    public void InitializeManager(DIOManager fatherDioManager)
    {
        dioManager = fatherDioManager;
        /*
         * This has all five actions in VORTICES, these are:
         * Accept, Inside, Outisde, PossitiveAccept, Negative Accept, zoomin and zoomout
         * Accept is select/deselect image, it works fine.
         * Inside and outside are to change planes, which is also fine         
         * zoom in and out do work, but while an image is zoomed, all the other images can be "pre-selected" with whatever lookpointer option is available
         * possitive accept and nevative accept do not work
         * The null action exist ease the use of the pairing function while taking parameters of dropdown lists of vortices actions, look NeuroSkuConfigMenu and EmotivConfigMenu
         */
        vorticesActionList = new Action[] { () => dioManager.lookPointerInstance.AcceptObject(),
            () => dioManager.MoveSphereInside(1, dioManager.initialSphereAction, dioManager.finalSphereAction),
            () => dioManager.MoveSphereOutside(1, dioManager.initialSphereAction, dioManager.finalSphereAction),            
            () => dioManager.lookPointerInstance.DirectZoomInCall(null),
            () => dioManager.lookPointerInstance.DirectZoomOutCall(null),
            null,
            () => dioManager.buttonPanel.PositiveAcceptButton(),
            () => dioManager.buttonPanel.NegativeAcceptButton()};

        bgiiesActionList = new Action[]
        {
            () => dioManager.panelBgiies.Inside(),
            () => dioManager.panelBgiies.Outside(),
            () => dioManager.panelBgiies.SelectBt1(),
            () => dioManager.panelBgiies.SelectBt2(),
            () => dioManager.panelBgiies.SelectBt3(),
            () => dioManager.panelBgiies.SelectBt4(),
            () => dioManager.lookPointerInstanceBgiies.DirectZoomInCall(null),
            () => dioManager.lookPointerInstanceBgiies.DirectZoomOutCall(null),
            null
        };

        updateActionsNeuroSky = new Action[3];
        updateActionsVorticesEmotiv = new Action[9];
        updateActionsKinectGestures = new Action[13];

        initialized = true;
    }

    public void Start()
    {
        
    }

    // Update is called once per frame
    /*
     * Here is where the actions should be added with keys or any other input (like Emotiv Mental Commands, Neurosky values or Keyboard).
     * 
     * useful functions or codes for reference:

     Input.GetKey(KeyCode.keyboardkey )
     Input.GetKeyDown(Keycode)
     Input.GetButton("ButtonName")
     Input.GetButtonDown("ButtonName")

     */

    /*
     * Emotiv neural commands for the Emotiv functions are:
     MC_NEUTRAL
     MC_PUSH
     MC_PULL
     MC_LIFT
     MC_DROP 
     MC_LEFT 
     MC_RIGHT 
     MC_ROTATE_LEFT
     MC_ROTATE_RIGHT 
     MC_ROTATE_CLOCKWISE
     MC_ROTATE_COUNTER_CLOCKWISE
     MC_ROTATE_FORWARDS 
     MC_ROTATE_REVERSE
     MC_DISAPPEAR
     */
    void Update ()
    {
        if (!initialized)
            return;
        foreach (var function in updateActionsVorticesEmotivConfig)
        {
            function();
        }

        if (EEGManager.Instance.useNeuroSky)
        {
            foreach (Action function in updateActionsNeuroSky)
            {
                if (function != null)
                    function();
            }
        }

        
        
    }

    //This function is called every time the emo state is updated. The tickTimer is there so that the functions are not all called every single update, but rather only
    //  after a fixed amount of time. If the ticks are used, it means that each fixed amount of second (1 by defaul, setted by the endTime variable) the current emo state
    //  is compared with the previous one, if it's the same, a emoStateTick is added, if it's different, it resets to 0. So, for example, if you keep the same emo state for
    //  four seconds, you would get four emoStateTicks.
    //  Then, using the overloaded version of ActionConditionEmotiv with the ticks parameter, you can set how many ticks you want the action to be triggered.

    //Say, for example, you set the PUSH command to trigger an action with 2 ticks, so for said action to be triggered the user needs to be focused for 2 seconds on
    //  the same mental command. Then you can use the PULL command to trigger an action with 4 ticks for example, to completely avoid any unwanted triggering of the function.
    //
    //The ticks update very second, but this time is a float value, you can lower (or increase) this as much as you want. At endTime 0.5f for example, you could use 5 ticks so that
    //  the mental command needs to be active for 2.5 seconds.
    //
    //In order to allow for mistakes caused either by lack of concentration, distractions or equipment, a emoStateTicksMistake counter is set. If, for example, the user needs 4 ticks with the
    //  Push command, has 3 ticks and when the 4º arrives the mental command is Lift, the emoStateTick will not reset nor rise, but the emoStateTickMistake will be reduced. So if in the next
    //  tick the user goes back to Push, it'll trigger the fourth tick, otherwise the mistake counter will continue to rise until it reaches the amount specified in the ActiveConditionEmotiv function,
    //  in which case no more mistakes are allowed and the tick counter resets. The mistake counter is reseted once a new command intention is detected
    public void EmoStateUpdate()
    {
        //Debug.Log(EEGManager.Instance.mentalCommand);
        float elapsedTime = Time.unscaledTime - startTime;
        tickTimer = endTime - elapsedTime;
        if (tickTimer <= 0f)
        {            
            if (EEGManager.Instance.MentalCommandCurrentAction == previousCommand)
            {                
                if (emoStateTicks == 0)
                {
                    //new mental command intention detected
                    //If this new command is neutral, it's not stored as current (as it should not have any actions attached)
                    if(EEGManager.Instance.MentalCommandCurrentAction != EdkDll.IEE_MentalCommandAction_t.MC_NEUTRAL)
                    {
                        currentCommand = EEGManager.Instance.MentalCommandCurrentAction;
                        emoStateTicks++;
                        emoStateTicksMistakes = 0;
                        emoStateNeutralTicking = 0;
                    }                    
                }
                else if (EEGManager.Instance.MentalCommandCurrentAction == currentCommand)
                {   emoStateTicks++;    }
                else
                {
                    emoStateTicksMistakes++;
                    if (EEGManager.Instance.MentalCommandCurrentAction == EdkDll.IEE_MentalCommandAction_t.MC_NEUTRAL)
                    {
                        //This "if" exist only as a precaution. The reset condition for the tick variable exist only in the ActionConditionEmotiv function. If by any reason there is a
                        //    mental command that IS being detected but it has NOT been included into an ActionConditionEmotiv function, it will trigger an infinite loop that would prevent
                        //    any new mental commands to be triggered. To avoid any such case, the neutral command is used to reset the tick counter, regardless of the active action.
                        emoStateNeutralTicking++;
                        if (emoStateNeutralTicking > 6)
                        {
                            emoStateNeutralTicking = 0;
                            emoStateTicks = 0;
                            emoStateTicksMistakes = 0;
                        }
                    }
                }                
            }
            else
            {
                if (EEGManager.Instance.MentalCommandCurrentAction == currentCommand)
                {   emoStateTicks++;    }
                else
                {   emoStateTicksMistakes++;    }
            }
            
            foreach (var function in updateActionsVorticesEmotiv)
            {
                function();
            }            
            previousCommand = EEGManager.Instance.MentalCommandCurrentAction;
            startTime = Time.unscaledTime;
            tickTimer = endTime;
        }        
    }

    public void KinectGestureUpdate()
    {
        foreach (Action function in updateActionsKinectGestures)
        {
            if (function != null)
                function();
        }
    }

    //Adds a condition that is a mental command in Emotiv, to be used with ActionPairing function
    public bool ActionConditionEmotiv(EdkDll.IEE_MentalCommandAction_t condition){
        if(EEGManager.Instance.MentalCommandCurrentAction == condition)
            return true;
        return false;
    }

    //Overload version of the Emotiv condition to include the ticks mechanic.
    public bool ActionConditionEmotiv(EdkDll.IEE_MentalCommandAction_t condition, int ticks, int mistakes)
    {
        if (EEGManager.Instance.MentalCommandCurrentAction == condition && emoStateTicks >= ticks)
        {
            emoStateTicks = 0;
            return true;
        }else if(mistakes <= emoStateTicksMistakes && currentCommand == condition){
            emoStateTicks = 0;
        }            
        return false;
    }

    //Another overload to accept mental power too
    public bool ActionConditionEmotiv(EdkDll.IEE_MentalCommandAction_t condition, int ticks, int mistakes, float mentalPower)
    {
        if (EEGManager.Instance.MentalCommandCurrentAction == condition && emoStateTicks >= ticks && mentalPower >= EEGManager.Instance.MentalCommandCurrentActionPower)
        {
            emoStateTicks = 0;
            return true;
        }
        else if (mistakes <= emoStateTicksMistakes && currentCommand == condition)
        {
            emoStateTicks = 0;
        }
        return false;
    }

    //another condition to accept facial expresions with power
    public bool ActionConditionEmotiv(EdkDll.IEE_FacialExpressionAlgo_t facialExpression, bool isUpperFace, float statePower)
    {
        if (EEGManager.Instance.FacialExpressionLowerFaceAction == facialExpression && EEGManager.Instance.FacialExpressionUpperFaceActionPower >= statePower && isUpperFace)
        {
            return true;
        }else if (EEGManager.Instance.FacialExpressionLowerFaceAction == facialExpression && EEGManager.Instance.FacialExpressionLowerFaceActionPower >= statePower && !isUpperFace)
        {
            return true;
        }
        return false;
    }

    //Adds a condition that is an int value that has to be greater than the second value
    public bool ActionConditionIntValueGreaterThan(ref int value, ref int threshold)
    {
        if(value > threshold)
        {
            return true;
        }
        return false;
    }

    //Adds a condition that is a key button, to be used with ActionPairing function
    public bool ActionConditionButtons(KeyCode button){
        if (Input.GetKeyDown(button))
            return true;
        return false;
    }

    //kinect hand detectioj
    public bool ActionConditionKinect(HandState gesture, bool isRightHand)
    {
        if (isRightHand)
        {
            if (gesture == KinectDetectGestures.kinectCurrentRightHandGesture)
            {
                return true;
            }
        }
        else
        {
            if (gesture == KinectDetectGestures.kinectCurrentLeftHandGesture)
            {
                return true;
            }
        }        
        return false;
    }

    public bool ActionConditionKinect(int gestureIndex, float gestureTrigger)
    {
        if (KinectGestureManager.gestureNames[gestureIndex].Equals(KinectGestureManager.currentContinuousGesture.name))
        {
            if (gestureTrigger < KinectGestureManager.currentContinuousGesture.result && KinectGestureManager.isGestureActive[gestureIndex])
            {
                return true;
            }
        }
        return false;
    }

    //A simple condition->function template to be used with an action condition and any function, meant to be added to any updateActions lists.
    public void ActionPairing(bool condition, Action function){
        if (condition)
            function();
    }

    //An overload to allow the bool to be a reference to a variable
    public void ActionPairing(ref bool condition, Action function)
    {
        if (condition)
            function();
    }

    //An overload of ActionPairing meant to add a function as a consequence of the condition being false, in case it's needed.
    public void ActionPairing(bool condition, Action function, Action consequence)
    {
        if (condition)
        {
            function();
        }
        else
        {
            consequence();
        }
            
    }


}
