using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HexCoord
{

    public int i;
    public int j;

    public HexCoord(int _i = 0, int _j = 0)
    {
        i = _i;
        j = _j;
    }

    public bool Equals(HexCoord coord) => i == coord.i && j == coord.j;

    public HexCell HexCell() => SingletonManager.Instance.hexGridManager.hexCellLookup[this];
    public bool IsStraightLineTo(HexCoord destination)
    {
        int deltaI = i - destination.i;
        int deltaJ = j - destination.j;
        if (!((deltaI == 0 && deltaJ != 0) ||
            (deltaI != 0 && deltaJ == 0) ||
            (-deltaI == deltaJ)))
            return false;
        return true;
    }

    public int DistanceFrom(HexCoord coord)
    {
        int deltaI = i - coord.i;
        int deltaJ = j - coord.j;

        if ((deltaI !=0 && deltaJ != 0 )&&
            (Mathf.Sign(deltaI) + Mathf.Sign(deltaJ) == 0)) //destruct
        {
            int max = Mathf.Min(Mathf.Abs(deltaI), Mathf.Abs(deltaJ));
            int flat = deltaI + deltaJ;
            int distance = max + Mathf.Abs(flat);
            //Debug.Log($"dist from {i},{j} to {coord.i},{coord.j}: {distance}");
            return distance;
        }
        else //not destructive, just add deltas
        {
            int temp = Mathf.Abs(deltaI) + Mathf.Abs(deltaJ);
            //Debug.Log($"dist from {i},{j} to {coord.i},{coord.j}: {temp}");
            return temp;
        }
    }

    public HexCoord ConvertToWorldCoord(Piece _piece)
    {
        //consider piece orientation and this coord's i,j values
        HexCoord referencePieceCoord = _piece.currentCoord;
        Vector3Int referencePieceTriCoord = ConvertToTriCoord(referencePieceCoord);
        int facing = _piece.facing;
        Vector3Int relCoord = ConvertToTriCoord(this);
        var worldCoord = Vector3Int.zero;
        switch (facing)
        {
            case 0:
                worldCoord = relCoord;
                break;
            case 1:
                worldCoord.x = relCoord.y;
                worldCoord.y = relCoord.z;
                worldCoord.z = -relCoord.x;
                break;
            case 2:
                worldCoord.x = relCoord.z;
                worldCoord.y = -relCoord.x;
                worldCoord.z = -relCoord.y;
                break;
            case 3:
                worldCoord.x = -relCoord.x;
                worldCoord.y = -relCoord.y;
                worldCoord.z = -relCoord.z;
                break;
            case 4:
                worldCoord.x = -relCoord.y;
                worldCoord.y = -relCoord.z;
                worldCoord.z = -relCoord.x;
                break;
            case 5:
                worldCoord.x = -relCoord.z;
                worldCoord.y = relCoord.x;
                worldCoord.z = relCoord.y;
                break;
        }
        Vector3Int answer = referencePieceTriCoord + worldCoord;
        return ConvertToHexCoord(answer);
    }

    public HexCoord ConvertToRelativeCoord(Piece _piece)
    {
        //Debug.Log($"converting current coord: ({this.i}, {this.j}) to coord relative to player at: {_piece.currentCoord.i}, {_piece.currentCoord.j}");
        int facing = _piece.facing;
        HexCoord parentWorldPos = _piece.currentCoord;
        var offset = this;
        offset.i -= parentWorldPos.i;
        offset.j -= parentWorldPos.j;
        Vector3Int worldCoord = ConvertToTriCoord(offset);
        var relCoord = Vector3Int.zero;
        switch (facing)
        {
            case 0:
                relCoord = worldCoord;
                break;
            case 1:
                relCoord.y = worldCoord.x;
                relCoord.z = worldCoord.y;
                relCoord.x = -worldCoord.z;
                break;
            case 2:
                relCoord.z = worldCoord.x;
                relCoord.x = -worldCoord.y;
                relCoord.y = -worldCoord.z;
                break;
            case 3:
                relCoord.x = -worldCoord.x;
                relCoord.y = -worldCoord.y;
                relCoord.z = -worldCoord.z;
                break;
            case 4:
                relCoord.y = -worldCoord.x;
                relCoord.z = -worldCoord.y;
                relCoord.x = worldCoord.z;
                break;
            case 5:
                relCoord.z = -worldCoord.x;
                relCoord.x = worldCoord.y;
                relCoord.y = worldCoord.z;
                break;
        }
        Vector3Int answer = relCoord;
        return ConvertToHexCoord(answer);
    }

    public Vector3Int ConvertToTriCoord(HexCoord hC)
    {
        //*flatten* i,j if they are opposite signs
        
        if (hC.i != 0 && hC.j != 0 &&
            Mathf.Sign(hC.i) != Mathf.Sign(hC.j))
        {
            var ans = Vector3Int.zero;
            if (Mathf.Abs(hC.i) < Mathf.Abs(hC.j))
            {
                //*flatten* i and j if they are destructive
                //set k = -i
                //j += i;
                //set i = 0
                ans.z = -hC.i;
                ans.y = hC.j + hC.i;
                ans.x = 0;
            }
            else
            {
                ans.z = hC.j;
                ans.x = hC.i + hC.j;
                ans.y = 0;
            }
            return ans;
        }
        else return new Vector3Int(hC.i, hC.j, 0);
    }

    public HexCoord ConvertToHexCoord(Vector3Int triCoord)
    {
        int deltaK = triCoord.z;
        return new HexCoord(triCoord.x - deltaK, triCoord.y + deltaK);
    }
}
