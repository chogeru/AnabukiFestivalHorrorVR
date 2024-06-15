using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace DizzyMedia.HFPS_Components {

    [CustomEditor(typeof(HFPS_SubActionsUI))]
    public class HFPS_SubActionsUIEditor : Editor {


    //////////////////////////
    //
    //      EDITOR DISPLAY
    //
    //////////////////////////


        HFPS_SubActionsUI subActsUI;
        GUISkin oldSkin;

        public bool showTips;

        private void OnEnable() {

            subActsUI = (HFPS_SubActionsUI)target;

        }//OnEnable

        public override void OnInspectorGUI() {

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.Space(15); 

            EditorGUI.BeginChangeCheck();

            SerializedProperty holder = serializedObject.FindProperty("holder");

            SerializedProperty pauseInput = serializedObject.FindProperty("pauseInput");
            SerializedProperty pauseBackInput = serializedObject.FindProperty("pauseBackInput");
            SerializedProperty invInput = serializedObject.FindProperty("invInput");

            SerializedProperty anim = serializedObject.FindProperty("anim");
            SerializedProperty showAnim = serializedObject.FindProperty("showAnim");
            SerializedProperty hideAnim = serializedObject.FindProperty("hideAnim");

            SerializedProperty actionHolders = serializedObject.FindProperty("actionHolders");

            SerializedProperty playerInput = serializedObject.FindProperty("auto.playerInput");

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

            GUILayout.BeginHorizontal("Sub Actions UI", "HeaderText_Small");

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

            subActsUI.tabs = GUILayout.SelectionGrid(subActsUI.tabs, new string[] { "User Options", "Auto/Debug"}, 2);

            if(subActsUI.tabs == 0){

                EditorGUILayout.Space();

                subActsUI.genOpts = GUILayout.Toggle(subActsUI.genOpts, "General Options", GUI.skin.button);

                if(subActsUI.genOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Main parent object for sub action." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(holder, true);

                }//genOpts

                EditorGUILayout.Space();

                subActsUI.inputOpts = GUILayout.Toggle(subActsUI.inputOpts, "Input Options", GUI.skin.button);

                if(subActsUI.inputOpts){

                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "If TRUE detect pause input for hiding / showing UI when actions are active." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    subActsUI.detectPause = EditorGUILayout.Toggle("Detect Pause?", subActsUI.detectPause);

                    if(subActsUI.detectPause){

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "Input name used for pause." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(pauseInput, true);

                    }//detectPause

                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Checks for pause back input in play mode if TRUE." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    subActsUI.detectPauseBack = EditorGUILayout.Toggle("Detect Pause Back?", subActsUI.detectPauseBack);

                    if(subActsUI.detectPauseBack){

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "The input used to un-pause the game using a controller." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(pauseBackInput, true);

                    }//detectPauseBack
                    
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    
                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Checks for inventory input in play mode if TRUE." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    subActsUI.detectInventory = EditorGUILayout.Toggle("Detect Inventory?", subActsUI.detectInventory);

                    if(subActsUI.detectInventory){

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "The input used to trigger the inventory display." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(invInput, new GUIContent("Inventory Input"), true);

                    }//detectInventory
                    
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                }//inputOpts

                EditorGUILayout.Space();

                subActsUI.animOpts = GUILayout.Toggle(subActsUI.animOpts, "Animation Options", GUI.skin.button);

                if(subActsUI.animOpts){

                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(anim, true);

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "Animation used for UI show." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(showAnim, true);

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "Animation used for UI hide." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(hideAnim, true);

                }//animOpts

                EditorGUILayout.Space();

                subActsUI.actOpts = GUILayout.Toggle(subActsUI.actOpts, "Actions Options", GUI.skin.button);

                if(subActsUI.actOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "UI Holders for each sub action." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(actionHolders, true);

                }//actOpts

            }//tabs = user options

            if(subActsUI.tabs == 1){

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Automatic Values", EditorStyles.centeredGreyMiniLabel);

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "These values are automatically handled by the system." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(playerInput, true);

                EditorGUILayout.Space();

                subActsUI.auto.menuOpen = EditorGUILayout.Toggle("Menu Open?", subActsUI.auto.menuOpen);
                subActsUI.auto.menuHidden = EditorGUILayout.Toggle("Menu Hidden?", subActsUI.auto.menuHidden);

                EditorGUILayout.Space();

                subActsUI.auto.pauseKeyPressed = EditorGUILayout.Toggle("PauseKeyPressed", subActsUI.auto.pauseKeyPressed);
                subActsUI.auto.pauseBackKeyPressed = EditorGUILayout.Toggle("Pause Back Key Pressed?", subActsUI.auto.pauseBackKeyPressed);
                subActsUI.auto.invKeyPressed = EditorGUILayout.Toggle("Inventory Key Pressed?", subActsUI.auto.invKeyPressed);
                
                EditorGUILayout.Space();
                
                subActsUI.auto.pausedLocked = EditorGUILayout.Toggle("Paused Locked?", subActsUI.auto.pausedLocked);
                subActsUI.auto.paused = EditorGUILayout.Toggle("Game Paused?", subActsUI.auto.paused);
                subActsUI.auto.inventoryOpen = EditorGUILayout.Toggle("Inventory Open?", subActsUI.auto.inventoryOpen);

                EditorGUILayout.Space();

                subActsUI.auto.locked = EditorGUILayout.Toggle("Locked?", subActsUI.auto.locked);

            }//tabs = auto 

            EditorGUILayout.Space();

            if(EditorGUI.EndChangeCheck()){

                serializedObject.ApplyModifiedProperties();

            }//EndChangeCheck

            if(GUI.changed){

                EditorUtility.SetDirty(subActsUI);

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


    }//HFPS_SubActionsUIEditor


}//namespace