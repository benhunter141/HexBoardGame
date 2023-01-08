using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumPiece : Piece
{
    public HexCoord frontCoord;
    public HexCoord leftCoord;
    public HexCoord rightCoord;

    public override void TryToMoveTo(HexCoord coord, SquadControl sc)
    {
        throw new System.NotImplementedException();
    }

    protected override void UpdateCells(HexCoord coord)
    {
        //Purpose ???
        base.UpdateCells(coord);
        //frontCoord = 
    }


}
