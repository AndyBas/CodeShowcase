using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Block Settings", menuName = "LD/Block")]
public class BlockSettings : ScriptableObject
{

    public Transform prefab;
    /// <summary>
    /// Will spawn the prefab if the random value is between x and y
    /// </summary>
    public Vector2 luckFork;

    public BlockType type;
}
