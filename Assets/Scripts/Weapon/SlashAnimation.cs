using UnityEngine;
using System;

public class SlashAnimation : MonoBehaviour
{
    [SerializeField] public float projectileSpeed;
    [SerializeField] public Vector2 direction;
    [SerializeField] public WeaponBase weapon;

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void Update()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            transform.position + (Vector3)direction,
            projectileSpeed * Time.deltaTime
        );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.name);
        if (weapon.isAttacking)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                EnemyBase enemy = collision.GetComponentInParent<EnemyBase>();
            
                if (!enemy)
                    enemy = collision.GetComponent<EnemyBase>();

                enemy.isHit(weapon.getDamage(), weapon.knockbackForce, weapon.attackStartPointerPosition);
            }
    
            if (collision.gameObject.tag == "DestroyableObject")
            {
                ObjectController destroyableObject = collision.GetComponent<ObjectController>();
                destroyableObject.isHit(weapon.getDamage());
            }

            if (collision.gameObject.tag == "Obstacle")
            {
                DestroySelf();
            }
        }


    }

}
