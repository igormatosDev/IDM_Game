using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponBase : MonoBehaviour
{
    // WEAPON CONTROLLERS
    public float attackAnglePerFrame = 3f;
    public float attackDurationIn = 0.3f;
    public float attackDurationOut = 0.3f;
    public float attackDistancePerFrame = 5f;
    public float knockbackForce = 25f;
    public float attackPowerStart = 5f;
    public float attackPowerEnd = 5f;
    public SpriteRenderer playerSpriteController;
    public GameObject weapon;



    // Constants to all Weapons
    public bool isAttacking = false;
    protected bool isEndingAttack = false;
    protected Vector2 attackStartPointerPosition;
    protected Vector3 attackDirection = Vector3.forward;

    public Vector2 pointerPosition { get; set; }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && isAttacking)
        {
            int damage = (int)Math.Round(UnityEngine.Random.Range(attackPowerStart + 1, attackPowerEnd + 1), 0);
            EnemyBase enemy = collision.gameObject.GetComponentInChildren<EnemyBase>();
            enemy.isHit(damage, knockbackForce, attackStartPointerPosition);
        }
    }

    public void PerformAnAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            isEndingAttack = false;
            attackStartPointerPosition = pointerPosition;
        }
    }
}
