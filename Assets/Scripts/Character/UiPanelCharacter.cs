using UnityEngine;
using TMPro;

public class UiPanelCharacter : MonoBehaviour
{
    public TMP_Dropdown sorting;   // 정렬 옵션을 선택하는 TMP 드롭다운 컴포넌트
    public TMP_Dropdown filterring; // 필터링 옵션을 선택하는 TMP 드롭다운 컴포넌트

    public UiCharacterSlotList uiCharacterSlotList; // 슬롯 목록 UI를 관리하는 UiCharacterSlotList 참조

    // 씬 시작 시 드롭다운 값 변경 이벤트 리스너를 등록하는 메서드
    private void Start()
    {
        sorting.onValueChanged.AddListener(OnchangeSorting);     // 정렬 드롭다운 값 변경 시 OnchangeSorting 호출 등록
        filterring.onValueChanged.AddListener(OnchangeFiltering); // 필터 드롭다운 값 변경 시 OnchangeFiltering 호출 등록
    }

    // 패널이 활성화될 때 데이터를 로드하고 드롭다운 상태를 동기화하는 메서드
    private void OnEnable()
    {
        OnLoad();
        RefreshDropdownOptions();
        OnchangeFiltering(filterring.value);
        OnchangeSorting(sorting.value);
    }

    // 패널이 비활성화될 때 드롭다운 이벤트 리스너를 해제하는 메서드
    private void OnDisable()
    {
        sorting.onValueChanged.RemoveListener(OnchangeSorting);     // 정렬 드롭다운 이벤트 리스너를 제거
        filterring.onValueChanged.RemoveListener(OnchangeFiltering); // 필터 드롭다운 이벤트 리스너를 제거
    }

    // 정렬 드롭다운 인덱스를 받아 슬롯 목록의 정렬 옵션을 변경하는 메서드
    public void OnchangeSorting(int index)
    {
        uiCharacterSlotList.Sorting = (UiCharacterSlotList.SortingOptions)index; // 인덱스를 SortingOptions 열거형으로 변환하여 정렬 옵션을 설정
    }

    // 필터 드롭다운 인덱스를 받아 슬롯 목록의 필터링 옵션을 변경하는 메서드
    public void OnchangeFiltering(int index)
    {
        uiCharacterSlotList.Filtering = (UiCharacterSlotList.FilteringOptions)index; // 인덱스를 FilteringOptions 열거형으로 변환하여 필터링 옵션을 설정
    }

    // 현재 캐릭터 목록 상태(캐릭터 목록, 정렬/필터 옵션)를 파일에 저장하는 메서드
    public void OnSave()
    {
        SaveLoadManager.Data.characterList = uiCharacterSlotList.GetSaveCharacterDataList(); // 현재 슬롯 목록의 캐릭터 데이터를 세이브 데이터에 저장
        SaveLoadManager.Data.CharacterSortingOption = (int)uiCharacterSlotList.Sorting;     // 현재 정렬 옵션을 정수로 변환하여 세이브 데이터에 저장
        SaveLoadManager.Data.CharacterFilteringOption = (int)uiCharacterSlotList.Filtering; // 현재 필터링 옵션을 정수로 변환하여 세이브 데이터에 저장
        SaveLoadManager.Save(); // 세이브 데이터를 파일에 기록
    }

    // 저장된 파일에서 캐릭터 데이터를 불러와 UI를 복원하는 메서드
    public void OnLoad()
    {
        SaveLoadManager.Load(); // 파일에서 세이브 데이터를 불러옴
        uiCharacterSlotList.SetSaveCharacterDataList(SaveLoadManager.Data.characterList); // 불러온 캐릭터 목록을 슬롯 목록 UI에 적용

        uiCharacterSlotList.Sorting = (UiCharacterSlotList.SortingOptions)SaveLoadManager.Data.CharacterSortingOption;     // 저장된 정렬 옵션을 슬롯 목록에 복원
        uiCharacterSlotList.Filtering = (UiCharacterSlotList.FilteringOptions)SaveLoadManager.Data.CharacterFilteringOption; // 저장된 필터링 옵션을 슬롯 목록에 복원

        sorting.SetValueWithoutNotify(SaveLoadManager.Data.CharacterSortingOption);     // 저장된 정렬 인덱스로 드롭다운 표시를 갱신 (이벤트 미발생)
        filterring.SetValueWithoutNotify(SaveLoadManager.Data.CharacterFilteringOption); // 저장된 필터 인덱스로 드롭다운 표시를 갱신 (이벤트 미발생)
    }

    // 랜덤 캐릭터를 획득하여 목록에 추가하는 버튼 핸들러 메서드
    public void OnCreateCharacter()
    {
        uiCharacterSlotList.AddRandomCharacter(); // 슬롯 목록에 랜덤 캐릭터를 생성하여 추가
    }

    // Add 버튼에 연결할 캐릭터 추가 핸들러 메서드
    public void OnAddCharacter()
    {
        uiCharacterSlotList.AddRandomCharacter();
    }

    // 현재 선택된 캐릭터를 목록에서 제거하는 버튼 핸들러 메서드
    public void OnRemoveCharacter()
    {
        uiCharacterSlotList.RemoveCharacter(); // 슬롯 목록에서 선택된 캐릭터를 제거
    }

    // 다국어 테이블에서 텍스트를 가져와 드롭다운 옵션을 재구성하는 메서드
    private void RefreshDropdownOptions()
    {
        int savedSorting = sorting.value;
        sorting.ClearOptions();
        sorting.AddOptions(new System.Collections.Generic.List<string>
        {
            DataTableManager.StringTable.Get("SORT_CREATION_ASC"),
            DataTableManager.StringTable.Get("SORT_CREATION_DESC"),
            DataTableManager.StringTable.Get("SORT_NAME_ASC"),
            DataTableManager.StringTable.Get("SORT_NAME_DESC"),
            DataTableManager.StringTable.Get("SORT_ATTACK_ASC"),
            DataTableManager.StringTable.Get("SORT_ATTACK_DESC"),
            DataTableManager.StringTable.Get("SORT_DEFEND_ASC"),
            DataTableManager.StringTable.Get("SORT_DEFEND_DESC"),
            DataTableManager.StringTable.Get("SORT_HEALTH_ASC"),
            DataTableManager.StringTable.Get("SORT_HEALTH_DESC"),
        });
        sorting.SetValueWithoutNotify(savedSorting);
        sorting.RefreshShownValue();

        int savedFiltering = filterring.value;
        filterring.ClearOptions();
        filterring.AddOptions(new System.Collections.Generic.List<string>
        {
            DataTableManager.StringTable.Get("FILTER_NONE"),
            DataTableManager.StringTable.Get("FILTER_HIGH_ATTACK"),
            DataTableManager.StringTable.Get("FILTER_LOW_ATTACK"),
        });
        filterring.SetValueWithoutNotify(savedFiltering);
        filterring.RefreshShownValue();
    }
}
