using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSectionManager : MonoBehaviour
{
    [SerializeField]
    TutorialUI _tutorialUI;

    List<TutorialSectionInformation> _informationList;

    int _activeTutorialSectionNr = -1;

    void Awake()
    {
        _informationList = new()
        {
            new TutorialSectionInformation("dies ist ein text der probably aus einer Datei ausgelesen werden sollte", "Tension"),
            new TutorialSectionInformation("dies ist auch ein text der probably aus einer Datei ausgelesen werden sollte", "Pick-Bewegung"),
            new TutorialSectionInformation("dies ist auch ein text der probably aus einer Datei ausgelesen werden sollte", "3Pick-Bewegung"),
            new TutorialSectionInformation("dies ist auch ein text der probably aus einer Datei ausgelesen werden sollte", "4Pick-Bewegung"),
            new TutorialSectionInformation("dies ist auch ein text der probably aus einer Datei ausgelesen werden sollte", "5Pick-Bewegung"),
            new TutorialSectionInformation("dies ist auch ein text der probably aus einer Datei ausgelesen werden sollte", "Pick-Bewegung"),
            new TutorialSectionInformation("dies ist auch ein text der probably aus einer Datei ausgelesen werden sollte", "Pick-Bewegung"),
        };
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

    void SetSectionText(TutorialSectionInformation information)
    {
        _tutorialUI.SetSectionLabel(_activeTutorialSectionNr, information.GetLabelText());
        _tutorialUI.SetSectionText(information.GetInfoText());
    }

    public void GoToTutorialSection(int sectionNr)
    {
        if (sectionNr == _activeTutorialSectionNr)
        {
            return;
        }

        _activeTutorialSectionNr = sectionNr;

        SetSectionText(_informationList[sectionNr]);
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


}
