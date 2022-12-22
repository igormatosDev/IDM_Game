using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Inventory;

public class PlayerInventoryController : MonoBehaviour
{
    private Inventory inventory;
    [SerializeField] private UI_Inventory uiInventory;
    [SerializeField] private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] itemSlots = GameObject.FindGameObjectsWithTag("ItemSlotContainer");
        inventory = new Inventory(UseItem, itemSlots);
        uiInventory.SetPlayer(player);
        uiInventory.SetInventory(inventory);
    }

    public void UseItem(ItemListSlot itemSlot)
    {
        inventory.RemoveItem(itemSlot);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        ItemWorld itemWorld = collision.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            if (itemWorld.isPickable)
            {
                StartCoroutine(pickUpItem(itemWorld, player, inventory, 0.05f));
            }
        }
    }

    public static IEnumerator pickUpItem(ItemWorld itemWorld, PlayerController player, Inventory inventory, float speed)
    {
        if (!itemWorld.IsDestroyed())
        {
            float distance = Vector2.Distance(itemWorld.transform.position, player.transform.position);
            while (distance > 0.7f)
            {
                itemWorld.transform.position = Vector2.Lerp(itemWorld.transform.position, player.transform.position, speed * Time.deltaTime);
                distance = Vector2.Distance(itemWorld.transform.position, player.transform.position);
                speed += .03f;
                yield return null;
            }

            inventory.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();
        }

    }

}
