using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SmallPiece
{
    public SquadControl squadControl;
    public override void Awake()
    {
        base.Awake();
        squadControl = new SquadControl(this);
    }
    protected override void Start()
    {
        base.Start();
    }
    private void Update()
    {
        if(movementFinished)
        {
            SingletonManager.Instance.gameStateManager.playerMoveFinished = true;
            movementFinished = false;
        }
    }

    
}
