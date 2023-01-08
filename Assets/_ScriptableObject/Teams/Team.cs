using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Team")]
public class Team : ScriptableObject
{
    public int index; //0 for player, 1 for enemy, 2 for neutral
    public List<GameObject> pieces;
    public Material color;
    //formations to choose from (later)
}
