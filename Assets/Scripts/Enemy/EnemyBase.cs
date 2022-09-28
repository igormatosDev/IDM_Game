using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Processors;

public class EnemyBase : MonoBehaviour
{

    // ENEMY CONTROLLERS
    // Enemy variables
    [SerializeField] public float attackPower = 3f;
    [SerializeField] public float enemySpeed = 1.2f;
    [SerializeField] public string fullname = "Enemy";
    [SerializeField] public int health = 25;
    [SerializeField] public int defense = 1;

    public GameObject Player;

    // boolean Properties
    //private bool isChasingPlayer = false;
    private bool isDead = false;
    private bool isEnemyHit = false;

    private float hitKnockback = 0;
    private float hitDuration = .2f;
    private float hitPassedTime = 0;

    // Function variables
    private Animator animator;
    private Rigidbody2D enemyRigidBody;
    private SpriteRenderer EnemySpriteRenderer;
    private Vector3 defaultScale;
    private Vector2 enemyAttackedPosition;

    private Vector2 movementInput = Vector2.zero;
    private Vector2 pointerInput = Vector2.zero;

    // Animation Constants
    private string state = IDLE_ANIMATION;
    private const string IDLE_ANIMATION = "Idle";
    private const string RUN_ANIMATION = "Run";
    private const string DEAD_ANIMATION = "Dead";

    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyRigidBody = GetComponent<Rigidbody2D>();
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
        else{
            transform.position = Vector2.MoveTowards(transform.position, enemyAttackedPosition, hitKnockback/5 * Time.deltaTime);
        }
    }

    public void MovementInput(Vector2 input)
    {
        movementInput = input != Vector2.zero ? input : Vector2.zero;
    }

    public void PointerInput(Vector2 input)
    {
        pointerInput = input;
    }


    public void AttackInput()
    {
        print("Attack action pressed");
    }


    public void Movement()
    {
        if (isEnemyHit)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyAttackedPosition, hitKnockback * Time.deltaTime);
            hitPassedTime += Time.deltaTime;

            if (hitPassedTime >= hitDuration)
            {
                isEnemyHit = false;
            }

        }
        else
        {
            enemyRigidBody.velocity = movementInput != Vector2.zero ? movementInput * enemySpeed : Vector2.zero;
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


    public void isHit(int damage, float knockback, Vector3 attackStartPointerPosition)
    {
        // called once per hit
        if (!isEnemyHit)
        {
            damage = damage - defense;
            health -= (damage < 1 ? 1 : damage);
            print($"Enemy {fullname} was Hit. (damage: {damage}, left health: {health})");

            if (health > 0)
            {
                StartCoroutine(FlashSprite(EnemySpriteRenderer, "damage", .3f, .3f));
                enemyAttackedPosition = attackStartPointerPosition;
                hitPassedTime = 0;
                hitKnockback = knockback;
                isEnemyHit = true;
            }
            else
            {
                Die();
            }
        }
    }

    public void Die()
    {
        isDead = true;
        //isChasingPlayer = false;
        StartCoroutine(FlashSprite(EnemySpriteRenderer, "dead", .3f, .3f));
        animator.Play(DEAD_ANIMATION);
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    public static IEnumerator FlashSprite(SpriteRenderer renderer, string colorType, float interval, float duration)
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
        else if(colorType == "dead")
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
