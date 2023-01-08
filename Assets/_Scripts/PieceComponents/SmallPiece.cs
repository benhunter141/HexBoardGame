using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallPiece : Piece
{
    public Pather pather;
    public bool moving = false;
    protected override void Start()
    {
        base.Start();
        //Debug.Log("small piece start");
        pather = new SmallPather(this);
    }
    public virtual void Awake()
    {
        
    }

    public override void TryToMoveTo(HexCoord coord, SquadControl squadControl) //this 'try' is for subordinates... or more general?
    {
        //is coord within range?
        //is path blocked? Does smallPather take care of this?
        MoveTo(coord);
    }

    public void MoveTo(HexCoord coord)
    {
        if (coord.Equals(currentCoord)) StartCoroutine(pather.Hop());
        else pather.MoveTo(coord.HexCell());
    }
}
