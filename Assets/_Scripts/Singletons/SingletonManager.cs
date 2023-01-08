using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonManager : MonoBehaviour
{
    #region Singleton
    public static SingletonManager Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    public InputManager inputManager;
    public PieceManager pieceManager;
    public HexGridManager hexGridManager;
    public GameStateManager gameStateManager;
    public TeamsManager teamsManager;
    public UIManager uiManager;
    public GameRuleManager gameRuleManager;

    public Player player;

}
