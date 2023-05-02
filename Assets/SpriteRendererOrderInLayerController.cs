using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRendererOrderInLayerController : MonoBehaviour
{
    public int extraOrderInLayer = 0;
    public bool update;

    void alterSortingOrder(){
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        // change sortingOrder in a way that works properly in top-down view, considering that Y can be negative.
        int sortingOrder = 1 - (int)(transform.position.y * 100);
        spriteRenderer.sortingOrder = sortingOrder + extraOrderInLayer;
    }

    void Start()
    {
        alterSortingOrder();
        if(update){
            InvokeRepeating("alterSortingOrder", 0.0f, 0.1f);
        }
    }
}
