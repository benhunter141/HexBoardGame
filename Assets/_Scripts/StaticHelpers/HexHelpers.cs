using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexHelpers
{
    public static Vector3 HexCellPosition(int i, int j, float cellRadius)
    {
        float x = i * 3f / 2f * cellRadius;
        float z = (i * Mathf.Sqrt(3)/2 + j * Mathf.Sqrt(3)) * cellRadius;
        float y = 0; // height
        return new Vector3(x, y, z);
    }
    public static HexCell CellUnderfoot(GameObject go)
    {
        RaycastHit hit;
        if (Physics.Raycast(go.transform.position + Vector3.up * 0.1f, Vector3.down, out hit))
        {
            return hit.collider.GetComponent<HexCell>();
        }
        else return null;
    }
    public static int FacingDirection(Vector3 pieceForward)
    {
        float deltaTheta = Vector3.SignedAngle(pieceForward, Vector3.forward, Vector3.up);
        deltaTheta /= 60f;
        int rounded = Mathf.RoundToInt(deltaTheta);
        if (rounded < 0) rounded += 6;
        return rounded;
    }

}

public enum HexDirection
{
    N,
    NE,
    SE,
    S,
    SW,
    NW
}
