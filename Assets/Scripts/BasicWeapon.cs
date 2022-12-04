using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BasicWeapon : WeaponBase
{
    private Vector3 scale;

    [SerializeField] private float attackAngle;

    private float weaponAttackZ;
    private float weaponAttackZtraveled = 0;
    private float weaponAttackStartTime;

    private int attackphase = 1;

    private void Start()
    {
        scale = weapon.transform.localScale;

    }
    protected override void AttackVariables()
    {
        weaponAttackZ = weapon.transform.rotation.z;
        weaponAttackZtraveled = 0;


    }

    private void Update()
    {
        if (isAttacking)
        {
            var currZ = weapon.transform.rotation.z;
            weapon.transform.Rotate(-attackAxis * attackphase, attackRotationSpeedIn);
            weaponAttackZtraveled += Mathf.Abs(currZ - weapon.transform.rotation.z);
           
            if (weaponAttackZtraveled >= (attackAngle/180))
            {
                attackphase *= -1;
                isAttacking = false;
            }

        }
        else
        {
            Vector2 direction = (pointerPosition - (Vector2)player.transform.position);
            direction = direction.normalized;
            float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (direction.x < 0)
            {
                scale.y = 1;
                attackAxis = Vector3.forward;
            }
            else
            {
                scale.y = -1;
                attackAxis = Vector3.back; 

            }

            if(attackphase > 0)
            {
                rot_z -= direction.x < 0 ? 180 : 180;
                weapon.transform.rotation = Quaternion.Euler(0f, 0f, (rot_z));
                scale.x = 1;
            }
            else
            {
                rot_z -= direction.x < 0 ? 60 : -60;
                weapon.transform.rotation = Quaternion.Euler(0f, 0f, (rot_z));
                scale.x = -1;
            }
            
            weapon.transform.localScale = scale;

            //print("attackphase");
            //print(attackphase);
            //print("scale x");
            //print(scale.x);
            //print("scale y");
            //print(scale.y);

        }

    }



}
