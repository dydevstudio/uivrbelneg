using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class DoMoveObject 
{
    private Transform _targetLocation { set; get; }
    private DoMoveBehaviour _doMoveBehaviour { set; get; }
    private Transform _myTransform { set; get; }
    private Ease _selectEase { set; get; }
    private UnityEvent _onCompleteMove { set; get; }
    private UnityEvent _onCompleteMoveBack { set; get; }
    private float _targetValue { set; get; }
    private float _speed { set; get; }
    private InitialPosRotBehaviour InitialPosRotBehaviour { set; get; }

    public DoMoveObject(DoMoveBehaviour doMoveBehaviour, Transform myTransform,Transform targetLocation, Ease selectEase, 
        UnityEvent onCompleteMove, UnityEvent onCompleteMoveBack, float targetValue, float speed, InitialPosRotBehaviour initialPosRotBehaviour)
    {
        _targetLocation = targetLocation;
        
        _doMoveBehaviour = doMoveBehaviour;
        
        _myTransform = myTransform;
        
        _selectEase = selectEase;
        
        _onCompleteMove = onCompleteMove;

        _onCompleteMoveBack = onCompleteMoveBack;
        
        _targetValue = targetValue;
        
        _speed = speed;
        
        InitialPosRotBehaviour = initialPosRotBehaviour;
    }

    public void BeginDoSelectMove()
    {
        _doMoveBehaviour.StopAllCoroutines();
        DoSelectMove(_targetValue);
    }
    
    public void BeginDoSelectLocalMove()
    {
        _doMoveBehaviour.StopAllCoroutines();
        DoSelectLocalMove(_targetValue);
    }
    
    public void DoMoveBacktoInitPosition(Vector3 _myTarget,float delay)
    {
        _doMoveBehaviour.StartCoroutine(CoroutineDoMoveBacktoInitPosition(_myTarget,delay));
    }

    private IEnumerator CoroutineDoMoveBacktoInitPosition(Vector3 _myTarget,float delay)
    {
        yield return new WaitForSeconds(delay);
        
        _myTransform.DOMove(_myTarget, _speed)
            .SetEase(_selectEase).SetId("DoMove").OnComplete(_doMoveBehaviour.OnCompleteMovingBack);
    }
    
    public void DoLocalMoveBacktoInitPosition(Vector3 _myTarget,float delay)
    {
        _doMoveBehaviour.StartCoroutine(CoroutineDoLocalMoveBacktoInitPosition(_myTarget,delay));
    }

    private IEnumerator CoroutineDoLocalMoveBacktoInitPosition(Vector3 _myTarget,float delay)
    {
        yield return new WaitForSeconds(delay);
        bool normalComplete = false;
        Tween a=_myTransform.DOLocalMove(_myTarget, _speed)
            .SetEase(_selectEase).SetId("DoLocalMove").OnComplete(()=>
            {
                normalComplete = true;
                _doMoveBehaviour.OnCompleteMovingBack();
            });
        bool b = a.IsActive();
        while (b)
        {
            b = a.IsActive();
            yield return null;
        }
        
        if (normalComplete)
        {
            yield break;
        }
        DoLocalMoveBacktoInitPosition(_myTarget, 0);
       
    }
    public void DoMove()
    {
        _myTransform.DOMove(_targetLocation.position, _speed)
            .SetEase(_selectEase).SetId("DoMove");
        
        _myTransform.DORotateQuaternion(_targetLocation.rotation, _speed)
            .SetEase(_selectEase).SetId("DoMove").OnComplete(_doMoveBehaviour.OnCompleteMoving);
    }

    public void DoLocalMove()
    {
        _myTransform.DOLocalMove(_targetLocation.localPosition, _speed)
            .SetEase(_selectEase).SetId("DoLocalMove");
        
        _myTransform.DOLocalRotateQuaternion(_targetLocation.rotation, _speed)
            .SetEase(_selectEase).SetId("DoMove").OnComplete(_doMoveBehaviour.OnCompleteMoving);
    }

    public void DoSelectLocalMove(float myTarget)
         {
             switch (_doMoveBehaviour.Mode)
             {
                 case DoMoveBehaviour.MyMode.X:
                     _myTransform.DOLocalMoveX(myTarget, _speed)
                         .SetEase(_selectEase).SetId("DoSelectLocalMove").OnComplete(_doMoveBehaviour.OnCompleteMoving);
                     break;
                 case DoMoveBehaviour.MyMode.Y:
                     _myTransform.DOLocalMoveY(myTarget, _speed)
                         .SetEase(_selectEase).SetId("DoSelectLocalMove").OnComplete(_doMoveBehaviour.OnCompleteMoving);
                     break;
                 case DoMoveBehaviour.MyMode.Z:
                     _myTransform.DOLocalMoveZ(myTarget, _speed)
                         .SetEase(_selectEase).SetId("DoSelectLocalMove").OnComplete(_doMoveBehaviour.OnCompleteMoving);
                     break;
                 default:
                     throw new ArgumentOutOfRangeException();
             }
         }

    public void DoSelectMove(float myTarget)
    {
        if (_myTransform)
        {
            switch (_doMoveBehaviour.Mode)
            {
                case DoMoveBehaviour.MyMode.X:
                    _myTransform.DOMoveX(myTarget, _speed)
                        .SetEase(_selectEase).SetId("DoSelectMove").OnComplete(_doMoveBehaviour.OnCompleteMoving);
                    break;
                case DoMoveBehaviour.MyMode.Y:
                    _myTransform.DOMoveY(myTarget, _speed)
                        .SetEase(_selectEase).SetId("DoSelectMove").OnComplete(_doMoveBehaviour.OnCompleteMoving);
                    break;
                case DoMoveBehaviour.MyMode.Z:
                    _myTransform.DOMoveZ(myTarget, _speed)
                        .SetEase(_selectEase).SetId("DoSelectMove").OnComplete(_doMoveBehaviour.OnCompleteMoving);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
