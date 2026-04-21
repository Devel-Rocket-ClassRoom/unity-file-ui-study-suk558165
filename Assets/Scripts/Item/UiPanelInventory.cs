using TMPro; 
using UnityEngine;
using System.Collections.Generic; 

public class UiPanelInventory : MonoBehaviour
{
    public TMP_Dropdown sorting; // 정렬 옵션을 선택하는 TMP 드롭다운 컴포넌트

    public TMP_Dropdown filterring; // 필터링 옵션을 선택하는 TMP 드롭다운 컴포넌트

    public UiInvenSlotList uiInvenSlotList; // 슬롯 목록 UI를 관리하는 UiInvenSlotList 참조

    // 씬 시작 시 드롭다운 값 변경 이벤트 리스너를 등록하는 메서드
    private void Start()
    {
        sorting.onValueChanged.AddListener(OnchangeSorting); // 정렬 드롭다운 값 변경 시 OnchangeSorting 호출 등록
        filterring.onValueChanged.AddListener(OnchangeFiltering); // 필터 드롭다운 값 변경 시 OnchangeFiltering 호출 등록
    }

    // 패널이 활성화될 때 데이터를 로드하고 드롭다운 상태를 동기화하는 메서드
    private void OnEnable()
    {

        OnLoad(); // 저장된 인벤토리 데이터를 불러와 슬롯 목록을 갱신
        OnchangeFiltering(filterring.value); // 현재 필터 드롭다운 값으로 필터링 옵션을 적용
        OnchangeSorting(sorting.value); // 현재 정렬 드롭다운 값으로 정렬 옵션을 적용
    }

    // 패널이 비활성화될 때 드롭다운 이벤트 리스너를 해제하는 메서드
    private void OnDisable()
    {
        sorting.onValueChanged.RemoveListener(OnchangeSorting); // 정렬 드롭다운 이벤트 리스너를 제거
        filterring.onValueChanged.RemoveListener(OnchangeFiltering); // 필터 드롭다운 이벤트 리스너를 제거
    }

    // 정렬 드롭다운 인덱스를 받아 슬롯 목록의 정렬 옵션을 변경하는 메서드
    public void OnchangeSorting(int index)
    {
        uiInvenSlotList.Sorting = (UiInvenSlotList.SortingOptions)index; // 인덱스를 SortingOptions 열거형으로 변환하여 정렬 옵션을 설정
    }

    // 필터 드롭다운 인덱스를 받아 슬롯 목록의 필터링 옵션을 변경하는 메서드
    public void OnchangeFiltering(int index)
    {
        uiInvenSlotList.Filtering = (UiInvenSlotList.FilteringOptions)index; // 인덱스를 FilteringOptions 열거형으로 변환하여 필터링 옵션을 설정
    }

    // 현재 인벤토리 상태(아이템 목록, 정렬/필터 옵션)를 파일에 저장하는 메서드
    public void OnSave()
    {
        SaveLoadManager.Data.itemList = uiInvenSlotList.GetSaveItemDataList(); // 현재 슬롯 목록의 아이템 데이터를 세이브 데이터에 저장
        SaveLoadManager.Data.SortingOption = (int)uiInvenSlotList.Sorting; // 현재 정렬 옵션을 정수로 변환하여 세이브 데이터에 저장
        SaveLoadManager.Data.FilteringOption = (int)uiInvenSlotList.Filtering; // 현재 필터링 옵션을 정수로 변환하여 세이브 데이터에 저장
        SaveLoadManager.Save(); // 세이브 데이터를 파일에 기록
    }

    // 저장된 파일에서 인벤토리 데이터를 불러와 UI를 복원하는 메서드
    public void OnLoad()
    {
        SaveLoadManager.Load(); // 파일에서 세이브 데이터를 불러옴
        uiInvenSlotList.SetSaveItemDataList(SaveLoadManager.Data.itemList); // 불러온 아이템 목록을 슬롯 목록 UI에 적용

        uiInvenSlotList.Sorting = (UiInvenSlotList.SortingOptions)SaveLoadManager.Data.SortingOption; // 저장된 정렬 옵션을 슬롯 목록에 복원
        uiInvenSlotList.Filtering = (UiInvenSlotList.FilteringOptions)SaveLoadManager.Data.FilteringOption; // 저장된 필터링 옵션을 슬롯 목록에 복원

        sorting.SetValueWithoutNotify(SaveLoadManager.Data.SortingOption); // 저장된 정렬 인덱스로 드롭다운 표시를 갱신 (이벤트 미발생)
        filterring.SetValueWithoutNotify(SaveLoadManager.Data.FilteringOption); // 저장된 필터 인덱스로 드롭다운 표시를 갱신 (이벤트 미발생)
    }

    // 랜덤 아이템을 인벤토리에 추가하는 버튼 핸들러 메서드
    public void OnCreateItem()
    {
        uiInvenSlotList.AddRandomItem(); // 슬롯 목록에 랜덤 아이템을 생성하여 추가
    }

    // 현재 선택된 아이템을 인벤토리에서 제거하는 버튼 핸들러 메서드
    public void OnRemoveItem()
    {
        uiInvenSlotList.RemoveItem(); // 슬롯 목록에서 선택된 아이템을 제거
    }

    // 다국어 테이블에서 텍스트를 가져와 드롭다운 옵션 레이블을 갱신하는 메서드
   private void RefreshDropdownOptions()
{
    var sortOpts = sorting.options; // 정렬 드롭다운의 옵션 목록 참조
    sortOpts[0].text = DataTableManager.StringTable.Get("SORT_CREATION_ASC");  // 생성 시간 오름차순 레이블을 다국어 텍스트로 갱신
    sortOpts[1].text = DataTableManager.StringTable.Get("SORT_CREATION_DESC"); // 생성 시간 내림차순 레이블을 다국어 텍스트로 갱신
    sortOpts[2].text = DataTableManager.StringTable.Get("SORT_NAME_ASC");      // 이름 오름차순 레이블을 다국어 텍스트로 갱신
    sortOpts[3].text = DataTableManager.StringTable.Get("SORT_NAME_DESC");     // 이름 내림차순 레이블을 다국어 텍스트로 갱신
    sorting.RefreshShownValue(); // 정렬 드롭다운의 현재 선택 항목 표시를 갱신

    var filterOpts = filterring.options; // 필터 드롭다운의 옵션 목록 참조
    filterOpts[0].text = DataTableManager.StringTable.Get("FILTER_NONE");       // 필터 없음 레이블을 다국어 텍스트로 갱신
    filterOpts[1].text = DataTableManager.StringTable.Get("FILTER_WEAPON");     // 무기 필터 레이블을 다국어 텍스트로 갱신
    filterOpts[2].text = DataTableManager.StringTable.Get("FILTER_EQUIP");      // 장비 필터 레이블을 다국어 텍스트로 갱신
    filterOpts[3].text = DataTableManager.StringTable.Get("FILTER_CONSUMABLE"); // 소비 아이템 필터 레이블을 다국어 텍스트로 갱신
    filterring.RefreshShownValue(); // 필터 드롭다운의 현재 선택 항목 표시를 갱신
}
}
