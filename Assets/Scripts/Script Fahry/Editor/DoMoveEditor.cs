using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DoMoveBehaviour))]
public class DoMoveEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var doMoveBehaviour = (DoMoveBehaviour)target;

        EditorGUILayout.Space();

        EditorUtility.SetDirty(target);

        doMoveBehaviour.Logo = (Texture2D)Resources.Load("Prefabs Faiz/Logo Data/SD Logo", typeof(Texture2D));
        
        if(doMoveBehaviour.Logo!=null)
            GUI.DrawTexture(new Rect(0, 200, Screen.width, Screen.height - 1100), doMoveBehaviour.Logo,
            ScaleMode.StretchToFill, true, 10.0f);

        EditorGUILayout.Space(180);

        doMoveBehaviour.SelectType =
            (DoMoveBehaviour.Type)EditorGUILayout.EnumPopup("Select Type", doMoveBehaviour.SelectType);

        switch (doMoveBehaviour.SelectType)
        {
            case DoMoveBehaviour.Type.Unset:

                doMoveBehaviour.MyTarget = null;

                doMoveBehaviour.TargetLocation = null;

                break;

            case DoMoveBehaviour.Type.SpesificTransform:

                if (doMoveBehaviour.MyTarget)
                {
                    doMoveBehaviour.InitialAttach();
                }
                else
                {
                    doMoveBehaviour.RemoveAttach();
                }

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Type of SpesificTransform :", EditorStyles.foldoutHeader);

                EditorGUILayout.Space();

                doMoveBehaviour.MyTarget =
                    EditorGUILayout.ObjectField("Target Transform", doMoveBehaviour.MyTarget, typeof(Transform),
                        true) as Transform;

                EditorGUILayout.Space();

                doMoveBehaviour.TargetLocation = EditorGUILayout.ObjectField("Target Location",
                    doMoveBehaviour.TargetLocation, typeof(Transform), true) as Transform;

                EditorGUILayout.Space();

                doMoveBehaviour._selectEase =
                    (Ease)EditorGUILayout.EnumPopup("Select Ease", doMoveBehaviour._selectEase);

                EditorGUILayout.Space();
                doMoveBehaviour._speed = EditorGUILayout.Slider("Speed", doMoveBehaviour._speed, 0f, 10f);

                EditorGUILayout.Space();
                doMoveBehaviour.EndAnimDelay =
                    EditorGUILayout.Slider("End Animation Delay", doMoveBehaviour.EndAnimDelay, 0f, 10f);

                EditorGUILayout.Space();
                doMoveBehaviour.BoolDelayTime =
                    EditorGUILayout.Toggle("Use Delayed Start?", doMoveBehaviour.BoolDelayTime);

                EditorGUILayout.Space();

                doMoveBehaviour.OnStart =
                    EditorGUILayout.Toggle("On Start?", doMoveBehaviour.OnStart);


                if (doMoveBehaviour.BoolDelayTime)
                {
                    doMoveBehaviour.DelayedTime =
                        EditorGUILayout.Slider("Delayed Time", doMoveBehaviour.DelayedTime, 0f, 10f);
                }
                else
                {
                    doMoveBehaviour.DelayedTime = 0;
                }

                EditorGUILayout.Space();

                EditorGUILayout.Space();

                SerializedProperty SpesificTransform = serializedObject.FindProperty("OnCompleteMove");

                EditorGUIUtility.labelWidth = 100;

                EditorGUIUtility.fieldWidth = 100;

                EditorGUILayout.PropertyField(SpesificTransform, true);

                serializedObject.ApplyModifiedProperties();
                break;

            case DoMoveBehaviour.Type.XYZMode:

                doMoveBehaviour.TargetLocation = null;

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Type of XYZ Mode :", EditorStyles.foldoutHeader);

                EditorGUILayout.Space();

                doMoveBehaviour.MyTarget =
                    EditorGUILayout.ObjectField("My Transform", doMoveBehaviour.MyTarget, typeof(Transform),
                        true) as Transform;

                EditorGUILayout.Space();

                doMoveBehaviour.Mode =
                    (DoMoveBehaviour.MyMode)EditorGUILayout.EnumPopup("Mode", doMoveBehaviour.Mode);

                EditorGUILayout.Space();

                doMoveBehaviour._selectEase =
                    (Ease)EditorGUILayout.EnumPopup("Select Ease", doMoveBehaviour._selectEase);

                EditorGUILayout.Space();

                switch (doMoveBehaviour.Mode)
                {
                    case DoMoveBehaviour.MyMode.X:
                        doMoveBehaviour._speed = EditorGUILayout.Slider("Speed", doMoveBehaviour._speed, 0f, 10f);

                        EditorGUILayout.Space();
                        doMoveBehaviour.EndAnimDelay = EditorGUILayout.Slider("End Animation Delay",
                            doMoveBehaviour.EndAnimDelay, 0f, 10f);

                        doMoveBehaviour._targetValue =
                            EditorGUILayout.FloatField("Target Position (X/Y/Z)", doMoveBehaviour._targetValue);
                        break;
                    case DoMoveBehaviour.MyMode.Y:
                        doMoveBehaviour._speed = EditorGUILayout.Slider("Speed", doMoveBehaviour._speed, 0f, 10f);

                        EditorGUILayout.Space();
                        doMoveBehaviour.EndAnimDelay = EditorGUILayout.Slider("End Animation Delay",
                            doMoveBehaviour.EndAnimDelay, 0f, 10f);

                        doMoveBehaviour._targetValue =
                            EditorGUILayout.FloatField("Target Position (X/Y/Z)", doMoveBehaviour._targetValue);
                        break;
                    case DoMoveBehaviour.MyMode.Z:
                        doMoveBehaviour._speed = EditorGUILayout.Slider("Speed", doMoveBehaviour._speed, 0f, 10f);

                        EditorGUILayout.Space();
                        doMoveBehaviour.EndAnimDelay = EditorGUILayout.Slider("End Animation Delay",
                            doMoveBehaviour.EndAnimDelay, 0f, 10f);

                        doMoveBehaviour._targetValue =
                            EditorGUILayout.FloatField("Target Position (X/Y/Z)", doMoveBehaviour._targetValue);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                EditorGUILayout.Space();

                doMoveBehaviour.BoolDelayTime =
                    EditorGUILayout.Toggle("Use Delayed Start?", doMoveBehaviour.BoolDelayTime);

                EditorGUILayout.Space();

                doMoveBehaviour.OnStart =
                    EditorGUILayout.Toggle("On Start?", doMoveBehaviour.OnStart);

                if (doMoveBehaviour.BoolDelayTime)
                {
                    doMoveBehaviour.DelayedTime =
                        EditorGUILayout.Slider("Delayed Time", doMoveBehaviour.DelayedTime, 0f, 10f);
                }
                else
                {
                    doMoveBehaviour.DelayedTime = 0;
                }

                /*serializedObject = new SerializedObject(this);
            property = serializedObject.FindProperty("OnCompleteMove");
            EditorGUILayout.PropertyField(property, true);*/
                EditorGUILayout.Space();

                EditorGUILayout.Space();

                var XYZMode = serializedObject.FindProperty("OnCompleteMove");

                EditorGUIUtility.labelWidth = 100;

                EditorGUIUtility.fieldWidth = 100;

                EditorGUILayout.PropertyField(XYZMode, true);

                serializedObject.ApplyModifiedProperties();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        EditorGUILayout.LabelField("Note To Users : " + "\n" + "\n" +
                                   "1. When select type is 'Spesific Transform', these function will be executed : " +
                                   "\n" +
                                   "    A. BeginDoMove " + "\n" +
                                   "    B. BeginDoLocalMove" + "\n" +
                                   "    C. BeginDoMoveInitialPosition" + "\n" +
                                   "    D. BeginDoLocalMoveInitialPosition" + "\n" + "\n" +
                                   "2. When select type is 'XYZ Mode', these function will be executed : " + "\n" +
                                   "    A. BeginDoSelectMove " + "\n" +
                                   "    B. BeginDoSelectLocalMove",
            GUILayout.Width(500), GUILayout.Height(180));

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Copyright to MattJr", EditorStyles.miniButtonRight, GUILayout.Width(300));

        EditorGUILayout.Space();
    }
}