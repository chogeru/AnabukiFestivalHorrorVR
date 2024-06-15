using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using DizzyMedia.Shared;

using ThunderWire.Input;
using HFPS.Systems;

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/Systems/Sub Actions/Sub Actions UI")]
    public class HFPS_SubActionsUI : MonoBehaviour {


    //////////////////////////
    //
    //      INSTANCE
    //
    //////////////////////////


        public static HFPS_SubActionsUI instance;


    //////////////////////////
    //
    //      CLASSES
    //
    //////////////////////////


        [Serializable]
        public class ActionHolder {

            [Space]

            public string name;

            [Space]

            public GameObject holder;
            public Image icon;
            public Image highlight;
            public Text text;

            [Space]

            public bool showInput;
            public List<Input_Holder> inputHolders;

            [Space]

            [Header("Auto")]

            public bool isActive;

        }//ActionHolder

        [Serializable]
        public class Input_Holder {

            [Space]

            public string name;
            public DM_InternEnums.PlayInput_Type input;

            [Space]

            public GameObject holder;
            public Image inputImage;

            public Sprite noInputSprite;
            public Sprite inputSprite;

        }//Input_Holder
        
        [Serializable]
        public class Auto {
        
            public DM_InternEnums.PlayInput_Type playerInput;
            
            public bool menuOpen;
            public bool menuHidden;
            
            public bool pauseKeyPressed;
            public bool pauseBackKeyPressed;
            public bool invKeyPressed;
            public bool pausedLocked;
            public bool paused;
            public bool inventoryOpen;
            
            public bool locked;
        
        }//Auto
        
        
    //////////////////////////////////////
    ///
    ///     ENUMS
    ///
    ///////////////////////////////////////
    
        
        public enum PauseType {

            Regular = 0,
            Inventory = 1,

        }//PauseType


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


        public GameObject holder;


    /////////////////////////
    ///
    ///     INPUT OPTIONS
    ///
    /////////////////////////


        public bool detectPause = true;
        public string pauseInput = "Pause";
        
        public bool detectPauseBack = true;
        public string pauseBackInput = "Crouch";
        
        public bool detectInventory = true;
        public string invInput = "Inventory";


    //////////////////////////
    //
    //      ANIMATION
    //
    //////////////////////////


        public Animator anim;
        public AnimationClip showAnim;
        public AnimationClip hideAnim;


    //////////////////////////
    //
    //      ACTIONS
    //
    //////////////////////////


        public List<ActionHolder> actionHolders;


    //////////////////////////
    //
    //      AUTO
    //
    //////////////////////////


        public Auto auto;

        public int tabs;

        public bool genOpts;
        public bool inputOpts;
        public bool animOpts;
        public bool actOpts;


    //////////////////////////
    //
    //      START ACTIONS
    //
    //////////////////////////


        void Awake(){

            instance = this;

        }//Awake

        void Start() {

            StartInit();

        }//Start
        
        public void StartInit(){
        
            auto.menuOpen = false;
            auto.menuHidden = false;
            
            auto.pauseKeyPressed = false;
            auto.pauseBackKeyPressed = false;
            auto.invKeyPressed = false;
            
            auto.pausedLocked = false;
            auto.paused = false;
            auto.inventoryOpen = false;
            
            InputIcons_Reset();
            InputHolders_Reset();
            Locked_State(false);
            
        }//StartInit


    //////////////////////////////////////
    ///
    ///     UPDATE ACTIONS
    ///
    ///////////////////////////////////////


        void Update() {

            if(!auto.locked){

                if(detectPause){

                    if(InputHandler.InputIsInitialized) {

                        auto.pauseKeyPressed = InputHandler.ReadButton(pauseInput);

                    }//InputIsInitialized

                    if(auto.pauseKeyPressed){

                        if(!auto.pausedLocked){

                            auto.pausedLocked = true;

                            Pause_Check();

                        }//!pausedLocked

                    }//pauseKeyPressed

                    if(detectPause){

                        if(!auto.pauseKeyPressed && !auto.invKeyPressed){

                            auto.pausedLocked = false;

                        }//!pauseKeyPressed & !invKeyPressed

                    }//detectPause

                }//detectPause

                if(detectPauseBack){
                
                    if(auto.paused | auto.inventoryOpen){

                        if(InputHandler.InputIsInitialized) {

                            auto.pauseBackKeyPressed = InputHandler.ReadButton(pauseBackInput);

                        }//InputIsInitialized

                        if(auto.pauseBackKeyPressed){

                            BackCheck();

                        }//pauseBackKeyPressed
                    
                    }//paused or inventoryOpen

                }//detectPauseBack
                
                if(detectInventory){
                
                    if(!auto.paused){

                        if(InputHandler.InputIsInitialized) {

                            auto.invKeyPressed = InputHandler.ReadButton(invInput);

                        }//InputIsInitialized

                        if(auto.invKeyPressed){

                            if(!auto.pausedLocked){

                                auto.pausedLocked = true;
                                
                                Pause_Check();

                            }//!pausedLocked

                        }//invKeyPressed & menuOpen

                        if(detectInventory){

                            if(!auto.invKeyPressed && !auto.pauseKeyPressed){

                                auto.pausedLocked = false;

                            }//!invKeyPressed & !pauseKeyPressed

                        }//detectInventory
                    
                    }//!paused

                }//detectInventory

            }//!locked

        }//Update


    //////////////////////////
    //
    //      ACTION HOLDER ACTIONS
    //
    //////////////////////////


        public void ActionHolder_Reset(int slot){
        
            actionHolders[slot].holder.SetActive(false);
            
            if(actionHolders[slot].icon != null){

                actionHolders[slot].icon.sprite = null;

            }//icon != null

            if(actionHolders[slot].highlight != null){

                actionHolders[slot].highlight.fillAmount = 0;

            }//highlight != null

            if(actionHolders[slot].text != null){

                actionHolders[slot].text.text = "";

            }//text != null

            actionHolders[slot].isActive = false;
        
        }//ActionHolder_Reset

        public void ActionHolders_Reset(){

            for(int ah = 0; ah < actionHolders.Count; ah++) {

                actionHolders[ah].holder.SetActive(false);

                if(actionHolders[ah].icon != null){

                    actionHolders[ah].icon.sprite = null;

                }//icon != null

                if(actionHolders[ah].highlight != null){

                    actionHolders[ah].highlight.fillAmount = 0;

                }//highlight != null

                if(actionHolders[ah].text != null){

                    actionHolders[ah].text.text = "";

                }//text != null

                actionHolders[ah].isActive = false;

            }//for ah actionHolders

        }//ActionHolders_Reset

        public void ActionHolder_Update(int slot, string display, Sprite icon){

            actionHolders[slot].holder.SetActive(true);

            if(actionHolders[slot].icon != null){

                actionHolders[slot].icon.sprite = icon;

            }//images != null

            if(actionHolders[slot].text != null){

                actionHolders[slot].text.text = display;

            }//text != null

            if(actionHolders[slot].highlight != null){

                actionHolders[slot].highlight.fillAmount = 0;

            }//highlight != null

            actionHolders[slot].isActive = true;

        }//ActionHolder_Update


    //////////////////////////
    //
    //      FILL ACTIONS
    //
    //////////////////////////


        public void Fill_Update(int slot, float amount){

            if(actionHolders[slot].highlight != null){

                actionHolders[slot].highlight.fillAmount = amount;

            }//highlight != null

            if(actionHolders[slot].showInput){

                InputHolder_Update(slot);

            }//showInput

        }//Fill_Update

        public void Fill_Reset(){

            for(int ah = 0; ah < actionHolders.Count; ah++) {

                if(actionHolders[ah].highlight != null){

                    actionHolders[ah].highlight.fillAmount = 0;

                }//highlight != null

                if(actionHolders[ah].showInput){

                    if(actionHolders[ah].highlight.fillAmount > 0){

                        if(actionHolders[ah].inputHolders[(int)auto.playerInput].inputImage.sprite != actionHolders[ah].inputHolders[(int)auto.playerInput].inputSprite){

                            actionHolders[ah].inputHolders[(int)auto.playerInput].inputImage.sprite = actionHolders[ah].inputHolders[(int)auto.playerInput].inputSprite;

                        }//sprite != inputSprite

                    //fillAmount
                    } else {

                        if(actionHolders[ah].inputHolders[(int)auto.playerInput].inputImage.sprite != actionHolders[ah].inputHolders[(int)auto.playerInput].noInputSprite){

                            actionHolders[ah].inputHolders[(int)auto.playerInput].inputImage.sprite = actionHolders[ah].inputHolders[(int)auto.playerInput].noInputSprite;

                        }//sprite != noInputSprite

                    }//fillAmount

                }//showInput

            }//for ah actionHolders

        }//Fill_Reset


    //////////////////////////
    //
    //      ANIMATION ACTIONS
    //
    //////////////////////////


        public void Actions_Show(){
        
            StopCoroutine("HideBuff");

            holder.SetActive(true);
            anim.Play(showAnim.name);
            
            if(!auto.menuOpen && auto.menuHidden){
            
                #if COMPONENTS_PRESENT

                    if(!Get_Pause()){
                    
                        HFPS_References.instance.subActionsHandler.Locked_State(false);
                        
                    }//!paused
                    
                #endif
            
            }//!menuOpen & menuHidden

            auto.menuOpen = true;
            auto.menuHidden = false;

        }//Actions_Show

        public void Actions_ShowSolo(){

            if(!auto.menuOpen && auto.menuHidden){

                holder.SetActive(true);

                auto.menuOpen = true;
                auto.menuHidden = false;

                #if COMPONENTS_PRESENT

                    if(!Get_Pause()){
                    
                        HFPS_References.instance.subActionsHandler.Locked_State(false);

                    }//!paused
                    
                #endif

            }//!menuOpen & menuHidden

        }//Actions_ShowSolo

        public void Actions_Hide(){

            if(holder.activeSelf){

                anim.Play(hideAnim.name);

                StartCoroutine("HideBuff");

            }//activeSelf

        }//Actions_Hide

        private IEnumerator HideBuff(){

            yield return new WaitForSeconds(hideAnim.length + 0.2f);

            holder.SetActive(false);

            InputHolders_Reset();
            Fill_Reset();

            auto.menuOpen = false;

        }//HideBuff

        public void Actions_HideSolo(){

            holder.SetActive(false);

            auto.menuOpen = false;
            auto.menuHidden = true;

            #if COMPONENTS_PRESENT

                HFPS_References.instance.subActionsHandler.Locked_State(true);
                
            #endif
            
        }//Actions_HideSolo


    //////////////////////////
    //
    //      INPUT ACTIONS
    //
    //////////////////////////


        public void InputHolder_Update(int slot){

            if(actionHolders[slot].highlight.fillAmount > 0){

                if(actionHolders[slot].inputHolders[(int)auto.playerInput].inputImage.sprite != actionHolders[slot].inputHolders[(int)auto.playerInput].inputSprite){

                    actionHolders[slot].inputHolders[(int)auto.playerInput].inputImage.sprite = actionHolders[slot].inputHolders[(int)auto.playerInput].inputSprite;

                }//sprite != inputSprite

            //fillAmount
            } else {

                if(actionHolders[slot].inputHolders[(int)auto.playerInput].inputImage.sprite != actionHolders[slot].inputHolders[(int)auto.playerInput].noInputSprite){

                    actionHolders[slot].inputHolders[(int)auto.playerInput].inputImage.sprite = actionHolders[slot].inputHolders[(int)auto.playerInput].noInputSprite;

                }//sprite != noInputSprite

            }//fillAmount

        }//InputHolder_Update

        public void InputHolders_Reset(){

            for(int ah = 0; ah < actionHolders.Count; ah++) {

                if(actionHolders[ah].inputHolders[(int)auto.playerInput].inputImage.sprite != actionHolders[ah].inputHolders[(int)auto.playerInput].noInputSprite){

                    actionHolders[ah].inputHolders[(int)auto.playerInput].inputImage.sprite = actionHolders[ah].inputHolders[(int)auto.playerInput].noInputSprite;

                }//sprite != noInputSprite

            }//for ah actionHolders

        }//InputHolders_Reset
        
        public void InputIcons_Reset(){
        
            if(auto.menuOpen | auto.menuHidden){
        
                //Debug.Log("Input Icons Reset");

                for(int ah = 0; ah < actionHolders.Count; ah++) {

                    for(int ih = 0; ih < actionHolders[ah].inputHolders.Count; ih++) {

                        if(actionHolders[ah].inputHolders[ih].holder.activeSelf){

                            actionHolders[ah].inputHolders[ih].holder.SetActive(false);

                        }//activeSelf    

                    }//for ih inputHolders

                }//for ah actionHolders
            
            }//menuOpen or menu hidden
        
        }//InputIcons_Reset

        public void InputCheck_Icons(){
        
            if(auto.menuOpen | auto.menuHidden){
            
                //Debug.Log("Input Icons Check");

                for(int ah = 0; ah < actionHolders.Count; ah++) {

                    if(actionHolders[ah].isActive){

                        for(int ih = 0; ih < actionHolders[ah].inputHolders.Count; ih++) {

                            if(actionHolders[ah].inputHolders[ih].input == auto.playerInput){

                                if(!actionHolders[ah].inputHolders[ih].holder.activeSelf){

                                    actionHolders[ah].inputHolders[ih].holder.SetActive(true);

                                }//!activeSelf

                            //input = playerInput
                            } else {

                                if(actionHolders[ah].inputHolders[ih].holder.activeSelf){

                                    actionHolders[ah].inputHolders[ih].holder.SetActive(false);

                                }//activeSelf

                            }//input = playerInput

                        }//for ih inputHolders

                    }//isActive

                }//for ah actionHolders
            
            }//menu open or menu hidden

        }//InputCheck_Icons
        
        
    //////////////////////////
    //
    //      PAUSE ACTIONS
    //
    //////////////////////////
    
        
        public void Pause_Check(){
        
            auto.paused = HFPS_GameManager.Instance.isPaused;
            auto.inventoryOpen = HFPS_GameManager.Instance.isInventoryShown;
        
            if(auto.menuOpen){
            
                if(auto.paused | auto.inventoryOpen){

                    Actions_HideSolo();
                
                }//pause or inventoryOpen

            //menuOpen
            } else if(auto.menuHidden){
            
                if(!auto.paused && !auto.inventoryOpen){
                
                    #if COMPONENTS_PRESENT

                        HFPS_References.instance.subActionsHandler.InputCheck_Type();

                    #endif

                    InputIcons_Reset();
                    InputCheck_Icons();

                    Actions_Show();
                
                }//!paused & !inventoryOpen

            }//menuHidden
                            
        }//Pause_Check
        
        public void BackCheck(){

            if(auto.paused | auto.inventoryOpen){
                
                auto.paused = HFPS_GameManager.Instance.isPaused;
                auto.inventoryOpen = HFPS_GameManager.Instance.isInventoryShown;

                if(!auto.paused | !auto.inventoryOpen){

                    auto.pausedLocked = true;
                        
                    Pause_Check();

                    StartCoroutine("Pause_Buff");

                }//!paused & !inventoryOpen
            
            }//paused or inventoryOpen
            
        }//BackCheck
        
        private IEnumerator Pause_Buff(){
        
            yield return new WaitForSeconds(0.1f);
            
            auto.pauseBackKeyPressed = false;
            auto.pausedLocked = false;
            
        }//Pause_Buff
        
        public bool Get_Pause(){
        
            return auto.paused;
            
        }//Get_Pause
        
        
    //////////////////////////
    //
    //      LOCK ACTIONS
    //
    //////////////////////////


        public void Locked_State(bool state){

            auto.locked = state;

        }//Locked_State


    }//HFPS_SubActionsUI


}//namespace