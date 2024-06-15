using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace DizzyMedia.HFPS_Components {

    [CustomEditor(typeof(HFPS_SubAction))]
    public class HFPS_SubActionEditor : Editor {


    //////////////////////////
    //
    //      EDITOR DISPLAY
    //
    //////////////////////////


        HFPS_SubAction subAct;
        GUISkin oldSkin;

        public bool showTips;

        private void OnEnable() {

            subAct = (HFPS_SubAction)target;

        }//OnEnable

        public override void OnInspectorGUI() {

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.Space(15); 

            EditorGUI.BeginChangeCheck();
            
            SerializedProperty actionTypeRef = serializedObject.FindProperty("actionType");
            SerializedProperty actionDisplayRef = serializedObject.FindProperty("actionDisplay");
            
            SerializedProperty actionTypes = serializedObject.FindProperty("actionTypes");

            SerializedProperty holder = serializedObject.FindProperty("holder");
            SerializedProperty actionAnim = serializedObject.FindProperty("actionAnim");
            
            SerializedProperty attributeTypeRef = serializedObject.FindProperty("attributeType");
            SerializedProperty attributeTrigRef = serializedObject.FindProperty("attributeTrig");
            
            SerializedProperty itemID = serializedObject.FindProperty("itemID");

            SerializedProperty soundLibrary = serializedObject.FindProperty("soundLibrary");
            
            SerializedProperty subActionsUI = serializedObject.FindProperty("auto.subActionsUI");
            SerializedProperty subActsHandler = serializedObject.FindProperty("auto.subActsHandler");

            var style = new GUIStyle(EditorStyles.largeLabel) {alignment = TextAnchor.MiddleCenter};

            if(oldSkin == null){

                if(oldSkin != Resources.Load("EditorContent/Components Skin") as GUISkin){

                    oldSkin = GUI.skin;

                    //Debug.Log("Old Skin Name " + GUI.skin.name);

                }//oldSkin != Components Skin

            }//oldSkin == null

            GUI.skin = Resources.Load("EditorContent/Components Skin") as GUISkin;

            Texture2D t = (Texture2D)Resources.Load("EditorContent/Components-Editor-Icon");
            Texture2D t2 = (Texture2D)Resources.Load("EditorContent/DM_InfoIcon");
            Texture2D t3 = (Texture2D)Resources.Load("EditorContent/DM_InfoIconActive");

            GUILayout.BeginHorizontal("Sub Action", "HeaderText");

            GUILayout.Label(t, "headerIcon");

            GUILayout.FlexibleSpace();

            if(!showTips){

                if(GUILayout.Button(t2, "infoIcon")){

                    ShowTips_Check();

                }//Button

            }//!showTips

            if(showTips){

                if(GUILayout.Button(t3, "infoIcon")){

                    ShowTips_Check();

                }//Button

            }//showTips

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);

            GUI.skin = oldSkin;

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical();

            subAct.tabs = GUILayout.SelectionGrid(subAct.tabs, new string[] { "User Options", "Auto/Debug"}, 2);

            if(subAct.tabs == 0){

                EditorGUILayout.Space();

                subAct.genOpts = GUILayout.Toggle(subAct.genOpts, "General Options", GUI.skin.button);

                if(subAct.genOpts){
                
                    EditorGUILayout.Space();
                
                    EditorGUILayout.PropertyField(actionTypeRef, true);
                    EditorGUILayout.PropertyField(actionDisplayRef, true);

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Main parent object for sub action." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(holder, true);

                }//genOpts

                EditorGUILayout.Space();

                subAct.animOpts = GUILayout.Toggle(subAct.animOpts, "Animation Options", GUI.skin.button);

                if(subAct.animOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Animation used for sub action." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(actionAnim, true);

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "Extra wait time after animation ends before hiding parent object." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    subAct.extraTime = EditorGUILayout.FloatField("Extra Time", subAct.extraTime);

                }//animOpts
                
                EditorGUILayout.Space();

                subAct.attributeOpts = GUILayout.Toggle(subAct.attributeOpts, "Attribute Options", GUI.skin.button);

                if(subAct.attributeOpts){

                    EditorGUILayout.Space();
                    
                    EditorGUILayout.PropertyField(attributeTypeRef, true);
                    
                    if(subAct.attributeType != HFPS_SubAction.Attribute_Type.None){
                    
                        EditorGUILayout.PropertyField(attributeTrigRef, true);
                    
                    }//attributeType != none
                    
                }//attributeOpts
                
                if(subAct.actionDisplay == HFPS_SubAction.Action_Display.Custom){
                
                    EditorGUILayout.Space();

                    subAct.actDispOpts = GUILayout.Toggle(subAct.actDispOpts, "Display Options", GUI.skin.button);

                    if(subAct.actDispOpts){

                        EditorGUILayout.Space();
                        
                        subAct.delayDisplay = EditorGUILayout.Toggle("Delay Display?", subAct.delayDisplay);
                        
                        if(subAct.delayDisplay){
                        
                            subAct.displayWait = EditorGUILayout.FloatField("Display Wait", subAct.displayWait);
                        
                        }//delayDisplay
                        
                        EditorGUILayout.Space();

                        EditorGUILayout.PropertyField(actionTypes, true);

                    }//actDispOpts
                
                }//actionDisplay = custom
                
                EditorGUILayout.Space();

                subAct.itemOpts = GUILayout.Toggle(subAct.itemOpts, "Item Options", GUI.skin.button);

                if(subAct.itemOpts){

                    EditorGUILayout.Space();
                    
                    subAct.requireItem = EditorGUILayout.Toggle("Require Item?", subAct.requireItem);
                    
                    if(subAct.requireItem){
                    
                        subAct.useItem = EditorGUILayout.Toggle("Use Item?", subAct.useItem);
                        
                        EditorGUILayout.Space();
                    
                        EditorGUILayout.PropertyField(itemID, true);
                    
                    }//requireItem
                    
                }//itemOpts

                EditorGUILayout.Space();

                subAct.soundOpts = GUILayout.Toggle(subAct.soundOpts, "Sounds Options", GUI.skin.button);

                if(subAct.soundOpts){

                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(soundLibrary, true);

                }//soundOpts

            }//tabs = user options

            if(subAct.tabs == 1){

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Automatic Values", EditorStyles.centeredGreyMiniLabel);

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "These values are automatically handled by the system." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(subActionsUI, true);
                EditorGUILayout.PropertyField(subActsHandler, true);
                
                EditorGUILayout.Space();
                
                subAct.auto.curSoundSlot = EditorGUILayout.IntField("Current Sound Slot", subAct.auto.curSoundSlot);
                subAct.auto.locked = EditorGUILayout.Toggle("Locked?", subAct.auto.locked);

            }//tabs = auto 

            EditorGUILayout.Space();

            if(EditorGUI.EndChangeCheck()){

                serializedObject.ApplyModifiedProperties();

            }//EndChangeCheck

            if(GUI.changed){

                EditorUtility.SetDirty(subAct);

                if(!EditorApplication.isPlaying){

                    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

                }//!isPlaying

            }//changed

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

        }//OnInspectorGUI


    //////////////////////////
    //
    //      TIPS ACTIONS
    //
    //////////////////////////


        public void ShowTips_Check(){

            if(showTips){

                showTips = false;

            //showTips
            } else {

                showTips = true;

            }//showTips

        }//ShowTips_Check


    }//HFPS_SubActionEditor


}//namespace