using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Player player;
    public void CellClick(HexCoord hexCoord) //When a cell is clicked, it calls this
    {
        if (SingletonManager.Instance.gameStateManager.currentState is not AwaitingMove)
        {
            return;
        }
        if (!SingletonManager.Instance.gameRuleManager.IsValidMove(player.currentCoord, hexCoord))
        {
            return;
        }

        SingletonManager.Instance.uiManager.DisplayCoords(hexCoord);
        var lookup = SingletonManager.Instance.hexGridManager.hexCellLookup;
        SingletonManager.Instance.gameStateManager.playerDestination = lookup[hexCoord];
    }

    public void WaitButtonPressed()
    {
        SingletonManager.Instance.gameStateManager.playerDestination = SingletonManager.Instance.player.currentCell;
    }
}
