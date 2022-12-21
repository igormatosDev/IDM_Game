using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using System;
using UnityEngine.U2D;
using Unity.VisualScripting;
using System.Xml.Linq;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;

    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float deacceleration;

    private bool isPlayerHit = false;
    private bool isImmune = false;
    private bool isDashing = false;

    private float currentSpeed = 0;
    public int health = 100;
    public int defense = 0;

    public float dashSpeed;
    public float dashTime;
    private float dashTimePassed = 0;
    private Vector2 dashDir = Vector2.zero;


    // children controllers
    public BasicWeapon weapon;
    private Vector2 pointerInput, movementInput, lookDirection;
    private PlayerSpriteController spriteController;
    [SerializeField] InputActionReference movement, attack, dash, pointerPosition;
    public ParticleSystem dustDashParticles;
    

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        weapon = GetComponentInChildren<BasicWeapon>();
        spriteController = GetComponentInChildren<PlayerSpriteController>();

    }

    void Update()
    {
        if (Helpers.isPaused()) { return; };
        if (isDashing)
        {
            Dash();
        }
        else
        {
            Movement();
        }
        lookDirection = pointerInput - (Vector2)transform.position;
        spriteController.AnimationController(lookDirection, rigidbody2d.velocity, weapon.isAttacking);
    }

    void Dash()
    {
        var particleEmission = dustDashParticles.emission;
        particleEmission.enabled = true;
        rigidbody2d.velocity = dashDir * dashSpeed;

        dashTimePassed += Time.deltaTime;
        if (dashTimePassed >= dashTime)
        {
            dashTimePassed = 0;
            isDashing = false;
            particleEmission.enabled = false;
        }
    }

    private void OnEnable()
    {
        attack.action.performed += PerformAttack;
        dash.action.performed += PerformDash;
    }

    private void OnDisable()
    {
        attack.action.performed -= PerformAttack;
        dash.action.performed -= PerformDash;

    }

    private void PerformAttack(InputAction.CallbackContext obj)
    {
        if (weapon == null)
        {
            Debug.LogError("Weapon parent is null", gameObject);
            return;
        }
        weapon.PerformAnAttack();
    }

    private void PerformDash(InputAction.CallbackContext obj)
    {
        dashDir = movement.action.ReadValue<Vector2>();
        isDashing = true;
    }

    void Movement()
    {
        // pointer actions
        pointerInput = GetPointerInput();
        weapon.pointerPosition = pointerInput;

        // movement actions
        movementInput = movement.action.ReadValue<Vector2>();

        Vector3 oldMovementInput = movementInput;
        if (movementInput.magnitude > 0 && currentSpeed >= 0)
        {
            currentSpeed += acceleration * maxSpeed * Time.deltaTime;
        }
        else
        {
            currentSpeed -= deacceleration * maxSpeed * Time.deltaTime;
        }

        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
        rigidbody2d.velocity = oldMovementInput * currentSpeed;
    }

    public Vector3 GetPointerInput()
    {
        Vector3 mousePosition = pointerPosition.action.ReadValue<Vector2>();
        mousePosition.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    public Vector2 GetLookDirection()
    {
        return lookDirection;
    }

    public bool isHit(int damage, float knockback, Vector3 attackStartPointerPosition)
    {
        // called once per hit
        if (!isPlayerHit && !isImmune)
        {
            isPlayerHit = true;
            damage = damage - defense;
            health -= (damage < 1 ? 1 : damage);
            print($"Player (Damage: {damage}, Health: {health})");

            if (health > 0)
            {
                Color minColor = new Color(255f / 255f, 240f / 255f, 240f / 255f);
                Color maxColor = new Color(255f / 255f, 200f / 255f, 200f / 255f);
                Color endColor = new Color(255f / 255f, 255f / 255f, 255f / 255f);

                spriteController.FlashDamage();
                StartCoroutine(Helpers.CallActionAfterSec(2f, setImmuneFalse));
                StartCoroutine(CommonAnimations.PerformKnockback(
                    this.transform,
                    (this.transform.position - attackStartPointerPosition).normalized,
                    knockback,
                    0.5f
                ));
            }
            else
            {
                Die();
            }
            return true;
        }
        return false;
    }

    private void Die()
    {
        // TODO: Die method
        SceneManager.LoadScene(0);
    }


    private void setImmuneFalse()
    {
        isImmune = false;
        isPlayerHit = false;
    }




}

