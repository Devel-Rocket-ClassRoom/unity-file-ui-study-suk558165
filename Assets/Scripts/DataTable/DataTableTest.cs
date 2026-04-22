using UnityEngine;

public class DataTableTest : MonoBehaviour // 키 입력 및 버튼으로 언어를 변경하는 테스트 컴포넌트
{
    public string NameStringTableKr = "StringTableKr"; // 한국어 스트링 테이블 파일명
    public string NameStringTableEn = "StringTableEn"; // 영어 스트링 테이블 파일명
    public string NameStringTableJp = "StringTableJp"; // 일본어 스트링 테이블 파일명
    public CharactoerTableTest characterTest; // 언어 변경 시 갱신할 캐릭터 테스트 UI 컴포넌트 참조

    void Update() // 매 프레임마다 키 입력을 감지하는 Unity 업데이트 메서드
    {
        // 숫자 1,2,3 키로 언어 변경
        if (Input.GetKeyDown(KeyCode.Alpha1)) // 숫자 1 키를 눌렀을 때
        {
            DataTableManager.ChangeLanguage(Languages.Korean); // 언어를 한국어로 변경
            characterTest.RefreshLanguage(); // 캐릭터 UI를 새 언어로 갱신
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) // 숫자 2 키를 눌렀을 때
        {
            DataTableManager.ChangeLanguage(Languages.English); // 언어를 영어로 변경
            characterTest.RefreshLanguage(); // 캐릭터 UI를 새 언어로 갱신
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) // 숫자 3 키를 눌렀을 때
        {
            DataTableManager.ChangeLanguage(Languages.Japanese); // 언어를 일본어로 변경
            characterTest.RefreshLanguage(); // 캐릭터 UI를 새 언어로 갱신
        }
        }

    // 버튼 클릭 시 언어 변경
    public void OnClickStringTbleKr() // 한국어 버튼 클릭 이벤트 핸들러
    {
        DataTableManager.ChangeLanguage(Languages.Korean); // 언어를 한국어로 변경
    }

    public void OnClickStringTbleEn() // 영어 버튼 클릭 이벤트 핸들러
    {
        DataTableManager.ChangeLanguage(Languages.English); // 언어를 영어로 변경
    }

    public void OnClickStringTbleJp() // 일본어 버튼 클릭 이벤트 핸들러
    {
        DataTableManager.ChangeLanguage(Languages.Japanese); // 언어를 일본어로 변경
    }

    [ContextMenu("한국어")]
    private void SetKorean() => DataTableManager.ChangeLanguage(Languages.Korean); // 인스펙터 컨텍스트 메뉴에서 한국어로 변경

    [ContextMenu("영어")]
    private void SetEnglish() => DataTableManager.ChangeLanguage(Languages.English); // 인스펙터 컨텍스트 메뉴에서 영어로 변경

    [ContextMenu("일본어")]
    private void SetJapanese() => DataTableManager.ChangeLanguage(Languages.Japanese); // 인스펙터 컨텍스트 메뉴에서 일본어로 변경
}
