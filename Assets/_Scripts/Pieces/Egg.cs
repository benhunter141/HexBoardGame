using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : SmallPiece //IFollow ?
{
    Player player;

    protected override void Start()
    {
        base.Start();
        //Debug.Log("Egg start");

    }

    private void Update()
    {
        RegisterMovementFinishWithGSM();
    }

    void RegisterMovementFinishWithGSM()
    {
        if (movementFinished) //all followers do this... should put this code in IFollow or something
        {
            SingletonManager.Instance.gameStateManager.subMovesFinished++;
            movementFinished = false;
        }
    }
}
