using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(FadingBehaviour))]
public class FadingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        FadingBehaviour fadingBehaviour = (FadingBehaviour)target;

        EditorGUILayout.Space();

        EditorUtility.SetDirty(target);

        fadingBehaviour.Logo = (Texture2D)Resources.Load("Prefabs Faiz/Logo Data/SD Logo", typeof(Texture2D));

        if(fadingBehaviour.Logo !=null)
            GUI.DrawTexture(new Rect(20, 30, 560, 100), fadingBehaviour.Logo, ScaleMode.StretchToFill, true, 10.0F);

        EditorGUILayout.Space(110);

        SerializedProperty OnBeginFadingIn = serializedObject.FindProperty("OnBeginFadingIn");

        SerializedProperty OnBeginFadingOut = serializedObject.FindProperty("OnBeginFadingOut");

        SerializedProperty OnCompleteFadingIn = serializedObject.FindProperty("OnCompleteFadingIn");

        SerializedProperty OnCompleteFadingOut = serializedObject.FindProperty("OnCompleteFadingOut");

        fadingBehaviour.FadingType =
            (FadingBehaviour.Type)EditorGUILayout.EnumPopup("Select Type", fadingBehaviour.FadingType);

        switch (fadingBehaviour.FadingType)
        {
            case FadingBehaviour.Type.Normal:

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Normal Fading", EditorStyles.miniButtonMid);

                EditorGUILayout.Space();

                fadingBehaviour._myCanvasGroup =
                    EditorGUILayout.ObjectField("Fading Panel", fadingBehaviour._myCanvasGroup, typeof(CanvasGroup),
                        true) as CanvasGroup;

                EditorGUILayout.Space();

                fadingBehaviour.InOut = EditorGUILayout.Toggle("In Out?", fadingBehaviour.InOut);

                EditorGUILayout.Space();

                fadingBehaviour.OnStart = EditorGUILayout.Toggle("On Start?", fadingBehaviour.OnStart);

                EditorGUILayout.Space();

                if (fadingBehaviour.OnStart)
                {
                    fadingBehaviour.FadingIn = EditorGUILayout.Toggle("Fading In?", fadingBehaviour.FadingIn);

                    EditorGUILayout.Space();

                    fadingBehaviour.FadingOut = EditorGUILayout.Toggle("Fading Out?", fadingBehaviour.FadingOut);

                    EditorGUILayout.Space();
                }

                fadingBehaviour._speed = EditorGUILayout.Slider("Fading Speed", fadingBehaviour._speed, 0f, 10f);
                
                EditorGUILayout.Space();
                fadingBehaviour._delay = EditorGUILayout.Slider("Delay", fadingBehaviour._delay, 0f, 10f);
                
                EditorGUILayout.Space();
                fadingBehaviour._endDelay = EditorGUILayout.Slider("End Delay", fadingBehaviour._endDelay, 0f, 10f);

                EditorGUILayout.Space();

                fadingBehaviour._selectEase =
                    (Ease)EditorGUILayout.EnumPopup("Select Ease", fadingBehaviour._selectEase);

                EditorGUILayout.Space();

                fadingBehaviour._targetValue =
                    EditorGUILayout.FloatField("Fading Panel Position", fadingBehaviour._targetValue);

                EditorGUILayout.Space();

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Fading In Event", EditorStyles.foldoutHeader);

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(OnBeginFadingIn, true);

                EditorGUILayout.PropertyField(OnCompleteFadingIn, true);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Fading Out Event", EditorStyles.foldoutHeader);

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(OnBeginFadingOut, true);

                EditorGUILayout.PropertyField(OnCompleteFadingOut, true);

                serializedObject.ApplyModifiedProperties();

                break;

            case FadingBehaviour.Type.Gradient:

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Gradient Fading", EditorStyles.miniButtonMid);

                EditorGUILayout.Space();

                fadingBehaviour._myCanvasGroup =
                    EditorGUILayout.ObjectField("Fading Panel", fadingBehaviour._myCanvasGroup, typeof(CanvasGroup),
                        true) as CanvasGroup;

                EditorGUILayout.Space();

                fadingBehaviour.InOut = EditorGUILayout.Toggle("In Out?", fadingBehaviour.InOut);

                EditorGUILayout.Space();

                fadingBehaviour._speed = EditorGUILayout.Slider("Fading Speed", fadingBehaviour._speed, 0f, 10f);

                EditorGUILayout.Space();

                fadingBehaviour._selectEase =
                    (Ease)EditorGUILayout.EnumPopup("Select Ease", fadingBehaviour._selectEase);

                EditorGUILayout.Space();

                fadingBehaviour.Mode =
                    (FadingBehaviour.MyMode)EditorGUILayout.EnumPopup("Mode", fadingBehaviour.Mode);

                fadingBehaviour._selectEase =
                    (Ease)EditorGUILayout.EnumPopup("Select Ease", fadingBehaviour._selectEase);

                if (fadingBehaviour.Mode == FadingBehaviour.MyMode.X)
                {
                    fadingBehaviour._targetValue =
                        EditorGUILayout.FloatField("Fading Panel Position (X/Y/Z)", fadingBehaviour._targetValue);
                }
                else if (fadingBehaviour.Mode == FadingBehaviour.MyMode.Y)
                {
                    fadingBehaviour._targetValue =
                        EditorGUILayout.FloatField("Fading Panel Position (X/Y/Z)", fadingBehaviour._targetValue);
                }
                else
                {
                    fadingBehaviour._targetValue =
                        EditorGUILayout.FloatField("Fading Panel Position (X/Y/Z)", fadingBehaviour._targetValue);
                }

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Fading In Event", EditorStyles.foldoutHeader);

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(OnBeginFadingIn, true);

                EditorGUILayout.PropertyField(OnCompleteFadingIn, true);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Fading Out Event", EditorStyles.foldoutHeader);

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(OnBeginFadingOut, true);

                EditorGUILayout.PropertyField(OnCompleteFadingOut, true);

                EditorGUILayout.Space();

                EditorGUILayout.Space();

                serializedObject.ApplyModifiedProperties();

                break;

            case FadingBehaviour.Type.Eye:

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Eye Fading", EditorStyles.miniButtonMid);

                EditorGUILayout.Space();

                fadingBehaviour.EyeController =
                    EditorGUILayout.ObjectField("Eye Controller", fadingBehaviour.EyeController, typeof(Transform),
                        true) as Transform;

                EditorGUILayout.Space();

                fadingBehaviour.InOut = EditorGUILayout.Toggle("In Out?", fadingBehaviour.InOut);

                EditorGUILayout.Space();

                fadingBehaviour._speed = EditorGUILayout.Slider("Fading Speed", fadingBehaviour._speed, 0f, 10f);

                EditorGUILayout.Space();

                fadingBehaviour._selectEase =
                    (Ease)EditorGUILayout.EnumPopup("Select Ease", fadingBehaviour._selectEase);

                EditorGUILayout.Space();

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Fading In Event", EditorStyles.foldoutHeader);

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(OnBeginFadingIn, true);

                EditorGUILayout.PropertyField(OnCompleteFadingIn, true);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Fading Out Event", EditorStyles.foldoutHeader);

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(OnBeginFadingOut, true);

                EditorGUILayout.PropertyField(OnCompleteFadingOut, true);

                serializedObject.ApplyModifiedProperties();

                break;

            case FadingBehaviour.Type.Clock:

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Clock Fading", EditorStyles.miniButtonMid);

                EditorGUILayout.Space();

                fadingBehaviour._myClockPanel =
                    EditorGUILayout.ObjectField("Clock Panel", fadingBehaviour._myClockPanel, typeof(Image), true) as
                        Image;

                EditorGUILayout.Space();

                fadingBehaviour.InOut = EditorGUILayout.Toggle("In Out?", fadingBehaviour.InOut);

                EditorGUILayout.Space();

                fadingBehaviour._speed = EditorGUILayout.Slider("Fading Speed", fadingBehaviour._speed, 0f, 10f);

                EditorGUILayout.Space();

                fadingBehaviour._selectEase =
                    (Ease)EditorGUILayout.EnumPopup("Select Ease", fadingBehaviour._selectEase);

                EditorGUILayout.Space();

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Fading In Event", EditorStyles.foldoutHeader);

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(OnBeginFadingIn, true);

                EditorGUILayout.PropertyField(OnCompleteFadingIn, true);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Fading Out Event", EditorStyles.foldoutHeader);

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(OnBeginFadingOut, true);

                EditorGUILayout.PropertyField(OnCompleteFadingOut, true);

                serializedObject.ApplyModifiedProperties();

                break;
        }

        EditorGUILayout.Space();
    }
}