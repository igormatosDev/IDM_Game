using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;


public class VectorHelper
{

    public static Vector2 getRandomDirection(float length)
    {
        float x = UnityEngine.Random.Range(-1f, 1f);
        float y = UnityEngine.Random.Range(-1f, 1f);
        Vector2 direction = new Vector3(x, y, 0f);
        direction = direction.normalized * length;
        return direction;

    }
}
