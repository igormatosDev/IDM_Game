using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class ProjectileSlime : ProjectileBase
{
    [SerializeField] float acceleration;
    
    protected override void Update()
    {
        base.Update();
        speed += acceleration * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string[] possibleCollisionTags = { "Player", "PlayerSprite"};
        if (possibleCollisionTags.Contains(collision.tag))
        {
            PlayerController player = Helpers.GetNearestPlayer(transform);
            if(player.isHit(attackDamage, attackKnockback, transform.position))
            {
                Destroy(gameObject);
            }
        }
    }
}
