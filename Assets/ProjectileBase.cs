using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    [SerializeField] protected float duration;
    [SerializeField] protected float speed;
    [SerializeField] protected float attackKnockback;
    [SerializeField] protected int attackDamage;
    protected Vector3 direction;


    public void setDirection(Vector3 direction)
    {
        this.direction = direction;
        gameObject.SetActive(true);
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);

        duration -= Time.deltaTime;
        if (duration < 0)
        {
            Destroy(gameObject);
        }
    }
}
