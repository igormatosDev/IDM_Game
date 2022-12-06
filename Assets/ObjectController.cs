using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{

    [SerializeField] private LootTable lootTable;
    [SerializeField] private int defense = 1;
    [SerializeField] private int health = 100;
    private HealthBarController healthBar;
    private int maxHealth = 100;

    public void Start()
    {
        maxHealth = health;
        healthBar = gameObject.GetComponentInChildren<HealthBarController>(true);
    }

    public void Update()
    {
        if (healthBar)
        {
            healthBar.manageHealthBar(health, maxHealth);
        }
    }

    public virtual void Die()
    {
        Drop();
        Destroy(gameObject);
    }


    public void Drop()
    {
        // Drop item
        List<Item> items = lootTable.getLoot();
        Vector2 currPosition = transform.position;

        for (int i = 0; i < items.Count; i++)
        {
            Vector2 dropPosition = Helpers.GetRandomDirection(0.5f);
            ItemWorld.DropItem(currPosition - dropPosition, items[i], Vector2.zero);
        }
    }

    public void isHit(int damage)
    {
        damage = damage - defense;
        health -= (damage < 1 ? 1 : damage);


        if (health <= 0)
        {
            Die();
        }


    }
}
