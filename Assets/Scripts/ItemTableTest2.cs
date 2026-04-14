using UnityEngine;
using UnityEngine.UI;

public class ItemTableTest2 : MonoBehaviour
{
    public Image icon;
    public LocallizationText textName;
    public LocallizationText textDesc;

    public void OnEnable()
    {
        SetEmpty();
    }
    public void SetEmpty()
    {
        icon.sprite = null;
        textName.Id = string.Empty;
        textDesc.Id = string.Empty;
        textName.OnChangedId();
        textDesc.OnChangedId();
    }
    public void SetItemData(string  itemId)
    {
        ItemData data = DataTableManager.ItemTable.Get(itemId);
        SetItemData(data);
    }
    public void SetItemData(ItemData data)
    {
        icon.sprite = data.SpriteIcon;
        textName.Id = data.Name;
        textDesc.Id = data.Desc;

        textName.OnChangedId();
        textDesc.OnChangedId();
    }
}
