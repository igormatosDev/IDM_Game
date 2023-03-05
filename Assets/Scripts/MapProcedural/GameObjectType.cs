using System;
using UnityEngine;


[Serializable]
public class GameObjectType
{
    public GameObject prefab;
    public float spacing = 60f;
    public float offset = 2f;
    public float noiseScale = 1f;
    public float noiseThreshold = 1f;

    public GameObjectType(GameObject prefab, float spacing, float offset, float noiseScale, float noiseThreshold)
    {
        this.prefab = prefab;
        this.spacing = spacing;
        this.offset = offset;
        this.noiseScale = noiseScale;
        this.noiseThreshold = noiseThreshold;
    }
}