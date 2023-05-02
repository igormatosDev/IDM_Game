using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PlayerSpriteController : MonoBehaviour
{
    public ParticleSystem dustWalkParticles;

    private Animator faceAnimator;
    private Animator helmetAnimator;
    private Animator chestAnimator;
    private Animator legsAnimator;

    private void Start()
    {
        helmetAnimator = GameObject.Find("Helmet").GetComponent<Animator>();
        faceAnimator = GameObject.Find("Face").GetComponent<Animator>();
        chestAnimator = GameObject.Find("Chest").GetComponent<Animator>();
        legsAnimator = GameObject.Find("Legs").GetComponent<Animator>();
    }

    // private void Update(){
        // set sortingLayer for player
        // int playerSortingOrderDefault = 1 - (int)(transform.position.y * 100);

        // helmetAnimator.GetComponent<SpriteRenderer>().sortingOrder = playerSortingOrderDefault + 8;
        // faceAnimator.GetComponent<SpriteRenderer>().sortingOrder = playerSortingOrderDefault + 7;
        // chestAnimator.GetComponent<SpriteRenderer>().sortingOrder = playerSortingOrderDefault + 6;
        // legsAnimator.GetComponent<SpriteRenderer>().sortingOrder = playerSortingOrderDefault + 5;
    // }

    public void AnimationController(Vector2 lookDirection, Vector2 velocity, bool isAttacking)
    {
        // SIDE
        if (!isAttacking)
        {
            if (lookDirection.x < 0)
            {
                // Looking left
                transform.localScale = new Vector2(-1, 1);
            }
            else
            {
                // Looking right
                transform.localScale = new Vector2(1, 1);
            }
        }

        var particleEmission = dustWalkParticles.emission;
        if (Mathf.Abs(velocity.x) > 2f || Mathf.Abs(velocity.y) > 2f)
        {
            particleEmission.enabled = true;

            faceAnimator.SetBool("isRunning", true);
            helmetAnimator.SetBool("isRunning", true);
            chestAnimator.SetBool("isRunning", true);
            legsAnimator.SetBool("isRunning", true);

        }
        else
        {
            particleEmission.enabled = false;

            faceAnimator.SetBool("isRunning", false);
            helmetAnimator.SetBool("isRunning", false);
            chestAnimator.SetBool("isRunning", false);
            legsAnimator.SetBool("isRunning", false);
        }
    }

    public void FlashDamage()
    {
        Color endColor = new Color(240f / 255f, 240f / 255f, 240f / 255f);
        float interval = .15f;
        float duration = 1f;

        StartCoroutine(CommonAnimations.BlinkSprite(
            faceAnimator.GetComponent<SpriteRenderer>(),
            endColor,
            interval,
            duration
        ));
        StartCoroutine(CommonAnimations.BlinkSprite(
            helmetAnimator.GetComponent<SpriteRenderer>(),
            endColor,
            interval,
            duration
        ));
        StartCoroutine(CommonAnimations.BlinkSprite(
            chestAnimator.GetComponent<SpriteRenderer>(),
            endColor,
            interval,
            duration
        ));
        StartCoroutine(CommonAnimations.BlinkSprite(
            legsAnimator.GetComponent<SpriteRenderer>(),
            endColor,
            interval,
            duration
        ));
    }
}
