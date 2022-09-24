using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    private Animator animator;
    private Vector3 defaultScale;

    // Animation Constants
    private string state = IDLE_SIDE_ANIMATION;
    // Idle
    private const string IDLE_SIDE_ANIMATION = "PlayerIdle-X";
    private const string IDLE_DOWN_ANIMATION = "PlayerIdle-Y0";
    private const string IDLE_UP_ANIMATION = "PlayerIdle-Y1";
    // Run
    private const string RUN_SIDE_ANIMATION = "PlayerRun-X";
    private const string RUN_DOWN_ANIMATION = "PlayerRun-Y0";
    private const string RUN_UP_ANIMATION = "PlayerRun-Y1";


    private void Start()
    {
        animator = GetComponent<Animator>();
        defaultScale = transform.localScale;

    }

    public void AnimationController(Vector2 lookDirection, Vector2 velocity)
    {

        if (lookDirection.x > -1 && lookDirection.x < 1)
        {
            if(lookDirection.y > 0)
            {
                // UP
                state = AnimationDecider("up", velocity);
            }
            else
            {
                // DOWN
                state = AnimationDecider("down", velocity);
            }
        }
        else
        {
            // SIDE
            state = AnimationDecider("side", velocity);
            if (lookDirection.x < 0)
            {
                // Looking left
                transform.localScale = new Vector2(-defaultScale.x, defaultScale.y);
            }
            else
            {
                // Looking right
                transform.localScale = new Vector2(defaultScale.x, defaultScale.y);
            }

        }

        animator.Play(state);
    }

    private string AnimationDecider(string direction, Vector2 velocity)
    {

        if (Mathf.Abs(velocity.x) > 2f || Mathf.Abs(velocity.y) > 2f)
        {
            // Run
            switch (direction){
                case "up":
                    return RUN_UP_ANIMATION;

                case "down":
                    if(velocity.y < -1f)
                    {
                        return RUN_DOWN_ANIMATION;
                    }
                    else
                    {
                        return RUN_SIDE_ANIMATION;
                    }

                default:
                    return RUN_SIDE_ANIMATION;
            }

        }
        else
        {
            // Idle
            switch (direction)
            {
                case "up":
                    return IDLE_UP_ANIMATION;

                case "down":
                    return IDLE_DOWN_ANIMATION;

                default:
                    return IDLE_SIDE_ANIMATION;
            }

        }
    }
}
