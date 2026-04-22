using TMPro;
using UnityEngine;

public class LanguageSelector : MonoBehaviour // 드롭다운으로 언어를 선택하는 컴포넌트
{
    private TMP_Dropdown dropdown; // 이 오브젝트에 부착된 TMP_Dropdown 컴포넌트

    private void Awake() // 오브젝트 초기화 시 호출
    {
        dropdown = GetComponent<TMP_Dropdown>(); // 자신에게 붙어 있는 TMP_Dropdown 컴포넌트를 가져옴
    }

    private void Start() // 씬 시작 시 한 번 호출
    {
        if (dropdown != null) // dropdown이 존재하는 경우에만 초기화
        {
            dropdown.SetValueWithoutNotify((int)Variables.language); // 저장된 언어 설정에 맞게 드롭다운 초기값 설정 (이벤트 없이)
            dropdown.onValueChanged.AddListener(OnChangeLanguage); // 드롭다운 값이 바뀔 때 언어 변경 메서드를 등록
        }
    }

    private void OnDisable() // 오브젝트가 비활성화될 때 호출
    {
        if (dropdown != null)
            dropdown.onValueChanged.RemoveListener(OnChangeLanguage); // 비활성화 시 이벤트 리스너를 제거해 메모리 누수 방지
    }

    public void OnChangeLanguage(int index) // 드롭다운 선택 인덱스를 받아 언어를 변경하는 메서드
    {
        DataTableManager.ChangeLanguage((Languages)index); // 인덱스를 Languages 열거형으로 변환하여 언어 변경
    }

    public void SetKorean()   => DataTableManager.ChangeLanguage(Languages.Korean); // 언어를 한국어로 직접 변경
    public void SetEnglish()  => DataTableManager.ChangeLanguage(Languages.English); // 언어를 영어로 직접 변경
    public void SetJapanese() => DataTableManager.ChangeLanguage(Languages.Japanese); // 언어를 일본어로 직접 변경
}
