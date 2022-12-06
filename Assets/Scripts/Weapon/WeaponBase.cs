using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.IO.LowLevel.Unsafe;
using System.Data.SqlTypes;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class WeaponBase : MonoBehaviour
{


    // WEAPON CONTROLLERS
    public float attackRotationSpeedIn;
    public float knockbackForce;
    public float attackPowerStart;
    public float attackPowerEnd;
    public GameObject player;
    public GameObject weapon;




    // Constants to all Weapons
    public bool isAttacking = false;
    public Vector2 attackStartPointerPosition;
    protected Vector3 attackAxis = Vector3.forward;
    protected Vector3 attackDirection;

    #nullable enable
    [SerializeField] private GameObject? prefabProjectile;
    [SerializeField] private float projectileSpeed = 0;
    #nullable disable

    public Vector2 pointerPosition { get; set; }


    protected virtual void AttackVariables()
    {
        // This method is to populate anything based on weapon type
        // at the moment of the attack

        // override it in the weapon script
    }

    public int getDamage()
    {
        
        return (int)Math.Round(UnityEngine.Random.Range(attackPowerStart + 1, attackPowerEnd + 1), 0);
    }

    public void PerformAnAttack()
    {
        if (Helpers.isPaused()) { return; };
        if (!isAttacking)
        {
            AttackVariables();
           
            isAttacking = true;
            //isEndingAttack = false;
            attackStartPointerPosition = pointerPosition;
            attackDirection = ((Vector3)attackStartPointerPosition - player.transform.position).normalized;
            if (prefabProjectile != null)
            {
                SpawnProjectile(prefabProjectile, player.transform.position, attackDirection, projectileSpeed, this);
            }
        }
    }


    public static GameObject SpawnProjectile(GameObject prefabProjectile, Vector3 position, Vector2 direction, float speed, WeaponBase weapon)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg ;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        GameObject projectile = Instantiate(prefabProjectile.gameObject, position, rotation);
        projectile.transform.rotation = rotation;

        SlashAnimation _projectile = projectile.GetComponent<SlashAnimation>();
        // defining parameters
        _projectile.projectileSpeed = speed;
        _projectile.direction = direction;
        _projectile.weapon = weapon; 

        return projectile;
    }

}
