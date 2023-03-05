using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class TutorialSectionManager : MonoBehaviour
{
    public static TutorialSectionManager Instance { get; private set; }


    [SerializeField]
    TutorialUI _tutorialUI;

    [SerializeField]
    TextAsset _jsonText;

    [SerializeField]
    GameObject _playCanvas;

    [SerializeField]
    GameObject _startButton;

    [SerializeField]
    GameObject _nextButton;

    [SerializeField]
    GameObject _resetButton;

    TutorialSections tutorialSection;


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
        tutorialSection = JsonConvert.DeserializeObject<TutorialSections>(_jsonText.text);

        _playCanvas.SetActive(false);
        _startButton.SetActive(true);
        _nextButton.SetActive(false);
        _resetButton.SetActive(false);
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
        LockManager.Instance.CreateNewLock(_activeTutorialSectionNr);
    }

    void SetSectionUI(TutorialSectionInformation information)
    {
        _tutorialUI.SetSectionLabel(_activeTutorialSectionNr, information.Label);
        _tutorialUI.SetSectionText(information.Info);

        _startButton.SetActive(false);
        _resetButton.SetActive(true);

        _nextButton.SetActive(_activeTutorialSectionNr < tutorialSection.TutorialSectionInformationList.Count - 1);
    }

    public void GoToTutorialSection(int sectionNr)
    {
        _activeTutorialSectionNr = sectionNr;

        SetSectionUI(tutorialSection.TutorialSectionInformationList[sectionNr]);
        ReloadLock();
    }

    public int GetActiveTutorialSectionNr()
    {
        return _activeTutorialSectionNr;
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
        GoToTutorialSection(0);
    }

    public void NextPressed()
    {
        GoToTutorialSection(TutorialSectionManager.Instance.GetActiveTutorialSectionNr() + 1);
    }

    public void ResetPressed()
    {
        GoToTutorialSection(TutorialSectionManager.Instance.GetActiveTutorialSectionNr());
    }

    public void ExitPressed()
    {
        Application.Quit();
    }



}
