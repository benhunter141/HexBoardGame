using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/PieceStats")]
public class PieceStats : ScriptableObject
{
    public int moveRange;
    public float moveSpeed;
    public float jumpHeight;
    public Team team;
    public int jumpRange;
}
