using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharactoerTableTest2 : MonoBehaviour
{
    [Header("버튼 목록")]
    public string[] characterIds = { "class1", "class2", "class3", "class4" };
    public Image[] buttonIcons;
    public LocallizationText[] buttonNames;
    public LocallizationText[] buttonJobs;

    [Header("상세 정보 (가운데)")]
    public Image icon;
    public LocallizationText textName;
    public LocallizationText textDesc;
    public TextMeshProUGUI textStat;

    public void OnEnable()
    {
        SetEmpty();
        for (int i = 0; i < characterIds.Length; i++)
            SetButtonData(i);
    }

    private void SetButtonData(int index)
    {
        CharacterData data = DataTableManager.CharacterTable.Get(characterIds[index]);

        if (buttonIcons != null && index < buttonIcons.Length)
            buttonIcons[index].sprite = data.SpriteIcon;

        if (buttonNames != null && index < buttonNames.Length)
        {
            buttonNames[index].Id = data.Name;
            buttonNames[index].OnChangedId();
        }

        if (buttonJobs != null && index < buttonJobs.Length)
        {
            buttonJobs[index].Id = data.JOB;
            buttonJobs[index].OnChangedId();
        }
    }

    public void SetEmpty()
    {
        icon.sprite = null;
        textName.Id = string.Empty;
        textDesc.Id = string.Empty;
        textName.OnChangedId();
        textDesc.OnChangedId();
        if (textStat != null) textStat.text = string.Empty;
    }

    public void OnClickButton(int index)
    {
        CharacterData data = DataTableManager.CharacterTable.Get(characterIds[index]);

        icon.sprite = data.SpriteIcon;
        textName.Id = data.Name;
        textDesc.Id = data.JOB;
        textName.OnChangedId();
        textDesc.OnChangedId();

        if (textStat != null)
            textStat.text = $"{DataTableManager.StringTable.Get("Stat_Attack")}: {data.Attack}\n" +
                            $"{DataTableManager.StringTable.Get("Stat_Defend")}: {data.Dffend}\n" +
                            $"{DataTableManager.StringTable.Get("Stat_Health")}: {data.Health}";
    }

    public void RefreshLanguage()
    {
        for (int i = 0; i < characterIds.Length; i++)
            SetButtonData(i);
    }
}