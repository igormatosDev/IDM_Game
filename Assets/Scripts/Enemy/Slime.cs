using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Slime : EnemyBase
{
    // Animation Constants
    private string state = IDLE_ANIMATION;
    private const string IDLE_ANIMATION = "Idle";
    private const string RUN_ANIMATION = "Idle";
    private const string DEAD_ANIMATION = "Dead";
    private const string HURT_ANIMATION = "Hurt";
    private const string ATTACK01_ANIMATION = "Attack01";
    private const string ATTACK02_ANIMATION = "Attack02";
    private const string ATTACK03_ANIMATION = "Attack03";


    // Custom constants
    private bool isAttacking;
    private Vector3 attackDirection = Vector3.zero;
    [SerializeField] private float attackJumpSpeed;

    private void Update()
    {
        if (isAttacking)
        {
            if (state == ATTACK02_ANIMATION)
            {
                enemySpriteRenderer.transform.position = Vector2.Lerp(
                    enemySpriteRenderer.transform.position,
                    enemySpriteRenderer.transform.position + attackDirection,
                    attackJumpSpeed * Time.deltaTime
                );
            }
            else if (state == ATTACK03_ANIMATION)
            {
                enemySpriteRenderer.transform.position = Vector2.Lerp(
                    enemySpriteRenderer.transform.position,
                    enemySpriteRenderer.transform.position + attackDirection,
                    (attackJumpSpeed / 2) * Time.deltaTime
                );

            }

        }
        if (!isDead)
        {
            Movement();
            AnimationController();
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyAttackedPosition, hitKnockback / 5 * Time.deltaTime);
        }
    }

    public void AnimationController()
    {
        // SIDE
        if (enemyRigidBody.velocity.x > 0)
        {
            // Looking right
            transform.localScale = new Vector2(-defaultScale.x, defaultScale.y);
        }
        else if (enemyRigidBody.velocity.x < 0)
        {
            // Looking left
            transform.localScale = new Vector2(defaultScale.x, defaultScale.y);
        }



        if (!isAttacking)
        {
            if (isEnemyHit)
            {
                state = HURT_ANIMATION;
            }

            if (movementInput != Vector2.zero)
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
        animator.Play(state);

    }

    public override void Die()
    {
        isDead = true;
        enemySpeed = 0;
        animator.Play(DEAD_ANIMATION);
        StartCoroutine(CommonAnimations.FadeOut(enemySpriteRenderer, 1.5f, 1.5f, Destroy));
        enemyCollider.enabled = false;
    }

    public void Destroy()
    {
        Drop();
        DestroyEnemy();
    }

    
    public override void AttackInput()
    {
        if (!isAttacking)
        {
            print("STARTED ATTACK");
            isAttacking = true;
            state = ATTACK01_ANIMATION;
            attackDirection = (Player.transform.position - enemySpriteRenderer.transform.position).normalized;
        }
    }

    public void AttackJump()
    {
        state = ATTACK02_ANIMATION;
        isImmune = true;
        // AQUI EU DEVERIA TRATAR PARA FAZER UMA COROUTINE PARA ELE SE DESLOCAR ATÉ O PONTO DE QUEDA
        // UMA VEZ Q ELE FINALIZAR O DESLOCAMENTO, CHAMO A FUNÇÃO DE FALL
    }
    public void AttackFall()
    {
        state = ATTACK03_ANIMATION;
    }

    public void AttackEnd()
    {
        isImmune = false;
        isAttacking = false;
        state = IDLE_ANIMATION;
    }
}
