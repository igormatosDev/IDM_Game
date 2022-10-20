using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Slime : EnemyBase
{
    // Custom constants
    [SerializeField] public float attackJumpSpeed;
    [SerializeField] public float attackFallSpeed;
    private SlimeSpriteController slimeSpriteController;

    protected override void Start()
    {
        base.Start();
        slimeSpriteController = gameObject.GetComponentInChildren<SlimeSpriteController>();
        enemySpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        enemyCollider = gameObject.GetComponentInChildren<CapsuleCollider2D>();
    }

    protected override void Update()
    {
        base.Update();
        if (isDead)
        {
            //transform.position = Vector2.MoveTowards(transform.position, enemyAttackedPosition, hitKnockback / 5 * Time.deltaTime);
            enemyRigidBody.velocity = Vector2.zero;
        }
        else if (isAttacking)
        {
            slimeSpriteController.AttackMovement();
        }
        else
        {
            Movement();
            slimeSpriteController.ScaleController(enemyRigidBody.velocity);
        }
        slimeSpriteController.AnimationController();
    }


    public override void Die()
    {
        base.Die();
        isAttacking = false;
        enemyCollider.enabled = false;
        slimeSpriteController.PlayDeadAnimation();
    }

    public void Destroy()
    {
        DestroyEnemy();
    }

    
    public override void AttackInput()
    {
        slimeSpriteController.StartAttack((Player.transform.position - enemySpriteRenderer.transform.position).normalized);
    }

}
