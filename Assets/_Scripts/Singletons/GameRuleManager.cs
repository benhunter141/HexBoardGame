using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRuleManager : MonoBehaviour
{
    //Rulesets:
    //1. Chicken can move and turn in same move
    //2. Player clicks to select destination, holds mouse down and drags to select orientation
    //3. After player has clicked and dragged some minimum distance,
    //preview of orientation shows up as a translucent green arrow
    StraightLineMovement slm;
    private void Start()
    {
        slm = new StraightLineMovement();
    }

    public bool IsValidMove(HexCoord origin, HexCoord destination) //this is only for player, straight lines, within moveRange
    {
        return slm.IsValidMove(origin, destination);
    }
}

public abstract class Ruleset
{
    public abstract bool IsValidMove(HexCoord origin, HexCoord destination);

    public bool DestinationIsOrigin(HexCoord origin, HexCoord destination) => origin.i == destination.i && origin.j == destination.j;

}

//public class StrafeAndTurnIndependently : Ruleset
//{
//    public override bool IsValidMove(HexCoord origin, HexCoord destination)
//    {
//        if (DestinationIsOrigin(origin, destination)) return false;

//        if (destination.HexCell().IsOccupied()) return false;

//        //3 cases: Fwd, FwdL/R, BackL/R/straightBack
//        int facing = SingletonManager.Instance.player.facing;
        

//        //decouple turning and moving in MoveTo
//        Debug.Log("decouple turning and moving in MoveTo");
//    }
//}


public class StraightLineMovement : Ruleset
{
    public override bool IsValidMove(HexCoord origin, HexCoord destination)
    {
        if (DestinationIsOrigin(origin, destination)) return false;

        if (!origin.IsStraightLineTo(destination))
            return false;

        if (!(origin.DistanceFrom(destination) <= SingletonManager.Instance.player.pieceStats.moveRange))
            return false;

        if (destination.HexCell().IsOccupied())
            return false;

        return true;
    }
}


