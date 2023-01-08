using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwaitingMove : GameState
{
    public override void OnEnter()
    {
        SingletonManager.Instance.hexGridManager.ColorValidMoves();
        SingletonManager.Instance.uiManager.EnableWaitButton();
    }

    public override void OnExit()
    {
        SingletonManager.Instance.uiManager.DisableWaitButton();
        TransitionToNextState(); 
    }

    public override void Update()
    {
        if(SingletonManager.Instance.gameStateManager.playerDestination is not null)
        {
            OnExit();
        }
        if(!initialized)
        {
            initialized = true;
            OnEnter();
        }
    }

    public AwaitingMove(Player _player, GameState next)
    {
        nextState = next;
        player = _player;
    }
}
