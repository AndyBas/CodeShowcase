using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : PickableItem
{
    public static event Action onHit;

    protected override void CollideWithSnake(Vector3 contactPos)
    {
        InvokeOnHit();
        base.CollideWithSnake(contactPos);
    }

    private void InvokeOnHit()
    {
        onHit?.Invoke();
    }
}
