using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponController : WeaponBase
{
    public SpriteRenderer playerSpriteController;
    private SpriteRenderer WeaponRenderer;
    private float timePassed = 0;



    private void Start()
    {
        WeaponRenderer = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        Vector2 direction = (pointerPosition - (Vector2)transform.position).normalized;
        

        if (isAttacking && !isEndingAttack)
        {
            transform.position = Vector3.Lerp(transform.position, attackStartPointerPosition, Time.deltaTime * attackDistancePerFrame);

            timePassed += Time.deltaTime;


            transform.Rotate(attackDirection, attackAnglePerFrame);


            if (timePassed >= attackDurationIn)
            {
                timePassed = 0;
                isEndingAttack = true;
            }
        }
        else if (isAttacking && isEndingAttack)
        {
            transform.position = Vector3.Lerp(transform.position, playerSpriteController.transform.position, Time.deltaTime * attackDistancePerFrame);
            timePassed += Time.deltaTime;
            transform.Rotate(attackDirection, attackAnglePerFrame);


            if (timePassed >= attackDurationOut)
            {
                timePassed = 0;
                isAttacking = false;
                isEndingAttack = false;
                transform.position = playerSpriteController.transform.position;
            }
        }
        else
        {
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
                WeaponRenderer.sortingOrder = playerSpriteController.sortingOrder + 1;
            }
            else
            {
                WeaponRenderer.sortingOrder = playerSpriteController.sortingOrder - 1;
            }
        }

    }

}
