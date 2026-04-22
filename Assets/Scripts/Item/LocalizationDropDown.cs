using System.Collections.Generic;
using TMPro;
using UnityEngine;

// 에디터와 런타임 모두에서 드롭다운 옵션 레이블을 다국어로 자동 갱신하는 컴포넌트
[ExecuteInEditMode] // 에디터 모드에서도 이 스크립트가 실행되도록 설정
public class LocalizationDropDown : MonoBehaviour
{
#if UNITY_EDITOR
    public Languages editorLang; // 에디터 미리보기용으로 선택하는 언어 (에디터 전용)
#endif
    public string[] ids;          // 드롭다운 각 옵션에 대응하는 문자열 테이블 키 배열
    public TMP_Dropdown dropdown;  // 옵션 레이블을 갱신할 TMP_Dropdown 컴포넌트

    // 오브젝트가 활성화될 때 언어 변경 이벤트를 등록하고 현재 언어로 옵션을 갱신하는 메서드
    private void OnEnable()
    {
        if (Application.isPlaying) // 실제 플레이 중일 때
        {
            Variables.OnLanguageChanged += OnChangeLanguage; // 언어 변경 이벤트 구독
            OnChangeLanguage(); // 현재 언어로 드롭다운 옵션 즉시 갱신
        }
#if UNITY_EDITOR
        else // 에디터 모드일 때
        {
            OnChangeLanguage(editorLang); // 에디터에서 선택한 언어로 미리보기 적용
        }
#endif
    }

    // 오브젝트가 비활성화될 때 언어 변경 이벤트 구독을 해제하는 메서드
    private void OnDisable()
    {
        if (Application.isPlaying) // 실제 플레이 중일 때만 해제 (에디터 모드는 구독 안 했으므로 불필요)
        {
            Variables.OnLanguageChanged -= OnChangeLanguage; // 언어 변경 이벤트 구독 해제
        }
    }

    // 인스펙터 값이 변경될 때 에디터 미리보기를 갱신하는 메서드 (에디터 전용)
    private void OnValidate()
    {
#if UNITY_EDITOR
        OnChangeLanguage(editorLang); // 에디터 언어로 드롭다운 옵션 미리보기 갱신
#endif
    }

    // 런타임에서 언어 변경 이벤트가 발생할 때 호출되는 메서드
    private void OnChangeLanguage()
    {
        Apply(DataTableManager.StringTable); // 현재 언어의 스트링 테이블로 드롭다운 옵션 갱신
    }

#if UNITY_EDITOR
    // 에디터에서 지정 언어로 드롭다운 옵션을 갱신하는 메서드 (에디터 전용)
    private void OnChangeLanguage(Languages lang)
    {
        Apply(DataTableManager.GetStringTable(lang)); // 지정 언어의 스트링 테이블로 드롭다운 옵션 갱신
    }

    // 에디터 컨텍스트 메뉴에서 씬 내 모든 LocalizationDropDown의 언어를 일괄 변경하는 메서드
    [ContextMenu("ChangeLanguage")]
    private void ChangeLanguage()
    {
        var all = FindObjectsByType<LocalizationDropDown>(FindObjectsSortMode.None); // 씬의 모든 LocalizationDropDown 컴포넌트 검색
        foreach (var item in all) // 각 컴포넌트에 대해 반복
        {
            item.editorLang = editorLang;          // 에디터 언어를 현재 선택한 언어로 동기화
            item.OnChangeLanguage(editorLang);     // 해당 언어로 드롭다운 옵션 갱신
        }
    }
#endif

    // 전달받은 스트링 테이블을 사용하여 드롭다운 옵션 레이블을 갱신하는 내부 메서드
    private void Apply(StringTable table)
    {
        if (dropdown == null || ids == null) // 드롭다운 또는 키 배열이 없으면 처리하지 않음
            return;

        int prevValue = dropdown.value; // 갱신 전 현재 선택 인덱스를 저장 (갱신 후 복원용)
        dropdown.ClearOptions();        // 기존 드롭다운 옵션을 모두 제거

        var options = new List<TMP_Dropdown.OptionData>(ids.Length); // 새 옵션 리스트 생성
        for (int i = 0; i < ids.Length; i++)
            options.Add(new TMP_Dropdown.OptionData(table.Get(ids[i]))); // 각 키에 해당하는 다국어 텍스트를 옵션으로 추가
        dropdown.AddOptions(options); // 새 옵션 리스트를 드롭다운에 적용

        if (ids.Length > 0)
            dropdown.value = Mathf.Clamp(prevValue, 0, ids.Length - 1); // 이전 선택 인덱스를 유효 범위 내로 클램핑하여 복원
        dropdown.RefreshShownValue(); // 드롭다운 표시 텍스트를 현재 값으로 갱신
    }
}
