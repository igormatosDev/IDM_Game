using UnityEngine;

public class WoodsGuardian : EnemyBase
{

    // Custom constants
    [SerializeField] public float attackDuration;
    private float attackPassed = 0;
    [SerializeField] public float enemySpeedInAttack;

    [SerializeField] private ProjectileBase pfProjectile;

    private WoodsGuardianSpriteController wgSpriteController;

    protected override void Start()
    {
        base.Start();
        wgSpriteController = gameObject.GetComponentInChildren<WoodsGuardianSpriteController>();
        enemySpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        enemyCollider = gameObject.GetComponentInChildren<CapsuleCollider2D>();
    }

    protected override void Update()
    {
        base.Update();
        if (isDead)
        {
            enemyRigidBody.velocity = Vector2.zero;
        }
        else if (isAttacking)
        {
            MovementInAttack();
            attackPassed += Time.deltaTime;
            if(attackPassed >= attackDuration)
            {
                attackPassed = 0;
                wgSpriteController.Attack3();
            }
        }
        else
        {
            Movement();
            wgSpriteController.ScaleController(enemyRigidBody.velocity);
        }
        wgSpriteController.AnimationController();
    }


    public void MovementInAttack()
    {
        Vector2 mov = getMovementInput();
        enemyRigidBody.velocity = mov != Vector2.zero ? mov * enemySpeedInAttack : Vector2.zero;
    }


    public override void Die()
    {
        base.Die();
        isAttacking = false;
        enemyCollider.enabled = false;
        wgSpriteController.PlayDeadAnimation();
    }

    public void Destroy()
    {
        DestroyEnemy();
    }


    public override void AttackInput()
    {
        wgSpriteController.StartAttack((Player.transform.position - enemySpriteRenderer.transform.position).normalized);
    }

    public void ShootProjectiles()
    {
        Vector2 basePos = gameObject.transform.position;
        ShootProjectile(basePos, Vector2.left);
        ShootProjectile(basePos, Vector2.up);
        ShootProjectile(basePos, Vector2.right);
        ShootProjectile(basePos, Vector2.down);
        ShootProjectile(basePos, (Vector2.left + Vector2.up) / 2);
        ShootProjectile(basePos, (Vector2.up + Vector2.right) / 2);
        ShootProjectile(basePos, (Vector2.right + Vector2.down) / 2);
        ShootProjectile(basePos, (Vector2.down + Vector2.left) / 2);
    }

    private void ShootProjectile(Vector2 basePos, Vector2 direction)
    {
        ProjectileBase projectile = Instantiate(pfProjectile, basePos + direction, Quaternion.identity);
        projectile.setDirection(direction);
    }
}
