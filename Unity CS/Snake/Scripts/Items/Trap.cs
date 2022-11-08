using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : PickableItem
{
    public static event Action onHit;


    protected override void CollideWithSnake(Vector3 contactPos)
    {
        InvokeOnHit();
        base.CollideWithSnake(contactPos);
    }

    private static void InvokeOnHit()
    {
        onHit?.Invoke();
    }
}
