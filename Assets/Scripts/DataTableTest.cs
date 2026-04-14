using UnityEngine;

public class DataTableTest : MonoBehaviour
{
    public string NameStringTableKr = "StringTableKr";
    public string NameStringTableEn = "StringTableEn";
    public string NameStringTableJp = "StringTableJp";
    public CharactoerTableTest characterTest;

    void Update()
    {
        // 숫자 1,2,3 키로 언어 변경
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            DataTableManager.ChangeLanguage(Languages.Korean);
            characterTest.RefreshLanguage();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            DataTableManager.ChangeLanguage(Languages.English);
            characterTest.RefreshLanguage();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            DataTableManager.ChangeLanguage(Languages.Japanese);
            characterTest.RefreshLanguage();
        }
        }

    // 버튼 클릭 시 언어 변경
    public void OnClickStringTbleKr()
    {
        DataTableManager.ChangeLanguage(Languages.Korean);
    }

    public void OnClickStringTbleEn()
    {
        DataTableManager.ChangeLanguage(Languages.English);
    }

    public void OnClickStringTbleJp()
    {
        DataTableManager.ChangeLanguage(Languages.Japanese);
    }

    [ContextMenu("한국어")]
    private void SetKorean() => DataTableManager.ChangeLanguage(Languages.Korean);

    [ContextMenu("영어")]
    private void SetEnglish() => DataTableManager.ChangeLanguage(Languages.English);

    [ContextMenu("일본어")]
    private void SetJapanese() => DataTableManager.ChangeLanguage(Languages.Japanese);
}