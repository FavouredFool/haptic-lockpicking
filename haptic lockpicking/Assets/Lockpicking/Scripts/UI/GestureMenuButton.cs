using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG;
using SGCore;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GestureMenuButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    [SerializeField]
    SG_BasicGesture _leftGesture;

    [SerializeField]
    SG_BasicGesture _rightGesture;

    [SerializeField]
    MenuManager _menuManager;
    
    [SerializeField, Range(1, 5)]
    float _buttonHoldTime;

    [SerializeField]
    Slider _backgroundSlider;

    Button _button;

    bool _buttonIsPressed = false;

    float _pressStartTime = float.PositiveInfinity;

    SG_HandPose _leftHandPose;

    SG_HandPose _rightHandPose;

    bool _leftGestureActive = false;

    bool _rightGestureActive = false;

    bool _gesturesHolding = false;

    public void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void Update()
    {
        UpdateGestures();

        _backgroundSlider.value = Mathf.Clamp01(MathLib.Remap(Time.time, _pressStartTime, _pressStartTime + _buttonHoldTime, 0, 1));

        if (Time.time - _pressStartTime > _buttonHoldTime)
        {
            _pressStartTime = float.PositiveInfinity;
            _leftGestureActive = false;
            _rightGestureActive = false;

            ButtonActivated();
        }
    }

    public void ButtonActivated()
    {
        Debug.Log("do stuff");
    }

    public void UpdateGestures()
    {
        if (_leftGesture.GestureMade)
        {
            _leftGestureActive = true;
        }
        if (_leftGesture.GestureStopped)
        {
            _leftGestureActive = false;
        }

        if (_rightGesture.GestureMade)
        {
            _rightGestureActive = true;
        }
        if (_rightGesture.GestureStopped)
        {
            _rightGestureActive = false;
        }

        if (_leftGestureActive && _rightGestureActive && !_gesturesHolding)
        {
            _pressStartTime = Time.time;
            _gesturesHolding = true;
        }

        if ((!_leftGestureActive || !_rightGestureActive) && _gesturesHolding)
        {
            _pressStartTime = float.PositiveInfinity;
            _gesturesHolding = false;
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        _pressStartTime = Time.time;
    }

    public void OnPointerUp(PointerEventData data)
    {
        _pressStartTime = float.PositiveInfinity;
    }




}
