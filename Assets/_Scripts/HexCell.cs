using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour
{
    //filled in when generated by manager
    //what about when generated by self on start?
    public HexCoord hexCoord;
    Renderer _renderer;
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        ConstructHexCoord();
    }
    private void Start()
    {
        SingletonManager.Instance.hexGridManager.hexCellLookup.Add(hexCoord, this);
        _renderer.material = SingletonManager.Instance.hexGridManager.greyCells;
    }
    private void OnMouseDown()
    {
        //send info to input manager
        SingletonManager.Instance.inputManager.CellClick(hexCoord);
    }
    void ConstructHexCoord()
    {
        int indexFrom = name.IndexOf(" (") + 2;
        int indexTo = name.IndexOf(")");
        string coords = name.Substring(indexFrom, indexTo - indexFrom);
        string[] split = coords.Split(",");
        int i = int.Parse(split[0]);
        int j = int.Parse(split[1]);
        hexCoord = new HexCoord(i, j);
    }

    HexCell NeighborToThe(HexDirection direction)
    {
        var lookup = SingletonManager.Instance.hexGridManager.hexCellLookup;
        int i = hexCoord.i;
        int j = hexCoord.j;
        //N is +j, S is -j
        //NE is +i, SW is -i
        //NW is (-1,1), SE is (1,-1)
        switch (direction)
        {
            case HexDirection.N:
                HexCoord n = new HexCoord(i, j + 1);
                if (lookup.ContainsKey(n)) return lookup[n];
                break;
            case HexDirection.NE:
                HexCoord ne = new HexCoord(i + 1, j);
                if (lookup.ContainsKey(ne)) return lookup[ne];
                break;
            case HexDirection.SE:
                HexCoord se = new HexCoord(i + 1, j - 1);
                if (lookup.ContainsKey(se)) return lookup[se];
                break;
            case HexDirection.S:
                HexCoord s = new HexCoord(i, j - 1);
                if (lookup.ContainsKey(s)) return lookup[s];
                break;
            case HexDirection.SW:
                HexCoord sw = new HexCoord(i - 1, j);
                if (lookup.ContainsKey(sw)) return lookup[sw];
                break;
            case HexDirection.NW:
                HexCoord nw = new HexCoord(i - 1, j + 1);
                if (lookup.ContainsKey(nw)) return lookup[nw];
                break;
        }
        return null;
}

    public List<HexCell> Neighbors()
    {
        var list = new List<HexCell>();
        for(int _i = 0; _i < 6; _i++)
        {
            HexCell neighbor = NeighborToThe((HexDirection)_i);
            if(neighbor is not null) list.Add(neighbor);
        }
        return list;
    }

    public void ChangeMaterial(Material material)
    {
        _renderer.material = material;
    }

    public bool IsOccupied() => SingletonManager.Instance.hexGridManager.pieceLocationLookup.ContainsKey(hexCoord);
    public Piece Occupant() => SingletonManager.Instance.hexGridManager.pieceLocationLookup[hexCoord];
    public bool IsBlocked(Team team)
    {
        if (!IsOccupied()) return false;
        if (Occupant().pieceStats.team != team) return true;
        return false;
    }

    
}