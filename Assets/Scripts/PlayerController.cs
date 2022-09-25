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



    // New movement tryout
    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float deacceleration;
    private float currentSpeed = 0;

    private Vector2 pointerInput, movementInput;

    // children controllers
    public WeaponController weaponController;
    private SpriteController spriteController;
    [SerializeField] InputActionReference movement, attack, pointerPosition;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        weaponController = GetComponentInChildren<WeaponController>();
        spriteController = GetComponentInChildren<SpriteController>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        Vector2 lookDirection = pointerInput - (Vector2)transform.position;
        spriteController.AnimationController(lookDirection, rigidbody2d.velocity);
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
        if (weaponController == null)
        {
            Debug.LogError("Weapon parent is null", gameObject);
            return;
        }
        weaponController.PerformAnAttack();
    }

    void Movement()
    {
        // pointer actions
        pointerInput = GetPointerInput();
        weaponController.pointerPosition = pointerInput;

        
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

    public Vector2 GetPointerInput()
    {
        Vector3 mousePosition = pointerPosition.action.ReadValue<Vector2>();
        mousePosition.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePosition);

    }

    //void Movement()
    //{
    //    rigidbody2d.velocity = new Vector2(0, 0);
    //    float hDirection = Input.GetAxis("Horizontal");
    //    float vDirection = Input.GetAxis("Vertical");
    //    if (hDirection < 0)
    //    {
    //        rigidbody2d.velocity = new Vector2(-characterVelocity, rigidbody2d.velocity.y);
    //        transform.localScale = new Vector2(-_scalex, _scaley);
    //    }

    //    else if (hDirection > 0)
    //    {
    //        rigidbody2d.velocity = new Vector2(characterVelocity, rigidbody2d.velocity.y);
    //        transform.localScale = new Vector2(_scalex, _scaley);
    //    }


    //    if (vDirection < 0)
    //    {
    //        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, -characterVelocity);
    //    }

    //    else if (vDirection > 0)
    //    {
    //        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, characterVelocity);
    //    }

    

}
