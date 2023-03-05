using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialUI : MonoBehaviour
{
    [SerializeField]
    TMP_Text _sectionText;

    [SerializeField]
    TMP_Text _sectionLabel;


    public void SetSectionText(string text)
    {
        _sectionText.text = text;
    }

    public void SetSectionLabel(int nr, string label)
    {
        _sectionLabel.text = "Section " + (nr+1) + "/10: " + label;
    }
}
