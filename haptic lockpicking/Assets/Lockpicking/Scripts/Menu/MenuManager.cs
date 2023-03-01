using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject _menuCanvas;

    [SerializeField]
    GameObject _playCanvas;

    [SerializeField]
    GameObject _tutorialCanvas;

    [SerializeField]
    GameObject _leftHandModel;

    [SerializeField]
    GameObject _rightHandModel;

    public void Start()
    {
        _menuCanvas.SetActive(true);
        _playCanvas.SetActive(false);
        _tutorialCanvas.SetActive(false);

        _leftHandModel.SetActive(false);
        _rightHandModel.SetActive(false);
    }

    public void StartPressed()
    {
        _menuCanvas.SetActive(false);
        _playCanvas.SetActive(true);
        _tutorialCanvas.SetActive(false);

        _leftHandModel.SetActive(true);
        _rightHandModel.SetActive(true);
    }

    public void TutorialPressed()
    {
        _menuCanvas.SetActive(false);
        _playCanvas.SetActive(false);
        _tutorialCanvas.SetActive(true);

        _leftHandModel.SetActive(false);
        _rightHandModel.SetActive(false);
    }

    public void ExitPressed()
    {
        Application.Quit();
    }
}
