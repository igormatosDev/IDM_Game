using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SlimeSpriteController : MonoBehaviour
{
    // Custom constants
    [SerializeField] private Slime slime;
    private Vector3 attackDirection = Vector3.zero;
    private SpriteRenderer enemySpriteRenderer;
    private Animator animator;

    private bool isJumping = false;

    // Animation Constants
    private string state = IDLE_ANIMATION;
    private const string IDLE_ANIMATION = "Idle";
    private const string RUN_ANIMATION = "Idle";
    private const string DEAD_ANIMATION = "Dead";
    private const string HURT_ANIMATION = "Hurt";
    private const string ATTACK01_ANIMATION = "Attack01";
    private const string ATTACK02_ANIMATION = "Attack02";
    private const string ATTACK03_ANIMATION = "Attack03";

    private void Start()
    {
        animator = gameObject.GetComponentInChildren<Animator>();
        enemySpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    public void StartAttack(Vector3 attackDirection)
    {
        if (!slime.isAttacking && !slime.isInAttackCooldown)
        {
            slime.isAttacking = true;
            state = ATTACK01_ANIMATION;
            this.attackDirection = attackDirection;
        }
    }

    public void AttackJump()
    {
        isJumping = true;
        state = ATTACK02_ANIMATION;
        slime.isImmune = true;
    }
    public void AttackFall()
    {
        slime.isImmune = false;
        isJumping = false;
        state = ATTACK03_ANIMATION;
    }

    public void AttackEnd()
    {
        slime.isAttacking = false;
        slime.isInAttackCooldown = true;
        StartCoroutine(Helpers.CallActionAfterSec(slime.attackCooldown, () => { slime.isInAttackCooldown = false; }));
        state = IDLE_ANIMATION;
    }

    public void AttackMovement()
    {
        float speed = isJumping ? slime.attackJumpSpeed : slime.attackFallSpeed;
        if (state == ATTACK01_ANIMATION)
            speed = 0.5f;
        slime.transform.position = Vector2.Lerp(
            enemySpriteRenderer.transform.position,
            enemySpriteRenderer.transform.position + attackDirection,
            speed * Time.deltaTime
        );
    }

    public void ScaleController(Vector3 velocity)
    {
        // SIDE
        if (velocity.x > 0)
        {
            // Looking right
            transform.localScale = new Vector2(-slime.defaultScale.x, slime.defaultScale.y);
        }
        else if (velocity.x < 0)
        {
            // Looking left
            transform.localScale = new Vector2(slime.defaultScale.x, slime.defaultScale.y);
        }
    }

    public void AnimationController()
    {
        if (!slime.isAttacking)
        {
            if (slime.isEnemyHit)
            {
                state = HURT_ANIMATION;
            }
            else if (slime.movementInput != Vector2.zero)
            {
                // Run
                state = RUN_ANIMATION;
            }
            else
            {
                // Idle
                state = IDLE_ANIMATION;
            }
        }

        if(!slime.isDead)
            animator.Play(state);

    }

    public void PlayDeadAnimation()
    {
        state = DEAD_ANIMATION;
        animator.Play(DEAD_ANIMATION);
        StartCoroutine(CommonAnimations.FadeOut(enemySpriteRenderer, 1.5f, 1.5f, slime.Destroy));
    }
}
