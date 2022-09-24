using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class EnemyBase : MonoBehaviour
{

    // ENEMY CONTROLLERS
    // Enemy variables
    [SerializeField] public float attackPower = 3f;
    [SerializeField] public float enemySpeed = 1.2f;
    [SerializeField] public float seekDistanceRadius = 4;
    [SerializeField] public float health = 25;
    [SerializeField] public float defense = 1;
    [SerializeField] public string fullname = "Enemy";

    public GameObject Player;
    public GameObject EnemySpriteController;
    private float distanceFromTarget;
    private Vector2 direction;
    private bool isChasingPlayer = false;
    private bool isDead= false;

    // Function variables
    private Animator animator;
    private SpriteRenderer EnemySpriteRenderer;
    private Vector3 defaultScale;


    // Animation Constants
    private string state = IDLE_ANIMATION;
    private const string IDLE_ANIMATION = "Idle";
    private const string RUN_ANIMATION = "Run";
    private const string DEAD_ANIMATION = "Dead";


    private void Start()
    {
        animator = GetComponent<Animator>();
        EnemySpriteRenderer = GetComponent<SpriteRenderer>();
        defaultScale = transform.localScale;
    }


    private void Update()
    {
        if (!isDead)
        {
            Movement();
            AnimationController();
        }
    }


    public void Movement()
    {
        distanceFromTarget = Vector2.Distance(transform.position, Player.transform.position);
        if (distanceFromTarget < seekDistanceRadius)
        {
            isChasingPlayer = true;
            direction = Player.transform.position - transform.position;
            direction.Normalize();
            transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, enemySpeed * Time.deltaTime);
        }
        else
        {
            isChasingPlayer = false;
        }

    }

    public void Die()
    {
        isDead = true;
        isChasingPlayer = false;
        transform.position = Vector2.MoveTowards(transform.position, -Player.transform.position, 15 * Time.deltaTime);
        Color minColor = new Color(255f / 255f, 255f / 255f, 255f / 255f);
        Color maxColor = new Color(70f / 255f, 70f / 255f, 70f / 255f);
        Color endColor = new Color(50f / 255f, 50f / 255f, 50f / 255f);
        StartCoroutine(FlashSprite(EnemySpriteRenderer, minColor, maxColor, endColor, .3f, .3f));
        animator.Play(DEAD_ANIMATION);
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    public void AnimationController()
    {
        // SIDE
        if (direction.x < 0)
        {
            // Looking left
            transform.localScale = new Vector2(-defaultScale.x, defaultScale.y);
        }
        else
        {
            // Looking right
            transform.localScale = new Vector2(defaultScale.x, defaultScale.y);
        }

        if (isChasingPlayer)
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


    public void isHit(float damage, float knockback)
    {
        health -= damage - defense;
        print($"Enemy {fullname} was Hit. (damage: {damage}, left health: {health})");

        if(health > 0) {
            transform.position = Vector2.MoveTowards(transform.position, -Player.transform.position, knockback * Time.deltaTime);
            Color minColor = new Color(215f / 255f, 64f / 255f, 64f / 255f);
            Color maxColor = new Color(255f / 255f, 196f / 255f, 196f / 255f);
            Color endColor = new Color(255f / 255f, 255f / 255f, 255f / 255f);
            StartCoroutine(FlashSprite(EnemySpriteRenderer, minColor, maxColor, endColor, .3f, .3f));
        }
        else
        {
            Die();
        }
    }

    public static IEnumerator FlashSprite(SpriteRenderer renderer, Color minColor, Color maxColor, Color endColor, float interval, float duration)
    {

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
