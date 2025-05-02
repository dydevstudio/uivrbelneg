using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class NavigationController : MonoBehaviour
{
    [SerializeField] private FixedJoystick _horizontalJoystick;
    [SerializeField] private FixedJoystick _verticalJoystick;
    [SerializeField] private FixedJoystick _rotateJoystick;

    [SerializeField] private Slider _sliderHorizontal;
    [SerializeField] private Slider _sliderVertical;

    [SerializeField] private TMP_Text _horizontalPercent;
    [SerializeField] private TMP_Text _verticalPercent;

    [SerializeField] private GameObject _navigationToolCanvas;
    [SerializeField] private GameObject _camera;

    [SerializeField] private float _horizontalMoveSpeed;
    [SerializeField] private float _verticalMoveSpeed;
    [SerializeField] private int _rotateSpeed;
    
    public Vector3 DefaultCameraPos, DefaultCameraRot;
    
    private bool _onResetting = false;

    private void Awake()
    {
        DefaultCameraPos = _camera.transform.position;
        DefaultCameraRot = _camera.transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the navigation canvas is enabled and active then move and rotate based on joystick input
        if(_navigationToolCanvas.activeInHierarchy && !_onResetting)
        {
            var horizontalDir = _horizontalJoystick.Direction;
            var verticalDir = _verticalJoystick.Direction;
            var rotationDir = _rotateJoystick.Direction;
            
            //Control movement and rotation
            _camera.transform.Translate(new Vector3(horizontalDir.x * _horizontalMoveSpeed, verticalDir.y * _verticalMoveSpeed, horizontalDir.y * _horizontalMoveSpeed) * Time.deltaTime);
            _camera.transform.eulerAngles += new Vector3( -rotationDir.y,  rotationDir.x, transform.rotation.z) * _rotateSpeed * Time.deltaTime;
        }
    }

    public void SetHorizontalSpeed()
    {
        _horizontalMoveSpeed = _sliderHorizontal.value;
    }
    public void SetVerticalSpeed()
    {
        _verticalMoveSpeed = _sliderVertical.value;
    }
    public void SetPercentageTextHor()
    {
        _horizontalPercent.text = (_sliderHorizontal.value / _sliderHorizontal.maxValue * 100).ToString("0") + "%";
    }
    public void SetPercentageTextVer()
    {
        _verticalPercent.text = (_sliderVertical.value / _sliderVertical.maxValue * 100).ToString("0") + "%";
    }

    public void ResetTransform()
    {
        _onResetting = true;
        
        StartCoroutine(ResetPosRoutine(DefaultCameraPos, 1f, () => _onResetting = false));
        StartCoroutine(ResetRotRoutine(DefaultCameraRot, 1f, () => _onResetting = false));
    }

    private IEnumerator ResetPosRoutine(Vector3 target, float duration, Action onComplete)
    {
        float time = 0;
        Vector3 startPosition = _camera.transform.position;

        while (time < duration)
        {
            _camera.transform.position = Vector3.Lerp(startPosition, target, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        _camera.transform.position = target;
        onComplete?.Invoke();
    }

    private IEnumerator ResetRotRoutine(Vector3 target, float duration, Action onComplete)
    {
        float time = 0;
        Vector3 startRotation = _camera.transform.eulerAngles;

        while (time < duration)
        {
            _camera.transform.eulerAngles = Vector3.Lerp(startRotation, target, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        _camera.transform.eulerAngles = target;
        
        onComplete?.Invoke();
    }
}
