using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponController : WeaponBase
{
    private SpriteRenderer WeaponRenderer;
    private float timePassed = 0;



    private void Start()
    {
        WeaponRenderer = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        Vector2 direction = (pointerPosition - (Vector2)WeaponController.transform.position).normalized;
        

        if (isAttacking && !isEndingAttack)
        {
            WeaponController.transform.position = Vector3.Lerp(WeaponController.transform.position, attackStartPointerPosition, Time.deltaTime * attackDistancePerFrame);

            timePassed += Time.deltaTime;


            WeaponController.transform.Rotate(attackDirection, attackAnglePerFrame);


            if (timePassed >= attackDurationIn)
            {
                timePassed = 0;
                isEndingAttack = true;
            }
        }
        else if (isAttacking && isEndingAttack)
        {
            WeaponController.transform.position = Vector3.Lerp(WeaponController.transform.position, playerSpriteController.transform.position, Time.deltaTime * attackDistancePerFrame);
            timePassed += Time.deltaTime;
            WeaponController.transform.Rotate(attackDirection, attackAnglePerFrame);


            if (timePassed >= attackDurationOut)
            {
                timePassed = 0;
                isAttacking = false;
                isEndingAttack = false;
                WeaponController.transform.position = playerSpriteController.transform.position;
            }
        }
        else
        {
            WeaponController.transform.right = direction;

            Vector2 scale = WeaponController.transform.localScale;
        
            if(direction.x < 0)
            {
                scale.y = -1;
            }
            else
            {
                scale.y = 1;
            }

            WeaponController.transform.localScale = scale;

            if(WeaponController.transform.eulerAngles.z > 0 && WeaponController.transform.eulerAngles.z < 180)
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
