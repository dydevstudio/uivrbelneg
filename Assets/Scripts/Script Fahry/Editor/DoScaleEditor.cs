using System;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DoScaleBehaviour))]
public class DoScaleEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var doScaleBehaviour = (DoScaleBehaviour)target;

        EditorGUILayout.Space();

        EditorUtility.SetDirty(target);

        doScaleBehaviour.Logo = (Texture2D)Resources.Load("Prefabs Faiz/Logo Data/SD Logo", typeof(Texture2D));

        /*GUI.DrawTexture(new Rect(20, 30, 560, 100), doScaleBehaviour.Logo, ScaleMode.StretchToFill, true, 10.0F);*/
        GUI.DrawTexture(new Rect(0, 200, Screen.width, Screen.height - 1100), doScaleBehaviour.Logo,
            ScaleMode.StretchToFill, true, 10.0f);

        EditorGUILayout.Space(110);

        doScaleBehaviour.SelectType =
            (DoScaleBehaviour.Type)EditorGUILayout.EnumPopup("Select Type", doScaleBehaviour.SelectType);

        /*GUILayout.Label(doMoveBehaviour.m_Logo);*/

        switch (doScaleBehaviour.SelectType)
        {
            case DoScaleBehaviour.Type.SpesificTransform:

                EditorGUILayout.Space();


                EditorGUILayout.LabelField("Type of Spesific Transform :", EditorStyles.foldoutHeader);

                EditorGUILayout.Space();

                doScaleBehaviour.MyTransform =
                    EditorGUILayout.ObjectField("My Transform", doScaleBehaviour.MyTransform, typeof(Transform),
                        true) as Transform;

                EditorGUILayout.Space();

                doScaleBehaviour.SelectEase =
                    (Ease)EditorGUILayout.EnumPopup("Select Ease", doScaleBehaviour.SelectEase);

                EditorGUILayout.Space();

                doScaleBehaviour.Speed = EditorGUILayout.Slider("Speed", doScaleBehaviour.Speed, 0f, 10f);

                EditorGUILayout.Space();

                doScaleBehaviour.ScaleValue =
                    EditorGUILayout.Slider("Scale Value", doScaleBehaviour.ScaleValue, 0f, 10f);

                EditorGUILayout.Space();

                doScaleBehaviour.InitValue = EditorGUILayout.Slider("Init Value", doScaleBehaviour.InitValue, 0f, 10f);

                EditorGUILayout.Space();

                doScaleBehaviour.BoolDelayTime =
                    EditorGUILayout.Toggle("Use Delayed Start?", doScaleBehaviour.BoolDelayTime);

                if (doScaleBehaviour.BoolDelayTime)
                {
                    doScaleBehaviour.DelayedTime =
                        EditorGUILayout.Slider("Delayed Time", doScaleBehaviour.DelayedTime, 0f, 10f);
                }
                else
                {
                    doScaleBehaviour.DelayedTime = 0;
                }

                EditorGUILayout.Space();

                EditorGUILayout.Space();

                var SpesificTransform = serializedObject.FindProperty("OnCompleteScaling");

                EditorGUIUtility.labelWidth = 100;

                EditorGUIUtility.fieldWidth = 100;

                EditorGUILayout.PropertyField(SpesificTransform, true);

                serializedObject.ApplyModifiedProperties();
                break;

            case DoScaleBehaviour.Type.XYZMode:

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Type of XYZ Mode :", EditorStyles.foldoutHeader);

                EditorGUILayout.Space();

                doScaleBehaviour.MyTransform =
                    EditorGUILayout.ObjectField("My Transform", doScaleBehaviour.MyTransform, typeof(Transform),
                        true) as Transform;

                EditorGUILayout.Space();

                doScaleBehaviour.SelectEase =
                    (Ease)EditorGUILayout.EnumPopup("Select Ease", doScaleBehaviour.SelectEase);

                doScaleBehaviour.MyMode =
                    (DoScaleObject.MyMode)EditorGUILayout.EnumPopup("Mode", doScaleBehaviour.MyMode);

                if (doScaleBehaviour.MyMode != DoScaleObject.MyMode.Unset)
                {
                    doScaleBehaviour.Speed = EditorGUILayout.Slider("Speed", doScaleBehaviour.Speed, 0f, 10f);

                    doScaleBehaviour.ScaleValue =
                        EditorGUILayout.Slider("Scale Value (X/Y/Z)", doScaleBehaviour.ScaleValue, 0f, 10f);
                }

                EditorGUILayout.Space();

                EditorGUILayout.Space();

                var XYZMode = serializedObject.FindProperty("OnCompleteScaling");

                EditorGUIUtility.labelWidth = 100;

                EditorGUIUtility.fieldWidth = 100;

                EditorGUILayout.PropertyField(XYZMode, true);

                serializedObject.ApplyModifiedProperties();
                break;
            case DoScaleBehaviour.Type.Unset:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        EditorGUILayout.Space();
    }
}