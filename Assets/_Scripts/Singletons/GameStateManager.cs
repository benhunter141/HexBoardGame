using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    /*  Game states:
     *  1. Waiting for input (finishes when valid input received)
     *  2. Player Moving (finishes when move finishes)
     *  3. Player Acting (finishes when action finishes)
     *  4. Enemy Moving (finishes when move finishes)
     *  5. Enemy Acting (finishes when action finishes)
     *      repeat
     *      
     */
    public Player player;

    public GameState currentState;
    public PlayerMove playerMove;
    public AwaitingMove awaitingMove;
    public EggMove eggMove;
    public SettingUp settingUp;

    public bool playerMoveFinished;
    public int subMovesFinished;
    public HexCell playerDestination; //trigger (!null) for awaiting --> player moving

    private void Awake()
    {
        //list states in reverse order to avoid nullref
        eggMove = new EggMove(player, awaitingMove); //awaiting is null right now, need to fix later in Awake:
        playerMove = new PlayerMove(player, eggMove);
        awaitingMove = new AwaitingMove(player, playerMove);
        settingUp = new SettingUp(player, awaitingMove);
        
        eggMove.nextState = awaitingMove; //need this to avoid nullref
        currentState = settingUp;
        
    }

    private void Start()
    {
        currentState.OnEnter();
    }

    private void Update()
    {
        currentState.Update();
    }
}
