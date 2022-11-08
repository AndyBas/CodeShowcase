using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : PickableItem
{
    public static event Action onAte;



    protected override void CollideWithSnake(Vector3 contactPos)
    {
        InvokeOnAte();
        base.CollideWithSnake(contactPos);    
    }


    private static void InvokeOnAte()
    {
        onAte?.Invoke();
    }
}
