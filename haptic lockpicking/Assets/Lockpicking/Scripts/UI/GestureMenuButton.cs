using UnityEngine;
using SG;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GestureMenuButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum UseCase { START, NEXT, EXIT, RESET, CALIBRATE };

    public enum Hand { RIGHT, LEFT, BOTH };


    [SerializeField]
    SG_BasicGesture _leftGesture;

    [SerializeField]
    SG_BasicGesture _rightGesture;
    
    [SerializeField, Range(1, 5)]
    float _buttonHoldTime;

    [SerializeField]
    Slider _backgroundSlider;

    [SerializeField]
    UseCase _useCase;

    [SerializeField]
    Hand _hand;

    float _pressStartTime = float.PositiveInfinity;

    bool _leftGestureActive = false;

    bool _rightGestureActive = false;

    bool _gesturesHolding = false;

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

        if (_useCase == UseCase.START && Input.GetKeyDown(KeyCode.R))
        {
            ButtonActivated();
        }
    }

    public void ButtonActivated()
    {
        switch (_useCase)
        {
            case UseCase.START:
                TutorialSectionManager.Instance.StartPressed();
                break;
            case UseCase.NEXT:
                TutorialSectionManager.Instance.NextPressed();
                break;
            case UseCase.EXIT:
                TutorialSectionManager.Instance.ExitPressed();
                break;
            case UseCase.RESET:
                TutorialSectionManager.Instance.ResetPressed();
                break;
            case UseCase.CALIBRATE:
                TutorialSectionManager.Instance.CalibratePressed();
                break;
        }

    }

    public void UpdateGestures()
    {
        if (_hand == Hand.BOTH)
        {
            BothHandsGesture();
            return;
        }

        OneHandGesture(_hand);

    }

    public void OneHandGesture(Hand hand)
    {
        SG_BasicGesture handGesture = (hand == Hand.LEFT) ? _leftGesture : _rightGesture;

        if (!handGesture)
        {
            return;
        }

        if (handGesture.GestureMade)
        {
            _pressStartTime = Time.time;

        }
        if (handGesture.GestureStopped)
        {
            _pressStartTime = float.PositiveInfinity;
        }
    }

    public void BothHandsGesture()
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
            _rightGestureActive = false;
        }

        if (_rightGesture.GestureMade)
        {
            _rightGestureActive = true;
        }
        if (_rightGesture.GestureStopped)
        {
            _rightGestureActive = false;
            _leftGestureActive = false;
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
