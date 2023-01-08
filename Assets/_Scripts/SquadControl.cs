using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadControl
{
    Piece player;
    public SquadControl(Piece _player)
    {
        player = _player;
        subordinatePositions = new Dictionary<Piece, HexCoord>();
    }
    public Dictionary<Piece, HexCoord> subordinatePositions; //relative to player
    public void IssueSubordinateMovement(EggMove eggState) //need to trigger movementFinished in eggState
    {
        eggState.subCount = subordinatePositions.Count;
        foreach (var entry in subordinatePositions)
        {
            Piece sub = entry.Key;
            HexCoord relPos = entry.Value;
            HexCoord worldCoord = relPos.ConvertToWorldCoord(player);
            sub.TryToMoveTo(worldCoord, this);
        }
    }

}
