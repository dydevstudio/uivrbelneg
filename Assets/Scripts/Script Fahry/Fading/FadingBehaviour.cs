using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FadingBehaviour : MonoBehaviour
{
    [HideInInspector] public CanvasGroup _myCanvasGroup;
    
    [HideInInspector] public Image _myClockPanel;
    
    [HideInInspector] public Transform EyeController;

    [HideInInspector] public float _speed;
    
    [HideInInspector] public float _delay;

    [HideInInspector] public float _endDelay = 0.5f;
    
    public enum Type
    {
        Unset,
        Normal,
        Gradient,
        Eye,
        Clock
    }
    [HideInInspector] public MyMode Mode;
    
    public enum MyMode
    {
        X,
        Y,
        Z
    }
    
    [HideInInspector] public float _targetValue;
    
    [HideInInspector] public Ease _selectEase;
    
    [HideInInspector] public bool InOut;
    
    [HideInInspector] public bool OnStart;
    
    [HideInInspector] public bool FadingIn;
    
    [HideInInspector] public bool FadingOut;
        
    [HideInInspector] public Type FadingType;

    [HideInInspector] public UnityEvent OnBeginFadingIn;
    
    [HideInInspector] public UnityEvent OnBeginFadingOut;
    
    [HideInInspector] public UnityEvent OnCompleteFadingIn;
    
    [HideInInspector] public UnityEvent OnCompleteFadingOut;
    
    [HideInInspector] public Texture2D Logo = null;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            BeginFadingIn();
        
        if(Input.GetKeyDown(KeyCode.N))
            BeginFadingOut();
    }

    private void OnValidate()
    {
        if (GetComponent<CanvasGroup>())
        {
            _myCanvasGroup = GetComponent<CanvasGroup>();
        }
    }

    private void Awake()
    {
        if (OnStart)
        {
            if (FadingIn)
            {
                BeginFadingIn();
            }

            if (FadingOut)
            {
                BeginFadingOut();
            }
        }
        switch (FadingType)
        {
            case Type.Normal:
                
                _myCanvasGroup.gameObject.AddComponent<InitialPosRotBehaviour>();
                
                break;
            
            case Type.Gradient:
                
                _myCanvasGroup.gameObject.AddComponent<InitialPosRotBehaviour>();
                
                break;
            
            case Type.Eye:
                
                break;
            
            case Type.Clock:
                
                break;
        }
    }

    /// <summary>
    /// Function to set delay on Fading In/Out
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    public float SetDelay(float delay)
    {
        _delay = delay;
        
        return _delay;
    }

    /// <summary>
    /// Function to call Coroutine FadingIn
    /// </summary>
    public void BeginFadingIn()
    {
        StartCoroutine(CoBeginFadingIn());
    }

    /// <summary>
    /// Coroutine Fading In, had 4 type of Fading (Normal, Gradient, Eye, Clock)
    /// </summary>
    /// <returns></returns>
    IEnumerator CoBeginFadingIn()
    {
        switch (FadingType)
        {
            case Type.Normal:
                
                //Reset();
                
                yield return new WaitForSeconds(_delay);
                
                OnBeginFadingIn.Invoke();

                _myCanvasGroup.alpha = 0;
                
                //_myCanvasGroup.transform.localPosition = new Vector3(_targetValue,0,0);
                
                _myCanvasGroup.DOFade(1, _speed).SetId("Fade In Normal").OnComplete(OnCompleteFadeIn);
                

                break;
            
            case Type.Gradient:
                
                //Reset();
                
                yield return new WaitForSeconds(_delay);
                
                OnBeginFadingIn.Invoke();
                
                _myCanvasGroup.alpha = 1;
                
                _myCanvasGroup.transform.localPosition = _myCanvasGroup.GetComponent<InitialPosRotBehaviour>().GetLocalPos;
                
                switch (Mode)
                {
                    case MyMode.X:
                        _myCanvasGroup.transform.DOLocalMoveX(_targetValue, _speed)
                            .SetEase(_selectEase).SetId("Fade In Gradient").OnComplete(OnCompleteFadeIn);
                        break;
                    case MyMode.Y:
                        _myCanvasGroup.transform.DOLocalMoveY(_targetValue, _speed)
                            .SetEase(_selectEase).SetId("Fade In Gradient").OnComplete(OnCompleteFadeIn);
                        break;
                    case MyMode.Z:
                        _myCanvasGroup.transform.DOLocalMoveZ(_targetValue, _speed)
                            .SetEase(_selectEase).SetId("Fade In Gradient").OnComplete(OnCompleteFadeIn);
                        break;
                }
                
                break;
            
            case Type.Eye:
                
                yield return new WaitForSeconds(_delay);
                
                OnBeginFadingIn.Invoke();
                
                EyeController.DOScaleY(0, _speed)
                    .SetEase(_selectEase).SetId("Fade In Eye").OnComplete(OnCompleteFadeIn);
                break;
            
            case Type.Clock:
                
                yield return new WaitForSeconds(_delay);
                
                OnBeginFadingIn.Invoke();

                _myClockPanel.DOFillAmount(1, _speed).SetEase(_selectEase).SetId("Fade In Clock").OnComplete(OnCompleteFadeIn);
                 
                break;
        }
    }

    private IEnumerator NormalFadingOutRoutine()
    {
        yield return new WaitForSeconds(_endDelay);
        Tween fadeOut = _myCanvasGroup.DOFade(0, _speed).SetId("Fade In Normal");

        StartCoroutine(CompletionCheck(fadeOut));
    }
    
    public void BeginFadingOut()
    {
        switch (FadingType)
        {
            case Type.Normal:

                OnBeginFadingOut.Invoke();
                
                StartCoroutine(NormalFadingOutRoutine());
                
                break;
            
            case Type.Gradient:
                
                OnBeginFadingOut.Invoke();

                _myCanvasGroup.transform.DOLocalMove(_myCanvasGroup.GetComponent<InitialPosRotBehaviour>().GetLocalPos, _speed)
                    .SetEase(_selectEase).SetId("Fade Out Gradient").OnComplete(OnCompleteFadeOut);;
                
                break;
            
            case Type.Eye:
                
                OnBeginFadingOut.Invoke();
                
                EyeController.DOScaleY(0.5f, _speed)
                    .SetEase(_selectEase).SetId("Fade Out Eye").OnComplete(OnCompleteFadeOut);
                
                break;
            
            case Type.Clock:
                
                OnBeginFadingOut.Invoke();

                _myClockPanel.DOFillAmount(0, _speed).SetEase(_selectEase).SetId("Fade Out Clock").OnComplete(OnCompleteFadeIn);
                 
                break;
            
        }
    }

    private IEnumerator CompletionCheck(Tween tween)
    {
        bool normalcomplete = false;
        tween.OnComplete(() => { normalcomplete = true; });
        bool b = tween.IsActive();
        while (b)
        {
            b = tween.IsActive();
            yield return null;
        }
        
        if (normalcomplete)
        {
            OnCompleteFadeOut();

            yield break;
        }

        BeginFadingOut();
    }
    
    private void OnCompleteFadeOut()
    {
        OnCompleteFadingOut.Invoke();
        
        _myCanvasGroup.interactable = false;
        
        _myCanvasGroup.blocksRaycasts = false;

    }

    public void OnCompleteFadeIn()
    {
        if (InOut)
        {
            BeginFadingOut();
        }
        else
        {
            OnCompleteFadingIn.Invoke();
            
            _myCanvasGroup.interactable = true;
            
            _myCanvasGroup.blocksRaycasts = true;
        }
        
    }

    public void Reset()
    {
        if (_myCanvasGroup)
        {
            _myCanvasGroup.GetComponent<Image>().type = Image.Type.Simple;

            _myCanvasGroup.transform.localScale =
                _myCanvasGroup.transform.GetComponent<InitialPosRotBehaviour>().GetLocalScale;
                
            _myCanvasGroup.transform.GetComponent<RectTransform>().sizeDelta =
                _myCanvasGroup.transform.GetComponent<InitialPosRotBehaviour>().SizeDelta;
        }
        
    }
}
