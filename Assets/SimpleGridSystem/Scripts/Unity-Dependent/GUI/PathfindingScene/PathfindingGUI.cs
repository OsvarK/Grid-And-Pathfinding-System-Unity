using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathfindingGUI : MonoBehaviour
{
    [SerializeField] private Button buttonWallTool;
    [SerializeField] private Button buttonStartTool;
    [SerializeField] private Button buttonEndTool;

    public Tools CurrentTool { get; private set; }

    public enum Tools {
        wall,
        start,
        end
    }

    private void EnableButtonsExcept(Tools tool)
    {
        buttonWallTool.interactable = true;
        buttonStartTool.interactable = true;
        buttonEndTool.interactable = true;

        switch (tool)
        {
            case Tools.wall:
                buttonWallTool.interactable = false;
                break;
            case Tools.start:
                buttonStartTool.interactable = false;
                break;
            case Tools.end:
                buttonEndTool.interactable = false;
                break;
            default:
                break;
        }
    }

    public void SetWallTool()
    {
        CurrentTool = Tools.wall;
        EnableButtonsExcept(CurrentTool);
    }
    public void SetStartTool()
    {
        CurrentTool = Tools.start;
        EnableButtonsExcept(CurrentTool);
    }
    public void SetEndTool()
    {
        CurrentTool = Tools.end;
        EnableButtonsExcept(CurrentTool);
    }
}
