/*===============================================================================
Copyright (c) 2019 PTC Inc. All Rights Reserved.

Confidential and Proprietary - Protected under copyright and other laws.
Vuforia is a trademark of PTC Inc., registered in the United States and other 
countries.
===============================================================================*/

using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
    [CustomEditor(typeof(DefaultTrackableEventHandler))]
    [CanEditMultipleObjects]
    public class DefaultTrackableEventHandlerEditor : Editor
    {
        SerializedProperty mStatusFilterProp;
        SerializedProperty mOnTargetFoundProp;
        SerializedProperty mOnTargetLostProp;

        void OnEnable()
        {
            // Setup the SerializedProperties.
            mStatusFilterProp = serializedObject.FindProperty("StatusFilter");
            mOnTargetFoundProp = serializedObject.FindProperty("OnTargetFound");
            mOnTargetLostProp = serializedObject.FindProperty("OnTargetLost");
        }

        public override void OnInspectorGUI()
        {
            // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
            serializedObject.Update();
            
            // render the standard script selector so that users can find the DefaultTrackableEventHandler
            // to customize it:
            GUI.enabled = false;
            SerializedProperty prop = serializedObject.FindProperty("m_Script");
            EditorGUILayout.PropertyField(prop, true);
            GUI.enabled = true;
            
            
            GUILayout.Label("Consider target as visible if its status is:");
            string[] options =
                new[] { "Tracked", 
                        "Tracked or Extended Tracked",
                        "Tracked, Extended Tracked or Limited"};
            mStatusFilterProp.enumValueIndex = EditorGUILayout.Popup(mStatusFilterProp.enumValueIndex, options);
            
            GUILayout.Label("Event(s) when target is found:");
            EditorGUILayout.PropertyField(mOnTargetFoundProp);
            
            GUILayout.Label("Event(s) when target is lost:");
            EditorGUILayout.PropertyField(mOnTargetLostProp);

            // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
            serializedObject.ApplyModifiedProperties();
        }
    }
}