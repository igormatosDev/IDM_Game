using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    private Animator animator;
    private enum State { idle, run, jump, fall }
    private State state = State.idle;
    private Vector3 defaultScale;


    private void Start()
    {
        animator = GetComponent<Animator>();
        defaultScale = transform.localScale;

    }

    public void AnimationController(Vector2 lookDirection, Vector2 velocity)
    {
        if (lookDirection.x < 0)
        {
            transform.localScale = new Vector2(-defaultScale.x, defaultScale.y);
        }
        else
        {
            transform.localScale = new Vector2(defaultScale.x, defaultScale.y);
        }


        if (Mathf.Abs(velocity.x) > 2f || Mathf.Abs(velocity.y) > 2f)
        {
            // Running
            state = State.run;
        }
        else
        {
            // Idle
            state = State.idle;

        }

        animator.SetInteger("State", (int)state);
    }
}

