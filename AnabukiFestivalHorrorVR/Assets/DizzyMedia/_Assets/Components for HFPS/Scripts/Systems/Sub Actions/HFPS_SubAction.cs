using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

using HFPS.Systems;

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/Systems/Sub Actions/Sub Action")]
    public class HFPS_SubAction : MonoBehaviour {


    //////////////////////////
    //
    //      CLASSES
    //
    //////////////////////////


        [Serializable]
        public class SubAction_Types {

            [Space]

            public string name;
            
            [Space]
            
            public string display;
            public Sprite icon;
            public int inputSlot = -1;
            
            [Space]
            
            public UnityEvent actionEvent;

        }//SubAction_Types

        [Serializable]
        public class Sound_Library {

            public string name;

            [Space]

            public AudioSource source;
            public AudioClip clip;
            public float volume;

        }//Sound Library
        
        [Serializable]
        public class Auto {
        
            public HFPS_SubActionsUI subActionsUI;
            public HFPS_SubActionsHandler subActsHandler;

            public int curSoundSlot;
            public bool locked;
        
        }//Auto
        
        
    //////////////////////////
    //
    //      ENUMS
    //
    //////////////////////////
    
        
        public enum Action_Type {
        
            Basic,
        
        }//Action_Type
        
        public enum Action_Display {
        
            None,
            Custom,
        
        }//Action_Display
        
        public enum Attribute_Type {
        
            None,
            Heal,
            Item,
        
        }//Attribute_Type
        
        public enum Attribute_Trigger {
        
            Auto,
            Manual,
        
        }//Attribute_Trigger


    //////////////////////////
    //
    //      VALUES
    //
    //////////////////////////
    

        public Action_Type actionType;
        public Action_Display actionDisplay;
        
        public bool delayDisplay;
        public float displayWait;
        
        public List<SubAction_Types> actionTypes;

        public GameObject holder;
        
        public AnimationClip actionAnim;
        public float extraTime;
        
        public Attribute_Type attributeType;
        public Attribute_Trigger attributeTrig;
        
        public bool requireItem;
        public bool useItem;
            
        [InventorySelector]
        public int itemID;

        public List<Sound_Library> soundLibrary;


    //////////////////////////
    //
    //      AUTO
    //
    //////////////////////////


        public Auto auto;

        public int tabs;

        public bool genOpts;
        public bool actDispOpts;
        public bool animOpts;
        public bool attributeOpts;
        public bool itemOpts;
        public bool soundOpts;


    //////////////////////////
    //
    //      START ACTIONS
    //
    //////////////////////////


        void Start() {

            StartInit();

        }//Start

        public void StartInit(){
        
            if(HFPS_SubActionsUI.instance != null){
            
                auto.subActionsUI = HFPS_SubActionsUI.instance;

            }//instance != null
        
            if(HFPS_SubActionsHandler.instance != null){
            
                auto.subActsHandler = HFPS_SubActionsHandler.instance;
            
            }//instance != null

            auto.curSoundSlot = -1;
            Locked_State(false);

        }//StartInit


    //////////////////////////
    //
    //      SUB ACTION ACTIONS
    //
    //////////////////////////


        public void SubAction_Init(){

            holder.SetActive(true);

            if(actionType == Action_Type.Basic){

                SubAction_EndBuffInit();
            
            }//actionType = basic
            
            if(actionDisplay == Action_Display.Custom){
            
                if(!delayDisplay){
                
                    CustomActions_Create();
                
                //!delayDisplay
                } else {
                
                    StartCoroutine("CustomActionsCreate_Buff");
                
                }//!delayDisplay
            
            }//actionDisplay = custom
            
            if(attributeType != Attribute_Type.None){
            
                if(attributeTrig == Attribute_Trigger.Auto){
            
                    Attribute_Init();
                
                }//attributeTrig = auto
            
            }//attributeType != none

        }//SubAction_Init
        
        public void SubAction_EndBuffInit(){
        
            StartCoroutine("SubAction_EndBuff");
            
        }//SubAction_EndBuffInit

        private IEnumerator SubAction_EndBuff(){

            yield return new WaitForSeconds(actionAnim.length + extraTime);

            SubAction_End();

        }//SubAction_EndBuff

        public void SubAction_End(){
            
            if(actionDisplay == Action_Display.Custom){

                if(!delayDisplay){
                
                    CustomActions_Reset();
                
                //!delayDisplay
                } else {
                
                    StartCoroutine("CustomActionsReset_Buff");
                
                }//!delayDisplay

            }//actionDisplay = custom
                
            holder.SetActive(false);
        
        }//SubAction_End
        
        
    ///////////////////////////////
    ///
    ///     CUSTOM ACTION ACTIONS
    ///
    ///////////////////////////////
    
        
        private IEnumerator CustomActionsCreate_Buff(){
        
            yield return new WaitForSeconds(displayWait);
            
            CustomActions_Create();
            
        }//CustomActionsCreate_Buff
        
        private void CustomActions_Create(){
        
            if(actionTypes.Count > 0){
                
                auto.subActionsUI.ActionHolders_Reset();

                for(int at = 0; at < actionTypes.Count; at++){

                    if(actionTypes[at].inputSlot > -1){
                        
                        auto.subActsHandler.CustomActions_Set(actionTypes[at].inputSlot, actionTypes[at].actionEvent);
                        auto.subActionsUI.ActionHolder_Update(actionTypes[at].inputSlot, actionTypes[at].display, actionTypes[at].icon);
                        
                    }//inputSlot > -1

                }//for at actionTypes
                    
                auto.subActionsUI.Fill_Reset();
                auto.subActionsUI.Actions_Show();
                    
                auto.subActsHandler.LockDelay_Update(0.2f);
                auto.subActsHandler.LockState_Delayed(false);

            }//actionTypes.Count > 0
                
        }//CustomActions_Create
        
        private IEnumerator CustomActionsReset_Buff(){
        
            auto.subActsHandler.Locked_State(true);
            auto.subActionsUI.Actions_Hide();
        
            yield return new WaitForSeconds(displayWait);
            
            auto.subActionsUI.ActionHolders_Reset();
            CustomActions_Reset();
            
            auto.subActsHandler.InputCheck_Icons();
            auto.subActsHandler.Locked_State(false);
            
        }//CustomActionsReset_Buff
        
        private void CustomActions_Reset(){
        
            auto.subActsHandler.CustomActions_Reset();
        
        }//CustomActions_Reset
        
        
    ///////////////////////////////
    ///
    ///     ATTRIBUTE ACTIONS
    ///
    ///////////////////////////////
    
    
        public void Attribute_Init(){
        
            #if COMPONENTS_PRESENT
        
                if(attributeType != Attribute_Type.None){

                    if(attributeType == Attribute_Type.Heal){

                        if(auto.subActsHandler.refs != null){

                            if(auto.subActsHandler.refs.healthManager.Health != auto.subActsHandler.refs.healthManager.maxRegenerateHealth){

                                auto.subActsHandler.refs.healthManager.RegenCoroutine = StartCoroutine(auto.subActsHandler.refs.healthManager.Regenerate());

                            }//Health != maxRegenerateHealth

                        }//refs != null

                    }//attributeType = heal

                    if(attributeType == Attribute_Type.Item){

                        if(useItem){

                            if(Inventory.Instance.CheckItemInventory(itemID)){

                                int slotID = -1;
                                slotID = Inventory.Instance.GetItemSlotID(itemID);

                                if(slotID > -1){

                                    Inventory.Instance.UseItem(slotID);

                                }//slotID > -1

                            }//has item

                        }//useItem

                    }//attributeType = item

                }//attributeType != none
            
            #endif
        
        }//Attribute_Init


    //////////////////////////
    //
    //      SUB ACTION EVENTS
    //
    //////////////////////////


        public void SubAction_Sound(){

            soundLibrary[auto.curSoundSlot - 1].source.PlayOneShot(soundLibrary[auto.curSoundSlot - 1].clip, soundLibrary[auto.curSoundSlot - 1].volume);

        }//SubAction_Sound

        public void SoundSlot_Set(int slot){

            auto.curSoundSlot = slot;

        }//SoundSlot_Set

        public void SoundSlot_Reset(){

            auto.curSoundSlot = -1;

        }//SoundSlot_Reset

        
    //////////////////////////
    //
    //      LOCK ACTIONS
    //
    //////////////////////////


        public void Locked_State(bool state){

            auto.locked = state;

        }//Locked_State


    }//HFPS_SubAction


}//namespace