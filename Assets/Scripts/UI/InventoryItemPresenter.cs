using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections.Generic;

public class InventoryItemPresenter : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField]
    public Image itemImage = null;

    [SerializeField]
    public GameObject emptyCell = null;

    [SerializeField]
    private Sprite baseImg = null;

    private SkillBase currentSkill = null;
    private Inventory inventory = null;

    private Transform draggingParent;
    private Transform originalParent;
    private Sprite origineFrame;
    bool onDrag = false;


    public void Init(Transform draggingparent)
    {
        draggingParent = draggingparent;
        originalParent = transform.parent;
        Reboot();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        onDrag = true;
        origineFrame = transform.parent.GetComponent<Image>().sprite;
        transform.parent.GetComponent<Image>().sprite = inventory.BaseFrame;
        transform.SetParent(draggingParent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
       int closestInex = 0;
       for (int i = 0; i < originalParent.parent.transform.childCount; i++)
       {
           if (Vector3.Distance(transform.position, originalParent.parent.transform.GetChild(i).position) <
               Vector3.Distance(transform.position, originalParent.parent.transform.GetChild(closestInex).position))
           {
               closestInex = i;
           }
       }
       Transform cell = null;
       if (originalParent.parent.GetChild(closestInex).childCount == 0)
       {
            transform.SetParent(originalParent.parent.GetChild(closestInex));
            transform.parent.GetComponent<Image>().sprite = origineFrame;
            transform.localPosition = new Vector2(0, 0);
       }
       else
       {
           cell = originalParent.parent.GetChild(closestInex).GetChild(0);
            var tmp = cell.parent.GetComponent<Image>().sprite; 
           cell.SetParent(originalParent);
           cell.transform.localPosition = new Vector2(0, 0);
           cell.GetComponent<InventoryItemPresenter>().SetOriginalParent(originalParent);
           transform.SetParent(originalParent.parent.GetChild(closestInex));
           transform.parent.GetComponent<Image>().sprite = origineFrame;
            cell.parent.GetComponent<Image>().sprite = tmp;
           transform.localPosition = new Vector2(0, 0);
       }
        originalParent = transform.parent;
        onDrag = false;
    }

    public void Render(SkillBase item, Inventory inventory)
    {
        itemImage.sprite = item.pickupSprite;
        Image img = GetComponent<Image>();
        var tmp = img.color;
        currentSkill = item;
        this.inventory = inventory;
    }

    private void Reboot()
    {
        itemImage.sprite = baseImg;
    }

    public void SetOriginalParent(Transform parent)
    {
        originalParent = parent;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!onDrag)
           inventory.OnCellClick(currentSkill, transform);
    }
}
