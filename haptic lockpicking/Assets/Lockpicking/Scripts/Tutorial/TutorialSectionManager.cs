using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class TutorialSectionManager : MonoBehaviour
{
    public static TutorialSectionManager Instance { get; private set; }

    [SerializeField]
    GameObject _calibrateButton;

    [SerializeField]
    GameObject _videoCanvas;

    [SerializeField]
    GameObject _forceIndicatorCanvas;

    [SerializeField]
    TutorialUI _tutorialUI;

    [SerializeField]
    TextAsset _jsonText;

    [SerializeField]
    GameObject _playCanvas;

    [SerializeField]
    GestureMenuToggle _pinColorToggle;

    [SerializeField]
    GestureMenuToggle _pickIndicatorToggle;

    [SerializeField]
    GestureMenuToggle _forceIndicatorToggle;

    [SerializeField]
    GameObject _startButton;

    [SerializeField]
    GameObject _nextButton;

    [SerializeField]
    GameObject _resetButton;

    TutorialSections _tutorialSection;

    int _activeTutorialSectionNr = -1;

    private void Awake()
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
    private void Start()
    {
        _tutorialSection = JsonConvert.DeserializeObject<TutorialSections>(_jsonText.text);

        GoToTutorialSection(0);
    }

    void Update()
    {
        int keyPressed = GetSectionNrFromKeyPressed();

        if (keyPressed == -1)
        {
            return;
        }
       
        GoToTutorialSection(keyPressed);
    }

    void ReloadLock()
    {
        LockManager.Instance.CreateNewLock(_tutorialSection.TutorialSectionInformationList[_activeTutorialSectionNr]);
    }

    void SetSectionUI(TutorialSectionInformation information)
    {
        // Read from lock-JSON
        LockBuilder.Instance.SetUI(information.ColorCodePins, information.ShowTensionIndicator, information.ShowPinPositionIndicator, information.CutoutState, information.EnableCustomization);



        _startButton.SetActive(false);
        _resetButton.SetActive(true);
        _calibrateButton.SetActive(true);

        _nextButton.SetActive(_activeTutorialSectionNr < _tutorialSection.TutorialSectionInformationList.Count - 1);

        EnableForceIndicatorCanvas(true);

        _videoCanvas.SetActive(false);
    }

    public void DeleteLock()
    {
        if(LockManager.Lock != null)
        {
            Destroy(LockManager.Lock.gameObject);
        }
        
    }

    public void SetCalibrationUI()
    {
        DeleteLock();

        _playCanvas.SetActive(false);

        _tutorialUI.SetSectionLabel(_activeTutorialSectionNr, _tutorialSection.TutorialSectionInformationList[_activeTutorialSectionNr].Label);
        _tutorialUI.SetSectionText(_tutorialSection.TutorialSectionInformationList[_activeTutorialSectionNr].Info);

        _startButton.SetActive(true);

        _resetButton.SetActive(false);
        _nextButton.SetActive(false);
        _calibrateButton.SetActive(false);

        EnableForceIndicatorCanvas(false);


        // if applicable, display video

        if (VideoManager.Instance.SectionHasVideo(_activeTutorialSectionNr))
        {
            _videoCanvas.SetActive(true);
            VideoManager.Instance.PlayVideo(_activeTutorialSectionNr);
        }
        else
        {
            _videoCanvas.SetActive(false);
        }
    }

    public void GoToTutorialSection(int sectionNr)
    {
        _activeTutorialSectionNr = sectionNr;
        SetCalibrationUI();
    }


    public void SetSectionUIAfterCalibration()
    {
        SetSectionUI(_tutorialSection.TutorialSectionInformationList[_activeTutorialSectionNr]);
        ReloadLock();
    }

    public int GetActiveTutorialSectionNr()
    {
        return _activeTutorialSectionNr;
    }


    public void EnableCustomization(bool enableCustomization)
    {
        _playCanvas.SetActive(enableCustomization);
    }

    public void EnableForceIndicatorCanvas(bool active)
    {
        _forceIndicatorCanvas.SetActive(active);
    }

    int GetSectionNrFromKeyPressed()
    {
        // this is terrible, but a better solution is suprisingly complicated. This works.
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            return 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            return 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            return 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            return 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            return 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            return 5;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            return 6;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            return 7;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            return 8;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            return 9;
        }

        return -1;
    }


    public void StartPressed()
    {
        SetSectionUIAfterCalibration();
        CalibrationManager.Instance.InitialCalibration();
    }

    public void NextPressed()
    {
        GoToTutorialSection(TutorialSectionManager.Instance.GetActiveTutorialSectionNr() + 1);
    }

    public void ResetPressed()
    {
          GoToTutorialSection(TutorialSectionManager.Instance.GetActiveTutorialSectionNr());
    }

    public void CalibratePressed()
    {
        CalibrationManager.Instance.PickCalibration();
    }

    public void ExitPressed()
    {
        Application.Quit();
    }



}
