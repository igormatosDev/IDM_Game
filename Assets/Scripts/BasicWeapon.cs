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
    private SpriteRenderer WeaponRenderer;
    float _distance = 0;
    float travel = 0;

    private void Start()
    {
        WeaponRenderer = GetComponent<SpriteRenderer>();

    }


    private void Update()
    {
        if (isAttacking)
        {
            if (!isEndingAttack)
            {
                if(_distance == 0)
                {
                    weaponAnimator.Play("BasicSwordSlash");
                }

                travel = (attackMovePerFrameIn / 10) * Time.deltaTime;
                weapon.transform.position = Vector3.Lerp(weapon.transform.position, weapon.transform.position + attackDirection, travel);
                _distance += travel;
                weapon.transform.Rotate(attackAxis, attackRotationSpeedIn);

                isEndingAttack = _distance >= attackDistance; // goes back if distance reached
            }

            else
            {
                weaponAnimator.Play("BasicSwordIdle");
                travel = (attackMovePerFrameOut / 10) * Time.deltaTime;
                weapon.transform.position = Vector3.Lerp(weapon.transform.position, player.transform.position, travel);
                _distance -= travel;
                weapon.transform.Rotate(attackAxis, attackRotationSpeedOut);

                if (_distance <= 0)
                {
                    isAttacking = false;
                    isEndingAttack = false;
                    _distance = 0;
                }

            }
        }
        else
        {
            Vector2 direction = (pointerPosition - (Vector2)player.transform.position);
            direction = direction.normalized;
            float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            weapon.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 180);
            weapon.transform.position = player.transform.position;


            Vector3 scale = weapon.transform.localScale;

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
            weapon.transform.localScale = Vector3.Lerp(weapon.transform.localScale, scale, 0.12f);


            if (weapon.transform.eulerAngles.z > 0 && weapon.transform.eulerAngles.z < 240
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
