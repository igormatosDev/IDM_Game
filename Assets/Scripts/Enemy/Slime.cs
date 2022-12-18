using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Slime : EnemyBase
{
    // Custom constants
    [SerializeField] public float attackJumpSpeed;
    [SerializeField] public float attackFallSpeed;

    [SerializeField] private ProjectileBase pfProjectile;

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

    public void ShootProjectiles()
    {
        Vector2 basePos = gameObject.transform.position;
        ShootProjectile(basePos, Vector2.left);
        ShootProjectile(basePos, Vector2.up);
        ShootProjectile(basePos, Vector2.right);
        ShootProjectile(basePos, Vector2.down);
        ShootProjectile(basePos, (Vector2.left + Vector2.up)/2);
        ShootProjectile(basePos, (Vector2.up + Vector2.right)/2);
        ShootProjectile(basePos, (Vector2.right + Vector2.down)/2);
        ShootProjectile(basePos, (Vector2.down + Vector2.left)/2);
    }

    private void ShootProjectile(Vector2 basePos, Vector2 direction)
    {
        ProjectileBase projectile = Instantiate(pfProjectile, basePos + direction, Quaternion.identity);
        projectile.setDirection(direction);
    }
}
