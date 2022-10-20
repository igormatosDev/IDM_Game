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

    protected override void Update()
    {
        base.Update();
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
        StartCoroutine(FlashSprite(enemySpriteRenderer, "dead", .3f, .3f));
        animator.Play(DEAD_ANIMATION);
    }

    public override IEnumerator FlashSprite(SpriteRenderer renderer, string colorType, float interval, float duration)
    {
        Color minColor = new Color(215f / 255f, 64f / 255f, 64f / 255f);
        Color maxColor = new Color(255f / 255f, 196f / 255f, 196f / 255f);
        Color endColor = new Color(255f / 255f, 255f / 255f, 255f / 255f);

        if (colorType == "damage")
        {
            minColor = new Color(215f / 255f, 64f / 255f, 64f / 255f);
            maxColor = new Color(255f / 255f, 196f / 255f, 196f / 255f);
            endColor = new Color(255f / 255f, 255f / 255f, 255f / 255f);
        }
        else if (colorType == "dead")
        {
            minColor = new Color(255f / 255f, 255f / 255f, 255f / 255f);
            maxColor = new Color(70f / 255f, 70f / 255f, 70f / 255f);
            endColor = new Color(50f / 255f, 50f / 255f, 50f / 255f);
        }
        float currentInterval = 0;
        while (duration > 0)
        {
            float tColor = currentInterval / interval;
            renderer.color = Color.Lerp(minColor, maxColor, tColor);
            currentInterval += Time.deltaTime;

            if (currentInterval >= interval)
            {
                Color temp = minColor;
                minColor = maxColor;
                maxColor = temp;
                currentInterval = currentInterval - interval;
            }
            duration -= Time.deltaTime;
            yield return null;
        }

        renderer.color = endColor;
    }
}
