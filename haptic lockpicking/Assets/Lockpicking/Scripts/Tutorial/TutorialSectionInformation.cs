using System.Collections.Generic;
using static CutoutManager;

public class TutorialSectionInformation
{
    public string Info { get; set; }

    public string Label { get; set; }

    public int PinCount { get; set; }

    public List<int> PinOrder { get; set; }

    public bool RespectOrder { get; set; }

    public bool HasPick { get; set; }

    public bool HasTension { get; set; }

    public bool ColorCodePins { get; set; }

    public bool ShowTensionIndicator { get; set; }

    public bool ShowPinPositionIndicator { get; set; }

    public CutoutState CutoutState { get; set; }

    public bool EnableCustomization { get; set; }

    public bool KeyAnimation { get; set; }

}
