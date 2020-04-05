using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    private Sprite originalFrame;
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
        var cellFrameImage = transform.parent.GetComponent<Image>();
        originalFrame = cellFrameImage.sprite;
        cellFrameImage.sprite = inventory.BaseFrame;
        transform.SetParent(draggingParent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        int closestIndex = 0;
        for (int i = 0; i < originalParent.parent.transform.childCount; i++)
        {
            if (Vector3.Distance(transform.position, originalParent.parent.transform.GetChild(i).position) <
                Vector3.Distance(transform.position, originalParent.parent.transform.GetChild(closestIndex).position))
            {
                closestIndex = i;
            }
        }
        var destinationCell = originalParent.parent.GetChild(closestIndex);
        if (destinationCell.childCount == 0)
        {
            transform.SetParent(destinationCell);
            transform.localPosition = new Vector2(0, 0);
            transform.parent.GetComponent<Image>().sprite = originalFrame;
        }
        else
        {
            var tmp = destinationCell.GetComponent<Image>().sprite;
            Transform destinationSkillImage = destinationCell.GetChild(0);
            destinationSkillImage.SetParent(originalParent);
            destinationSkillImage.transform.localPosition = new Vector2(0, 0);
            destinationSkillImage.GetComponent<InventoryItemPresenter>().SetOriginalParent(originalParent);
            destinationSkillImage.parent.GetComponent<Image>().sprite = tmp;
            transform.SetParent(destinationCell);
            transform.parent.GetComponent<Image>().sprite = originalFrame;
            transform.localPosition = new Vector2(0, 0);
        }
        originalParent = transform.parent;
        onDrag = false;
    }

    public void Render(SkillBase item, Inventory inventory)
    {
        itemImage.sprite = item.pickupSprite;
        Image img = GetComponent<Image>();
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
