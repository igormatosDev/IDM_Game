using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponController : MonoBehaviour
{
    public Vector2 pointerPosition { get; set; }
    public SpriteRenderer characterRenderer;
    public GameObject weapon;
    
    
    private WeaponBase Weapon;
    private SpriteRenderer WeaponRenderer;

    private void Start()
    {
        WeaponRenderer = weapon.GetComponent<SpriteRenderer>();
        Weapon = weapon.GetComponent<WeaponBase>();
    }



    // Constants to all Weapons
    public bool isAttacking = false;
    public bool isEndingAttack = false;
    private float timePassed = 0;
    private Vector3 attackDirection = Vector3.zero;

    private Vector2 AttackStartPointerPosition;

    private void Update()
    {
        Vector2 direction = (pointerPosition - (Vector2)transform.position).normalized;
        

        if (isAttacking && !isEndingAttack)
        {
            transform.position = Vector3.Lerp(transform.position, AttackStartPointerPosition, Time.deltaTime * Weapon.attackDistancePerFrame);

            timePassed += Time.deltaTime;


            transform.Rotate(attackDirection, Weapon.angleAttackPerFrame);


            if (timePassed >= Weapon.attackDurationIn)
            {
                isEndingAttack = true;
                timePassed = 0;
            }
        }
        else if (isAttacking && isEndingAttack)
        {
            transform.position = Vector3.Lerp(transform.position, characterRenderer.transform.position, Time.deltaTime * Weapon.attackDistancePerFrame);
            timePassed += Time.deltaTime;
            transform.Rotate(attackDirection, Weapon.angleAttackPerFrame);


            if (timePassed >= Weapon.attackDurationOut)
            {
                isAttacking = false;
                isEndingAttack = false;
                transform.position = characterRenderer.transform.position;
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
                WeaponRenderer.sortingOrder = characterRenderer.sortingOrder + 1;
            }
            else
            {
                WeaponRenderer.sortingOrder = characterRenderer.sortingOrder - 1;
            }
        }

    }

    public void PerformAnAttack()
    {
        AttackStartPointerPosition = pointerPosition;
        isAttacking = true;
        isEndingAttack = false;
        timePassed = 0;
        attackDirection = (pointerPosition - (Vector2)transform.position).normalized.x > 0 ? Vector3.forward : Vector3.back;
    }
}
