using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponBase : MonoBehaviour
{
    // WEAPON CONTROLLERS
    [SerializeField] public float attackAnglePerFrame = 3f;
    [SerializeField] public float attackDurationIn = 0.3f;
    [SerializeField] public float attackDurationOut = 0.3f;
    [SerializeField] public float attackDistancePerFrame = 5f;
    [SerializeField] public float knockbackForce = 25f;
    [SerializeField] public float attackPowerStart = 5f;
    [SerializeField] public float attackPowerEnd = 5f;


    // Constants to all Weapons
    protected bool isAttacking = false;
    protected bool isEndingAttack = false;
    protected Vector3 attackDirection = Vector3.zero;
    protected Vector2 attackStartPointerPosition;

    public Vector2 pointerPosition { get; set; }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            int damage = (int)Math.Round(UnityEngine.Random.Range(attackPowerStart, attackPowerEnd), 0);
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
            attackDirection = (pointerPosition - (Vector2)transform.position).normalized.x > 0 ? Vector3.forward : Vector3.back;
        }
    }
}
