using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour //controls UI display, Button OnClicks go through inputmanager (better way?)
{
    public TextMeshProUGUI coordDisplay;
    public TextMeshProUGUI stateDisplay;
    public Button waitButton;
    public void DisplayCoords(HexCoord hexCoord)
    {
        coordDisplay.text = $"Coords:{hexCoord.i}, {hexCoord.j}";
    }

    public void DisplayState()
    {
        string _name = SingletonManager.Instance.gameStateManager.currentState.GetType().Name;
        stateDisplay.text = $"State:{_name}";
    }

    public void DisableWaitButton()
    {
        waitButton.interactable = false;
    }

    public void EnableWaitButton()
    {
        waitButton.interactable = true;
    }
}
