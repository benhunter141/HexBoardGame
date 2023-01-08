using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pather
{
    protected Piece piece;
    public abstract void SnapToGrid();
    public abstract void SnapToGrid(HexCell cell);
    public abstract void MoveTo(HexCell destination);
    public abstract IEnumerator FollowPath(List<HexCell> path);
    public abstract List<HexCell> PathTo(HexCell origin, HexCell destination, int maxSteps);
    protected void SnapToFacing()
    {
        //angle between global zfwd and piece's zfwd
        //round to nearest 60 degrees
        //snap to that rounded angle
        //cache facing integer
        float angle = Vector3.SignedAngle(Vector3.forward, piece.transform.forward, Vector3.up);
        angle /= 60f;
        int facing = Mathf.RoundToInt(angle);
        if (facing < 0) facing += 6;
        angle = (float)facing * 60f;
        piece.facing = facing;
        //Debug.Log("facing set to: " + facing, piece.gameObject);
        piece.transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    public abstract IEnumerator Hop();
}
