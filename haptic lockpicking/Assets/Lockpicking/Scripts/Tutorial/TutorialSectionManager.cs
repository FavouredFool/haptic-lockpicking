using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class TutorialSectionManager : MonoBehaviour
{
    [SerializeField]
    TutorialUI _tutorialUI;

    [SerializeField]
    TextAsset _jsonText;

    TutorialSections tutorialSection;


    int _activeTutorialSectionNr = -1;


    private void Awake()
    {
        tutorialSection = JsonConvert.DeserializeObject<TutorialSections>(_jsonText.text);
        Debug.Log(tutorialSection.TutorialSectionInformationList[0].Label);
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
        _tutorialUI.SetSectionLabel(_activeTutorialSectionNr, information.Label);
        _tutorialUI.SetSectionText(information.Info);
    }

    public void GoToTutorialSection(int sectionNr)
    {
        if (sectionNr == _activeTutorialSectionNr)
        {
            return;
        }

        _activeTutorialSectionNr = sectionNr;

        SetSectionText(tutorialSection.TutorialSectionInformationList[sectionNr]);
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
