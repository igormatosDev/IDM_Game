using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    // WEAPON CONTROLLERS
    [SerializeField] public float angleAttackPerFrame = 3f;
    [SerializeField] public float attackDurationIn = 0.3f;
    [SerializeField] public float attackDurationOut = 0.3f;
    [SerializeField] public float attackDistancePerFrame = 5f;
    [SerializeField] public float knockbackForce = 25f;
    [SerializeField] public float damage = 5f;

    private EnemyBase enemy;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            enemy = collision.gameObject.GetComponentInChildren<EnemyBase>();
            enemy.isHit(damage, knockbackForce);
        }
    }
}
