using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridManager : MonoBehaviour
{
    public GameObject hexCellPrefab;
    public int gridRadius;
    public float cellRadius;
    public Dictionary<HexCoord, HexCell> hexCellLookup;
    public Dictionary<HexCoord, Piece> pieceLocationLookup; //what a dogshit name...
    public bool generate;
    public Material greyCells;
    public Material greenCells;

    private void Awake()
    {
        hexCellLookup = new Dictionary<HexCoord, HexCell>();
        pieceLocationLookup = new Dictionary<HexCoord, Piece>();
        if (generate) Generate();
    }
    private void Start()
    {
        //HexCells should reset their own color in start... why is this commented out?
        //ResetCellColor();
    }
    void Generate()
    {
        for (int i = -gridRadius; i <= gridRadius; i++)
        {
            for (int j = -gridRadius; j <= gridRadius; j++)
            {
                CreateHexCell(i, j);
            }
        }
    }

    void CreateHexCell(int i, int j)
    {
        if (Mathf.Abs(i + j) > gridRadius) return;
        Vector3 position = HexHelpers.HexCellPosition(i, j, cellRadius);
        GameObject go = Instantiate(hexCellPrefab, position, Quaternion.identity);
        HexCell hC = go.GetComponent<HexCell>();
        hC.name = $"HexCell ({i},{j})";
    }
    public void ResetCellColor()
    {
        ColorAllCells(greyCells);
    }
    public void ColorValidMoves()
    {
        ColorValidMoves(greenCells);
    }

    void ColorAllCells(Material material)
    {
        foreach(KeyValuePair<HexCoord, HexCell> entry in hexCellLookup)
        {
            // do something with entry.Value or entry.Key
            entry.Value.ChangeMaterial(greyCells);
        }
    }

    void ColorValidMoves(Material material)
    {
        foreach (KeyValuePair<HexCoord, HexCell> entry in hexCellLookup)
        {
            //need player's current coord
            HexCoord playerCoord = SingletonManager.Instance.player.currentCoord;
            if(SingletonManager.Instance.gameRuleManager.IsValidMove(playerCoord, entry.Key))
            {
                entry.Value.ChangeMaterial(material);
            }
        }
    }


}


