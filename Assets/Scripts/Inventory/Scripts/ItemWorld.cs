using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Experimental.Rendering.LWRP;
using TMPro;
//using CodeMonkey.Utils;

public class ItemWorld : MonoBehaviour
{

    private Item item;
    private SpriteRenderer spriteRenderer;
    //private Light2D light2D;
    private TextMeshPro textMeshPro;
    public bool isPickable = true;

    public static ItemWorld SpawnItemWorld(Vector3 position, Item item)
    {
        Transform transform = Instantiate(ItemAssets.Instance.pfItemWorld, position, Quaternion.identity);

        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);

        return itemWorld;
    }

    public static ItemWorld DropItem(Vector3 playerPosition, Item item)
    {
        ItemWorld itemWorld = SpawnItemWorld(playerPosition, item);

        return itemWorld;
    }

    public IEnumerator pushItemAway(Vector3 dropPosition, ItemWorld itemWorld)
    {
        float distance = Vector3.Distance(itemWorld.transform.position, dropPosition);
        float speed = 3f;
        while (distance > 0.5f)
        {

            itemWorld.transform.position = Vector2.Lerp(itemWorld.transform.position, dropPosition, speed * Time.deltaTime);
            speed -= .01f;
            speed = speed < .5f ? .5f : speed;
            distance = Vector3.Distance(itemWorld.transform.position, dropPosition);
            yield return null;
        }
    }

    public IEnumerator setPickableTrue(ItemWorld itemWorld, int waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        itemWorld.isPickable = true;
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //light2D = transform.Find("Light").GetComponent<Light2D>();
        textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();
    }

    public void SetItem(Item item)
    {
        this.item = item;
        spriteRenderer.sprite = item.GetSprite();
        if (item.amount > 1)
        {
            textMeshPro.SetText(item.amount.ToString());
        }
        else
        {
            textMeshPro.SetText("");
        }
    }

    public Item GetItem()
    {
        return item;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

}
