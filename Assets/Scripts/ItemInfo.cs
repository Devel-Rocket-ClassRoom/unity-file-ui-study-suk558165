using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInpo : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image sprite;
    public ItemData itemData;
    public Itemscreen itemscreen;
    public string id;

    public void SetData(ItemData data, Itemscreen screen)
    {
       itemData = data;
       itemscreen = screen;

        text.text = itemData.SringName;
        sprite.sprite = itemData.SpriteIcon;
    }
    public void OnClick()
    {
        itemscreen.ShowDetail(itemData);
    }
}
