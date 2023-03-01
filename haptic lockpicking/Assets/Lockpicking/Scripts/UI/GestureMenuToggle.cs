using UnityEngine;
using SG;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GestureMenuToggle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum UseCaseToggle { PINCOLOR, FORCEINDICATOR, PICKINDICATOR };


    [SerializeField]
    SG_BasicGesture _leftGesture;

    [SerializeField]
    SG_BasicGesture _rightGesture;

    [SerializeField]
    SupportElementManager _supportElementManager;
    
    [SerializeField, Range(1, 5)]
    float _buttonHoldTime;

    [SerializeField]
    Slider _backgroundSlider;

    [SerializeField]
    bool _toggleActive = false;

    [SerializeField]
    UseCaseToggle _useCase;

    float _pressStartTime = float.PositiveInfinity;

    bool _leftGestureActive = false;

    bool _rightGestureActive = false;

    bool _gesturesHolding = false;



    public void Start()
    {
        if (_toggleActive)
        {
            _backgroundSlider.value = 1;
        }
        else
        {
            _backgroundSlider.value = 0;
        }
    }

    public void Update()
    {
        UpdateGestures();

        float upperBound = _toggleActive ? 0 : 1;

        _backgroundSlider.value = Mathf.Clamp01(MathLib.Remap(Time.time, _pressStartTime, _pressStartTime + _buttonHoldTime, 1-upperBound, upperBound));

        if (Time.time - _pressStartTime > _buttonHoldTime)
        {
            _pressStartTime = float.PositiveInfinity;
            _leftGestureActive = false;
            _rightGestureActive = false;

            ToggleActivated();
        }
    }

    public void ToggleActivated()
    {

        _toggleActive = !_toggleActive;

        switch (_useCase)
        {
            case UseCaseToggle.PICKINDICATOR:
                _supportElementManager.TogglePickIndicator(_toggleActive);
                break;
            case UseCaseToggle.FORCEINDICATOR:
                _supportElementManager.ToggleForceIndicator(_toggleActive);
                break;
            case UseCaseToggle.PINCOLOR:
                _supportElementManager.TogglePinColor(_toggleActive);
                break;
        }

    }

    public void UpdateGestures()
    {
        if (!(_leftGesture != null && _rightGesture != null))
        {
            return;
        }

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
