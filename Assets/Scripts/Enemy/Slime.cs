using UnityEngine;
using UnityEngine.UIElements;

public class Slime : EnemyBase
{
    // Custom constants
    [SerializeField] public float attackJumpSpeed;
    [SerializeField] public float attackFallSpeed;
    [SerializeField] private ProjectileBase pfProjectile;

    private Vector3 attackDirection;
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

    public void ShootProjectiles(Vector3 attackDirection)
    {
        Vector2 basePos = gameObject.transform.position;

        ProjectileBase prefab = pfProjectile;
        Vector3 direction = attackDirection;



        // Set the rotation of the first projectile towards the desired direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Calculate the offset positions of the second and third projectiles based on the rotation of the first projectile
        Vector3 offset1 = Quaternion.Euler(0f, 0f, 120f) * attackDirection;
        Vector3 offset2 = Quaternion.Euler(0f, 0f, 240f) * attackDirection;

        // Instantiate the first projectile
        // Instantiate the second and third projectiles at the offset positions
        ProjectileBase projectile1 = Instantiate(prefab, transform.position + attackDirection, Quaternion.identity);
        ProjectileBase projectile2 = Instantiate(prefab, transform.position + attackDirection + offset1, Quaternion.identity);
        ProjectileBase projectile3 = Instantiate(prefab, transform.position + attackDirection + offset2, Quaternion.identity);

        // Set the rotation of the second and third projectiles to match the rotation of the first projectile
        projectile1.transform.rotation = rotation;
        projectile2.transform.rotation = rotation;
        projectile3.transform.rotation = rotation;

        projectile1.setDirection(direction);
        projectile2.setDirection(direction);
        projectile3.setDirection(direction);

    }

    //private void ShootProjectile(Vector2 basePos, Vector2 direction)
    //{
    //    Vector2 pos = basePos;
        

    //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //    Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    //    ProjectileBase projectile = Instantiate(pfProjectile, pos, rotation);
        
    //    projectile.transform.rotation = rotation;
        
    //    projectile.setDirection(direction);
    //}
}


