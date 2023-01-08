using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public PieceStats pieceStats;

    public HexCoord currentCoord; //for medium pieces this is the front cell, for large the center
    public HexCell currentCell;
    public HexBase hexBase;
    public bool movementFinished;
    public int facing; //0 is fwd z, 1 is rightFwd, 2 is rightBack etc. clockwise

    protected virtual void Start()
    {
        //Debug.Log("piece start");
        SingletonManager.Instance.pieceManager.pieces.Add(this);
        if (pieceStats is null) Debug.Log("Error! assign pieceStats in inspector");
    }

    protected virtual void UpdateCells(HexCoord coord)
    {
        currentCoord = coord;
    }

    public abstract void TryToMoveTo(HexCoord coord, SquadControl squadControl);


}
