using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using System;
using UnityEngine.U2D;
using Unity.VisualScripting;
using System.Xml.Linq;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;

    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float deacceleration;

    private bool isPlayerHit = false;
    private bool isImmune = false;

    private float currentSpeed = 0;
    public int health = 100;
    public int defense = 0;


    // children controllers
    public BasicWeapon weapon;
    private Vector2 pointerInput, movementInput, lookDirection;
    private PlayerSpriteController spriteController;
    [SerializeField] InputActionReference movement, attack, pause, pointerPosition;


    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        weapon = GetComponentInChildren<BasicWeapon>();
        spriteController = GetComponentInChildren<PlayerSpriteController>();
    }

    void Update()
    {
        if (Helpers.isPaused()) { return; };
        Movement();
        lookDirection = pointerInput - (Vector2)transform.position;
        spriteController.AnimationController(lookDirection, rigidbody2d.velocity, weapon.isAttacking);
    }


    private void OnEnable()
    {
        attack.action.performed += PerformAttack;
    }

    private void OnDisable()
    {
        attack.action.performed -= PerformAttack;
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

    public void isHit(int damage, float knockback, Vector3 attackStartPointerPosition)
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

                StartCoroutine(Helpers.CallActionAfterSec(2f, setImmuneFalse));

                //StartCoroutine(CommonAnimations.FlashSprite(
                //    spriteController.spriteRenderer,
                //    Helpers.GetColorHex("#FFF0F0"),
                //    Helpers.GetColorHex("#FFC8C8"),
                //    Helpers.GetColorHex("#FFFFFF"),
                //    .15f,
                //    2f
                //));

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
        }
    }

    private void Die()
    {
        // TODO: Die method
    }


    private void setImmuneFalse()
    {
        isImmune = false;
        isPlayerHit = false;
    }




}

