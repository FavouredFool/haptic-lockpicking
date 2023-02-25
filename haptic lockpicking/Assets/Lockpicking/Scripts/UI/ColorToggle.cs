using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorToggle : MonoBehaviour
{
    [SerializeField]
    Color _enabledColor;

    [SerializeField]
    Color _disabledColor;

    Toggle _toggle;

    Image _image;
    void Start()
    {
        _toggle = GetComponent<Toggle>();
        _image = GetComponent<Image>();
    }

    void Update()
    {
        if (_toggle.isOn)
        {
            _image.color = _enabledColor;
        }
        else
        {
            _image.color = _disabledColor;
        }
    }
}
