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
    protected bool isDead = false;
    protected bool isEnemyHit = false;
    protected bool isImmune = false;

    protected float hitKnockback = 0;
    private float hitDuration = .2f;
    private float hitPassedTime = 0;

    // Function variables
    protected Animator animator;
    protected Rigidbody2D enemyRigidBody;
    protected SpriteRenderer enemySpriteRenderer;
    protected Collider2D enemyCollider;
    protected Vector2 enemyAttackedPosition;
    protected Vector3 defaultScale;

    protected Vector2 movementInput = Vector2.zero;
    protected Vector2 pointerInput = Vector2.zero;


    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyRigidBody = GetComponent<Rigidbody2D>();
        enemySpriteRenderer = GetComponent<SpriteRenderer>();
        enemyCollider = GetComponent<Collider2D>();
        defaultScale = transform.localScale;
    }

    public void MovementInput(Vector2 input)
    {
        movementInput = input != Vector2.zero ? input : Vector2.zero;
    }

    public void PointerInput(Vector2 input)
    {
        pointerInput = input;
    }

    public virtual void AttackInput()
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



    public virtual void isHit(int damage, float knockback, Vector3 attackStartPointerPosition)
    {
        // called once per hit
        if (!isEnemyHit && !isImmune)
        {
            damage = damage - defense;
            health -= (damage < 1 ? 1 : damage);
            print($"{fullname} (Damage: {damage}, Health: {health})");

            if (health > 0)
            {
                StartCoroutine(FlashSprite(enemySpriteRenderer, "damage", .3f, .3f));
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

    public virtual void Die()
    {
        isDead = true;
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    public virtual IEnumerator FlashSprite(SpriteRenderer renderer, string colorType, float interval, float duration)
    {
        // Flashes sprite if you want to
        yield return null;
    }
}
