using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemTableTest1 : MonoBehaviour
{
    public string itemId;
    public Image icon;
    public LocallizationText textName;

    public ItemTableTest2 itemInfo;
    private void OnEnable()
    {
        OnChangeItemId();
    }

    private void OnValidate()
    {
        textName.OnChangedId();
    }
    public void OnChangeItemId()
    {
        if (itemId != null)
        {
            ItemData data = DataTableManager.ItemTable.Get(itemId); 
            icon.sprite = data.SpriteIcon;
            textName.Id = data.Name;
            textName.OnChangedId();
        }
    }

    public void Onclick()
    {
        itemInfo.SetItemData(itemId);
    }
}
