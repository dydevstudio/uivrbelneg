using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(PointerHandlerBehaviour))]
public class ButtonController : DoScaleBehaviour
{
    [HideInInspector] public AudioSource _audioSource;
    [HideInInspector] public Button _sourceButton;
    
    [HideInInspector] public AudioClip _onHoverSound;
    [HideInInspector] public AudioClip _onClickUpSound;
    [HideInInspector] public AudioClip _onClickDownSound;
    [HideInInspector] public AudioClip _onExitSound;

    [HideInInspector] public Sprite _onHoverImage;
    [HideInInspector] public Sprite _onClickImage;
    [HideInInspector] public Sprite _onIdleImage;
    [HideInInspector] public Sprite _onDisableImage;

    [HideInInspector] public Color _hoverColor;
    [HideInInspector] public Color _clickColor;
    [HideInInspector] public Color _idleColor;
    [HideInInspector] public Color _disableColor;

    [HideInInspector] public Color _hoverFontColor;
    [HideInInspector] public Color _clickFontColor;
    [HideInInspector] public Color _idleFontColor;
    [HideInInspector] public Color _disableFontColor;

    [HideInInspector] public bool _isFollowingCursorTooltip;
    [HideInInspector] public Vector2 _gapWithCursor;
    
    [HideInInspector] public bool _useTooltips;
    [HideInInspector] public bool _isToggle;
    [HideInInspector] public bool _activateToggleOnStart;
    [HideInInspector] public bool _reorderTooltipParent;
    
    private bool _tooltipIsCurrentlyOn;
    private bool _buttonInteractableState;
    private bool _toggleInteractableState;
    private bool _toggleIsOn;

    public bool ToggleIsOn
    {
        get { return _toggleIsOn; }
        set
        {
            _toggleIsOn = value;
            OnToggleValueChanged();
        }
    }
    
    public bool ButtonInteractableState
    {
        get { return _buttonInteractableState; }
        set
        {
            _buttonInteractableState = value;
            OnButtonInteractableChanged();
        }
    }

    public bool ToggleInteractableState
    {
        get { return _toggleInteractableState; }
        set
        {
            _toggleInteractableState = value;
            OnToggleInteractableChanged();
        }
    }

    [HideInInspector] public int _reorderTooltipIteration;
    
    [HideInInspector] public GameObject _tooltipsPrefab;
    [HideInInspector] public GameObject _tooltipObject;
    
    [HideInInspector] public string _tooltipsText;

    public string TooltipTextSetter
    {
        get { return _tooltipsText; }
        set
        {
            _tooltipsText = value;
            if(_tooltipsPlaceholder == null)
                return;
            
            _tooltipsPlaceholder.GetComponent<PlaceHolderController>()._placeHolderText.text = _tooltipsText;
        }
    }

    [HideInInspector] [Tooltip("Adjust your pivot")]
    public float _tooltipsPivot;

    [HideInInspector] [Tooltip("Adjust your Image pivot")]
    public float _tooltipsImagePivot;

    [HideInInspector] [Tooltip("Choose tooltip animation transition")]
    public TooltipTransition _tooltipTransition;

    /// <summary>
    /// Transition mode on tooltip, unset, move, and fade
    /// </summary>
    public enum TooltipTransition
    {
        Unset,
        Move,
        Fade
    }

    [HideInInspector] public TooltipObjectMode _tooltipObjectMode;
    
    public enum TooltipObjectMode
    {
        Instantiate,
        NotInstantiate
    }

    [HideInInspector] [Tooltip("Select your pivot direction")]
    public MyMode PivotMode;

    public enum MyMode
    {
        X,
        Y,
        Z
    }

    [HideInInspector] public Image TargetImage;

    public enum ButtonMode
    {
        Unset,
        Image,
        ImageColor,
        FontColor,
        ImageAndImageColor,
        ImageColorAndFontColor,
        ImageAndFontColor
    }

    public enum Tooltips_Type
    {
        Icon,
        Text
    }

    [HideInInspector] public ButtonMode ImageMode;

    [HideInInspector] public Tooltips_Type TooltipsType;

    [HideInInspector] public TooltipAnchorPreset _tooltipAnchorPreset;

