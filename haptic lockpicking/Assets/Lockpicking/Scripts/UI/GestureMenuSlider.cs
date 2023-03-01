using UnityEngine;
using SG;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static CutoutManager;

public class GestureMenuSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
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
    CutoutState _cutoutState;

    int _cutoutInt;

    int sign = 1;

    float _pressStartTime = float.PositiveInfinity;

    bool _leftGestureActive = false;

    bool _rightGestureActive = false;

    bool _gesturesHolding = false;



    public void Start()
    {

        _cutoutInt = (int)_cutoutState;

        _backgroundSlider.value = _cutoutInt;

        if (_cutoutInt == 2)
        {
            sign = -1;
        }

        SliderMoved();
    }

    public void Update()
    {
        UpdateGestures();

        _backgroundSlider.value = Mathf.Clamp(MathLib.Remap(Time.time, _pressStartTime, _pressStartTime + _buttonHoldTime, (float)_cutoutInt, (float)(_cutoutInt + sign)), 0, 2);

        if (Time.time - _pressStartTime > _buttonHoldTime)
        {
            _pressStartTime = float.PositiveInfinity;
            _leftGestureActive = false;
            _rightGestureActive = false;

            _cutoutInt += sign;

            if (_cutoutInt == 0)
            {
                sign = 1;
            }
            if (_cutoutInt == 2)
            {
                sign = -1;
            }


            SliderMoved();
        }
    }

    public void SliderMoved()
    {
        _supportElementManager.SetCutout(_cutoutInt);
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
