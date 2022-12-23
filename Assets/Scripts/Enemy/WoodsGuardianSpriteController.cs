using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodsGuardianSpriteController : MonoBehaviour
{
    // Custom constants
    [SerializeField] private WoodsGuardian enemy;
    //private Vector3 attackDirection = Vector3.zero;
    private SpriteRenderer enemySpriteRenderer;
    private Animator animator;

    private bool isJumping = false;
    private Vector3 defaultScale;

    // Animation Constants
    private string state = IDLE_ANIMATION;
    private const string IDLE_ANIMATION = "Idle";
    private const string ATTACK01_ANIMATION = "Attack1";
    private const string ATTACK02_ANIMATION = "Attack2";
    private const string ATTACK03_ANIMATION = "Attack3";

    private void Start()
    {
        animator = gameObject.GetComponentInChildren<Animator>();
        enemySpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();

    }

    public void StartAttack(Vector3 attackDirection)
    {
        if (!enemy.isAttacking && !enemy.isInAttackCooldown)
        {
            enemy.isAttacking = true;
            state = ATTACK01_ANIMATION;
            //this.attackDirection = attackDirection;
        }
    }

    public void Attack2()
    {
        state = ATTACK02_ANIMATION;
    }
    public void Attack3()
    {
        state = ATTACK03_ANIMATION;
    }

    public void AttackEnd()
    {
        enemy.isAttacking = false;
        enemy.isInAttackCooldown = true;
        StartCoroutine(Helpers.CallActionAfterSec(enemy.attackCooldown, () => { enemy.isInAttackCooldown = false; }));
        state = IDLE_ANIMATION;
    }

    public void AttackMovement()
    {
        
    }

    public void ScaleController(Vector3 velocity)
    {
        // SIDE
        if (velocity.x > 0)
        {
            // Looking right
            enemy.transform.localScale = new Vector2(-1, 1);
        }
        else if (velocity.x < 0)
        {
            // Looking left
            enemy.transform.localScale = new Vector2(1, 1);
        }
    }

    public void AnimationController()
    {
        if (!enemy.isAttacking)
        {
            // Idle
            state = IDLE_ANIMATION;
        }

        if (!enemy.isDead)
            animator.Play(state);

    }

    public void PlayDeadAnimation()
    {
        //state = DEAD_ANIMATION;
        //animator.Play(DEAD_ANIMATION);
        StartCoroutine(CommonAnimations.FadeOut(enemySpriteRenderer, 1.5f, 1.5f, enemy.Destroy));
    }
}
