using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEditor;
using DG.Tweening;
using UnityEditor.Rendering;
using UnityEngine.UI;

[CustomEditor(typeof(ButtonController))]
public class ButtonControllerEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var buttonController = (ButtonController)target;

        EditorGUILayout.Space();
        EditorUtility.SetDirty(target);

        buttonController.Logo = (Texture2D)Resources.Load("Prefabs Faiz/Logo Data/SD Logo", typeof(Texture2D));

        if(buttonController.Logo!=null)
            GUI.DrawTexture(new Rect(0, 200, Screen.width, Screen.height - 1100), buttonController.Logo,
            ScaleMode.StretchToFill, true, 10.0F);
        EditorGUILayout.Space(180);

        EditorGUILayout.LabelField("! Hover The Text Menu to Get More Detail !", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Audio Settings", EditorStyles.miniButtonMid);
        EditorGUILayout.Space();

        buttonController._audioSource =
            EditorGUILayout.ObjectField(
                    new GUIContent("Audio Source",
                        "If your Main Camera doesnt have any audio source, The Audio Source will be auto added to your Main Camera"),
                    buttonController._audioSource, typeof(AudioSource), true) as
                AudioSource;

        EditorGUILayout.Space();
        buttonController._onHoverSound =
            EditorGUILayout.ObjectField("On Hover Clip", buttonController._onHoverSound, typeof(AudioClip), true) as
                AudioClip;

        EditorGUILayout.Space();
        buttonController._onClickUpSound =
            EditorGUILayout.ObjectField("On Click Up Clip", buttonController._onClickUpSound, typeof(AudioClip),
                    true) as
                AudioClip;

        EditorGUILayout.Space();
        buttonController._onClickDownSound =
            EditorGUILayout.ObjectField("On Click Down Clip", buttonController._onClickDownSound, typeof(AudioClip),
                    true) as
                AudioClip;

        EditorGUILayout.Space();
        buttonController._onExitSound =
            EditorGUILayout.ObjectField("On Exit Clip", buttonController._onExitSound, typeof(AudioClip), true) as
                AudioClip;

        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField("Button Settings", EditorStyles.miniButtonMid);

        EditorGUILayout.Space();
        buttonController._sourceButton =
            EditorGUILayout.ObjectField("Source Button", buttonController._sourceButton, typeof(Button), true) as
                Button;
        
        EditorGUILayout.Space();
        buttonController._useTooltips =
            EditorGUILayout.Toggle(new GUIContent("Use Tooltips Text?", "This function is Instantiating a label"),
                buttonController._useTooltips);

        EditorGUILayout.Space();
        if (buttonController._useTooltips)
        {
            buttonController._isFollowingCursorTooltip =
                EditorGUILayout.Toggle(new GUIContent("Following Cursor Tooltip", "Dynamic tooltip position based on cursor position."),
                    buttonController._isFollowingCursorTooltip);
            
            EditorGUILayout.Space();
            if (buttonController._isFollowingCursorTooltip)
            {
                buttonController._gapWithCursor = EditorGUILayout.Vector2Field("Gap with cursor", buttonController._gapWithCursor);
                
                EditorGUILayout.Space();
                buttonController._canvas =
                    EditorGUILayout.ObjectField("Parent Canvas", buttonController._canvas, typeof(Canvas), true) as
                        Canvas;
            }
            
            EditorGUILayout.Space();
            buttonController._reorderTooltipParent =
                EditorGUILayout.Toggle(new GUIContent("Reorder Tooltip Parent", "Check if you want to re-order/release tooltip's transform parent."),
                    buttonController._reorderTooltipParent);

            if (buttonController._reorderTooltipParent)
            {
                EditorGUILayout.Space();
                buttonController._reorderTooltipIteration = EditorGUILayout.IntField("Reorder Iteration Number",
                    buttonController._reorderTooltipIteration);
            }
            
            EditorGUILayout.Space();
            buttonController.TooltipsType =
                (ButtonController.Tooltips_Type)EditorGUILayout.EnumPopup(
                    new GUIContent("Mode", "Select your preference tooltips type, Icon or Text"),
                    buttonController.TooltipsType);

            switch (buttonController.TooltipsType)
            {
                case ButtonController.Tooltips_Type.Text:

                case ButtonController.Tooltips_Type.Icon:
                    EditorGUILayout.Space();
                    buttonController._tooltipObjectMode =
                        (ButtonController.TooltipObjectMode)EditorGUILayout.EnumPopup(
                            new GUIContent("Start Tooltip Object Mode", "Select your preference tooltip object type, Instantiate first or not"),
                            buttonController._tooltipObjectMode);

                    switch (buttonController._tooltipObjectMode)
                    {
                        case ButtonController.TooltipObjectMode.Instantiate:
                            EditorGUILayout.Space();
                            buttonController._tooltipsPrefab =
                                EditorGUILayout.ObjectField(
                                        new GUIContent("Tooltips Prefab", "This is prefab for Tooltips, edit as you like"),
                                        buttonController._tooltipsPrefab, typeof(GameObject), true) as
                                    GameObject;

                            EditorGUILayout.Space();
                            buttonController._tooltipsText = EditorGUILayout.TextField(
                                new GUIContent("Tooltips Text", "Set your tooltips text here"), buttonController._tooltipsText,
                                EditorStyles.textField);
                            break;
                        
                        case ButtonController.TooltipObjectMode.NotInstantiate:
                            EditorGUILayout.Space();
                            buttonController._tooltipObject =
                                EditorGUILayout.ObjectField(
                                        new GUIContent("Tooltips Object", "This is prefab for Tooltips, edit as you like"),
                                        buttonController._tooltipObject, typeof(GameObject), true) as
                                    GameObject;
                            
                            EditorGUILayout.Space();
                            buttonController._tooltipsText = EditorGUILayout.TextField(
                                new GUIContent("Tooltips Text", "Set your tooltips text here"), buttonController._tooltipsText,
                                EditorStyles.textField);
                            break;
                    }
                    
                    

                    EditorGUILayout.Space();
                    buttonController._tooltipTransition = (ButtonController.TooltipTransition)EditorGUILayout.EnumPopup(
                        new GUIContent("Tooltip Transition", "Choose tooltip transition while tooltip is showing up"),
                        buttonController._tooltipTransition);

                    switch (buttonController._tooltipTransition)
                    {
                        case ButtonController.TooltipTransition.Unset:
                            /*EditorGUILayout.Space();
                            buttonController._tooltipAnchorPreset =
                                (ButtonController.TooltipAnchorPreset)EditorGUILayout.EnumPopup(
                                    new GUIContent("Tooltip Anchor", "Choose tooltip anchor preset"),
                                    buttonController._tooltipAnchorPreset);*/
                            break;

                        case ButtonController.TooltipTransition.Fade:
                            /*EditorGUILayout.Space();
                            buttonController._tooltipAnchorPreset =
                                (ButtonController.TooltipAnchorPreset)EditorGUILayout.EnumPopup(
                                    new GUIContent("Tooltip Anchor", "Choose tooltip anchor preset"),
                                    buttonController._tooltipAnchorPreset);*/
                            break;

                        case ButtonController.TooltipTransition.Move:
                            EditorGUILayout.Space();
                            buttonController.PivotMode =
                                (ButtonController.MyMode)EditorGUILayout.EnumPopup(
                                    new GUIContent("Pivot Mode", "Set value to place your Pivot Text"),
                                    buttonController.PivotMode);
                            break;
                    }

                    EditorGUILayout.Space();
                    buttonController._tooltipsPivot = EditorGUILayout.Slider(
                        new GUIContent("Range Tooltip Pivot", "Set value to place your Pivot Text"),
                        buttonController._tooltipsPivot, -1000f, 1000f);

                    EditorGUILayout.Space();
                    buttonController._tooltipsImagePivot = EditorGUILayout.Slider(
                        new GUIContent("Range Image Pivot", "Set value to place your Image Pivot Text"),
                        buttonController._tooltipsImagePivot, -1000f, 1000f);

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        else
        {
            buttonController._tooltipsPivot = 0;
            buttonController._tooltipsPrefab = null;
        }

        EditorGUILayout.Space();
        var tmpTextsProperty = serializedObject.FindProperty("TMPTexts");
        EditorGUILayout.PropertyField(tmpTextsProperty, true);

        EditorGUILayout.Space();
        buttonController._isToggle = EditorGUILayout.Toggle(
            new GUIContent("Is Toggle", "On hover will not working if this true and the button is pressed"),
            buttonController._isToggle);

        if (buttonController._isToggle)
        {
            EditorGUILayout.Space();
            buttonController._activateToggleOnStart = EditorGUILayout.Toggle(
                new GUIContent("Activate Toggle On Start", "Should select one toggle on each toggle group"),
                buttonController._activateToggleOnStart);
            
            EditorGUILayout.Space();
            buttonController._toggle =
                EditorGUILayout.ObjectField("Toggle Component", buttonController._toggle, typeof(Toggle), true) as
                    Toggle;
            
            EditorGUILayout.Space();
            var toggleActiveEvent = serializedObject.FindProperty("_onToggleActive");
            EditorGUILayout.PropertyField(toggleActiveEvent, true);
            
            var toggleInactiveEvent = serializedObject.FindProperty("_onToggleInactive");
            EditorGUILayout.PropertyField(toggleInactiveEvent, true);
            
            EditorGUILayout.Space();
            var toggleActiveHoverEvent = serializedObject.FindProperty("_onToggleActiveHover");
            EditorGUILayout.PropertyField(toggleActiveHoverEvent, true);
            
            var toggleActiveExitEvent = serializedObject.FindProperty("_onToggleActiveExit");
            EditorGUILayout.PropertyField(toggleActiveExitEvent, true);
        }

        EditorGUILayout.Space();
        buttonController.ImageMode =
            (ButtonController.ButtonMode)EditorGUILayout.EnumPopup(
                new GUIContent("Mode", "Select your preference button mode, Image or Color"),
                buttonController.ImageMode);

        EditorGUILayout.Space();
        switch (buttonController.ImageMode)
        {
            case ButtonController.ButtonMode.Image:
                buttonController._onHoverImage = EditorGUILayout.ObjectField("On Hover Sprite",
                    buttonController._onHoverImage, typeof(Sprite), true) as Sprite;

                EditorGUILayout.Space();
                buttonController._onClickImage = EditorGUILayout.ObjectField("On Click Sprite",
                    buttonController._onClickImage, typeof(Sprite), true) as Sprite;

                EditorGUILayout.Space();
                buttonController._onIdleImage = EditorGUILayout.ObjectField("On Idle Sprite",
                    buttonController._onIdleImage, typeof(Sprite), true) as Sprite;
                
                EditorGUILayout.Space();
                buttonController._onDisableImage = EditorGUILayout.ObjectField("On Disable Sprite",
                    buttonController._onDisableImage, typeof(Sprite), true) as Sprite;

                EditorGUILayout.Space(10);

                break;
            case ButtonController.ButtonMode.ImageColor:
                buttonController._hoverColor =
                    EditorGUILayout.ColorField("On Hover Color", buttonController._hoverColor);

                EditorGUILayout.Space();
                buttonController._clickColor =
                    EditorGUILayout.ColorField("On Click Color", buttonController._clickColor);

                EditorGUILayout.Space();
                buttonController._idleColor =
                    EditorGUILayout.ColorField("On Idle Color", buttonController._idleColor);
                
                EditorGUILayout.Space();
                buttonController._disableColor =
                    EditorGUILayout.ColorField("On Disable Color", buttonController._disableColor);

                EditorGUILayout.Space(10);

                break;
            case ButtonController.ButtonMode.FontColor:
                buttonController._hoverFontColor =
                    EditorGUILayout.ColorField("On Hover Font Color", buttonController._hoverFontColor);

                EditorGUILayout.Space();
                buttonController._clickFontColor =
                    EditorGUILayout.ColorField("On Click Font Color", buttonController._clickFontColor);

                EditorGUILayout.Space();
                buttonController._idleFontColor =
                    EditorGUILayout.ColorField("On Idle Font Color", buttonController._idleFontColor);
                
                EditorGUILayout.Space();
                buttonController._disableFontColor =
                    EditorGUILayout.ColorField("On Disable Font Color", buttonController._disableFontColor);

                EditorGUILayout.Space(10);
                
                break;
            case ButtonController.ButtonMode.ImageAndImageColor:
                buttonController._onHoverImage = EditorGUILayout.ObjectField("On Hover Sprite",
                    buttonController._onHoverImage, typeof(Sprite), true) as Sprite;

                EditorGUILayout.Space();
                buttonController._onClickImage = EditorGUILayout.ObjectField("On Click Sprite",
                    buttonController._onClickImage, typeof(Sprite), true) as Sprite;

                EditorGUILayout.Space();
                buttonController._onIdleImage = EditorGUILayout.ObjectField("On Idle Sprite",
                    buttonController._onIdleImage, typeof(Sprite), true) as Sprite;
                
                EditorGUILayout.Space();
                buttonController._onDisableImage = EditorGUILayout.ObjectField("On Disable Sprite",
                    buttonController._onDisableImage, typeof(Sprite), true) as Sprite;

                EditorGUILayout.Space();
                buttonController._hoverColor =
                    EditorGUILayout.ColorField("On Hover Color", buttonController._hoverColor);

                EditorGUILayout.Space();
                buttonController._clickColor =
                    EditorGUILayout.ColorField("On Click Color", buttonController._clickColor);

                EditorGUILayout.Space();
                buttonController._idleColor =
                    EditorGUILayout.ColorField("On Idle Color", buttonController._idleColor);
                
                EditorGUILayout.Space();
                buttonController._disableColor =
                    EditorGUILayout.ColorField("On Disable Color", buttonController._disableColor);

                EditorGUILayout.Space(10);

                break;
            case ButtonController.ButtonMode.ImageAndFontColor:
                buttonController._onHoverImage = EditorGUILayout.ObjectField("On Hover Sprite",
                    buttonController._onHoverImage, typeof(Sprite), true) as Sprite;

                EditorGUILayout.Space();
                buttonController._onClickImage = EditorGUILayout.ObjectField("On Click Sprite",
                    buttonController._onClickImage, typeof(Sprite), true) as Sprite;

                EditorGUILayout.Space();
                buttonController._onIdleImage = EditorGUILayout.ObjectField("On Idle Sprite",
                    buttonController._onIdleImage, typeof(Sprite), true) as Sprite;
                
                EditorGUILayout.Space();
                buttonController._onDisableImage = EditorGUILayout.ObjectField("On Disable Sprite",
                    buttonController._onDisableImage, typeof(Sprite), true) as Sprite;

                EditorGUILayout.Space();
                buttonController._hoverFontColor =
                    EditorGUILayout.ColorField("On Hover Color", buttonController._hoverFontColor);

                EditorGUILayout.Space();
                buttonController._clickFontColor =
                    EditorGUILayout.ColorField("On Click Color", buttonController._clickFontColor);

                EditorGUILayout.Space();
                buttonController._idleFontColor =
                    EditorGUILayout.ColorField("On Idle Color", buttonController._idleFontColor);
                
                EditorGUILayout.Space();
                buttonController._disableFontColor =
                    EditorGUILayout.ColorField("On Disable Color", buttonController._disableFontColor);

                EditorGUILayout.Space(10);

                break;
            case ButtonController.ButtonMode.ImageColorAndFontColor:
                buttonController._hoverColor =
                    EditorGUILayout.ColorField("On Hover Image Color", buttonController._hoverColor);

                EditorGUILayout.Space();
                buttonController._clickColor =
                    EditorGUILayout.ColorField("On Click Image Color", buttonController._clickColor);

                EditorGUILayout.Space();
                buttonController._idleColor =
                    EditorGUILayout.ColorField("On Idle Image Color", buttonController._idleColor);
                
                EditorGUILayout.Space();
                buttonController._disableColor =
                    EditorGUILayout.ColorField("On Disable Image Color", buttonController._disableColor);

                EditorGUILayout.Space();
                buttonController._hoverFontColor =
                    EditorGUILayout.ColorField("On Hover Font Color", buttonController._hoverFontColor);

                EditorGUILayout.Space();
                buttonController._clickFontColor =
                    EditorGUILayout.ColorField("On Click Font Color", buttonController._clickFontColor);

                EditorGUILayout.Space();
                buttonController._idleFontColor =
                    EditorGUILayout.ColorField("On Idle Font Color", buttonController._idleFontColor);
                
                EditorGUILayout.Space();
                buttonController._disableFontColor =
                    EditorGUILayout.ColorField("On Disable Font Color", buttonController._disableFontColor);
                
                EditorGUILayout.Space(10);

                break;
            case ButtonController.ButtonMode.Unset:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Scaling Settings", EditorStyles.miniButtonMid);

        EditorGUILayout.Space();
        buttonController.MyTransform =
            EditorGUILayout.ObjectField("My Transform", buttonController.MyTransform, typeof(Transform), true) as
                Transform;

        EditorGUILayout.Space();
        buttonController.TargetImage =
            EditorGUILayout.ObjectField("Target Image", buttonController.TargetImage, typeof(Image), true) as
                Image;

        EditorGUILayout.Space();
        buttonController.SelectEase = (Ease)EditorGUILayout.EnumPopup(
            new GUIContent("Select Animation", "Select your animation when hovered and exit this button"),
            buttonController.SelectEase);

        EditorGUILayout.Space();
        buttonController.Speed =
            EditorGUILayout.Slider(
                new GUIContent("Animation Speed",
                    "0 to speed up your animation, to slow down your animation higher your value"),
                buttonController.Speed, 0f, 10f);

        EditorGUILayout.Space();
        buttonController.ScaleValue = EditorGUILayout.Slider(
            new GUIContent("Scale Up Button Value",
                "This option is to scale up your Button when it hovered. Set to 0 to unused this function"),
            buttonController.ScaleValue, 0f, 10f);

        EditorGUILayout.Space();
        buttonController.InitValue = EditorGUILayout.Slider(
            new GUIContent("Scale Down Button Value", "This option is to scale down your Button, default setting is 1"),
            buttonController.InitValue, 0f, 10f);

        EditorGUILayout.Space();
        buttonController.BoolDelayTime =
            EditorGUILayout.Toggle(new GUIContent("Use Delayed Start?", "This option to delay your Hovered function"),
                buttonController.BoolDelayTime);

        EditorGUILayout.Space();
        if (buttonController.BoolDelayTime)
        {
            buttonController.DelayedTime =
                EditorGUILayout.Slider("Delay Time", buttonController.DelayedTime, 0f, 10f);
        }
        else
        {
            buttonController.DelayedTime = 0;
        }

        EditorGUILayout.Space();

        EditorGUILayout.Space();
        var spesificTransform = serializedObject.FindProperty("OnCompleteScaling");

        EditorGUIUtility.labelWidth = 100;
        EditorGUIUtility.fieldWidth = 100;

        EditorGUILayout.PropertyField(spesificTransform, true);
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Copyright to MattJr", EditorStyles.miniButtonRight, GUILayout.Width(300));
    }
}