    public enum TooltipAnchorPreset
    {
        Unset,
        CornerTopLeft,
        Top,
        CornerTopRight,
        MiddleLeft,
        Middle,
        MiddleRight,
        CornerBottomLeft,
        Bottom,
        CornerBottomRight
    }
    
    public enum ButtonTransition
    {
        Idle,
        Hover,
        Click,
        Disable
    }

    private PointerHandlerBehaviour _pointerHandlerBehaviour;

    private DoMoveBehaviour _doMoveBehaviour;

    private FadingBehaviour _fadingBehaviour;
    
    [HideInInspector] public Canvas _canvas;
    
    [HideInInspector] public Toggle _toggle;

    [HideInInspector] public UnityEvent _onToggleActive;
    [HideInInspector] public UnityEvent _onToggleInactive;
    [HideInInspector] public UnityEvent _onToggleActiveHover;
    [HideInInspector] public UnityEvent _onToggleActiveExit;

    [HideInInspector] public List<TextMeshProUGUI> TMPTexts;

    private GameObject _tooltipsPlaceholder;

    /// <summary>
    /// Get Audiosource from MainCamera, if there is no AudioSource, add it automatically
    /// </summary>
    public void InitAudioSource()
    {
        GameObject audio = GameObject.FindWithTag("Audio");
        
        if (audio == null) return;
        if (audio.GetComponent<AudioSource>() == null) audio.AddComponent<AudioSource>();
        
        _audioSource = audio.GetComponent<AudioSource>();
    }
    
    /// <summary>
    /// Initialize toggle system and toggle component, send an error message if there are missing required components.
    /// </summary>
    private void InitToggle()
    {
        if (_isToggle)
        {
            if (_toggle)
                ToggleIsOn = _toggle.isOn;
            
            if(_activateToggleOnStart)
            {
                OnDown();
                _toggle.isOn = true;
                ToggleIsOn = true;
            }
            
            if (_toggle == null) throw new NullReferenceException();
            
            _toggle.onValueChanged.AddListener(SetLocalToggle);
            
            if(_toggle.group != null)
                StartCoroutine(WaitAnySecondToEvent(0.8f, () => { _onToggleInactive.AddListener(OnExit); }));
            
            OnToggleValueChanged();

            ToggleInteractableState = _toggle.interactable;
            OnToggleInteractableChanged();
        }
    }

    /// <summary>
    /// Initialize button if using default Button component. This function is used if you need to get Interactable state
    /// </summary>
    private void InitButton()
    {
        if(_sourceButton != null)
        {
            ButtonInteractableState = _sourceButton.interactable;
            
            //Force change UI transition to disabled or idle
            OnButtonInteractableChanged();
        }
    }

