using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUp : GameState
{
    public override void OnEnter()
    {
        //do nothing
    }

    public override void OnExit()
    {
        Debug.Log("First update loop and setup finished, exiting setup");
        TransitionToNextState();
    }

    public override void Update()
    {
        SingletonManager.Instance.pieceManager.SetupEggRelativePositions();
        OnExit();
    }

    public SettingUp(Player _player, GameState next)
    {
        nextState = next;
        player = _player;
    }
}
