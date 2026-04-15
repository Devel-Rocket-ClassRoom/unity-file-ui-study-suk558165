using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class Itemscreen : MonoBehaviour
{
    public ItemInpo[] itemInpos;
    public Image detailIcon;
    public TextMeshProUGUI detailName;
    public TextMeshProUGUI detailInfo;
    public void ShowDetail(ItemData data)
    {
        detailIcon.sprite = data.SpriteIcon;
        detailName.text = data.SringName;
        detailInfo.text = $"타입: {data.Type}\n설명: {data.SringDesc}\n수치: {data.Value}\n비용: {data.Cost}";
    }
      public void Start()
      {
            for (int i = 0; i < itemInpos.Length; i++)
            {
                var data = DataTableManager.ItemTable.Get(itemInpos[i].id);
                itemInpos[i].SetData(data, this);
            }
        }

}

