using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Vector2 pointerPosition { get; set; }
    public SpriteRenderer characterRenderer, WeaponRenderer;

    private void Update()
    {
        Vector2 direction = (pointerPosition - (Vector2)transform.position).normalized;
        transform.right = direction;

        Vector2 scale = transform.localScale;
        
        if(direction.x < 0)
        {
            scale.y = -1;
        }
        else
        {
            scale.y = 1;
        }

        transform.localScale = scale;

        if(transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180)
        {
            WeaponRenderer.sortingOrder = characterRenderer.sortingOrder + 1;
        }
        else
        {
            WeaponRenderer.sortingOrder = characterRenderer.sortingOrder - 1;
        }
    }

    public void PerformAnAttack()
    {
        return;
    }
}
