using UnityEngine;
using UnityEngine.UI;

public class CharactoerTableTest : MonoBehaviour
{
   public Sprite[] icons;
    public Image icon;
    public LocallizationText textName;
    public LocallizationText textDesc;

    private string[] ids = { "class1", "class2", "class3", "class4" };
    private int index = 0;
    public void OnEnable()
    {
        SetItemData(ids[index]);
    }
    public void SetItemData(string itemId)
    {
        CharacterData data = DataTableManager.CharacterTable.Get(itemId);
        SetItemData(data);
    }
  public void SetItemData(CharacterData data)
{
    textName.text.text = data.Name;
    textDesc.text.text = data.JOB;
    icon.sprite = icons[index];
    }
    public void OnClick()
    {
        index = (index + 1) % ids.Length;
        SetItemData(ids[index]);
    }

}