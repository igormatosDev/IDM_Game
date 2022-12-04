using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Processors;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditor.Progress;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyBase : MonoBehaviour
{

    // ENEMY CONTROLLERS
    // Enemy variables
    [SerializeField] public LootTable lootTable;

    [SerializeField] public int attackPower = 3;
    [SerializeField] public float attackCooldown = 0.5f;
    [SerializeField] public float attackKnockbackForce = 3f;
    [SerializeField] public float enemySpeed = 1.2f;
    [SerializeField] public string fullname = "Enemy";
    [SerializeField] public int health = 25;
    [SerializeField] public int defense = 1;
    
    [SerializeField] public int healthRegen = 1;
    [SerializeField] public float healthRegenDelay = 5;


    // boolean Properties
    public bool isDead = false;
    public bool isEnemyHit = false;
    public bool isImmune = false;
    public bool isAttacking = false;
    public bool isInAttackCooldown = false;
    public bool isIdleing = false;
    public bool isRegeneratingHealth = false;

    // player weapon
    protected float hitKnockback = 0;

    // Function variables
    protected Animator animator;
    protected SpriteRenderer enemySpriteRenderer;
    protected Collider2D enemyCollider;
    protected Rigidbody2D enemyRigidBody;
    protected GameObject Player;

    public Vector3 defaultScale;
    public Vector2 movementInput = Vector2.zero;
    public Vector2 idleMovementInput = Vector2.zero;

    protected int maxHealth;
    protected Vector2 enemyAttackedPosition;
    protected Vector2 pointerInput = Vector2.zero;


    protected virtual void Start()
    {
        maxHealth = health;
        animator = GetComponent<Animator>();
        enemyRigidBody = GetComponent<Rigidbody2D>();
        enemySpriteRenderer = GetComponent<SpriteRenderer>();
        enemyCollider = GetComponent<Collider2D>();
        defaultScale = transform.localScale;
        StartCoroutine(SetPlayer(this));
    }


    protected virtual void Update()
    {
        // Health Bar controller
        HealthBarController healthBar = gameObject.GetComponentInChildren<HealthBarController>(true);
        if (healthBar)
        {
            healthBar.manageHealthBar(health, maxHealth);
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

    public virtual void AttackInput()
    {
        print("Attack action pressed");
    }


    public void Movement()
    {
        if (isEnemyHit && !isAttacking)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyAttackedPosition, hitKnockback / 5 * Time.deltaTime);
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
                enemyAttackedPosition = attackStartPointerPosition;
                hitKnockback = knockback;
                isEnemyHit = true;
                //isImmune = true;

                float attackDuration = .3f;

                StartCoroutine(FlashSprite(
                    enemySpriteRenderer,
                    "damage",
                    .3f,
                    attackDuration
                ));
                StartCoroutine(Helpers.PushGameObject(
                    this.gameObject,
                    attackStartPointerPosition,
                    knockback,
                    attackDuration
                ));
                StartCoroutine(Helpers.CallActionAfterSec(
                    attackDuration, () =>
                    {
                        isEnemyHit = false;
                        //isImmune = false;
                    }
                ));

                if (!isRegeneratingHealth)
                {
                    isRegeneratingHealth = true;
                    StartCoroutine(RegenerateHealth(this));
                }

            }
            else
            {
                Die();
            }
        }
    }

    public virtual void Die()
    {
        Drop();
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

    public void Drop()
    {
        // Drop item
        List<Item> items = lootTable.getLoot();
        Vector2 currPosition = enemySpriteRenderer.transform.position;

        for (int i = 0; i < items.Count; i++)
        {
            Vector2 dropPosition = Helpers.GetRandomDirection(0.5f);
            ItemWorld.DropItem(currPosition - dropPosition, items[i], Vector2.zero);
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (this.isAttacking && collision.tag == "PlayerSprite")
        {
            PlayerSpriteController playerSprite = collision.GetComponent<PlayerSpriteController>();
            PlayerController player = playerSprite.GetComponentInParent<PlayerController>();
            player.isHit(this.attackPower, this.attackKnockbackForce, this.transform.position);
        }
    }


    public static IEnumerator SetPlayer(EnemyBase enemy)
    {
        PlayerController tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = enemy.transform.position;
        foreach (PlayerController t in FindObjectsOfType<PlayerController>())
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        enemy.Player = tMin ? tMin.gameObject : null;
        yield return new WaitForSeconds(10f);
        SetPlayer(enemy);
    }

    protected static IEnumerator RegenerateHealth(EnemyBase enemy)
    {
        int enemyLeftHealthToHeal = enemy.maxHealth - enemy.health;
        //print($"enemy.healthRegen: {enemy.healthRegen} || enemyLeftHealthToHeal: {enemyLeftHealthToHeal}");

        if (enemyLeftHealthToHeal == 0)
        {
            enemy.isRegeneratingHealth = false;
            yield break;
        }
        else
        {
            // Regenerates life
            yield return new WaitForSeconds(enemy.healthRegenDelay);
            if (!enemy.isDead)
            {
                enemy.health += (enemy.healthRegen > enemyLeftHealthToHeal) ? enemyLeftHealthToHeal : enemy.healthRegen; 
            }
        }
        yield return RegenerateHealth(enemy);
    }
}
