

public class TutorialSectionInformation
{
    string _infoText;

    string _labelText;

    public TutorialSectionInformation(string infoText, string labelText)
    {
        _infoText = infoText;
        _labelText = labelText;
    }

    public string GetInfoText()
    {
        return _infoText;
    }

    public string GetLabelText()
    {
        return _labelText;
    }
}
