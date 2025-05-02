using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class DoScaleObject 
{
    public MyMode _myMode;
    public enum MyMode
    {
        Unset,
        X,
        Y,
        Z
    }
    private UnityEvent _onCompleteScaling { get; }
    private Ease _ease { get; }
    private Transform _myTransform { get; }
    private float _speed { get; }
    private float _scaleValue { get; }
    private float _initValue{ get; }

    public DoScaleObject(Transform myTransform, float speed,float initValue, float scaleValue, Ease ease, MyMode myMode ,UnityEvent onCompleteScaling)
    {
        _initValue = initValue;
        
        _myMode = myMode;

        _onCompleteScaling = onCompleteScaling;
        
        _ease = ease;
        
        _myTransform = myTransform;
        
        _speed = speed;
        
        _scaleValue = scaleValue;
    }

    /// <summary>
    /// Do Scale
    /// </summary>
    public void DoScale()
    {
        _myTransform.DOScale(_scaleValue, _speed).SetEase(_ease).SetId("DoScale").OnComplete(OnCompleteScaling);
    }
    
    /// <summary>
    /// Do Select Scale
    /// </summary>
    public void DoSelectScale()
    {
        switch (_myMode)
        {
            case MyMode.X:
                
                _myTransform.DOScaleX(_scaleValue,_speed).SetEase(_ease).SetId("DoSelectScale").OnComplete(OnCompleteScaling);
                
                break;
            
            case MyMode.Y:
                
                _myTransform.DOScaleY(_scaleValue,_speed).SetEase(_ease).SetId("DoSelectScale").OnComplete(OnCompleteScaling);
                
                break;
            
            case MyMode.Z:
                
                _myTransform.DOScaleZ(_scaleValue,_speed).SetEase(_ease).SetId("DoSelectScale").OnComplete(OnCompleteScaling);
                
                break;
            case MyMode.Unset:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <summary>
    /// On Complete Scaling
    /// </summary>
    private void OnCompleteScaling()
    {
        _onCompleteScaling.Invoke();
    }
    
    /// <summary>
    /// Reset Scale
    /// </summary>
    public void Reset()
    {
        _myTransform.DOScale(_initValue, _speed).SetEase(_ease).SetId("DoScale").OnComplete(OnCompleteScaling);
    }
}
