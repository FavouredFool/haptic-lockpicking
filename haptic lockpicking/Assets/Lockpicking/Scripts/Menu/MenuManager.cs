using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [SerializeField]
    GameObject _menuCanvas;

    [SerializeField]
    GameObject _playCanvas;

    [SerializeField]
    GameObject _tutorialCanvas;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            throw new System.Exception();
        }
        else
        {
            Instance = this;
        }
    }

    public void Start()
    {
        _menuCanvas.SetActive(true);
        _playCanvas.SetActive(false);
        _tutorialCanvas.SetActive(false);
    }

    public void StartPressed()
    {
        _menuCanvas.SetActive(false);
        _playCanvas.SetActive(true);
        _tutorialCanvas.SetActive(false);
    }

    public void TutorialPressed()
    {
        _menuCanvas.SetActive(false);
        _playCanvas.SetActive(false);
        _tutorialCanvas.SetActive(true);
    }

    public void ExitPressed()
    {
        Application.Quit();
    }

    public void ResetPressed()
    {
        Debug.Log("reset not implemented");
    }
}
