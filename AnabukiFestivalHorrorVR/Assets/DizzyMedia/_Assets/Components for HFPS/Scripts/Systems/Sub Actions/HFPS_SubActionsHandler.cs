using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

using DizzyMedia.Shared;

using ThunderWire.Helpers;
using ThunderWire.Input;
using HFPS.Systems;

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/Systems/Sub Actions/Sub Actions Handler")]
    public class HFPS_SubActionsHandler : MonoBehaviour {
    
    
    //////////////////////////
    //
    //      INSTANCE
    //
    //////////////////////////
    
    
        public static HFPS_SubActionsHandler instance;


    //////////////////////////
    //
    //      CLASSES
    //
    //////////////////////////


        [Serializable]
        public class Sub_Action {

            [Space]

            public string name;    
            public DM_InternEnumsComp.SubAction_Type type;

            [Space]

            public string display;
            public Sprite icon;

            [Space]

            public HFPS_SubAction action;

            [Header("Auto")]

            public int inputSlot = -1;
            public bool isActive;
            public string actName;

        }//Sub_Action
        
        [Serializable]
        public class ActionInput_Type {
        
            [Space]
            
            public string name;
            public DM_InternEnums.PlayInput_Type input;
            
            [Space]
            
            public List<Action_Input> actionInputs;
        
        }//ActionInput_Type

        [Serializable]
        public class Action_Input {

            [Space]

            public string name;
            public string input;
            public Interact_Type inputType;
            public Input_Direction inputDirection;

            [Header("Auto")]

            public bool isActive;
            public bool isCustomAction;
            public bool isPressed;
            public bool isHolding;
            public bool isFull;
            
            [HideInInspector]
            public float tempFill;
            
            [HideInInspector]
            public float curHoldTime;
            
            [HideInInspector]
            public UnityEvent actionEvent;

        }//Action_Input
        
        [Serializable]
        public class Auto {
        
            public HFPS_SubActionsUI subActionsUI;
            public DM_InternEnums.PlayInput_Type playerInput;

            public int curAction;
            public int tempInt;
            public int curInt;
            public int tempITSlot;

            public bool isHolding;
            public float curHoldTime;
            public float tempFill;

            public bool locked;
            public float lockDelay;
        
        }//Auto


    //////////////////////////
    //
    //      ENUMS
    //
    //////////////////////////


        public enum Input_Type {

            Press = 0,
            Hold = 1,

        }//Input_Type
        
        public enum Interact_Type {
        
            Button,
            Input,
            
        }//Interact_Type
        
        public enum Input_Direction {

            Up,
            Down,
            Left,
            Right,
        
        }//Input_Direction


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////

    //////////////////////////
    //
    //      GENERAL
    //
    //////////////////////////

        
        public bool createInstance = true;
        
        public HFPS_References refs;


    //////////////////////////
    //
    //      ACTIONS
    //
    //////////////////////////


        public List<Sub_Action> subActions;
        public List<ActionInput_Type> inputTypes;

        public bool useActionDelay;
        public float actionDelay;


    //////////////////////////
    //
    //      INPUT
    //
    //////////////////////////


        public Input_Type inputType;
        public float holdTime;
        public float holdMulti;


    //////////////////////////
    //
    //      AUTO
    //
    //////////////////////////


        public Auto auto;

        public int tabs;

        public bool genOpts;
        public bool actOpts;
        public bool inputOpts;


    //////////////////////////
    //
    //      START ACTIONS
    //
    //////////////////////////


        void Awake(){
        
            if(createInstance){
            
                instance = this;
                
            }//createInstance
            
        }//awake

        void Start() {

            StartInit();

        }//Start

        public void StartInit(){

            auto.curAction = -1;
            auto.curInt = -1;
            auto.tempInt = -1;
            auto.tempITSlot = -1;
            auto.isHolding = false;
            auto.curHoldTime = 0;
            auto.tempFill = 0;

            Locked_State(true);
            LockDelay_Update(0);
            
            if(HFPS_SubActionsUI.instance != null){
            
                auto.subActionsUI = HFPS_SubActionsUI.instance;

            }//instance != null
            
            if(subActions.Count > 0){

                for(int sa = 0; sa < subActions.Count; sa++){

                    subActions[sa].inputSlot = -1;
                    subActions[sa].isActive = false;

                }//for sa subActions

            }//subActions.Count > 0

            Inputs_Reset();
            StartCoroutine("InputCheckBuff");

        }//StartInit


    //////////////////////////
    //
    //      UPDATE ACTIONS
    //
    //////////////////////////


        void Update() {

            if(!auto.locked){

                if(InputHandler.InputIsInitialized) {
                
                    InputCheck_Type();
                    
                    if(auto.tempITSlot > -1){
                    
                        if(inputTypes[auto.tempITSlot].actionInputs.Count > 0){

                            for(int ai = 0; ai < inputTypes[auto.tempITSlot].actionInputs.Count; ai++){

                                if(inputTypes[auto.tempITSlot].actionInputs[ai].isActive){

                                    if(inputTypes[auto.tempITSlot].actionInputs[ai].inputType == Interact_Type.Button){
                                        
                                        inputTypes[auto.tempITSlot].actionInputs[ai].isPressed = InputHandler.ReadButton(inputTypes[auto.tempITSlot].actionInputs[ai].input);

                                    }//inputType = button
                                    
                                    if(inputTypes[auto.tempITSlot].actionInputs[ai].inputType == Interact_Type.Input){
                                    
                                        Vector2 tempInput;

                                        if((tempInput = InputHandler.ReadInput<Vector2>(inputTypes[auto.tempITSlot].actionInputs[ai].input)) != null){
                                        
                                            if(inputTypes[auto.tempITSlot].actionInputs[ai].inputDirection == Input_Direction.Up){
                                            
                                                if(tempInput.y > 0.000001){
                                                
                                                    inputTypes[auto.tempITSlot].actionInputs[ai].isPressed = true;

                                                }//tempInput.y > 0.000001
                                            
                                            }//inputDirection = up
                                            
                                            if(inputTypes[auto.tempITSlot].actionInputs[ai].inputDirection == Input_Direction.Down){
                                            
                                                if(tempInput.y < -0.000001){
                                                
                                                    inputTypes[auto.tempITSlot].actionInputs[ai].isPressed = true;

                                                }//tempInput.y < -0.000001
                                            
                                            }//inputDirection = down
                                            
                                            if(inputTypes[auto.tempITSlot].actionInputs[ai].inputDirection == Input_Direction.Left){
                                            
                                                if(tempInput.x < -0.000001){
                                                
                                                    inputTypes[auto.tempITSlot].actionInputs[ai].isPressed = true;

                                                }//inputX < -0.000001
                                            
                                            }//inputDirection = left
                                            
                                            if(inputTypes[auto.tempITSlot].actionInputs[ai].inputDirection == Input_Direction.Right){

                                                if(tempInput.x > 0.000001){
                                                
                                                    inputTypes[auto.tempITSlot].actionInputs[ai].isPressed = true;

                                                }//inputX > 0.000001
                                            
                                            }//inputDirection = right

                                            if(inputTypes[auto.tempITSlot].actionInputs[ai].inputDirection == Input_Direction.Up | inputTypes[auto.tempITSlot].actionInputs[ai].inputDirection == Input_Direction.Down){
                                            
                                                if(tempInput.y == 0){
                                                
                                                    inputTypes[auto.tempITSlot].actionInputs[ai].isPressed = false;

                                                }//tempInput.y = 0
                                            
                                            }//inputDirection = up or down
                                            
                                            if(inputTypes[auto.tempITSlot].actionInputs[ai].inputDirection == Input_Direction.Left | inputTypes[auto.tempITSlot].actionInputs[ai].inputDirection == Input_Direction.Right){
                                        
                                                if(tempInput.x == 0){
                                                
                                                    inputTypes[auto.tempITSlot].actionInputs[ai].isPressed = false;

                                                }//inputX = 0

                                            }//inputDirection = left or right

                                        }//input catch
                                    
                                    }//inputType = input
                                    
                                    if(inputType == Input_Type.Press){

                                        if(inputTypes[auto.tempITSlot].actionInputs[ai].isPressed) {

                                            inputTypes[auto.tempITSlot].actionInputs[ai].isFull = true;

                                            if(!inputTypes[auto.tempITSlot].actionInputs[ai].isCustomAction){

                                                Locked_State(true);

                                            }//isCustomAction

                                            SubAction_Init(ai);

                                        }//isPressed

                                    }//inputType = Press

                                    if(inputType == Input_Type.Hold){

                                        if(inputTypes[auto.tempITSlot].actionInputs[ai].isPressed) {

                                            auto.tempInt = ai;

                                        //isPressed
                                        } else {

                                            if(inputTypes[auto.tempITSlot].actionInputs[ai].tempFill > 0){

                                                inputTypes[auto.tempITSlot].actionInputs[ai].curHoldTime -= holdMulti * Time.deltaTime;

                                                inputTypes[auto.tempITSlot].actionInputs[ai].tempFill = inputTypes[auto.tempITSlot].actionInputs[ai].curHoldTime / holdTime;

                                                auto.subActionsUI.Fill_Update(ai, inputTypes[auto.tempITSlot].actionInputs[ai].tempFill);

                                            }//tempFill > 0
                                        
                                        }//isPressed

                                    }//inputType = Hold

                                }//isActive

                            }//for ai actionInputs

                            if(auto.tempInt > -1){

                                if(auto.tempInt != auto.curInt){
                                    
                                    auto.curHoldTime = 0;
                                    auto.tempFill = 0;
                                    
                                    auto.curInt = auto.tempInt;
                                
                                }//tempInt != curInt

                                if(inputTypes[auto.tempITSlot].actionInputs[auto.tempInt].isPressed) {

                                    auto.isHolding = true;

                                    if(auto.isHolding && !inputTypes[auto.tempITSlot].actionInputs[auto.tempInt].isFull){

                                        auto.curHoldTime += holdMulti * Time.deltaTime;
                                        inputTypes[auto.tempITSlot].actionInputs[auto.tempInt].curHoldTime = auto.curHoldTime;

                                        if(auto.subActionsUI != null){

                                            auto.tempFill = auto.curHoldTime / holdTime;
                                            inputTypes[auto.tempITSlot].actionInputs[auto.tempInt].tempFill = auto.tempFill;

                                            auto.subActionsUI.Fill_Update(auto.tempInt, auto.tempFill);

                                        }//subActionsUI != null

                                        if(auto.curHoldTime >= holdTime){

                                            inputTypes[auto.tempITSlot].actionInputs[auto.tempInt].isFull = true;

                                            if(!inputTypes[auto.tempITSlot].actionInputs[auto.tempInt].isCustomAction){

                                                Locked_State(true);

                                            }//isCustomAction

                                            SubAction_Init(auto.tempInt);

                                        }//curHoldTime >= holdTime

                                    }//isHolding & !isFull

                                //isPressed
                                } else if(auto.isHolding) {

                                    auto.curHoldTime -= holdMulti * Time.deltaTime;
                                    inputTypes[auto.tempITSlot].actionInputs[auto.tempInt].curHoldTime = auto.curHoldTime;

                                    auto.tempFill = auto.curHoldTime / holdTime;
                                    inputTypes[auto.tempITSlot].actionInputs[auto.tempInt].tempFill = auto.tempFill;

                                    auto.subActionsUI.Fill_Update(auto.tempInt, auto.tempFill);

                                    if(auto.tempFill <= 0){

                                        inputTypes[auto.tempITSlot].actionInputs[auto.tempInt].isFull = false;

                                        auto.curHoldTime = 0;
                                        auto.tempFill = 0;
                                        
                                        inputTypes[auto.tempITSlot].actionInputs[auto.tempInt].tempFill = auto.tempFill;
                                        
                                        auto.tempInt = -1;
                                        auto.isHolding = false;

                                    }//tempFill <= 0

                                }//isHolding 

                            //tempInt > -1
                            } else {

                                InputsFull_Check();

                            }//tempInt > -1

                        }//actionInputs.Count > 0
                    
                    }//tempITSlot > -1

                }//InputIsInitialized

            }//!locked

        }//Update


    //////////////////////////
    //
    //      ACTION ACTIONS
    //
    //////////////////////////
    
    
        public void SubAction_Init(int slot){

            StartCoroutine("SubAct_InitBuff", slot);

        }//SubAction_Init

        private IEnumerator SubAct_InitBuff(int slot){
            
            if(auto.tempITSlot > -1){

                if(!inputTypes[auto.tempITSlot].actionInputs[slot].isCustomAction){

                    auto.curHoldTime = 0;
                    auto.tempFill = 0;
                    auto.tempInt = -1;
                    auto.curInt = -1;
                    
                    inputTypes[auto.tempITSlot].actionInputs[slot].tempFill = auto.tempFill;
                    inputTypes[auto.tempITSlot].actionInputs[slot].curHoldTime = auto.curHoldTime;

                    for(int sa = 0; sa < subActions.Count; sa++){

                        if(subActions[sa].inputSlot == slot){

                            auto.curAction = sa;

                            if(subActions[sa].type != DM_InternEnumsComp.SubAction_Type.Exit){

                                if(subActions[sa].action.actionType == HFPS_SubAction.Action_Type.Basic){

                                    UI_Fade(subActions[sa].action.actionAnim.length);

                                    LockDelay_Update(subActions[sa].action.actionAnim.length + subActions[sa].action.extraTime);
                                    LockState_Delayed(false);

                                //actionType = basic
                                } else {

                                    Locked_State(true);
                                    auto.subActionsUI.Actions_Hide();

                                }//actionType = basic

                                if(useActionDelay){

                                    yield return new WaitForSeconds(actionDelay);

                                }//useActionDelay

                                subActions[sa].action.SubAction_Init();

                            }//type != Exit

                            if(subActions[sa].type == DM_InternEnumsComp.SubAction_Type.Exit){

                                refs.charAction.Action_Check();

                            }//type != Exit

                        }//inputSlot = slot

                    }//for sa subActions

                //!isCustomAction
                } else {

                    auto.curAction = slot;

                    inputTypes[auto.tempITSlot].actionInputs[slot].actionEvent.Invoke();

                }//!isCustomAction
            
            }//tempITSlot > -1

        }//SubAct_InitBuff

        public void SubActions_Reset(){

            if(subActions.Count > 0){

                for(int sa = 0; sa < subActions.Count; sa++){

                    subActions[sa].inputSlot = -1;
                    subActions[sa].isActive = false;

                }//for sa subActions

            }//subActions.Count > 0

        }//SubActions_Reset

        public void Inputs_Reset(){
        
            if(inputTypes.Count > 0){
                    
                for(int it = 0; it < inputTypes.Count; it++){
                        
                    if(inputTypes[it].actionInputs.Count > 0){
                    
                        for(int ai = 0; ai < inputTypes[it].actionInputs.Count; ai++){
                        
                            inputTypes[it].actionInputs[ai].isActive = false;
                            inputTypes[it].actionInputs[ai].isCustomAction = false;
                            inputTypes[it].actionInputs[ai].isPressed = false;
                            inputTypes[it].actionInputs[ai].isHolding = false;
                            inputTypes[it].actionInputs[ai].isFull = false;

                            inputTypes[it].actionInputs[ai].actionEvent = new UnityEvent();
                        
                        }//for ai actionInputs
                    
                    }//actionInputs.Count > 0
                        
                }//for it inputTypes
                    
            }//inputTypes.Count > 0

        }//Inputs_Reset
        
        
    //////////////////////////
    //
    //      CUSTOM ACTIONS
    //
    //////////////////////////
    
    
        public void CustomActions_Set(int slot, UnityEvent newEvent){
        
            if(inputTypes.Count > 0){
                    
                for(int it = 0; it < inputTypes.Count; it++){
                        
                    if(inputTypes[it].actionInputs.Count > 0){

                        inputTypes[it].actionInputs[slot].isActive = true;
                        inputTypes[it].actionInputs[slot].isCustomAction = true;
                        inputTypes[it].actionInputs[slot].actionEvent = newEvent;
                        
                    }//actionInputs.Count > 0
                        
                }//for it inputTypes
                    
            }//inputTypes.Count > 0

        }//CustomActions_Set

        public void CustomActions_Reset(){

            if(inputTypes.Count > 0){
                    
                for(int it = 0; it < inputTypes.Count; it++){
                        
                    if(inputTypes[it].actionInputs.Count > 0){

                        for(int ai = 0; ai < inputTypes[it].actionInputs.Count; ai++){

                            inputTypes[it].actionInputs[ai].isActive = false;
                            inputTypes[it].actionInputs[ai].isCustomAction = false;
                            inputTypes[it].actionInputs[ai].actionEvent = new UnityEvent();

                        }//for ai actionInputs
                    
                    }//actionInputs.Count > 0
                        
                }//for it inputTypes
                    
            }//inputTypes.Count > 0
            
            if(subActions.Count > 0){

                for(int sa = 0; sa < subActions.Count; sa++){

                    if(subActions[sa].inputSlot > -1){
                    
                        if(inputTypes.Count > 0){

                            for(int it = 0; it < inputTypes.Count; it++){

                                if(inputTypes[it].actionInputs.Count > 0){

                                    inputTypes[it].actionInputs[subActions[sa].inputSlot].isActive = true;
                                    
                                }//actionInputs.Count > 0

                            }//for it inputTypes

                        }//inputTypes.Count > 0
                                    
                        auto.subActionsUI.ActionHolder_Update(subActions[sa].inputSlot, subActions[sa].display, subActions[sa].icon);

                    }//inputSlot > -1 & isActive

                }//for sa subActions

            }//subActions.Count > 0

            auto.curHoldTime = 0;
            auto.tempFill = 0;
            auto.tempInt = -1;
            auto.curInt = -1;
            auto.isHolding = false;

            auto.subActionsUI.Fill_Reset();
            auto.subActionsUI.Actions_Show();

            LockDelay_Update(0.2f);
            LockState_Delayed(false);

        }//CustomActions_Reset


    //////////////////////////
    //
    //      UI ACTIONS
    //
    //////////////////////////


        public void UI_Fade(float delay){

            StartCoroutine("UIFade_Buff", delay);

        }//UI_Fade

        private IEnumerator UIFade_Buff(float delay){

            if(auto.subActionsUI != null){

                auto.subActionsUI.Actions_Hide();

                yield return new WaitForSeconds(delay);
                
                if(auto.curAction > -1){
                
                    if(subActions[auto.curAction].action.requireItem){
                    
                        if(!Inventory.Instance.CheckItemInventory(subActions[auto.curAction].action.itemID)){
                        
                            auto.subActionsUI.ActionHolder_Reset(subActions[auto.curAction].inputSlot);
                            
                            if(inputTypes.Count > 0){

                                for(int it = 0; it < inputTypes.Count; it++){

                                    if(inputTypes[it].actionInputs.Count > 0){
                            
                                        inputTypes[it].actionInputs[subActions[auto.curAction].inputSlot].isActive = false;
                            
                                    }//actionInputs.Count > 0

                                }//for it inputTypes

                            }//inputTypes.Count > 0
                            
                            subActions[auto.curAction].inputSlot = -1;
                            subActions[auto.curAction].isActive = false;
                            
                            InputCheck_Icons();

                        }//does not have item
                    
                    }//requireItem
                
                }//curAction > -1
                
                if(!auto.subActionsUI.Get_Pause()){

                    auto.subActionsUI.Actions_Show();
                
                //!paused
                } else {
                
                    auto.subActionsUI.auto.menuHidden = true;
                
                }//!paused

            }//subActionsUI != null

        }//UIFade_Buff


    //////////////////////////
    //
    //      INPUT ACTIONS
    //
    //////////////////////////


        private void InputsFull_Check(){
        
            if(inputTypes.Count > 0){

                if(inputTypes[auto.tempITSlot].actionInputs.Count > 0){
        
                    for(int ai = 0; ai < inputTypes[auto.tempITSlot].actionInputs.Count; ai++){

                        if(!inputTypes[auto.tempITSlot].actionInputs[ai].isPressed && inputTypes[auto.tempITSlot].actionInputs[ai].isFull){

                            inputTypes[auto.tempITSlot].actionInputs[ai].isFull = false;

                        }//!isPressed & isFull

                    }//for ai actionInputs

                }//actionInputs.Count > 0

            }//inputTypes.Count > 0
        
        }//Full_Check

        public void InputCheck_Type(){

            if(InputHandler.CurrentDevice == InputHandler.Device.MouseKeyboard) {

                auto.playerInput = DM_InternEnums.PlayInput_Type.Keyboard;

            //deviceType = keyboard
            } else if(InputHandler.CurrentDevice.IsGamepadDevice() > 0) {

                auto.playerInput = DM_InternEnums.PlayInput_Type.Gamepad;

            }//deviceType = gamepad
            
            if(inputTypes.Count > 0){
                    
                for(int it = 0; it < inputTypes.Count; it++){
                        
                    if(inputTypes[it].input == auto.playerInput){
                            
                        auto.tempITSlot = it;
                            
                    }//input = playerInput
                        
                }//for it inputTypes
                    
            }//inputTypes.Count > 0

            auto.subActionsUI.auto.playerInput = auto.playerInput;

        }//InputCheck_Type
        
        public void InputIcons_Reset(){
        
            auto.subActionsUI.InputIcons_Reset();
        
        }//InputIcons_Reset

        public void InputCheck_Icons(){

            auto.subActionsUI.InputCheck_Icons();

        }//InputCheck_Icons

        private IEnumerator InputCheckBuff(){

            yield return new WaitForSeconds(0.2f);

            if(InputHandler.InputIsInitialized) {

                InputCheck_Type();

            }//InputIsInitialized

        }//InputCheckBuff
        
        
    //////////////////////////
    //
    //      GET ACTIONS
    //
    //////////////////////////
    
    
        public int Get_ActionSlot(DM_InternEnumsComp.SubAction_Type newType){
        
            if(subActions.Count > 0){

                for(int sa = 0; sa < subActions.Count; sa++){

                    if(subActions[sa].type == newType){

                        return sa;

                    }//type = newType

                }//for sa subActions

            }//subActions.Count > 0
            
            return -1;
        
        }//Get_ActionSlot
        
        public int Get_CurrentAction(){
        
            return auto.curAction;
        
        }//Get_CurrentAction
        
        public DM_InternEnums.PlayInput_Type Get_PlayerInput(){
        
            return auto.playerInput;
        
        }//Get_PlayerInput
        
        public int Get_PlayerInput_Int(){
        
            return (int)auto.playerInput;
        
        }//Get_PlayerInput_Int
        
        public int Get_InputTypeSlot(){
        
            return auto.tempITSlot;
            
        }//Get_InputTypeSlot


    //////////////////////////
    //
    //      LOCK ACTIONS
    //
    //////////////////////////


        public void Locked_State(bool state){

            auto.locked = state;

        }//Locked_State

        public void LockDelay_Update(float delay){

            auto.lockDelay = delay;

        }//LockDelay_Update

        public void LockState_Delayed(bool state){

            StartCoroutine("LockStateDelay_Buff", state);

        }//LockState_Delayed

        private IEnumerator LockStateDelay_Buff(bool state){

            yield return new WaitForSeconds(auto.lockDelay);
            
            if(!auto.subActionsUI.Get_Pause()){

                Locked_State(state);

            }//!paused

        }//LockStateDelay_Buff


    }//HFPS_SubActionsHandler


}//namespace