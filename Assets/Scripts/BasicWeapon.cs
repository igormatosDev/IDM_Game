using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BasicWeapon : WeaponBase
{
    private SpriteRenderer WeaponRenderer;
    private float timePassed = 0;

    private void Start()
    {
        WeaponRenderer = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        Vector2 direction = (pointerPosition - (Vector2)weapon.transform.position).normalized;

        
        if (isAttacking && !isEndingAttack)
        {
            weapon.transform.position = Vector3.Lerp(weapon.transform.position, attackStartPointerPosition, Time.deltaTime * attackDistancePerFrame);

            weapon.transform.Rotate(attackDirection, attackAnglePerFrame);
            if (timePassed >= attackDurationIn)
            {
                timePassed = 0;
                isEndingAttack = true;
            }
            timePassed += Time.deltaTime;

        }
        else if (isAttacking && isEndingAttack)
        {
            weapon.transform.position = Vector3.Lerp(weapon.transform.position, playerSpriteController.transform.position, Time.deltaTime * attackDistancePerFrame);
            weapon.transform.Rotate(attackDirection, attackAnglePerFrame) ;

            if (timePassed >= attackDurationOut)
            {
                timePassed = 0;
                isAttacking = false;
                isEndingAttack = false;
                weapon.transform.position = playerSpriteController.transform.position;
            }
            timePassed += Time.deltaTime;

        }
        else
        {
            weapon.transform.right = direction;

            Vector2 scale = weapon.transform.localScale;

            if (direction.x < 0)
            {
                scale.y = -1;
                attackDirection = Vector3.forward;
            }
            else
            {
                scale.y = 1;
                attackDirection = Vector3.back;

            }

            weapon.transform.localScale = scale;
            
            if(weapon.transform.eulerAngles.z > 0 && weapon.transform.eulerAngles.z < 240
               || weapon.transform.eulerAngles.z > 330)
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
