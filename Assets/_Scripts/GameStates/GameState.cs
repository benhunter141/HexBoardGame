using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState
{
    public bool initialized;
    public bool exitCondition;
    public GameState nextState;

    protected Player player;
    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void Update();

    public void TransitionToNextState()
    {
        SingletonManager.Instance.gameStateManager.currentState = nextState;
        SingletonManager.Instance.uiManager.DisplayState();
        SingletonManager.Instance.gameStateManager.currentState.OnEnter();
    }

}
