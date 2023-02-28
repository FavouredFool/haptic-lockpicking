using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForceDisplay : MonoBehaviour
{
    [SerializeField]
    TensionForceManager _tensionForceManager;

    [SerializeField]
    Image _fill;

    [SerializeField]
    Image _background;

    [SerializeField]
    Gradient _forceColorGradient;

    [SerializeField]
    Color _lockedColor;

    Slider _slider;

    private void Start()
    {
        _slider = GetComponent<Slider>();
    }

    private void Update()
    {
        _slider.value = _tensionForceManager.GetFingerPosition01();

        Color color;

        if (_slider.value >= 1)
        {
            color = _lockedColor;
            
        }
        else
        {
            color = _forceColorGradient.Evaluate(_slider.value);
        }

        _fill.color = color;
    }
}
