using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class DoMoveBehaviour : MonoBehaviour
{
    [HideInInspector] public Transform MyTarget;

    [HideInInspector] public Transform TargetLocation;

    //[SerializeField] private IntialPosRotBehaviour intialPosRotBehaviour;
    [HideInInspector] [Tooltip("Select your move type in ease mode")]
    public Ease _selectEase;

    [HideInInspector] [Tooltip("Select your move direction")]
    public MyMode Mode;

    [HideInInspector] [Tooltip("Adjust your move speed / duration, e.g : 1 is faster than 10")]
    public float _speed;

    [HideInInspector] [Tooltip("Adjust your delay duration, e.g : 1 is faster than 10")]
    public float EndAnimDelay = 0.5f;

    [HideInInspector] [Tooltip("Start function with delayed")]
    public float DelayedTime = 2;

    [HideInInspector] [Tooltip("Set your target (X/Y/Z) value")]
    public float _targetValue;

    [HideInInspector] public UnityEvent OnCompleteMove;

    [HideInInspector] public UnityEvent OnCompleteMoveBack;

    [HideInInspector] [Tooltip("Start function with delayed")]
    public bool BoolDelayTime;

        [HideInInspector] [Tooltip("Start function on start")]
    public bool OnStart;

    public enum MyMode
    {
        X,
        Y,
        Z
    }

    public enum Type
    {
        Unset,
        SpesificTransform,
        XYZMode
    }

    [HideInInspector] public Type SelectType;

    private DoMoveObject _doMoveObject;

    [HideInInspector] public Texture2D Logo = null;

    private Transform _savedAttach;

    public void InitialAttach()
    {
        if (!MyTarget) return;
        if (MyTarget.GetComponent<InitialPosRotBehaviour>())
        {
            return;
        }

        MyTarget.gameObject.AddComponent<InitialPosRotBehaviour>();

        if (_savedAttach)
        {
            if (MyTarget != _savedAttach)
            {
                RemoveAttach();
            }
        }
            
        _savedAttach = MyTarget;
    }

    public void RemoveAttach()
    {
        if (!_savedAttach) return;
        DestroyImmediate(_savedAttach.GetComponent<InitialPosRotBehaviour>());
        _savedAttach = null;
    }

    private void OnDisable()
    {
        var initpos = MyTarget.GetComponent<InitialPosRotBehaviour>();
        
        StopAllCoroutines();
        
        MyTarget.transform.localPosition = initpos.GetLocalPos;

        MyTarget.DOKill();
    }

    private void Awake()
    {
        if (MyTarget.GetComponent<InitialPosRotBehaviour>() != null)
        {
            _doMoveObject = new DoMoveObject(this, MyTarget, TargetLocation, _selectEase, OnCompleteMove, OnCompleteMoveBack, _targetValue,
                _speed, MyTarget.GetComponent<InitialPosRotBehaviour>());
        }
        else
        {
            Debug.LogWarning("You need to attach ''InitialPosRotBehaviour'' to your Target transform !");
        }
    }
    private void OnDestroy()
    {
        // Check if the script is still present and active before calling ExitSound()
        if (this != null && gameObject.activeSelf)
        {
            DOTween.Clear();
        }
    }

    private void Start()
    {
        if (!OnStart) return;
        switch (SelectType) {
            case Type.SpesificTransform:

                BeginDoLocalMove();

                break;
                
            case Type.XYZMode:

                BeginDoSelectLocalMove();    
                        
                break;

            case Type.Unset:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void SetTargetLocation(Transform transform)
    {
        TargetLocation = transform;

        _doMoveObject = new DoMoveObject(this, MyTarget, TargetLocation, _selectEase, OnCompleteMove, OnCompleteMoveBack, _targetValue,
            _speed, MyTarget.GetComponent<InitialPosRotBehaviour>());
    }

    /// <summary>
    /// Do Move
    /// </summary>
    public virtual void BeginDoMove()
    {
        if (BoolDelayTime)
        {
            StartCoroutine(CoroutineDoMove());
        }
        else
        {
            _doMoveObject.DoMove();
        }
    }

    /// <summary>
    /// Do Local Move
    /// </summary>
    public virtual void BeginDoLocalMove()
    {
        if (BoolDelayTime)
        {
            StartCoroutine(CoroutineDoLocalMove());
        }
        else
        {
            _doMoveObject.DoLocalMove();
        }
    }

    /// <summary>
    /// Do Select Move
    /// </summary>
    public virtual void BeginDoSelectMove()
    {
        if (BoolDelayTime)
        {
            StartCoroutine(CoroutineDoSelectMove());
        }
        else
        {
            _doMoveObject.BeginDoSelectMove();
        }
    }

    /// <summary>
    /// Do Select Local Move
    /// </summary>
    public virtual void BeginDoSelectLocalMove()
    {
        if (BoolDelayTime)
        {
            StartCoroutine(CoroutineDoSelectLocalMove());
        }
        else
        {
            _doMoveObject.BeginDoSelectLocalMove();
        }
    }
    
    /// <summary>
    /// Do Move Initial Position
    /// </summary>
    public virtual void BeginDoMoveInitialPosition()
    {
        if (MyTarget)
        {
            _doMoveObject.DoMoveBacktoInitPosition(MyTarget.GetComponent<InitialPosRotBehaviour>().GetPos,EndAnimDelay);
        }
    }

    /// <summary>
    /// Do Local Move Initial Position
    /// </summary>
    public virtual void BeginDoLocalMoveInitialPosition()
    {
        if (MyTarget)
        {
            _doMoveObject.DoLocalMoveBacktoInitPosition(MyTarget.GetComponent<InitialPosRotBehaviour>().GetLocalPos,EndAnimDelay);
        }
    }

    /// <summary>
    /// On Complete Moving
    /// </summary>
    public void OnCompleteMoving()
    {
        OnCompleteMove?.Invoke();
    }

    public void OnCompleteMovingBack()
    {
        OnCompleteMoveBack?.Invoke();
    }

    private IEnumerator CoroutineDoMove()
    {
        yield return new WaitForSeconds(DelayedTime);

        _doMoveObject.DoMove();
    }

    private IEnumerator CoroutineDoSelectMove()
    {
        yield return new WaitForSeconds(DelayedTime);

        _doMoveObject.BeginDoSelectMove();
    }

    private IEnumerator CoroutineDoLocalMove()
    {
        yield return new WaitForSeconds(DelayedTime);

        _doMoveObject.DoLocalMove();
    }

    private IEnumerator CoroutineDoSelectLocalMove()
    {
        yield return new WaitForSeconds(DelayedTime);

        _doMoveObject.BeginDoSelectLocalMove();
    }
}
