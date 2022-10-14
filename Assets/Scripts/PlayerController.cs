using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using System;
using UnityEngine.U2D;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;

    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float deacceleration;
    
    private float currentSpeed = 0;
    private Vector2 pointerInput, movementInput, lookDirection;


    // children controllers
    public BasicWeapon weapon;
    private SpriteController spriteController;
    [SerializeField] InputActionReference movement, attack, pointerPosition;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        weapon = GetComponentInChildren<BasicWeapon>();
        spriteController = GetComponentInChildren<SpriteController>();

    }
    void Update()
    {
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
    

}
