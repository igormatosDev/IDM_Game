using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteMaskedMan : EnemyBase
{
    // Animation Constants
    private string state = IDLE_ANIMATION;
    private const string IDLE_ANIMATION = "Idle";
    private const string RUN_ANIMATION = "Run";
    private const string DEAD_ANIMATION = "Dead";

    private void Update()
    {
        
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
            transform.localScale = new Vector2(defaultScale.x, defaultScale.y);
        }
        else
        {
            // Looking left
            transform.localScale = new Vector2(-defaultScale.x, defaultScale.y);
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

        animator.Play(state);
    }

    public override void Die()
    {
        isDead = true;
        StartCoroutine(FlashSprite(EnemySpriteRenderer, "dead", .3f, .3f));
        animator.Play(DEAD_ANIMATION);
    }
}
