using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    //start positions are corner hexes
    //eg. radius 5: (5,0),(0,5),(-5,5),(-5,0),(0,-5),(5,-5)
    //List<HexCoord> startingPositions;
    public GameObject chickenPrefab;
    public float pieceHeight;

    public List<Piece> pieces; //filled by Piece.Start();
    public List<Egg> eggs;


    public void SetupEggRelativePositions()
    {
        //Debug.Log("give eggs relpos");
        foreach(var egg in pieces)
        {
            if (egg is not Egg) continue;
            eggs.Add(egg as Egg);
            Player player = SingletonManager.Instance.player;
            HexCoord relPos = egg.currentCoord.ConvertToRelativeCoord(player);
            //Debug.Log($"egg has relPos:{relPos.i}, {relPos.j} ", gameObject);
            player.squadControl.subordinatePositions.Add(egg, relPos); //adding relPos to player's squadControl
        }
    }

    //void SpawnChicken()
    //{
    //    HexCoord startingCoords = OpposingCorners(5)[0];
    //    Vector3 position = HexHelpers.HexCellPosition(startingCoords.i, startingCoords.j, SingletonManager.Instance.hexGridGenerator.cellRadius);
    //    GameObject chicken = Instantiate(chickenPrefab, position, Quaternion.identity);
    //}
    void Generate(Team team, HexCoord startingPosition)
    {
        /* spawn units in spiral hex
         * later:front in front
         * later:custom setup
         * 
         * Ideally, I can build levels in the editor. I'd need:
         * 1. Grid to be generated and persistent out of play mode
         * 2. Be able to place pieces that snap to grid
         * 3. Be able to save setups
         * 
         */
    }

    List<HexCoord> OpposingCorners(int radius)
    {
        var coords = new List<HexCoord>();
        HexCoord playerStart = new HexCoord();
        playerStart.i = -radius;
        playerStart.j = 0;
        HexCoord enemyStart = new HexCoord();
        enemyStart.i = radius;
        enemyStart.j = 0;
        coords.Add(playerStart);
        coords.Add(enemyStart);
        return coords;
    }
    List<HexCoord> CornerPositions(int radius)
    {
        var coords = new List<HexCoord>();
        for(int i = -1; i <= 1; i++)
        {
            for(int j = -1; j <= 1; j++)
            {
                if (Mathf.Abs(i + j) >= 2) continue;
                HexCoord hc = new HexCoord();
                hc.i = i * radius;
                hc.j = j * radius;
                coords.Add(hc);
            }
        }
        return coords;
    }
}
