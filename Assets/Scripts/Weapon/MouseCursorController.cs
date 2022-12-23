using UnityEngine;
using static Inventory;

public class MouseCursorController : MonoBehaviour
{
    public Texture2D defaultCursorTexture;
    public Texture2D swordCursorTexture;

    // Drag item
    public GameObject itemDragControllerGameObject;
    public GameObject draggedGameObject;
    public ItemListSlot draggedItem;

    private ItemDragController itemDragController;
    private SpriteRenderer itemDragSpriteRenderer;

    private void Start()
    {
        itemDragSpriteRenderer = itemDragControllerGameObject.GetComponent<SpriteRenderer>();
        itemDragController = itemDragControllerGameObject.GetComponent<ItemDragController>();

        Cursor.lockState = CursorLockMode.Confined;
        SetCursor(defaultCursorTexture);
    }


    public void SetCursor(Texture2D cursor)
    {
        Vector2 hotspot = Vector2.zero;
        if(cursor == defaultCursorTexture || cursor == null)
        {
            cursor = defaultCursorTexture;
            hotspot = new Vector2(cursor.width / 2f, cursor.height / 2f);
        }
        Cursor.SetCursor(cursor, hotspot, CursorMode.Auto);
    }

    public void DragItem(ItemListSlot draggedItem, GameObject draggedGameObject)
    {
        print("DRAG CALLED");
        
        if (draggedItem != null && draggedGameObject != null)
        {
            print(draggedItem);
            print(draggedGameObject);

            itemDragControllerGameObject.SetActive(true);
            itemDragSpriteRenderer.sprite = draggedItem.item.GetSprite();
            this.draggedItem = draggedItem;
            //this.draggedGameObject = draggedGameObject;
        }
    }

    //private void OnMouseEnter()
    //{
    //    SetCursor(swordCursorTexture);
    //}

    //private void OnMouseDown()
    //{
    //    SetCursor(swordCursorTexture);
    //}
    //private void OnMouseExit()
    //{
    //    SetCursor(defaultCursorTexture);
    //}

}
