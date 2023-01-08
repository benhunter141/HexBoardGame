using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : GameState
{
    
    public override void OnEnter()
    {
        //chicken gets destination
        //each egg gets destination
        player.MoveTo(SingletonManager.Instance.gameStateManager.playerDestination.hexCoord);
        SingletonManager.Instance.gameStateManager.playerDestination = null;
    }

    public override void OnExit()
    {
        SingletonManager.Instance.hexGridManager.ResetCellColor();
        SingletonManager.Instance.gameStateManager.playerMoveFinished = false;
        TransitionToNextState();
    }
    
    public override void Update()
    {
        if(SingletonManager.Instance.gameStateManager.playerMoveFinished)
        {
            OnExit();
        }
    }

    public PlayerMove(Player _player, GameState next)
    {
        nextState = next;
        player = _player;
    }
}
