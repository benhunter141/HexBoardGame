using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggMove : GameState
{
    float timer = 0;
    float minTime = 3; //have eggs trigger state change so this doesn't matter
    public int subCount = 100;
    public override void OnEnter()
    {
        timer = 0;
        SingletonManager.Instance.gameStateManager.subMovesFinished = 0;
        //Debug.Log("about to move eggs");
        //PrintEggWorldPositions();
        player.squadControl.IssueSubordinateMovement(this);
        //Debug.Log("eggs should have started moving");
        //PrintEggWorldPositions();
    }

    void PrintEggWorldPositions()
    {
        foreach(var e in SingletonManager.Instance.pieceManager.eggs)
        {
            Debug.Log($"egg world pos: {e.currentCoord.i},{e.currentCoord.j}");
        }
    }

    public override void OnExit()
    {

    }

    public override void Update()
    {
        timer += Time.deltaTime;
        if (timer > minTime) TransitionToNextState();
        if (SingletonManager.Instance.gameStateManager.subMovesFinished == subCount) TransitionToNextState();
    }

    public EggMove(Player _player, GameState next)
    {
        nextState = next;
        player = _player;
    }
}
