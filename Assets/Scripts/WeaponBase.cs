using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.IO.LowLevel.Unsafe;

public class WeaponBase : MonoBehaviour
{
    // WEAPON CONTROLLERS
    public float attackDistance;
    public float attackMovePerFrameIn;
    public float attackMovePerFrameOut;
    public float attackRotationSpeedIn;
    public float attackRotationSpeedOut;


    public float knockbackForce;
    public float attackPowerStart;
    public float attackPowerEnd;
    public GameObject player;
    public GameObject weapon;
    public Animator weaponAnimator;




    // Constants to all Weapons
    public bool isAttacking = false;
    public bool isEndingAttack = false;
    protected Vector2 attackStartPointerPosition;
    protected Vector3 attackAxis = Vector3.forward;
    protected Vector3 attackDirection;

    public Vector2 pointerPosition { get; set; }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && isAttacking)
        {
            int damage = (int)Math.Round(UnityEngine.Random.Range(attackPowerStart + 1, attackPowerEnd + 1), 0);

            EnemyBase enemy = collision.GetComponentInParent<EnemyBase>();
            if(!enemy)
                enemy = collision.GetComponent<EnemyBase>();
            enemy.isHit(damage, knockbackForce, attackStartPointerPosition);
        }
    }

    protected virtual void AttackVariables()
    {
        // This method is to populate anything based on weapon type
        // at the moment of the attack

        // override it in the weapon script
    }

    public void PerformAnAttack()
    {
        if (!isAttacking)
        {
            AttackVariables();
            isAttacking = true;
            isEndingAttack = false;
            attackStartPointerPosition = pointerPosition;
            attackDirection = ((Vector3)attackStartPointerPosition - player.transform.position).normalized;
        }
    }
}
