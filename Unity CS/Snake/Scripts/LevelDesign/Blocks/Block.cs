using System;
using UnityEngine;

public enum BlockType
{
    Bonus,
    Neutral,
    Malus
}


public class Block : MonoBehaviour
{
    public static float WIDTH_HEIGHT = 5f;

    public static float HALF_WIDTH_HEIGHT => WIDTH_HEIGHT / 2f;

    [SerializeField] protected Transform _end = null;

    public Vector3 EndPos => _end.position;

}
