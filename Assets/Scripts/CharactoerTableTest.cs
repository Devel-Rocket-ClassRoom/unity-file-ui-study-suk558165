using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharactoerTableTest : MonoBehaviour
{
    public Sprite[] icons;
    public Image icon;
    public LocallizationText textName;
    public LocallizationText textDesc;
    public TextMeshProUGUI textStat;

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
        textName.Id = data.Name;
        textName.OnChangedId();
        textDesc.Id = data.JOB;
        textDesc.OnChangedId();
        icon.sprite = icons[index];
        textStat.text = $"{DataTableManager.StringTable.Get("Stat_Attack")}: {data.Attack}\n{DataTableManager.StringTable.Get("Stat_Defend")}: {data.Dffend}\n{DataTableManager.StringTable.Get("Stat_Health")}: {data.Health}";
    }

    public void OnClick()
    {
        index = (index + 1) % ids.Length;
        SetItemData(ids[index]);
    }

    public void RefreshLanguage()
    {
        SetItemData(ids[index]);
    }
}