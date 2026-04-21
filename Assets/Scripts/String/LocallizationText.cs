using TMPro; // TextMeshPro UI 컴포넌트 사용
using UnityEngine; // Unity 엔진 기본 기능 사용

[ExecuteInEditMode] // 에디터 모드에서도 스크립트가 실행되도록 설정
public class LocallizationText : MonoBehaviour // 다국어 지원 텍스트 컴포넌트
{
#if UNITY_EDITOR
    public Languages editorLang; // 에디터에서 미리보기용으로 선택하는 언어
#endif

    public string Id; // 문자열 테이블에서 참조할 키 ID
    public TextMeshProUGUI text; // 텍스트를 출력할 TextMeshPro UI 컴포넌트

    private void Awake() // 오브젝트 초기화 시 호출
    {
        text = GetComponent<TextMeshProUGUI>(); // 자신에게 붙어 있는 TextMeshProUGUI 컴포넌트를 가져옴
    }

    public void OnEnable() // 오브젝트가 활성화될 때마다 호출
    {
        if (Application.isPlaying) // 실제 플레이 중일 때
        {
            OnChangedId(); // 현재 언어에 맞는 텍스트로 갱신
        }
#if UNITY_EDITOR
        else // 에디터 모드일 때
        {
            OnChangeLanguage(editorLang); // 에디터에서 선택한 언어로 미리보기 적용
        }
#endif
    }

    private void OnValidate() // 인스펙터 값이 변경될 때 호출 (에디터 전용)
    {
        if (text == null)
            text = GetComponent<TextMeshProUGUI>(); // text가 없으면 다시 컴포넌트를 가져옴
#if UNITY_EDITOR
        OnChangeLanguage(editorLang); // 에디터 언어로 미리보기 갱신
#endif
    }

    public void OnChangedId() // 현재 언어 설정에 따라 텍스트를 갱신하는 메서드
    {
        if (text == null) return; // text 컴포넌트가 없으면 실행하지 않음
        text.text = DataTableManager.StringTable.Get(Id); // 문자열 테이블에서 ID에 해당하는 텍스트를 설정
    }

#if UNITY_EDITOR
    private void OnChangeLanguage(Languages lang) // 에디터 전용: 지정한 언어로 텍스트를 변경하는 메서드
    {
        if (text == null) return; // text 컴포넌트가 없으면 실행하지 않음
        DataTableManager.ChangeLanguage(lang); // DataTableManager의 현재 언어를 변경
        text.text = DataTableManager.StringTable.Get(Id); // 변경된 언어로 텍스트를 갱신
    }

    [ContextMenu("한국어로 전체 변경")] // 인스펙터 우클릭 메뉴에 항목 추가
    private void SetAllKorean() => SetAllLanguage(Languages.Korean); // 씬 내 모든 텍스트를 한국어로 변경

    [ContextMenu("영어로 전체 변경")] // 인스펙터 우클릭 메뉴에 항목 추가
    private void SetAllEnglish() => SetAllLanguage(Languages.English); // 씬 내 모든 텍스트를 영어로 변경

    [ContextMenu("일본어로 전체 변경")] // 인스펙터 우클릭 메뉴에 항목 추가
    private void SetAllJapanese() => SetAllLanguage(Languages.Japanese); // 씬 내 모든 텍스트를 일본어로 변경

    private void SetAllLanguage(Languages lang) // 씬 내 모든 LocallizationText 오브젝트에 언어를 일괄 적용하는 메서드
    {
        editorLang = lang; // 자신의 에디터 언어를 변경
        foreach (var t in FindObjectsByType<LocallizationText>(FindObjectsSortMode.None)) // 씬의 모든 LocallizationText를 순회
        {
            t.editorLang = lang; // 각 오브젝트의 에디터 언어를 변경
            t.OnChangeLanguage(lang); // 각 오브젝트의 텍스트를 새 언어로 갱신
        }
    }
#endif
}