    /// <summary>
    /// Start The Script
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private void Start()
    {
        InitButton();
        InitAudioSource();
        InitToggle();

        if (_useTooltips && (TooltipsType == Tooltips_Type.Icon || TooltipsType == Tooltips_Type.Text))
        {
            if(_isFollowingCursorTooltip && _canvas == null)
            {
                try
                {
                    _canvas = transform.root.GetComponent<Canvas>();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                    
            }
            
            if (_tooltipObjectMode == TooltipObjectMode.Instantiate)
                _tooltipsPlaceholder = Instantiate(_tooltipsPrefab, transform);
            else _tooltipsPlaceholder = _tooltipObject;

            if (_tooltipsPlaceholder != null && _reorderTooltipParent && _reorderTooltipIteration > 0)
            {
                for (int i = 0; i < _reorderTooltipIteration; i++)
                {
                    var currentParent = _tooltipsPlaceholder.transform.parent;
                    if(currentParent.parent == null)
                        break;
                        
                    _tooltipsPlaceholder.transform.parent = currentParent.parent;
                }
            }
            
            if(_tooltipsText != String.Empty) _tooltipsPlaceholder.GetComponent<PlaceHolderController>()._placeHolderText.text = _tooltipsText;
            
            var background_placehold = _tooltipsPlaceholder.GetComponent<DoMoveBehaviour>().MyTarget
                .GetComponent<RectTransform>();
            var initial_posrot = _tooltipsPlaceholder.GetComponentInChildren<InitialPosRotBehaviour>().GetLocalPos;

            switch (_tooltipTransition)
            {
                case TooltipTransition.Unset:
                    _tooltipsPlaceholder.SetActive(false);
                    //if(_tooltipObjectMode == TooltipObjectMode.Instantiate) TooltipAnchorPosConfiguration(_tooltipsPlaceholder.GetComponent<RectTransform>());
                    break;

                case TooltipTransition.Move:
                    TooltipMoveConfiguration(_tooltipsPlaceholder, background_placehold, initial_posrot);
                    break;

                case TooltipTransition.Fade:
                    _fadingBehaviour = _tooltipsPlaceholder.GetComponent<FadingBehaviour>();
                    _fadingBehaviour.OnCompleteFadingIn.AddListener(() => ToggleTooltipStatus(true));
                    _fadingBehaviour.OnCompleteFadingOut.AddListener(() => ToggleTooltipStatus(false));
                    //if(_tooltipObjectMode == TooltipObjectMode.Instantiate) TooltipAnchorPosConfiguration(_tooltipsPlaceholder.GetComponent<RectTransform>());
                    break;
            }
        }

        _pointerHandlerBehaviour = GetComponent<PointerHandlerBehaviour>();

        _pointerHandlerBehaviour.OnPointerEnterEvent.AddListener(OnHover);
        _pointerHandlerBehaviour.OnPointerUpEvent.AddListener(OnUp);
        _pointerHandlerBehaviour.OnPointerDownEvent.AddListener(OnDown);
        _pointerHandlerBehaviour.OnPointerExitEvent.AddListener(OnExit);
    }

    private void TooltipMoveConfiguration(GameObject tooltipObj, RectTransform tooltipRect, Vector3 tooltipStartPos)
    {
        switch (PivotMode)
        {
            case MyMode.X:
                tooltipRect.localPosition = new Vector3(
                    x: tooltipRect.localPosition.x + _tooltipsImagePivot,
                    y: tooltipRect.localPosition.y,
                    z: tooltipRect.localPosition.z);

                tooltipObj.transform.localPosition = new Vector3(
                    x: tooltipObj.transform.localPosition.x + _tooltipsPivot,
                    y: tooltipObj.transform.localPosition.y,
                    z: tooltipObj.transform.localPosition.z);

                tooltipStartPos = new Vector3(
                    x: tooltipStartPos.x + _tooltipsImagePivot,
                    y: tooltipStartPos.y,
                    z: tooltipStartPos.z);

                _doMoveBehaviour = tooltipObj.GetComponent<DoMoveBehaviour>();

                break;
            case MyMode.Y:
                tooltipRect.localPosition = new Vector3(
                    x: tooltipRect.localPosition.x,
                    y: tooltipRect.localPosition.y + _tooltipsImagePivot,
                    z: tooltipRect.localPosition.z);

                tooltipObj.transform.localPosition = new Vector3(
                    x: tooltipObj.transform.localPosition.x,
                    y: tooltipObj.transform.localPosition.y + _tooltipsPivot,
                    z: tooltipObj.transform.localPosition.z);

                tooltipStartPos = new Vector3(
                    x: tooltipStartPos.x,
                    y: tooltipStartPos.y + _tooltipsImagePivot,
                    z: tooltipStartPos.z);

                _doMoveBehaviour = tooltipObj.GetComponent<DoMoveBehaviour>();

                break;
            case MyMode.Z:
                tooltipRect.localPosition = new Vector3(
                    x: tooltipRect.localPosition.x,
                    y: tooltipRect.localPosition.y,
                    z: tooltipRect.localPosition.z + _tooltipsImagePivot);

                tooltipObj.transform.localPosition = new Vector3(
                    x: tooltipObj.transform.localPosition.x,
                    y: tooltipObj.transform.localPosition.y,
                    z: tooltipObj.transform.localPosition.z + _tooltipsPivot);

                tooltipStartPos = new Vector3(
                    x: tooltipStartPos.x,
                    y: tooltipStartPos.y,
                    z: tooltipStartPos.z + _tooltipsImagePivot);

                _doMoveBehaviour = tooltipObj.GetComponent<DoMoveBehaviour>();

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        try
        {
            _doMoveBehaviour.OnCompleteMove.AddListener(() => {ToggleTooltipStatus(true);});
            _doMoveBehaviour.OnCompleteMoveBack.AddListener(() => {ToggleTooltipStatus(false);});
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void TooltipAnchorPosConfiguration(RectTransform tooltipRect)
    {
        switch (_tooltipAnchorPreset)
        {
            case TooltipAnchorPreset.Unset:
                break;
            case TooltipAnchorPreset.CornerTopLeft:
                tooltipRect.anchorMax = new Vector2(0, 1);
                tooltipRect.anchorMin = new Vector2(0, 1);
                //tooltipRect.anchoredPosition3D = new Vector3(50, -50, 0);
                break;
            case TooltipAnchorPreset.Top:
                tooltipRect.anchorMax = new Vector2(0.5f, 1);
                tooltipRect.anchorMin = new Vector2(0.5f, 1);
                //tooltipRect.anchoredPosition3D = new Vector3(0, -50, 0);
                break;
            case TooltipAnchorPreset.CornerTopRight:
                tooltipRect.anchorMax = new Vector2(1, 1);
                tooltipRect.anchorMin = new Vector2(1, 1);
                //tooltipRect.anchoredPosition3D = new Vector3(-50, -50, 0);
                break;
            case TooltipAnchorPreset.MiddleLeft:
                tooltipRect.anchorMax = new Vector2(0, 0.5f);
                tooltipRect.anchorMin = new Vector2(0, 0.5f);
                //tooltipRect.anchoredPosition3D = new Vector3(50, 0, 0);
                break;
            case TooltipAnchorPreset.Middle:
                tooltipRect.anchorMax = new Vector2(0.5f, 0.5f);
                tooltipRect.anchorMin = new Vector2(0.5f, 0.5f);
                //tooltipRect.anchoredPosition3D = new Vector3(0, 0, 0);
                break;
            case TooltipAnchorPreset.MiddleRight:
                tooltipRect.anchorMax = new Vector2(1, 0.5f);
                tooltipRect.anchorMin = new Vector2(1, 0.5f);
                //tooltipRect.anchoredPosition3D = new Vector3(-50, 0, 0);
                break;
            case TooltipAnchorPreset.CornerBottomLeft:
                tooltipRect.anchorMax = new Vector2(0, 0);
                tooltipRect.anchorMin = new Vector2(0, 0);
                //tooltipRect.anchoredPosition3D = new Vector3(50, 50, 0);
                break;
            case TooltipAnchorPreset.Bottom:
                tooltipRect.anchorMax = new Vector2(0.5f, 0);
                tooltipRect.anchorMin = new Vector2(0.5f, 0);
                //tooltipRect.anchoredPosition3D = new Vector3(0, 50, 0);
                break;
            case TooltipAnchorPreset.CornerBottomRight:
                tooltipRect.anchorMax = new Vector2(1, 0);
                tooltipRect.anchorMin = new Vector2(1, 0);
                //tooltipRect.anchoredPosition3D = new Vector3(-50, 50, 0);
                break;
        }
        if(_tooltipAnchorPreset == TooltipAnchorPreset.Unset) return;
        
        /*tooltipRect.offsetMax = Vector2.zero;
        tooltipRect.offsetMin = Vector2.zero;*/
    }

    /// <summary>
    /// Check if toggle group is activating "Set All Toggles Off" function, then reset the font color
    /// </summary>
    private void Update()
    {
        if (_sourceButton != null && _sourceButton.interactable != ButtonInteractableState)
            ButtonInteractableState = _sourceButton.interactable;
    }

    /// <summary>
    /// Clear DOTween object which will kill all tweens and caches when game ends/closed
    /// </summary>
    private void OnDestroy()
    {
        // Check if the script is still present and active before calling ExitSound()
        if (this != null && gameObject.activeSelf)
        {
            DOTween.Clear();
        }
    }

    /// <summary>
    /// When your pointer hovering any button, this method will be executed.
    /// </summary>
    public void OnHover()
    {
        if(_isToggle && _toggle.interactable == false || _sourceButton && ButtonInteractableState == false)
            return;
        
        BeginDoScale();
        HoverSound();
        
        if(!_useTooltips) return;
        
        if (_isFollowingCursorTooltip && !_tooltipIsCurrentlyOn)
        {
            if(_canvas != null)
            {
                Vector2 movePos;

                RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform,
                    Input.mousePosition,
                    _canvas.worldCamera, out movePos);

                movePos += _gapWithCursor;

                _tooltipsPlaceholder.transform.position = _canvas.transform.TransformPoint(movePos);

                var tooltipRect = _tooltipsPlaceholder.GetComponent<RectTransform>();

                int clearCorner = tooltipRect.CheckUIOffScreen(null);

                while (clearCorner < 4)
                {
                    tooltipRect.FixUIOffScreen(null);
                    clearCorner = tooltipRect.CheckUIOffScreen(null);
                }
            }
            else
                Debug.LogWarning("Canvas can't be null");
        }

        switch (_tooltipTransition)
        {
            case TooltipTransition.Unset:
                _tooltipsPlaceholder.SetActive(true);
                _tooltipIsCurrentlyOn = true;
                break;

            case TooltipTransition.Move:
                _doMoveBehaviour.BeginDoSelectLocalMove();
                break;

            case TooltipTransition.Fade:
                _fadingBehaviour.BeginFadingIn();
                break;
        }

        if (_useTooltips && TooltipsType == Tooltips_Type.Text)
        {
            var placeHolderController = _doMoveBehaviour.gameObject.GetComponent<PlaceHolderController>();
            var rect = _doMoveBehaviour.MyTarget.gameObject.GetComponent<RectTransform>().rect;
            var yCustom = placeHolderController != null
                ? placeHolderController._placeHolderText.renderedHeight
                : 0;
            _doMoveBehaviour.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(rect.width, yCustom);
        }
    }

    /// <summary>
    /// When your pointer up select any button, this method will be executed
    /// </summary>
    public void OnUp()
    {
        OnExit();
        
        if(_sourceButton && _buttonInteractableState == false || _isToggle && _toggle.interactable == false)
            return;
        
        PointerUpSound();
    }

    /// <summary>
    /// When GameObject is disable, this code will be executed
    /// </summary>
    private void OnDisable()
    {
        OnExit();
    }

    /// <summary>
    /// When your pointer down select any button, this method will be executed
    /// </summary>
    public void OnDown()
    {
        if(_sourceButton && ButtonInteractableState == false || _isToggle && _toggle.interactable == false)
            return;
        
        if(ToggleIsOn && _toggle.group != null && !_toggle.group.allowSwitchOff)
            return;
        
        if (_isToggle && !ToggleIsOn)
            ToggleIsOn = true;
        
        PointerDownSound();

        ChangeButtonEffects(ButtonTransition.Click);
    }

    /// <summary>
    /// When your pointer clicked up any button, this method will be executed
    /// </summary>
    private void PointerUpSound()
    {
        if (_onClickUpSound)
        {
            _audioSource.PlayOneShot(_onClickUpSound);
        }
    }


    /// <summary>
    /// When your pointer clicked down any button, this method will be executed
    /// </summary>
    private void PointerDownSound()
    {
        if (_onClickDownSound)
        {
            _audioSource.PlayOneShot(_onClickDownSound);
        }
    }

    /// <summary>
    /// When your pointer select any button, this method will be executed
    /// </summary>
    private void HoverSound()
    {
        if (_onHoverSound)
        {
            _audioSource.PlayOneShot(_onHoverSound);
        }
    }

    /// <summary>
    /// When your pointer select any button, this method will be executed
    /// </summary>
    private void ExitSound()
    {
        if (_onExitSound)
        {
            _audioSource.PlayOneShot(_onExitSound);
        }
    }

    private IEnumerator WaitAnySecondToEvent(float seconds, Action onCompleteWait)
    {
        yield return new WaitForSeconds(seconds);
        onCompleteWait?.Invoke();
    }

    /// <summary>
    /// When your pointer select any button, this method will be executed
    /// </summary>
    public void OnExit()
    {
        if(_sourceButton && ButtonInteractableState == false || _isToggle && _toggle.interactable == false)
            return;
        
        ResetScale();

        ExitSound();

        if (_tooltipsPlaceholder != null && _useTooltips && _tooltipsPlaceholder.activeInHierarchy)
        {
            switch (_tooltipTransition)
            {
                case TooltipTransition.Unset:
                    _tooltipsPlaceholder.SetActive(false);
                    _tooltipIsCurrentlyOn = false;
                    break;

                case TooltipTransition.Move:
                    _doMoveBehaviour.BeginDoLocalMoveInitialPosition();
                    break;
    
                case TooltipTransition.Fade:
                    _fadingBehaviour.BeginFadingOut();
                    break;
            }
        }
    }

    /// <summary>
    /// This method overrided BeginDoScale function, if you dont want to scale your button, set "Scale Value to 0 (zero)"
    /// </summary>
    public override void BeginDoScale()
    {
        base.BeginDoScale();

        if (ToggleIsOn)
        {
            _onToggleActiveHover?.Invoke();
            return;
        }

        ChangeButtonEffects(ButtonTransition.Hover);
    }

    /// <summary>
    /// This method is used to change button components when the interactable state is changed
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private void OnButtonInteractableChanged()
    {
        if (!_sourceButton.interactable)
            ChangeButtonEffects(ButtonTransition.Disable);
        else
            ChangeButtonEffects(ButtonTransition.Idle);
    }

    private void OnToggleInteractableChanged()
    {
        if (!_toggle.interactable && !ToggleIsOn)
            ChangeButtonEffects(ButtonTransition.Disable);
        else if(!ToggleIsOn && _toggle.interactable)
            ChangeButtonEffects(ButtonTransition.Idle);
    }

    private void OnToggleValueChanged()
    {
        if(ToggleIsOn)
            ChangeButtonEffects(ButtonTransition.Click);
        else
            ChangeButtonEffects(ButtonTransition.Idle);
    }

    private void ChangeButtonEffects(ButtonTransition transition)
    {
        switch (transition)
        {
            case ButtonTransition.Idle:
                switch (ImageMode)
                {
                    case ButtonMode.Image:
                        if (_onIdleImage && TargetImage) TargetImage.sprite = _onIdleImage;

                        break;
                    case ButtonMode.ImageColor:
                        if (TargetImage) TargetImage.color = _idleColor;

                        break;
                    case ButtonMode.FontColor:
                        if (TMPTexts.Count != 0) TMPTexts.ForEach(x => x.color = _idleFontColor);
                
                        break;
                    case ButtonMode.ImageAndImageColor:
                        if (_onIdleImage && TargetImage) TargetImage.sprite = _onIdleImage;
                        if (TargetImage) TargetImage.color = _idleColor;

                        break;
                    case ButtonMode.ImageAndFontColor:
                        if(_onIdleImage && TargetImage) TargetImage.sprite = _onIdleImage;
                        if (TMPTexts.Count != 0) TMPTexts.ForEach(x => x.color = _idleFontColor);
                
                        break;
                    case ButtonMode.ImageColorAndFontColor:
                        if (TargetImage) TargetImage.color = _idleColor;
                        if (TMPTexts.Count != 0) TMPTexts.ForEach(x => x.color = _idleFontColor);
                
                        break;
                    case ButtonMode.Unset:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                break;
            case ButtonTransition.Hover:
                switch (ImageMode)
                {
                    case ButtonMode.Image:
                        if (_onHoverImage && TargetImage)
                        {
                            TargetImage.color = Color.white;
                            TargetImage.sprite = _onHoverImage;
                        }

                        break;
                    case ButtonMode.ImageColor:
                        if (TargetImage) TargetImage.color = _hoverColor;

                        break;
                    case ButtonMode.FontColor:
                        if (TMPTexts.Count != 0) TMPTexts.ForEach(x => x.color = _hoverFontColor);
                
                        break;
                    case ButtonMode.ImageAndImageColor:
                        if (_onHoverImage && TargetImage)
                        {
                            TargetImage.color = Color.white;
                            TargetImage.sprite = _onHoverImage;
                        }

                        if (TargetImage) TargetImage.color = _hoverColor;
                        break;
                    case ButtonMode.ImageAndFontColor:
                        if (_onHoverImage && TargetImage) TargetImage.sprite = _onHoverImage;
                        if (TMPTexts.Count != 0) TMPTexts.ForEach(x => x.color = _hoverFontColor);
                
                        break;
                    case ButtonMode.ImageColorAndFontColor:
                        if (TargetImage) TargetImage.color = _hoverColor;
                        if (TMPTexts.Count != 0) TMPTexts.ForEach(x => x.color = _hoverFontColor);
                
                        break;
                    case ButtonMode.Unset:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                break;
            case ButtonTransition.Click:
                switch (ImageMode)
                {
                    case ButtonMode.Image:
                        if (_onClickImage && TargetImage) TargetImage.sprite = _onClickImage;

                        break;
                    case ButtonMode.ImageColor:
                        if (TargetImage) TargetImage.color = _clickColor;

                        break;
                    case ButtonMode.FontColor:
                        if (TMPTexts.Count != 0) TMPTexts.ForEach(x => x.color = _clickFontColor);
                
                        break;
                    case ButtonMode.ImageAndImageColor:
                        if (_onClickImage && TargetImage) TargetImage.sprite = _onClickImage;
                        if (TargetImage) TargetImage.color = _clickColor;

                        break;
                    case ButtonMode.ImageAndFontColor:
                        if (_onClickImage && TargetImage) TargetImage.sprite = _onClickImage;
                        if (TMPTexts.Count != 0) TMPTexts.ForEach(x => x.color = _clickFontColor);
                
                        break;
                    case ButtonMode.ImageColorAndFontColor:
                        if (TargetImage) TargetImage.color = _clickColor;
                        if (TMPTexts.Count != 0) TMPTexts.ForEach(x => x.color = _clickFontColor);
                
                        break;
                    case ButtonMode.Unset:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                break;
            case ButtonTransition.Disable:
                switch (ImageMode)
                {
                    case ButtonMode.Image:
                        if (_onDisableImage && TargetImage)
                        {
                            TargetImage.color = Color.white;
                            TargetImage.sprite = _onDisableImage;
                        }

                        break;
                    case ButtonMode.ImageColor:
                        if (TargetImage) TargetImage.color = _disableColor;

                        break;
                    case ButtonMode.FontColor:
                        if (TMPTexts.Count != 0) TMPTexts.ForEach(x => x.color = _disableFontColor);
                
                        break;
                    case ButtonMode.ImageAndImageColor:
                        if (_onDisableImage && TargetImage)
                        {
                            TargetImage.color = Color.white;
                            TargetImage.sprite = _onDisableImage;
                        }

                        if (TargetImage) TargetImage.color = _disableColor;
                        break;
                    case ButtonMode.ImageAndFontColor:
                        if (_onDisableImage && TargetImage) TargetImage.sprite = _onDisableImage;
                        if (TMPTexts.Count != 0) TMPTexts.ForEach(x => x.color = _disableFontColor);
                
                        break;
                    case ButtonMode.ImageColorAndFontColor:
                        if (TargetImage) TargetImage.color = _disableColor;
                        if (TMPTexts.Count != 0) TMPTexts.ForEach(x => x.color = _disableFontColor);
                
                        break;
                    case ButtonMode.Unset:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                break;
        }
    }

    /// <summary>
    /// This method overrides ResetScale from DoScaleBehaviour, after scaling, your button will be reseted to normal scale
    /// </summary>
    public override void ResetScale()
    {
        base.ResetScale();

        if (ToggleIsOn)
        {
            _onToggleActiveExit?.Invoke();
            return;
        }

        ChangeButtonEffects(ButtonTransition.Idle);
    }
    
    private void ToggleTooltipStatus(bool isOn)
    {
        Debug.Log($"Completed");
        if (_useTooltips)
            _tooltipIsCurrentlyOn = isOn;
    }

    private void SetLocalToggle(bool isOn)
    {
        ToggleIsOn = isOn;
        
        if(ToggleIsOn)
            _onToggleActive?.Invoke();
        else
            _onToggleInactive?.Invoke();
    }
}