using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PassiveItemPresenter : MonoBehaviour
{
    [SerializeField]
    public Image itemImage = null;

    private SkillBase currentSkill = null;
    private Inventory inventory = null;
    public void Render(SkillBase item, Inventory inventory)
    {
        itemImage.sprite = item.pickupSprite;
        Image img = GetComponent<Image>();
        currentSkill = item;
        this.inventory = inventory;
    }
}
